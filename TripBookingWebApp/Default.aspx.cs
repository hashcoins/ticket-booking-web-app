using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web.Configuration;
using System.Xml;
using TripBookingLibrary;

namespace TripBookingWebApp
{
    [DataContract]
    public class FlightBooking
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string StartingLocation { get; set; }
        [DataMember]
        public string FinalLocation { get; set; }
        [DataMember]
        public DateTime UtcOriginDepartureTime { get; set; }
        [DataMember]
        public DateTime UtcDestinationArrivalTime { get; set; }
        [DataMember]
        public TimeSpan DepartureDuration { get; set; }
        [DataMember]
        public TimeSpan ReturnDuration { get; set; }
        [DataMember]
        public int TechnicalStops { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public double FirstCheckedBaggagePrice { get; set; }
        [DataMember]
        public double SecondCheckedBaggagePrice { get; set; }
        [DataMember]
        public double HandLuggagePrice { get; set; }
        [DataMember]
        public string HoldBaggageDimensions { get; set; }
        [DataMember]
        public string HoldBaggageWeight { get; set; }
        [DataMember]
        public string HandBaggageDimensions { get; set; }
        [DataMember]
        public string HandBaggageWeight { get; set; }
        [DataMember]
        public int AvailableSeats { get; set; }
        [DataMember]
        public int OutboundRouteCount { get; set; }
        [DataMember]
        public int InboundRouteCount { get; set; }
        [DataMember]
        public List<Route> Routes { get; set; }

        public FlightBooking(string id, string startingLocation, string finalLocation, DateTime utcOriginDepartureTime, DateTime utcDestinationArrivalTime, TimeSpan departureDuration, TimeSpan returnDuration, int technicalStops, double price, double firstCheckedBaggagePrice, double secondCheckedBaggagePrice, double handLuggagePrice, string holdBaggageDimensions, string holdBaggageWeight, string handBaggageDimensions, string handBaggageWeight, int availableSeats, int outboundRouteCount, int inboundRouteCount, List<Route> routes)
        {
            this.ID = id;
            this.StartingLocation = startingLocation;
            this.FinalLocation = finalLocation;
            this.UtcOriginDepartureTime = utcOriginDepartureTime;
            this.UtcDestinationArrivalTime = utcDestinationArrivalTime;
            this.DepartureDuration = departureDuration;
            this.ReturnDuration = returnDuration;
            this.TechnicalStops = technicalStops;
            this.Price = price;
            this.FirstCheckedBaggagePrice = firstCheckedBaggagePrice;
            this.SecondCheckedBaggagePrice = secondCheckedBaggagePrice;
            this.HandLuggagePrice = handLuggagePrice;
            this.HoldBaggageDimensions = holdBaggageDimensions;
            this.HoldBaggageWeight = holdBaggageWeight;
            this.HandBaggageDimensions = handBaggageDimensions;
            this.HandBaggageWeight = handBaggageWeight;
            this.AvailableSeats = availableSeats;
            this.OutboundRouteCount = outboundRouteCount;
            this.InboundRouteCount = inboundRouteCount;
            this.Routes = routes;
        }

        public override string ToString()
        {
            string result = "ID: " + ID;
            result += "\nStarting Location: " + StartingLocation;
            result += "\nFinal Location: " + FinalLocation;
            result += "\nUTC Origin Departure Time: " + UtcOriginDepartureTime.ToString("g", new CultureInfo("en-US")) + " UTC";
            result += "\nUTC Destination Arrival Time: " + UtcDestinationArrivalTime.ToString("g", new CultureInfo("en-US")) + " UTC";
            result += "\nDeparture Duration: " + DepartureDuration.Days + " days, " + DepartureDuration.Hours + " hours, " + DepartureDuration.Minutes + " minutes, " + DepartureDuration.Seconds + " seconds";
            result += "\nReturn Duration: " + ReturnDuration.Days + " days, " + ReturnDuration.Hours + " hours, " + ReturnDuration.Minutes + " minutes, " + ReturnDuration.Seconds + " seconds";
            result += "\nTechnical Stops: " + TechnicalStops.ToString();
            result += "\nPrice: $" + Price.ToString();
            result += "\nFirst Checked Baggage Price: $" + FirstCheckedBaggagePrice.ToString();
            result += "\nSecond Checked Baggage Price: $" + SecondCheckedBaggagePrice.ToString();
            result += "\nHand Luggage Price: $" + HandLuggagePrice.ToString();
            result += "\nHold Baggage Dimensions: " + HoldBaggageDimensions;
            result += "\nHold Baggage Weight: " + HoldBaggageWeight;
            result += "\nHand Baggage Dimensions: " + HandBaggageDimensions;
            result += "\nHand Baggage Weight: " + HandBaggageWeight;
            result += "\nAvailable Seats: " + AvailableSeats.ToString();

            result += "\n\nOutbound Route Amount: " + OutboundRouteCount;
            result += "\nInbound Route Amount: " + InboundRouteCount;
            result += "\nRoutes:";
            foreach (Route route in Routes)
            {
                if (route.IsOutbound)
                {
                    result += "\nOutbound Route —";
                }
                else
                {
                    result += "\nInbound Route —";
                }

                result += "\nDeparture City: " + route.DepartureCity;
                result += "\nArrival City: " + route.ArrivalCity;
                result += "\nAirline: " + route.Airline;
                result += "\nFlight Number: " + route.FlightNumber;
                result += "\nUTC Departure Time: " + route.UtcDepartureTime.ToString("g", new CultureInfo("en-US")) + " UTC";
                result += "\nUTC Arrival Time: " + route.UtcArrivalTime.ToString("g", new CultureInfo("en-US")) + " UTC\n";
            }

            return result;
        }
    }

