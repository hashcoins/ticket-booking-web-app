using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TripBookingLibrary;
using System.Web.Configuration;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Security;
using System.Globalization;

namespace TripBookingWebApp
{
    public partial class Member : System.Web.UI.Page
    {
        /// <summary>
        /// Displays the content of Member.xml when the page is not loaded by a postback (i.e. button press).
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            loadErrorLbl.Text = "";

            // Redirect the user to the student login page if they are not authenticated.
            if (Page.User == null || !Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            // Ignore the rest of the code if the page is loading by a post back.
            if (Page.IsPostBack) return;

            // Gather username and password from cookies.
            HttpCookie memberCookie = Request.Cookies["memberCredentials"];
            if (memberCookie == null || memberCookie["username"] == null || memberCookie["password"] == null)
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            string username = memberCookie["username"];
            string password = memberCookie["password"];

            // Collect the Member.xml file path.
            if (WebConfigurationManager.AppSettings["memberXmlFilePath"] == null)
            {
                loadErrorLbl.Text = "File path (memberXmlFilePath) to Member.xml in web.config is not set.";
                return;
            }
            string fLocation = Path.Combine(HttpRuntime.AppDomainAppPath, WebConfigurationManager.AppSettings["memberXmlFilePath"]);

            // Load Member.xml.
            Global.memberFileSem.WaitOne();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fLocation);
            }
            catch (Exception ex)
            {
                loadErrorLbl.Text = ex.Message;
                Global.memberFileSem.Release();
                return;
            }
            Global.memberFileSem.Release();

