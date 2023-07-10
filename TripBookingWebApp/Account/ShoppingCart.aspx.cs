using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TripBookingWebApp.Account
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        /// <summary>
        /// Displays the saved flight and hotel bookings along with their prices.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["savedFlight"] == null)
            {
                savedFlightTxt.Text = "There are no flights in your shopping cart.";
                return;
            }

            if (Session["savedHotel"] == null)
            {
                savedFlightTxt.Text = "There are no hotels in your shopping cart.";
                return;
            }

            Flight savedFlight = (Flight)Session["savedFlight"];
            HotelBooking savedHotel = (HotelBooking)Session["savedHotel"];

            savedFlightTxt.Text = savedFlight.ToString();
            savedHotelTxt.Text = savedHotel.ToString();

            flightPriceLbl.Text = "$" + savedFlight.price;
            hotelPriceLbl.Text = "$" + savedHotel.totalPrice;
            totalPriceLbl.Text = "Total Price: $" + (savedFlight.price + savedHotel.totalPrice);
        }

        protected void checkoutBtn_Click(object sender, EventArgs e)
        {
            // Redirect to Checkout page.
        }

        /// <summary>
        /// Redirect to Default page.
        /// </summary>
        protected void homeBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx");
        }
    }
}