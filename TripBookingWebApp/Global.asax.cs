using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;
using System.Threading;

namespace TripBookingWebApp
{
    public class Global : HttpApplication
    {
        /// <summary>
        /// Semaphore with an initial and max value of 1. This is utilized for preventing multiple, simultaneous accesses to Member.xml.
        /// </summary>
        public static Semaphore memberFileSem;

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["OnlineCount"] = 0;
            memberFileSem = new Semaphore(1, 1, "memberFileSem");
        }

        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 20;
            Application.Lock(); //Lock is used to prevent two threads from updating the variable at once.
            Application["OnlineCount"] = Convert.ToInt32(Application["OnlineCount"]) + 1;
            Application.UnLock(); //Allows another thread
        }

        void Session_End(object sender, EventArgs e)
        {
            Session.Timeout = 20;
            Application.Lock(); //Lock is used to prevent two threads from updating the variable at once.
            Application["OnlineCount"] = Convert.ToInt32(Application["OnlineCount"]) - 1;
            Application.UnLock(); //Allows another thread
        }
    }
}