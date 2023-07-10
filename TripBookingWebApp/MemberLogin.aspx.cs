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
using System.Web.Security;

namespace TripBookingWebApp.Account
{
    public partial class MemberLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Searches Member.xml for an inputted username and then checks if the inputted password matches the password in the XML file after encryption.
        /// </summary>
        protected void loginBtn_Click(object sender, EventArgs e)
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
                        // Add member credentials to cookies.
                        HttpCookie memberCookie = Request.Cookies["memberCredentials"];
                        if (memberCookie == null)
                        {
                            memberCookie = new HttpCookie("memberCredentials");
                        }
                        memberCookie["username"] = username;
                        memberCookie["password"] = password;
                        Response.Cookies.Add(memberCookie);
                        
                        FormsAuthentication.RedirectFromLoginPage(username, true);
                    }
                    else
                    {
                        errorLbl.Text = "That is the incorrect password.";
                        return;
                    }
                }
            }

            errorLbl.Text = "User '" + username + "' does not exist.";
        }

        /// <summary>
        /// Redirects to the Member Register page.
        /// </summary>
        protected void registerBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("MemberRegister.aspx");
        }

        /// <summary>
        /// Redirect to the Default page.
        /// </summary>
        protected void homeBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}