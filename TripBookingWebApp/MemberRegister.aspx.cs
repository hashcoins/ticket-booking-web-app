using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using TripBookingLibrary;
using System.Web.Configuration;

namespace TripBookingWebApp.Account
{
    public partial class MemberRegister : System.Web.UI.Page
    {
        string[] arrayofimageurls = new string[10];
        string[] arrayofcorrespondingstrings = new string[10];
        string thingtocheck;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
        }

        /// <summary>
        /// Registers a new member with the inputted username and password if the username is not already in Member.xml, and the username and password meet the minimum length requirements.
        /// </summary>
        protected void registerBtn_Click(object sender, EventArgs e)
        {
            errorLbl.Text = "";

            // Collect the Member.xml file path, username, and password.
            if (WebConfigurationManager.AppSettings["memberXmlFilePath"] == null)
            {
                errorLbl.Text = "File path (memberXmlFilePath) to Member.xml in web.config is not set.";
                return;
            }
            string fLocation = Path.Combine(HttpRuntime.AppDomainAppPath, WebConfigurationManager.AppSettings["memberXmlFilePath"]);
            string username = usernameTxt.Text;
            string password = passwordTxt.Text;

            // Collect the minimum username and password lengths.
            int minUsernameLength = 0, minPasswordLength = 0;
            if (WebConfigurationManager.AppSettings["minUsernameLength"] == null || !int.TryParse(WebConfigurationManager.AppSettings["minUsernameLength"], out minUsernameLength))
            {
                errorLbl.Text = "Minimum username length (minUsernameLength) is not set properly in web.config.";
                return;
            }
            if (WebConfigurationManager.AppSettings["minPasswordLength"] == null || !int.TryParse(WebConfigurationManager.AppSettings["minPasswordLength"], out minPasswordLength))
            {
                errorLbl.Text = "Minimum password length (minPasswordLength) is not set properly in web.config.";
                return;
            }

            // Check if the inputted username and password meet the minimum length requirements.
            if (username.Length < minUsernameLength)
            {
                errorLbl.Text = "Enter a username at least " + minUsernameLength + " characters long.";
                return;
            }
            if (password.Length < minPasswordLength)
            {
                errorLbl.Text = "Enter a password at least " + minPasswordLength + " character long.";
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
                errorLbl.Text = ex.Message;
                Global.memberFileSem.Release();
                return;
            }

            // Search for the inputted username in Member.xml. If it exists there, display an error message.
            XmlElement rootElement = doc.DocumentElement;
            foreach (XmlNode node in rootElement.ChildNodes)
            {
                if (node["Username"].InnerText.Equals(username))
                {
                    errorLbl.Text = String.Format("Account with username '{0}' already exists.", username);
                    Global.memberFileSem.Release();
                    return;
                }
            }

            // Compare the captcha string to the correlating user input. If they don't match, display an error message.
            if (this.captchaTxt.Text != ViewState["thething"].ToString())
            {
                Random rnd = new Random();
                int theonethatsup = rnd.Next(10);
                ViewState["current"] = theonethatsup;
                string[] anarray = (string[])ViewState["thearrayofimages"];
                this.captchaImg.ImageUrl = anarray[theonethatsup];
                anarray = (string[])ViewState["arrayofletters"];
                ViewState["thething"] = anarray[theonethatsup];

                this.captchaLbl.Text = "The text in the picture does not match the text in the textbox. Attempt again.";
                this.captchaLbl.Visible = true;
                Global.memberFileSem.Release();
                return;
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
            flightElem.InnerText = "";
            memberElem.AppendChild(flightElem);

            XmlElement hotelElem = doc.CreateElement("SavedHotelBooking", rootElement.NamespaceURI);
            hotelElem.InnerText = "";
            memberElem.AppendChild(hotelElem);

            doc.Save(fLocation);
            Global.memberFileSem.Release();

            // Add member credentials to cookies.
            HttpCookie memberCookie = Request.Cookies["memberCredentials"];
            if (memberCookie == null)
            {
                memberCookie = new HttpCookie("memberCredentials");
            }
            memberCookie["username"] = username;
            memberCookie["password"] = password;
            Response.Cookies.Add(memberCookie);

            // Redirect to Member page.
            Response.Redirect("Account/Member.aspx");
        }

        /// <summary>
        /// Redirect to Default page.
        /// </summary>
        protected void homeBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}