    [DataContract]
    public class Route
    {
        [DataMember]
        public bool IsOutbound { get; set; }
        [DataMember]
        public string DepartureCity { get; set; }
        [DataMember]
        public string ArrivalCity { get; set; }
        [DataMember]
        public string Airline { get; set; }
        [DataMember]
        public string FlightNumber { get; set; }
        [DataMember]
        public DateTime UtcDepartureTime { get; set; }
        [DataMember]
        public DateTime UtcArrivalTime { get; set; }

        public Route(bool isOutbound, string departureCity, string arrivalCity, string airline, string flightNumber, DateTime utcDepartureTime, DateTime utcArrivalTime)
        {
            this.IsOutbound = isOutbound;
            this.DepartureCity = departureCity;
            this.ArrivalCity = arrivalCity;
            this.Airline = airline;
            this.FlightNumber = flightNumber;
            this.UtcDepartureTime = utcDepartureTime;
            this.UtcArrivalTime = utcArrivalTime;
        }
    }

    [DataContract]
    public class HotelBooking
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string HotelName { get; set; }
        [DataMember]
        public string LodgingType { get; set; }
        [DataMember]
        public string Rating { get; set; }
        [DataMember]
        public int Rooms { get; set; }
        [DataMember]
        public double AvgPricePerNight { get; set; }
        [DataMember]
        public double TotalPrice { get; set; }
        [DataMember]
        public DateTime CheckInDate { get; set; }
        [DataMember]
        public DateTime CheckOutDate { get; set; }
        [DataMember]
        public List<string> Amenities { get; set; }

        public HotelBooking(string id, string hotelName, string lodgingType, string rating, int rooms, double avgPricePerNight, double totalPrice, DateTime checkInDate, DateTime checkOutDate, List<string> amenities)
        {
            this.ID = id;
            this.HotelName = hotelName;
            this.LodgingType = lodgingType;
            this.Rating = rating;
            this.Rooms = rooms;
            this.AvgPricePerNight = avgPricePerNight;
            this.TotalPrice = totalPrice;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
            this.Amenities = amenities;
        }

        public override string ToString()
        {
            string result = "ID: " + ID;
            result += "\nHotel Name: " + HotelName;
            result += "\nLodging Type: " + LodgingType;
            result += "\nRating: " + Rating;
            result += "\nRooms: " + Rooms.ToString();
            result += "\nAverage Price Per Night: $" + AvgPricePerNight.ToString();
            result += "\nTotal Price: $" + TotalPrice.ToString();
            result += "\nCheck-In Date: " + CheckInDate.ToString("d", new CultureInfo("en-US"));
            result += "\nCheck-Out Date: " + CheckOutDate.ToString("d", new CultureInfo("en-US"));
            return result;
        }
    }

    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void memberBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Account/Member.aspx");
        }

        protected void staffBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Protected/Staff.aspx");
        }

        protected void tryItBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("TryIt.aspx");
        }
    }
}