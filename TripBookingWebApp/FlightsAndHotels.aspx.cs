using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace TripBookingWebApp
{
    public partial class FlightsAndHotels : System.Web.UI.Page
    {
        /// <summary>
        /// Invokes the TripBooking service to gather flights based on previously collected inputs, and then associates those flights with items in a listbox.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // If the page was loaded due to a postback (i.e. button press), then ignore the operations below this if statement.
            if (Page.IsPostBack)
            {
                return;
            }

            // DUMMY INPUTS | REMOVE LATER
            string originLatitude = "33.4152";
            string originLongitude = "-111.8315";
            string destinationLatitude = "40.7128";
            string destinationLongitude = "-74.0060";
            string departDate = "12/5/2021";
            string returnDate = "12/11/2021";
            string adults = "1";
            string children = "1";

            // SAVE SOME DUMMY INPUTS | MIGHT REMOVE LATER
            ViewState["destinationLatitude"] = destinationLatitude;
            ViewState["destinationLongitude"] = destinationLongitude;
            ViewState["rooms"] = "1";
            ViewState["adults"] = adults;
            ViewState["children"] = children;
            
            // Collect flight data from the booking service.
            BookingService.AirportAndHotelServiceClient bookingReference = new BookingService.AirportAndHotelServiceClient();
            string[][] flightArray = bookingReference.GetFlightBookings(originLatitude, originLongitude, destinationLatitude, destinationLongitude, departDate, returnDate, adults, children);
            
            // If the service encounters an error, display the error message.
            if (flightArray[0][0].Equals("-1"))
            {
                errorLbl.Text = flightArray[0][1];
                errorLbl.Visible = true;
                return;
            }

            // Convert the flight data into Flight objects.
            // The data for a single flight is in 1 row of the jagged array.
            List<Flight> flights = new List<Flight>();
            Flight flight;
            for (int i = 0; i < flightArray.Length; i++)
            {
                string[] flightInfo = flightArray[i];

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

                flight = new Flight(id, startingLocation, finalLocation, utcOriginDepartureTime, utcDestinationArrivalTime, departureDuration, returnDuration, technicalStops, price, firstCheckedBaggagePrice, secondCheckedBaggagePrice, handLuggagePrice, holdBaggageDimensions, holdBaggageWeight, handBaggageDimensions, handBaggageWeight, availableSeats, outboundCount, inboundCount, routes);
                flights.Add(flight);
            }

            // Associate each Flight object with an item of the flight listbox, which shall contain a text summary of their respective flight.
            for (int i = 0; i < flights.Count; i++)
            {
                // Collect the airlines from the flight's routes.
                string airlinesStr = "";
                foreach (Route someRoute in flights[i].routes)
                {
                    if (airlinesStr.Contains(someRoute.airline) == false)
                    {
                        if (airlinesStr.Length == 0)
                        {
                            airlinesStr = someRoute.airline;
                        }
                        else
                        {
                            airlinesStr += ", " + someRoute.airline;
                        }
                    }
                }

                // Add a new item to the listbox with the flight summary text.
                flightListBox.Items.Add(new ListItem("(" + i + ") | Outbounds: " + flights[i].outboundRouteCount + " | Starting Location: " + flights[i].startingLocation + " | Departure Duration: " + flights[i].departureDuration.ToString("g", new CultureInfo("en-US")) + " | Airlines: " + airlinesStr + " | Price: $" + flights[i].price, i.ToString()));
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
                List<Flight> flights = (List<Flight>)Session["flights"];
                Flight savedFlight = flights[flightIndex];
                infoTxt.Text = savedFlight.ToString();
            }
        }

        /// <summary>
        /// Saves the flight associated with the selected item in the flight listbox, invokes the Booking Service to gather hotel bookings based on previously collected
        /// inputs (as well as departure dates of the saved flights), and then associates those bookings with items in the hotel listbox.
        /// </summary>
        protected void saveFlightBtn_Click(object sender, EventArgs e)
        {
            // If there are no flights gathered in the Session or an item in the flight listbox has an invalid value, display an error message.
            int flightIndex;
            if (Session["flights"] == null || !int.TryParse(flightListBox.SelectedValue, out flightIndex))
            {
                errorLbl.Text = "There are no flights available from the specified origin location to the destination.";
                errorLbl.Visible = true;
                return;
            }
            if (flightIndex <= -1)
            {
                errorLbl.Text = "No flight is selected.";
                errorLbl.Visible = true;
                return;
            }
            errorLbl.Visible = false;

            // Save the selected flight, and update the price and saved flight labels.
            List<Flight> flights = (List<Flight>)Session["flights"];
            Flight selectedFlight = flights[flightIndex];
            priceLbl.Text = "$" + selectedFlight.price;
            Session["savedFlight"] = selectedFlight;
            showSavedFlightBtn.Text = "(" + flightListBox.SelectedValue + ")";

            // Gather inputs for the booking service.
            string destinationLatitude = (string)ViewState["destinationLatitude"];
            string destinationLongitude = (string)ViewState["destinationLongitude"];
            string rooms = (string)ViewState["rooms"];
            string adults = (string)ViewState["adults"];
            string children = (string)ViewState["children"];
            string checkInDate = selectedFlight.utcDestinationArrivalTime.ToString("MM/dd/yyyy");
            // Iterate through all the routes of the saved flight and discover the time of the 1st inbound flight to figure out the check out date for a hotel.
            DateTime earliestInboundDepartureTime = new DateTime();
            bool inboundSet = false;
            foreach (Route route in selectedFlight.routes)
            {
                if (!route.isOutbound && (!inboundSet || earliestInboundDepartureTime > route.utcDepartureTime))
                {
                    inboundSet = true;
                    earliestInboundDepartureTime = route.utcArrivalTime;
                }
            }
            string checkOutDate = earliestInboundDepartureTime.ToString("MM/dd/yyyy");

            // Collect hotel booking data from the booking service.
            BookingService.AirportAndHotelServiceClient bookingReference = new BookingService.AirportAndHotelServiceClient();
            string[][] hotelArray = bookingReference.GetHotelBookings(destinationLatitude, destinationLongitude, checkInDate, checkOutDate, rooms, adults, children);

            // If the service encounters an error, display the error message.
            if (hotelArray[0][0].Equals("-1"))
            {
                errorLbl.Text = hotelArray[0][1] + " Please redirect to the Home page and attempt to book a trip again.";
                errorLbl.Visible = true;
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
                hotelListBox.Items.Add(new ListItem("(" + i + ") " + hotels[i].hotelName + ": $" + hotels[i].totalPrice, i.ToString()));
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
        /// Saves the hotel booking associated with the selected item in the hotel listbox.
        /// </summary>
        protected void saveHotelBtn_Click(object sender, EventArgs e)
        {
            // If there is no saved flight, no hotel bookings gathered in the Session, or an item in the flight listbox has an invalid value, display an error message.
            int hotelIndex;
            if (Session["savedFlight"] == null)
            {
                errorLbl.Text = "No flight was saved. Please save a flight from the Flights list.";
                errorLbl.Visible = true;
                return;
            }
            if (Session["hotels"] == null || !int.TryParse(hotelListBox.SelectedValue, out hotelIndex))
            {
                errorLbl.Text = "There are no hotels available at the specified destination.";
                errorLbl.Visible = true;
                return;
            }
            if (hotelIndex <= -1)
            {
                errorLbl.Text = "No hotel is selected.";
                errorLbl.Visible = true;
                return;
            }
            errorLbl.Visible = false;

            // Save the selected hotel booking, update the hotel and price labels, and save the price.
            List<HotelBooking> hotels = (List<HotelBooking>)Session["hotels"];
            HotelBooking savedHotel = hotels[hotelIndex];
            Session["savedHotel"] = savedHotel;
            showSavedHotelBtn.Text = "(" + hotelListBox.SelectedValue + ")";
            Flight selectedFlight = (Flight)Session["savedFlight"];
            double totalPrice = selectedFlight.price + savedHotel.totalPrice;
            priceLbl.Text = "$" + totalPrice;
            Session["totalPrice"] = totalPrice;
        }

        /// <summary>
        /// If the user is recognized as an existing member, redirects them to the ShoppingCart page.
        /// Otherwise, redirects the user to the MemberLogin parge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bookBtn_Click(object sender, EventArgs e)
        {
            // [IMPLEMENT FUNCTIONALITY]

        }

        /// <summary>
        /// Displays information on the saved flight.
        /// </summary>
        protected void showSavedFlightBtn_Click(object sender, EventArgs e)
        {
            if (Session["savedFlight"] != null)
            {
                Flight savedFlight = (Flight)Session["savedFlight"];
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