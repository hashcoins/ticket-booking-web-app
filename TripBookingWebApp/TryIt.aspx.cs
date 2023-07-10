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
    public partial class WebForm1 : System.Web.UI.Page
    {
        string[] arrayofimageurls = new string[10];
        string[] arrayofcorrespondingstrings = new string[10];
        string thingtocheck;

        protected void Page_Load(object sender, EventArgs e)
        {
            xmlLoadErrorLbl.Text = "";

            if (IsPostBack)
                return;

            // Collect the Member.xml file path.
            if (WebConfigurationManager.AppSettings["memberXmlFilePath"] == null)
            {
                xmlLoadErrorLbl.Text = "File path (memberXmlFilePath) to Member.xml in web.config is not set.";
                return;
            }
            string fLocation = Path.Combine(HttpRuntime.AppDomainAppPath, WebConfigurationManager.AppSettings["memberXmlFilePath"]);

            // Load Member.xml and display its content.
            Global.memberFileSem.WaitOne();
            XmlDocument doc;
            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);
            writer.Formatting = Formatting.Indented;
            try
            {
                doc = new XmlDocument();
                doc.Load(fLocation);

                doc.WriteTo(writer);
                writer.Flush();

                memberTxt.Text = sw.ToString();
            }
            catch (Exception ex)
            {
                memberTxt.Text = ex.Message;
            }
            finally
            {
                writer.Close();
                sw.Close();
            }
            Global.memberFileSem.Release();

            //
            HttpCookie myCookies = Request.Cookies["Cookiey"];
            if ((myCookies == null) || (myCookies["Location"] == ""))
            {
                cookieStatusLbl.Text = "No Cookies Saved";
            }
            else
            {
                acquireFiveDayForecast(zipcodeTextBox.Text);
                cookieStatusLbl.Text = "Zipcode collected from cookies: " + myCookies["Location"] + ".";
            }

            usersAmountLbl.Text = HttpContext.Current.Application["OnlineCount"].ToString();
            sessionIdLbl.Text = Session.SessionID;

            arrayofimageurls[0] = "https://www.pandasecurity.com/en/mediacenter/src/uploads/2014/09/avoid-captcha.jpg";
            arrayofcorrespondingstrings[0] = "qGphJD";
            arrayofimageurls[1] = "https://www.menosfios.com/wp-content/uploads/2014/12/CAPTCHA.jpg";
            arrayofcorrespondingstrings[1] = "CAPTCHA";
            arrayofimageurls[2] = "https://www.uh.edu/engines/3043-Captcha-smwm.svg.png";
            arrayofcorrespondingstrings[2] = "smwm";
            arrayofimageurls[3] = "https://www.researchgate.net/profile/Hwan-Gue-Cho/publication/224351698/figure/fig2/AS:571184220524544@1513192338732/A-sample-transparent-CAPTCHA-Image-600-x-400-with-Randomly-Assigned-Text-in-Step-4_Q640.jpg";
            arrayofcorrespondingstrings[3] = "iMKiZ";
            arrayofimageurls[4] = "https://www.researchgate.net/profile/Jonathan-Aigrain/publication/277007505/figure/fig3/AS:667814067204096@1536230689050/Example-of-a-Yahoo-captcha-that-uses-the-negative-kerning.png";
            arrayofcorrespondingstrings[4] = "4cz8JyAz";
            arrayofimageurls[5] = "https://www.nicepng.com/png/full/966-9667614_big-image-captcha-code-png.png";
            arrayofcorrespondingstrings[5] = "eating fish";
            arrayofimageurls[6] = "https://i.stack.imgur.com/EQAWO.png";
            arrayofcorrespondingstrings[6] = "fdamc";
            arrayofimageurls[7] = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSzAmKVDtROsU0bH8JfU4ED7l7W0xZoKilO7CnzKdZyYG47MhC_PoOzQFAJRjBnKOUa4g&usqp=CAU";
            arrayofcorrespondingstrings[7] = "Stack";
            arrayofimageurls[8] = "https://e1.pngegg.com/pngimages/21/950/png-clipart-free-kpop-logo-captcha.png";
            arrayofcorrespondingstrings[8] = "B1A4";
            arrayofimageurls[9] = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSpsbaV4B_lR-h7Yjq8cjbPyTOabotjG8CnugiWHOGIacQ2gSM0mScbszf5Y9M3p9_8ZtA&usqp=CAU";
            arrayofcorrespondingstrings[9] = "ic8";
            ViewState["thearrayofimages"] = arrayofimageurls;
            ViewState["arrayofletters"] = arrayofcorrespondingstrings;
            Random rnd = new Random();
            int theonethatsup = rnd.Next(10);
            ViewState["current"] = theonethatsup;
            this.captchaImg.ImageUrl = arrayofimageurls[theonethatsup];
            ViewState["thething"] = thingtocheck = arrayofcorrespondingstrings[theonethatsup];
        }

        /// <summary>
        /// Encrypts a string in an input textbox and displays the result.
        /// </summary>
        protected void encryptBtn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("HI");
            encOutputLbl.Text = Encryption.Encrypt(encInputTxt.Text);
        }

        /// <summary>
        /// Decrypts a string in an input textbox and displays the result.
        /// </summary>
        protected void decryptBtn_Click(object sender, EventArgs e)
        {
            encOutputLbl.Text = Encryption.Decrypt(encInputTxt.Text);
        }

        /// <summary>
        /// Adds a new member with the inputted username and password if the username is not already in Member.xml, and the username and password meet the minimum length requirements.
        /// If the new member is added successfully, the new content of Member.xml is displayed.
        /// </summary>
        protected void addBtn_Click(object sender, EventArgs e)
        {
            // Collect the Member.xml file path, username, and password.
            if (WebConfigurationManager.AppSettings["memberXmlFilePath"] == null)
            {
                memberTxt.Text = "File path to Member.xml in web.config is not set.";
                return;
            }
            string fLocation = Path.Combine(HttpRuntime.AppDomainAppPath, WebConfigurationManager.AppSettings["memberXmlFilePath"]);
            string username = addUsernameTxt.Text;
            string password = addPasswordTxt.Text;

            // Collect the minimum username and password lengths.
            int minUsernameLength = 0, minPasswordLength = 0;
            if (WebConfigurationManager.AppSettings["minUsernameLength"] == null || !int.TryParse(WebConfigurationManager.AppSettings["minUsernameLength"], out minUsernameLength))
            {
                addResultLbl.Text = "Minimum username length (minUsernameLength) is not set properly in web.config.";
                return;
            }
            if (WebConfigurationManager.AppSettings["minPasswordLength"] == null || !int.TryParse(WebConfigurationManager.AppSettings["minPasswordLength"], out minPasswordLength))
            {
                addResultLbl.Text = "Minimum password length (minPasswordLength) is not set properly in web.config.";
                return;
            }

            // Check if the inputted username and password meet the minimum length requirements.
            if (username.Length < minUsernameLength)
            {
                addResultLbl.Text = "Enter a username at least " + minUsernameLength + " characters long.";
                return;
            }
            if (password.Length < minPasswordLength)
            {
                addResultLbl.Text = "Enter a password at least " + minPasswordLength + " character long.";
                return;
            }

            // Load Member.xml.
            Global.memberFileSem.WaitOne();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fLocation);
            }
            catch (Exception ex)
            {
                memberTxt.Text = ex.Message;
                Global.memberFileSem.Release();
                return;
            }

            // Search for the inputted username in Member.xml. If it exists there, display an error message.
            XmlElement rootElement = doc.DocumentElement;
            foreach (XmlNode node in rootElement.ChildNodes)
            {
                if (node["Username"].InnerText.Equals(username))
                {
                    addResultLbl.Text = String.Format("Account with username '{0}' already exists.", username);
                    Global.memberFileSem.Release();
                    return;
                }
            }

            // Add a new member into Member.xml with the inputted username and encrypted inputted password.
            XmlElement memberElem = doc.CreateElement("Member", rootElement.NamespaceURI);
            rootElement.AppendChild(memberElem);

            XmlElement usernameElem = doc.CreateElement("Username", rootElement.NamespaceURI);
            usernameElem.InnerText = username;
            memberElem.AppendChild(usernameElem);

            XmlElement passwordElem = doc.CreateElement("Password", rootElement.NamespaceURI);
            string encryptedPassword = Encryption.Encrypt(password);
            passwordElem.InnerText = encryptedPassword;
            memberElem.AppendChild(passwordElem);

            XmlElement flightElem = doc.CreateElement("SavedFlightBooking", rootElement.NamespaceURI);
            memberElem.AppendChild(flightElem);

            XmlElement hotelElem = doc.CreateElement("SavedHotelBooking", rootElement.NamespaceURI);
            memberElem.AppendChild(hotelElem);

            doc.Save(fLocation);

            // Load the new Member.xml and display it.
            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);
            writer.Formatting = Formatting.Indented;
            try
            {
                doc.Load(fLocation);

                doc.WriteTo(writer);
                writer.Flush();

                memberTxt.Text = sw.ToString();
            }
            catch (Exception ex)
            {
                memberTxt.Text = ex.Message;
            }
            finally
            {
                writer.Close();
                sw.Close();
            }
            Global.memberFileSem.Release();
            addResultLbl.Text = "Member has been added successfully.";
        }

        /// <summary>
        /// Searches Member.xml for an inputted username and then checks if the inputted password matches the password in the XML file after encryption.
        /// </summary>
        protected void authBtn_Click(object sender, EventArgs e)
        {
            // Collect the Member.xml file path, username, and password.
            if (WebConfigurationManager.AppSettings["memberXmlFilePath"] == null)
            {
                memberTxt.Text = "File path to Member.xml in web.config is not set.";
                return;
            }
            string fLocation = Path.Combine(HttpRuntime.AppDomainAppPath, WebConfigurationManager.AppSettings["memberXmlFilePath"]);
            string username = authUsernameTxt.Text;
            string password = authPasswordTxt.Text;

            // Collect the minimum username and password lengths.
            int minUsernameLength = 0, minPasswordLength = 0;
            if (WebConfigurationManager.AppSettings["minUsernameLength"] == null || !int.TryParse(WebConfigurationManager.AppSettings["minUsernameLength"], out minUsernameLength))
            {
                authResultLbl.Text = "Minimum username length (minUsernameLength) is not set properly in web.config.";
                return;
            }
            if (WebConfigurationManager.AppSettings["minPasswordLength"] == null || !int.TryParse(WebConfigurationManager.AppSettings["minPasswordLength"], out minPasswordLength))
            {
                authResultLbl.Text = "Minimum password length (minPasswordLength) is not set properly in web.config.";
                return;
            }

            // Check if the inputted username and password meet the minimum length requirements.
            if (username.Length < minUsernameLength)
            {
                authResultLbl.Text = "Enter a username at least " + minUsernameLength + " characters long.";
                return;
            }
            if (password.Length < minPasswordLength)
            {
                authResultLbl.Text = "Enter a password at least " + minPasswordLength + " character long.";
                return;
            }

            // Load Member.xml.
            Global.memberFileSem.WaitOne();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fLocation);
            }
            catch (Exception ex)
            {
                memberTxt.Text = ex.Message;
                Global.memberFileSem.Release();
                return;
            }
            Global.memberFileSem.Release();

            // If the inputted username is in Member.xml, compare the username's associated password with the inputted password after encryption.
            // If the XML password and encrypted input password match, then redirect to the Member page.
            // Otherwise, display an error.
            XmlElement rootElement = doc.DocumentElement;
            foreach (XmlNode node in rootElement.ChildNodes)
            {
                if (node["Username"].InnerText.Equals(username))
                {
                    if (node["Password"].InnerText.Equals(Encryption.Encrypt(password)))
                    {
                        // [Save user info into cookie???]

                        authResultLbl.Text = String.Format("That is the correct password for {0}.", username);
                        return;
                    }
                    else
                    {
                        authResultLbl.Text = "That is the incorrect password.";
                        return;
                    }
                }
            }

            authResultLbl.Text = String.Format("User '{0}' does not exist.", username);
        }

        /// <summary>
        /// Lists available flights from an origin location to a destination location.
        /// </summary>
        protected void flightBtn_Click(object sender, EventArgs e)
        {
            // Collect inputs.
            string originLatitude = flightOriginLatTxt.Text;
            string originLongitude = flightOriginLonTxt.Text;
            string destinationLatitude = flightDestLatTxt.Text;
            string destinationLongitude = flightDestLonTxt.Text;
            string departDate = flightDepartDateTxt.Text;
            string returnDate = flightReturnDateTxt.Text;
            string adults = flightAdultsTxt.Text;
            string children = flightChildrenTxt.Text;

            // Call GetFlightBookings operation from the booking service.
            BookingService.AirportAndHotelServiceClient bookingRef = new BookingService.AirportAndHotelServiceClient();
            string[][] flights = bookingRef.GetFlightBookings(originLatitude, originLongitude, destinationLatitude, destinationLongitude, departDate, returnDate, adults, children);

            // If the operation returned an error, display it.
            if (flights[0][0].Equals("-1"))
            {
                flightResultTxt.Text = flights[0][1];
                return;
            }

            // Display all the available flight bookings.
            string listedFlights = "";
            foreach (string[] flight in flights)
            {
                foreach (string dataField in flight)
                {
                    listedFlights += dataField + "\n";
                }

                listedFlights += "\n";
            }
            flightResultTxt.Text = listedFlights;
        }

        /// <summary>
        /// Lists available hotel booking at destination location.
        /// </summary>
        protected void hotelBtn_Click(object sender, EventArgs e)
        {
            // Collect inputs.
            string destinationLatitude = hotelDestLatTxt.Text;
            string destinationLongitude = hotelDestLonTxt.Text;
            string checkInDate = hotelCheckInTxt.Text;
            string checkOutDate = hotelCheckOutTxt.Text;
            string rooms = hotelRoomsTxt.Text;
            string adults = hotelAdultsTxt.Text;
            string children = hotelChildrenTxt.Text;

            // Call GetHotelBookings operation from the booking service.
            BookingService.AirportAndHotelServiceClient bookingRef = new BookingService.AirportAndHotelServiceClient();
            string[][] hotelBookings = bookingRef.GetHotelBookings(destinationLatitude, destinationLongitude, checkInDate, checkOutDate, rooms, adults, children);

            // If the operation returned an error, display it.
            if (hotelBookings[0][0].Equals("-1"))
            {
                hotelResultTxt.Text = hotelBookings[0][1];
                return;
            }

            // Display all the available hotel bookings
            string listedBookings = "";
            foreach (string[] booking in hotelBookings)
            {
                foreach (string dataField in booking)
                {
                    listedBookings += dataField + "\n";
                }

                listedBookings += "\n";
            }
            hotelResultTxt.Text = listedBookings;
        }

        protected void zipcodeSubmitBtn_Click(object sender, EventArgs e)
        {
            acquireFiveDayForecast(zipcodeTextBox.Text);
        }

        private void acquireFiveDayForecast(string zipcode)
        {
            WeatherService.Service1Client client = new WeatherService.Service1Client();
            string[] details = client.Weather5day(zipcode);

            //Reset boxes
            day1ListBox.Items.Clear();
            day2ListBox.Items.Clear();
            day3ListBox.Items.Clear();
            day4ListBox.Items.Clear();
            day5ListBox.Items.Clear();

            //Print city
            selectedCityLbl.Text = "City: " + details[0];

            //Add weather
            day1ListBox.Items.Add(details[2]);
            day2ListBox.Items.Add(details[3]);
            day3ListBox.Items.Add(details[4]);
            day4ListBox.Items.Add(details[5]);
            day5ListBox.Items.Add(details[6]);

            //Add temp
            day1ListBox.Items.Add(details[7]);
            day2ListBox.Items.Add(details[8]);
            day3ListBox.Items.Add(details[9]);
            day4ListBox.Items.Add(details[10]);
            day5ListBox.Items.Add(details[11]);

            //Add date
            day1Lbl.Text = details[12];
            day2Lbl.Text = details[13];
            day3Lbl.Text = details[14];
            day4Lbl.Text = details[15];
            day5Lbl.Text = details[16];

            if (sessionCheckBox.Checked == true)
            {
                Session["City"] = details[0];
                sessionStatusLbl.Text = "City has been successfully saved to the session state: " + Session["City"];
                sessionCitiesListBox.Items.Add(Session["City"].ToString());
            }
            if (cookieCheckBox.Checked == true)
            {
                HttpCookie myCookies = new HttpCookie("Cookiey");
                myCookies["Location"] = zipcodeTextBox.Text;
                myCookies.Expires = DateTime.Now.AddMonths(6);
                Response.Cookies.Add(myCookies);
                cookieStatusLbl.Text = "Location has been successfully stored in cookies: " + myCookies["Location"];
            }
        }

        protected void vacationBtn_Click(object sender, EventArgs e)
        {
            // Collect inputs from textboxes.
            string originLatitude = vacationLatTextBox.Text;
            string originLongitude = this.vacationLonTextBox.Text;

            // Check latitude and longitude inputs.
            double lat, lon;
            if (double.TryParse(originLatitude, out lat) == false || lat < -90 || lat > 90)
            {
                vacationResultLbl.Text = ("The origin latitude is invalid.");
                return;
            }
            if (double.TryParse(originLongitude, out lon) == false || lon < -180 || lon > 180)
            {
                vacationResultLbl.Text = ("The origin longitude is invalid.");
                return;
            }

            // Acquire a vacation location.
            FindVacationService.Service1Client myprxy = new FindVacationService.Service1Client();
            decimal[] result = myprxy.Getvactionspot(System.Convert.ToDecimal(originLatitude), System.Convert.ToDecimal(originLongitude));
            this.vacationResultLbl.Text = "Result: " + (result[0].ToString() + ", " + result[1].ToString());
        }

        protected void captchaBtn_Click(object sender, EventArgs e)
        {
            // Compare the captcha string to the correlating user input. If they don't match, display an error message.
            if (this.captchaTxt.Text == ViewState["thething"].ToString())
            {
                this.captchaResultLbl.Text = "Correct.";
            }
            else
            {
                this.captchaResultLbl.Text = "The text in the picture does not match the text in the textbox. Attempt again.";
            }
        }

        protected void captchaChangeBtn_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int theonethatsup = rnd.Next(10);
            ViewState["current"] = theonethatsup;
            string[] anarray = (string[])ViewState["thearrayofimages"];
            this.captchaImg.ImageUrl = anarray[theonethatsup];
            anarray = (string[])ViewState["arrayofletters"];
            ViewState["thething"] = anarray[theonethatsup];
        }

        protected void homeBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}