            // Search for the user in Member.xml, and collect their saved flight and hotel bookings.
            bool playerFound = false;
            FlightBooking savedFlight = null;
            HotelBooking savedHotel = null;
            XmlElement rootElement = doc.DocumentElement;
            foreach (XmlNode node in rootElement.ChildNodes)
            {
                if (node["Username"].InnerText.Equals(username))
                {
                    playerFound = true;
                    if (node["Password"].InnerText.Equals(Encryption.Encrypt(password)))
                    {
                        string jsonFlight = node["SavedFlightBooking"].InnerText;
                        string jsonHotel = node["SavedHotelBooking"].InnerText;

                        DataContractJsonSerializer flightDS = new DataContractJsonSerializer(typeof(FlightBooking));
                        DataContractJsonSerializer hotelDS = new DataContractJsonSerializer(typeof(HotelBooking));

                        MemoryStream ms = new MemoryStream();
                        try
                        {
                            ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonFlight));
                            savedFlight = (FlightBooking)flightDS.ReadObject(ms);

                            ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonHotel));
                            savedHotel = (HotelBooking)hotelDS.ReadObject(ms);
                        }
                        catch (Exception ex) { }
                        finally { ms.Close(); }
                    }
                    else
                    {
                        FormsAuthentication.RedirectToLoginPage();
                    }
                }
            }
            if (!playerFound)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            // Gather saved flight and hotel bookings from XML file.
            double totalPrice = 0;
            if (savedFlight == null)
            {
                savedFlightTxt.Text = "There are no flight bookings in your shopping cart.";
            }
            else
            {
                Session["savedFlight"] = savedFlight;
                savedFlightTxt.Text = savedFlight.ToString();
                flightPriceLbl.Text = "$" + savedFlight.Price;
                totalPrice += savedFlight.Price;
            }
            if (savedHotel == null)
            {
                savedHotelTxt.Text = "There are no hotel bookings in your shopping cart.";
            }
            else
            {
                Session["savedHotel"] = savedHotel;
                savedHotelTxt.Text = savedHotel.ToString();
                hotelPriceLbl.Text = "$" + savedHotel.TotalPrice;
                totalPrice += savedHotel.TotalPrice;
            }
            totalPriceLbl.Text = "Total Price: $" + totalPrice;
        }

        /// <summary>
        /// Redirects to the Default page.
        /// </summary>
        protected void homeBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx");
        }

        /// <summary>
        /// Signs the user out and redirects them to the Default page.
        /// </summary>
        protected void signOutBtn_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("../Default.aspx");
        }

        /// <summary>
        /// Simulates the user purchasing flight and hotel bookings by removing them from the shopping cart and Member.xml.
        /// </summary>
        protected void purchaseBtn_Click(object sender, EventArgs e)
        {
            // Check for a saved flight and hotel bookings.
            if (Session["savedFlight"] == null || Session["savedHotel"] == null)
            {
                cartErrorLbl.Text = "There must be a flight booking AND hotel booking before a purchase can occur.";
                return;
            }

            // Collect the Member.xml file path.
            if (WebConfigurationManager.AppSettings["memberXmlFilePath"] == null)
            {
                cartErrorLbl.Text = "File path (memberXmlFilePath) to Member.xml in web.config is not set.";
                return;
            }
            string fLocation = Path.Combine(HttpRuntime.AppDomainAppPath, WebConfigurationManager.AppSettings["memberXmlFilePath"]);

            // Load Member.xml.
            Global.memberFileSem.WaitOne();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fLocation);
            }
            catch (Exception ex)
            {
                cartErrorLbl.Text = ex.Message;
                Global.memberFileSem.Release();
                return;
            }

            // Reset the saved flight and hotel bookings in Member.xml to empty.
            string username = Request.Cookies["memberCredentials"]["username"];
            XmlElement rootElement = doc.DocumentElement;
            foreach (XmlNode node in rootElement.ChildNodes)
            {
                if (node["Username"].InnerText.Equals(username))
                {
                    node["SavedFlightBooking"].InnerText = "";
                    node["SavedHotelBooking"].InnerText = "";
                    break;
                }
            }
            doc.Save(fLocation);
            Global.memberFileSem.Release();

            Session["savedFlight"] = null;
            Session["savedHotel"] = null;

            // Notify the user the bookings have been purchased.
            savedFlightTxt.Text = "There are no flight bookings in your shopping cart.";
            savedHotelTxt.Text = "There are no hotel booking in your shopping cart.";
            cartErrorLbl.Text = "Flight and hotel bookings have been purchased.";
            flightPriceLbl.Text = "$0";
            hotelPriceLbl.Text = "$0";
            totalPriceLbl.Text = "Total Price: $0";
        }

        /// <summary>
        /// Discovers a vacation location, and acquires available flight bookings from the origin location to the vacation location.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void findVacationBtn_Click(object sender, EventArgs e)
        {
            planErrorLbl.Text = "";

            // Collect inputs from textboxes.
            string originLatitude = this.originLatTxt.Text;
            string originLongitude = this.originLonTxt.Text;
            string departDate = departDateTxt.Text;
            string returnDate = returnDateTxt.Text;
            string adults = adultsTxt.Text;
            string children = childrenTxt.Text;
            string rooms = roomsTxt.Text;

            // Check latitude and longitude inputs.
            double lat, lon;
            if (double.TryParse(originLatitude, out lat) == false || lat < -90 || lat > 90)
            {
                vacationLbl.Text = ("The origin latitude is invalid.");
                return;
            }
            if (double.TryParse(originLongitude, out lon) == false || lon < -180 || lon > 180)
            {
                vacationLbl.Text = ("The origin longitude is invalid.");
                return;
            }

            // Acquire a vacation location.
            FindVacationService.Service1Client myprxy = new FindVacationService.Service1Client();
            decimal[] result = myprxy.Getvactionspot(System.Convert.ToDecimal(originLatitude), System.Convert.ToDecimal(originLongitude));
            this.vacationLbl.Text = "Vacation Location: " + (result[0].ToString() + ", " + result[1].ToString());

            // Save the latitude and longitude of the location.
            string destinationLatitude = result[0].ToString();
            string destinationLongitude = result[1].ToString();

            // Save destination inputs for finding available hotel bookings in a different function.
            ViewState["destinationLatitude"] = destinationLatitude;
            ViewState["destinationLongitude"] = destinationLongitude;

            // Collect flight data from the booking service.
            BookingService.AirportAndHotelServiceClient bookingReference = new BookingService.AirportAndHotelServiceClient();
            string[][] flightArray = bookingReference.GetFlightBookings(originLatitude, originLongitude, destinationLatitude, destinationLongitude, departDate, returnDate, adults, children);

            // If the service encounters an error, display the error message.
            if (flightArray[0][0].Equals("-1"))
            {
                planErrorLbl.Text = flightArray[0][1];
                return;
            }

            // Convert the flight data into Flight objects.
            // The data for a single flight is in 1 row of the jagged array.
            List<FlightBooking> flights = new List<FlightBooking>();
            FlightBooking flight;
            foreach (string[] flightInfo in flightArray)
            {
                // Parse through general flight data.
                string id = flightInfo[0].Substring(4);
                string startingLocation = flightInfo[1].Substring(24);
                string finalLocation = flightInfo[2].Substring(21);
                DateTime utcOriginDepartureTime; if (!DateTime.TryParse(flightInfo[3].Substring(27), out utcOriginDepartureTime)) continue;
                DateTime utcDestinationArrivalTime; if (!DateTime.TryParse(flightInfo[4].Substring(30), out utcDestinationArrivalTime)) continue;
                int departureSeconds = -1; if (!int.TryParse(flightInfo[5].Substring(20).Replace(" sec", ""), out departureSeconds)) continue; TimeSpan departureDuration = new TimeSpan(0, 0, departureSeconds);
                int returnSeconds = -1; if (!int.TryParse(flightInfo[6].Substring(17).Replace(" sec", ""), out returnSeconds)) continue; TimeSpan returnDuration = new TimeSpan(0, 0, returnSeconds);
                int technicalStops = 0; int.TryParse(flightInfo[7].Substring(17), out technicalStops);
                double price = -1; if (!double.TryParse(flightInfo[8].Substring(8), out price)) continue;
                double firstCheckedBaggagePrice = -1; double.TryParse(flightInfo[9].Substring(30), out firstCheckedBaggagePrice);
                double secondCheckedBaggagePrice = -1; double.TryParse(flightInfo[10].Substring(31), out secondCheckedBaggagePrice);
                double handLuggagePrice = -1; double.TryParse(flightInfo[11].Substring(21), out handLuggagePrice);
                string holdBaggageDimensions = flightInfo[12].Substring(25);
                string holdBaggageWeight = flightInfo[13].Substring(21);
                string handBaggageDimensions = flightInfo[14].Substring(25);
                string handBaggageWeight = flightInfo[15].Substring(21);
                int availableSeats = -1; if (!int.TryParse(flightInfo[16].Substring(17), out availableSeats)) continue;

                // Parse through the outbound and inbound routes of the flight.
                List<Route> routes = new List<Route>();
                Route route;
                int routeIndex = 17;
                int outboundCount = 0, inboundCount = 0;
                bool isOutbound;
                bool defectiveRouteDetected = false;
                while (routeIndex < flightInfo.Length)
                {
                    if (flightInfo[routeIndex].Equals("OUTBOUND BEGIN"))
                    {
                        outboundCount++;
                        isOutbound = true;
                    }
                    else if (flightInfo[routeIndex].Equals("INBOUND BEGIN"))
                    {
                        inboundCount++;
                        isOutbound = false;
                    }
                    else
                    {
                        break;
                    }

                    string departureCity = flightInfo[routeIndex + 1].Substring(16);
                    string arrivalCity = flightInfo[routeIndex + 2].Substring(14);
                    string airline = flightInfo[routeIndex + 3].Substring(9);
                    string flightNumber = flightInfo[routeIndex + 4].Substring(15);
                    DateTime utcDepartureTime; if (!DateTime.TryParse(flightInfo[routeIndex + 5].Substring(20), out utcDepartureTime)) { defectiveRouteDetected = true; break; }
                    DateTime utcArrivalTime; if (!DateTime.TryParse(flightInfo[routeIndex + 6].Substring(18), out utcArrivalTime)) { defectiveRouteDetected = true; break; }

                    route = new Route(isOutbound, departureCity, arrivalCity, airline, flightNumber, utcDepartureTime, utcArrivalTime);
                    routes.Add(route);
                    routeIndex += 8;
                }

                if (defectiveRouteDetected) continue;

                flight = new FlightBooking(id, startingLocation, finalLocation, utcOriginDepartureTime, utcDestinationArrivalTime, departureDuration, returnDuration, technicalStops, price, firstCheckedBaggagePrice, secondCheckedBaggagePrice, handLuggagePrice, holdBaggageDimensions, holdBaggageWeight, handBaggageDimensions, handBaggageWeight, availableSeats, outboundCount, inboundCount, routes);
                flights.Add(flight);
            }

            // Associate each Flight object with an item of the flight listbox, which shall contain a text summary of their respective flight.
            for (int i = 0; i < flights.Count; i++)
            {
                // Collect the airlines from the flight's routes.
                string airlinesStr = "";
                foreach (Route someRoute in flights[i].Routes)
                {
                    if (airlinesStr.Contains(someRoute.Airline) == false)
                    {
                        if (airlinesStr.Length == 0)
                        {
                            airlinesStr = someRoute.Airline;
                        }
                        else
                        {
                            airlinesStr += ", " + someRoute.Airline;
                        }
                    }
                }

                // Add a new item to the listbox with the flight summary text.
                flightListBox.Items.Add(new ListItem("(" + i + ") | Outbounds: " + flights[i].OutboundRouteCount + " | Starting Location: " + flights[i].StartingLocation + " | Departure Duration: " + flights[i].DepartureDuration.ToString("g", new CultureInfo("en-US")) + " | Airlines: " + airlinesStr + " | Price: $" + flights[i].Price, i.ToString()));
            }

            // Display the 1st Flight object's details in the info textbox.
            infoTxt.Text = flights[0].ToString();

            // Save the list of flights in the Session.
            Session["flights"] = flights;
        }

        /// <summary>
        /// Displays the details for the flight associated with the selected item in the flight listbox.
        /// </summary>
        protected void flightListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int flightIndex;
            if (Session["flights"] != null && int.TryParse(flightListBox.SelectedValue, out flightIndex) && flightIndex > -1)
            {
                List<FlightBooking> flights = (List<FlightBooking>)Session["flights"];
                FlightBooking savedFlight = flights[flightIndex];
                infoTxt.Text = savedFlight.ToString();
            }
        }

        /// <summary>
        /// Saves the flight associated with the selected item in the flight listbox, invokes the Booking Service to gather hotel bookings based on previously collected
        /// inputs (as well as departure dates of the saved flights), and then associates those bookings with items in the hotel listbox.
        /// </summary>
        protected void saveFlightBtn_Click(object sender, EventArgs e)
        {
            planErrorLbl.Text = "";

            // If there are no flights gathered in the Session or an item in the flight listbox has an invalid value, display an error message.
            int flightIndex;
            if (Session["flights"] == null || !int.TryParse(flightListBox.SelectedValue, out flightIndex))
            {
                planErrorLbl.Text = "There are no flights available from the specified origin location to the destination.";
                return;
            }
            if (flightIndex <= -1)
            {
                planErrorLbl.Text = "No flight is selected.";
                return;
            }

            // Save the selected flight.
            List<FlightBooking> flights = (List<FlightBooking>)Session["flights"];
            FlightBooking savedFlight = flights[flightIndex];
            Session["savedFlight"] = savedFlight;
            planTotalPriceLbl.Text = "$" + savedFlight.Price;
            showSavedFlightBtn.Text = "(" + flightListBox.SelectedValue + ")";

            // Serialize the saved flight into a JSON string.
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(FlightBooking));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, savedFlight);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string jsonFlight = sr.ReadToEnd();
            sr.Close();
            ms.Close();

            // Collect the Member.xml file path.
            if (WebConfigurationManager.AppSettings["memberXmlFilePath"] == null)
            {
                planErrorLbl.Text = "File path (memberXmlFilePath) to Member.xml in web.config is not set.";
                return;
            }
            string fLocation = Path.Combine(HttpRuntime.AppDomainAppPath, WebConfigurationManager.AppSettings["memberXmlFilePath"]);

            // Load Member.xml.
            Global.memberFileSem.WaitOne();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fLocation);
            }
            catch (Exception ex)
            {
                planErrorLbl.Text = ex.Message;
                Global.memberFileSem.Release();
                return;
            }

            // Gather username and password from cookie.
            HttpCookie memberCookie = Request.Cookies["memberCredentials"];
            string username = memberCookie["username"];
            string password = memberCookie["password"];

            // Search for the user in Member.xml
            XmlElement rootElement = doc.DocumentElement;
            foreach (XmlNode node in rootElement.ChildNodes)
            {
                if (node["Username"].InnerText.Equals(username))
                {
                    if (node["Password"].InnerText.Equals(Encryption.Encrypt(password)))
                    {
                        node["SavedFlightBooking"].InnerText = jsonFlight;
                    }
                }
            }

            // Save new Member.xml.
            doc.Save(fLocation);
            Global.memberFileSem.Release();

            // Gather inputs for the booking service.
            string destinationLatitude = (string)ViewState["destinationLatitude"];
            string destinationLongitude = (string)ViewState["destinationLongitude"];
            string rooms = roomsTxt.Text;
            string adults = adultsTxt.Text;
            string children = childrenTxt.Text;
            string checkInDate = savedFlight.UtcDestinationArrivalTime.ToString("MM/dd/yyyy");
            // Iterate through all the routes of the saved flight and discover the time of the 1st inbound flight to figure out the check out date for a hotel.
            DateTime earliestInboundDepartureTime = new DateTime();
            bool inboundSet = false;
            foreach (Route route in savedFlight.Routes)
            {
                if (!route.IsOutbound && (!inboundSet || earliestInboundDepartureTime > route.UtcDepartureTime))
                {
                    inboundSet = true;
                    earliestInboundDepartureTime = route.UtcArrivalTime;
                }
            }
            string checkOutDate = earliestInboundDepartureTime.ToString("MM/dd/yyyy");

            // Collect hotel booking data from the booking service.
            BookingService.AirportAndHotelServiceClient bookingReference = new BookingService.AirportAndHotelServiceClient();
            string[][] hotelArray = bookingReference.GetHotelBookings(destinationLatitude, destinationLongitude, checkInDate, checkOutDate, rooms, adults, children);

            // If the service encounters an error, display the error message.
            if (hotelArray[0][0].Equals("-1"))
            {
                loadErrorLbl.Text = hotelArray[0][1] + " Please redirect to the Home page and attempt to book a trip again.";
                loadErrorLbl.Visible = true;
                return;
            }

            // Convert the hotel data into HotelBooking objects.
            // The data for a single booking is in 1 row of the jagged array.
            List<HotelBooking> hotels = new List<HotelBooking>();
            foreach (string[] hotelInfo in hotelArray)
            {
                // Parse through general booking data.
                string id = hotelInfo[0].Substring(7);
                string name = hotelInfo[1].Substring(12);
                string lodgingType = hotelInfo[2].Substring(14);
                string rating = hotelInfo[3].Substring(8);
                int roomsResult = -1; if (int.TryParse(hotelInfo[4].Substring(7), out roomsResult) == false) continue;
                double avgPricePerNight = -1; if (double.TryParse(hotelInfo[5].Substring(26), out avgPricePerNight) == false) continue;
                double totalPrice = -1; if (double.TryParse(hotelInfo[6].Substring(14), out totalPrice) == false) continue;
                DateTime checkInTime = new DateTime(); DateTime.TryParse(hotelInfo[7].Substring(15), out checkInTime);
                DateTime checkOutTime = new DateTime(); DateTime.TryParse(hotelInfo[8].Substring(16), out checkOutTime);

                // Parse through amenities the booking provides.
                List<string> amenities = hotelInfo[9].Substring(11).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                for (int i = 0; i < amenities.Count; i++)
                {
                    amenities[i] = amenities[0].Trim(new char[] { ' ' });
                }

                HotelBooking hotel = new HotelBooking(id, name, lodgingType, rating, roomsResult, avgPricePerNight, totalPrice, checkInTime, checkOutTime, amenities);
                hotels.Add(hotel);
            }

            // Associate each HotelBooking object with an item of the hotel listbox, which shall contain a text summary of their respective bookings.
            for (int i = 0; i < hotels.Count; i++)
            {
                hotelListBox.Items.Add(new ListItem("(" + i + ") " + hotels[i].HotelName + ": $" + hotels[i].TotalPrice, i.ToString()));
            }

            // Display the 1st BookingHotel object's details in the info textbox.
            infoTxt.Text = hotels[0].ToString();

            // Save the hotel bookings in the Session.
            Session["hotels"] = hotels;
        }

        /// <summary>
        /// Displays the details for the hotel booking associated with the selected item in the hotel listbox.
        /// </summary>
        protected void hotelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int hotelIndex;
            if (Session["hotels"] != null && int.TryParse(hotelListBox.SelectedValue, out hotelIndex) && hotelIndex > -1)
            {
                List<HotelBooking> hotels = (List<HotelBooking>)Session["hotels"];
                HotelBooking selectedHotel = hotels[hotelIndex];
                infoTxt.Text = selectedHotel.ToString();
            }
        }

        /// <summary>
        /// Saves the hotel booking associated with the selected item in the hotel listbox, and displays the flight and hotel bookings in shopping cart.
        /// </summary>
        protected void saveHotelBtn_Click(object sender, EventArgs e)
        {
            planErrorLbl.Text = "";

            // If there is no saved flight, no hotel bookings gathered in the Session, or an item in the flight listbox has an invalid value, display an error message.
            int hotelIndex;
            if (Session["savedFlight"] == null)
            {
                planErrorLbl.Text = "No flight was saved. Please save a flight from the Flights list.";
                return;
            }
            if (Session["hotels"] == null || !int.TryParse(hotelListBox.SelectedValue, out hotelIndex))
            {
                planErrorLbl.Text = "There are no hotels available at the specified destination.";
                return;
            }
            if (hotelIndex <= -1)
            {
                planErrorLbl.Text = "No hotel is selected.";
                return;
            }

            // Save the selected hotel booking, update the hotel and price labels, and save the price.
            List<HotelBooking> hotels = (List<HotelBooking>)Session["hotels"];
            HotelBooking savedHotel = hotels[hotelIndex];
            Session["savedHotel"] = savedHotel;
            showSavedHotelBtn.Text = "(" + hotelListBox.SelectedValue + ")";
            FlightBooking selectedFlight = (FlightBooking)Session["savedFlight"];
            double totalPrice = selectedFlight.Price + savedHotel.TotalPrice;
            planTotalPriceLbl.Text = "$" + totalPrice;
            Session["totalPrice"] = totalPrice;

            // Serialize the booking into a JSON string.
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(HotelBooking));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, savedHotel);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string jsonHotel = sr.ReadToEnd();
            sr.Close();
            ms.Close();

            // Collect the Member.xml file path.
            if (WebConfigurationManager.AppSettings["memberXmlFilePath"] == null)
            {
                loadErrorLbl.Text = "File path (memberXmlFilePath) to Member.xml in web.config is not set.";
                return;
            }
            string fLocation = Path.Combine(HttpRuntime.AppDomainAppPath, WebConfigurationManager.AppSettings["memberXmlFilePath"]);

            // Load Member.xml.
            Global.memberFileSem.WaitOne();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fLocation);
            }
            catch (Exception ex)
            {
                loadErrorLbl.Text = ex.Message;
                Global.memberFileSem.Release();
                return;
            }

            // Gather username from cookie.
            HttpCookie memberCookie = Request.Cookies["memberCredentials"];
            string username = memberCookie["username"];

            // Insert the booking into Member.xml.
            XmlElement rootElement = doc.DocumentElement;
            foreach (XmlNode node in rootElement.ChildNodes)
            {
                if (node["Username"].InnerText.Equals(username))
                {
                    node["SavedHotelBooking"].InnerText = jsonHotel;
                }
            }
            doc.Save(fLocation);
            Global.memberFileSem.Release();
        }

        /// <summary>
        /// Displays the saved flight and hotel bookings on the shopping cart textboxes.
        /// </summary>
        protected void moveToCartBtn_Click(object sender, EventArgs e)
        {
            planErrorLbl.Text = "";

            if (Session["savedFlight"] == null || Session["savedHotel"] == null)
            {
                planErrorLbl.Text = "A flight booking and hotel booking from the lists must be saved.";
                return;
            }

            double totalPrice = 0;

            FlightBooking savedFlight = (FlightBooking)Session["savedFlight"];
            savedFlightTxt.Text = savedFlight.ToString();
            flightPriceLbl.Text = "$" + savedFlight.Price;
            totalPrice += savedFlight.Price;

            HotelBooking savedHotel = (HotelBooking)Session["savedHotel"];
            savedHotelTxt.Text = savedHotel.ToString();
            hotelPriceLbl.Text = "$" + savedHotel.TotalPrice;
            totalPrice += savedHotel.TotalPrice;

            totalPriceLbl.Text = "Total Price: $" + totalPrice;
        }

        /// <summary>
        /// Displays information on the saved flight.
        /// </summary>
        protected void showSavedFlightBtn_Click(object sender, EventArgs e)
        {
            if (Session["savedFlight"] != null)
            {
                FlightBooking savedFlight = (FlightBooking)Session["savedFlight"];
                infoTxt.Text = savedFlight.ToString();
            }
        }

        /// <summary>
        /// Displays information on the saved hotel booking.
        /// </summary>
        protected void showSavedHotelBtn_Click(object sender, EventArgs e)
        {
            if (Session["savedHotel"] != null)
            {
                HotelBooking savedHotel = (HotelBooking)Session["savedHotel"];
                infoTxt.Text = savedHotel.ToString();
            }
        }
    }
}