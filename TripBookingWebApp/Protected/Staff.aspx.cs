using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.Configuration;
using System.Web;

namespace TripBookingWebApp
{
    public partial class Staff : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            if (WebConfigurationManager.AppSettings["staffXmlFilePath"] == null)
            {
                Label5.Text = "File path to Member.xml in web.config is not set.";
                return;
            }
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, WebConfigurationManager.AppSettings["staffXmlFilePath"]);

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("<Person>\n <Username> TA </Username>\n <Password> Cse445ta! </Password>\n</Person>");
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (WebConfigurationManager.AppSettings["staffXmlFilePath"] == null)
            {
                Label5.Text = "File path to Member.xml in web.config is not set.";
                return;
            }
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, WebConfigurationManager.AppSettings["staffXmlFilePath"]);

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("<Person>\n <Username> " + this.TextBox1.Text + " </Username>\n <Password> " + this.TextBox2.Text + " </Password>\n</Person>");
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string theusername = this.TextBox3.Text;
            string thepassword = this.TextBox4.Text;
            if (WebConfigurationManager.AppSettings["staffXmlFilePath"] == null)
            {
                Label5.Text = "File path to Staff.xml in web.config is not set.";
                return;
            }
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, WebConfigurationManager.AppSettings["staffXmlFilePath"]);

            using (StreamReader sr = File.OpenText(path))
            {
                sr.ReadLine(); sr.ReadLine();
                string thisline = sr.ReadLine();
                string thefile = thisline;
                bool itsarealuser = false;
                bool keepgoing = true;
                while (keepgoing == true && itsarealuser == false)
                {
                    string ausername = "";
                    for (int i = 12; thefile[i] != ' '; i++)
                    {
                        ausername = ausername + thefile[i];
                    }
                    thefile = sr.ReadLine();
                    string apassword = "";
                    for (int i = 12; thefile[i] != ' '; i++)
                    {
                        apassword = apassword + thefile[i];
                    }
                    if (ausername == theusername && apassword == thepassword)
                    {
                        itsarealuser = true;
                    }
                    sr.ReadLine();
                    sr.ReadLine();
                    thefile = sr.ReadLine();
                    if (thefile == null)
                    {
                        keepgoing = false;
                    }
                }
                //
                if (itsarealuser == true)
                {
                    this.Label5.Text = "You entered a correct password and username";
                    this.Label1.Visible = true;
                    this.Label2.Visible = true;
                    this.TextBox1.Visible = true;
                    this.TextBox2.Visible = true;
                    this.Button1.Visible = true;
                    this.Button2.Visible = true;
                }
                else
                {
                    this.Label5.Text = "One or both of things you entered is not a real username / password";
                }
            }
        }
    }
}