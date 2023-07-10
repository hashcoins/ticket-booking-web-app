using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.IO.Compression;
using System.Web;


namespace AirplaneAndHotelSpace
{
    public class AirportAndHotelService : IAirportAndHotelService
    {
        /// <summary>
        /// Gets information about available flight bookings with outbound and inbound routes between the origin and destination locations.
        /// </summary>
        /// <param name="originLatitude"> latitude of starting location </param>
        /// <param name="originLongitude"> longitude of starting location </param>
        /// <param name="destinationLatitude"> latitude of location to stay at temporarily </param>
        /// <param name="destinationLongitude"> longitude of location to stay at temporarily </param>
        /// <param name="departDate"> date to fly towards destination </param>
        /// <param name="returnDate"> date to fly to starting location </param>
        /// <param name="adults"> amount of adults travelling </param>
        /// <param name="children"> amount of children travelling </param>
        /// <returns> jagged string array where each row represents info for an available flight </returns>
        public string[][] GetFlightBookings(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude, string departDate, string returnDate, string adults, string children)
        {
            // TEST INPUTS
            /*
            originLatitude = "33.3062";
            originLongitude = "-111.8413";
            destinationLatitude = "40.7128";
            destinationLongitude = "-74.0060";
            departDate = "12/5/2021";
            returnDate = "12/11/2021";
            adults = "1";
            children = "1";
            */

            // Check latitude and longitude inputs.
            double lat, lon;
            if (double.TryParse(originLatitude, out lat) == false || lat < -90 || lat > 90)
            {
                return GenerateErrorArray("The origin latitude is invalid.");
            }
            if (double.TryParse(originLongitude, out lon) == false || lon < -180 || lon > 180)
            {
                return GenerateErrorArray("The origin longitude is invalid.");
            }
            if (double.TryParse(destinationLatitude, out lat) == false || lat < -90 || lat > 90)
            {
                return GenerateErrorArray("The destination latitude is invalid.");
            }
            if (double.TryParse(destinationLongitude, out lon) == false || lon < -180 || lon > 180)
            {
                return GenerateErrorArray("The destination longitude is invalid.");
            }

            // Attempt to convert depart and return dates to the DateTime struct; if this cannot be done, return an error message.
            // If either the depart date or return date is earlier than today, then return an error message.
            // If the depart date is earlier than or the same as the return date, then return an error message.
            DateTime departTime, returnTime;
            if (DateTime.TryParse(departDate, out departTime) == false)
            {
                return GenerateErrorArray("The departure date is invalid.");
            }
            if (DateTime.TryParse(returnDate, out returnTime) == false)
            {
                return GenerateErrorArray("The return date is invalid.");
            }
            if (DateTime.Compare(departTime, DateTime.Today) < 0 || DateTime.Compare(returnTime, DateTime.Today) < 0)
            {
                return GenerateErrorArray("The depart date or return date is not later than today.");
            }
            if (DateTime.Compare(departTime, returnTime) >= 0)
            {
                return GenerateErrorArray("The return date is not at least 1 day later than the depart date.");
            }

            // Check the adults and children inputs.
            int adultCount, childCount;
            if (int.TryParse(adults, out adultCount) == false)
            {
                return GenerateErrorArray("The value for the amount of adults is not an integer.");
            }
            if (adultCount < 1)
            {
                return GenerateErrorArray("The amount of adults must be 1 or more.");
            }
            if (int.TryParse(children, out childCount) == false)
            {
                return GenerateErrorArray("The value for the amount of children is not an integer.");
            }
            if (childCount < 0)
            {
                return GenerateErrorArray("The amount of children must be 0 or more.");
            }

            // Reformat the depart and return dates.
            departDate = departTime.ToString("dd/MM/yyyy");
            returnDate = returnTime.ToString("dd/MM/yyyy");

            // Initialize input parameters for web request.
            string apiKey = "MkbVLIAXHiIN5ZQas4rNn14QN8JnBY36";
            string originLocation = originLatitude + "-" + originLongitude + "-500km";
            string destinationLocation = destinationLatitude + "-" + destinationLongitude + "-500km";

            // Construct a URL to acquire data from https://tequila-api.kiwi.com/v2.
            Uri baseUri = new Uri("https://tequila-api.kiwi.com/v2");
            UriTemplate template = new UriTemplate("search?fly_from={fromLocation}&fly_to={toLocation}&date_from={fromDate}&date_to={toDate}&return_from={minReturnDate}&return_to={maxReturnDate}&adults={adults}&children={children}&curr=USD&locale=en&limit=100");
            Uri completeUri = template.BindByPosition(baseUri, originLocation, destinationLocation, departDate, departDate, returnDate, returnDate, adults, children);

            // Prepare for downloading from the above URL.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            WebClient channel = new WebClient();
            channel.Headers.Add("apikey", apiKey);
            channel.Headers.Add("Accept-Encoding", "gzip");
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(FlightSearchResponse));

            // Download data from the above URL and deserialize it into a FlightSearchResponse instance.
            byte[] data;
            try
            {
                data = channel.DownloadData(completeUri);
            }
            catch (WebException webEx)
            {
                return GenerateErrorArray("The https://tequila-api.kiwi.com/v2 API is currently offline or has been deactivated.");
            }
            catch (Exception ex)
            {
                return GenerateErrorArray(ex.Message);
            }
            Stream stream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
            FlightSearchResponse flightSearchObj = (FlightSearchResponse)deserializer.ReadObject(stream);

            // If the FlightSearchResponse instance is null or has no itinerary info, then return an error message.
            if (flightSearchObj == null || flightSearchObj.data == null || flightSearchObj.data.Count <= 0)
            {
                return GenerateErrorArray("There are no bookable flights from the origin location to the destination location.");
            }

            // Construct a list of flight itineraries.
            List<List<string>> itineraries = new List<List<string>>();
            List<string> itinerary;
            foreach (Datum routeData in flightSearchObj.data)
            {
                // Create a new flight itinerary.
                itinerary = new List<string>();

                // Assign the itinerary multiple fields.
                itinerary.Add("ID: " + (routeData.id != null ? routeData.id : "N/A"));
                itinerary.Add("Starting City, Country: " + (routeData.cityFrom != null ? routeData.cityFrom + ", " : "N/A, ") + (routeData.countryFrom != null && routeData.countryFrom.name != null ? routeData.countryFrom.name : "N/A"));
                itinerary.Add("Final City, Country: " + (routeData.cityTo != null ? routeData.cityTo + ", " : "N/A, ") + (routeData.countryTo != null && routeData.countryTo.name != null ? routeData.countryTo.name : "N/A"));
                itinerary.Add("UTC Origin Departure Time: " + (routeData.utc_departure != null ? routeData.utc_departure : "N/A"));
                itinerary.Add("UTC Destination Arrival Time: " + (routeData.utc_arrival != null ? routeData.utc_arrival : "N/A"));
                itinerary.Add("Departure Duration: " + (routeData.duration != null && routeData.duration.departure != null ? routeData.duration.departure + " sec" : "N/A"));
                itinerary.Add("Return Duration: " + (routeData.duration != null && routeData.duration.@return != null ? routeData.duration.@return + " sec" : "N/A"));
                itinerary.Add("Technical Stops: " + routeData.technical_stops);
                itinerary.Add("Price: $" + (routeData.price != null ?  routeData.price : "N/A"));
                itinerary.Add("First Checked Baggage Price: $" + (routeData.bags_price != null && routeData.bags_price._1 != null ? routeData.bags_price._1 : "N/A"));
                itinerary.Add("Second Checked Baggage Price: $" + (routeData.bags_price != null && routeData.bags_price._2 != null ? routeData.bags_price._2 : "N/A"));
                itinerary.Add("Hand Luggage Price: $" + (routeData.bags_price != null && routeData.bags_price.hand != null ? routeData.bags_price.hand : "N/A"));
                
                if (routeData.baglimit != null)
                {
                    itinerary.Add("Hold Baggage Dimensions: " + routeData.baglimit.hold_length + "cm x " + routeData.baglimit.hold_width + "cm x " + routeData.baglimit.hold_height + "cm");
                    itinerary.Add("Hold Baggage Weight: " + routeData.baglimit.hold_weight + "kg");
                    itinerary.Add("Hand Baggage Dimensions: " + routeData.baglimit.hand_length + "cm x " + routeData.baglimit.hand_width + "cm x " + routeData.baglimit.hand_height + "cm");
                    itinerary.Add("Hand Baggage Weight: " + routeData.baglimit.hand_weight + "kg");
                }
                else
                {
                    itinerary.Add("Hold Baggage Dimensions: N/A");
                    itinerary.Add("Hold Baggage Weight: N/A");
                    itinerary.Add("Hand Baggage Dimensions: N/A");
                    itinerary.Add("Hand Baggage Weight: N/A");
                }
                
                itinerary.Add("Available Seats: " + (routeData.availability != null && routeData.availability.seats != null ?  routeData.availability.seats : "N/A"));

                // Add outbound and inbound routes to itinerary.
                if (routeData.route != null)
                {
                    foreach (Route trip in routeData.route)
                    {
                        if (trip.@return == null)
                        {
                            continue;
                        }

                        if (trip.@return.Equals("0"))
                        {
                            itinerary.Add("OUTBOUND BEGIN");
                        }
                        else
                        {
                            itinerary.Add("INBOUND BEGIN");
                        }

                        itinerary.Add("Departure City: " + (trip.cityFrom != null ? trip.cityFrom : "N/A"));
                        itinerary.Add("Arrival City: " + (trip.cityTo != null ? trip.cityTo : "N/A"));
                        itinerary.Add("Airline: " + (trip.airline != null ? trip.airline : "N/A"));
                        itinerary.Add("Flight Number: " + (trip.flight_no != null ? trip.flight_no : "N/A"));
                        itinerary.Add("UTC Departure Time: " + (trip.utc_departure != null ? trip.utc_departure : "N/A"));
                        itinerary.Add("UTC Arrival Time: " + (trip.utc_arrival != null ? trip.utc_arrival : "N/A"));

                        if (trip.@return.Equals("0"))
                        {
                            itinerary.Add("OUTBOUND END");
                        }
                        else
                        {
                            itinerary.Add("INBOUND END");
                        }
                    }
                }

                // Add the itinerary to the intinerary list.
                itineraries.Add(itinerary);
            }

            return itineraries.Select(x => x.ToArray()).ToArray();
        }

        /// <summary>
        /// Gets information about available hotel bookings around a specified destination.
        /// </summary>
        /// <param name="latitude"> latitude of destination </param>
        /// <param name="longitude"> longitude of destination </param>
        /// <param name="checkInDate"> date to claim hotel room(s) </param>
        /// <param name="checkOutDate"> date to relinquish hotel room(s) </param>
        /// <param name="rooms"> amount of rooms to book </param>
        /// <param name="adults"> amount of adults to stay in a hotel </param>
        /// <param name="children"> amount of children to stay in a hotel </param>
        /// <returns> jagged array where each row represents info for a hotel booking </returns>
        public string[][] GetHotelBookings(string latitude, string longitude, string checkInDate, string checkOutDate, string rooms, string adults, string children)
        {
            // TEST INPUTS
            /*
            latitude= "33.3062";
            longitude = "-111.8413";
            checkInDate = "11/20/2021";
            checkOutDate = "12/5/2021";
            rooms = "1";
            adults = "1";
            children = "1";
            */

            // Check the latitude and longitude inputs.
            double lat, lon;
            if (double.TryParse(latitude, out lat) == false || lat < -90 || lat > 90)
            {
                return GenerateErrorArray("The latitude is invalid.");
            }
            if (double.TryParse(longitude, out lon) == false || lon < -180 || lon > 180)
            {
                return GenerateErrorArray("The longitude is invalid.");
            }

            // Attempt to convert check-in and check-out dates to the DateTime struct; if this cannot be done, return an error message.
            // If either the check-in date or check-out date is earlier than today, then return an error message.
            // If the check-in date is earlier than or the same as the check-out date, then return an error message.
            DateTime checkInTime, checkOutTime;
            if (DateTime.TryParse(checkInDate, out checkInTime) == false)
            {
                return GenerateErrorArray("The check-in date is invalid.");
            }
            if (DateTime.TryParse(checkOutDate, out checkOutTime) == false)
            {
                return GenerateErrorArray("The check-out date is invalid.");
            }
            if (DateTime.Compare(checkInTime, DateTime.Today) < 0 || DateTime.Compare(checkOutTime, DateTime.Today) < 0)
            {
                return GenerateErrorArray("The check-in date or check-out date is not later than today.");
            }
            if (DateTime.Compare(checkInTime, checkOutTime) >= 0)
            {
                return GenerateErrorArray("The check-out date is not at least 1 day later than the check-in date.");
            }

            // Check the rooms, adults, and children inputs.
            int roomCount, adultCount, childCount;
            if (int.TryParse(rooms, out roomCount) == false)
            {
                return GenerateErrorArray("The value for the amount of rooms is not an integer.");
            }
            if (roomCount < 1)
            {
                return GenerateErrorArray("The amount of rooms must be 1 or more.");
            }
            if (int.TryParse(adults, out adultCount) == false)
            {
                return GenerateErrorArray("The value for the amount of adults is not an integer.");
            }
            if (adultCount < 1)
            {
                return GenerateErrorArray("The amount of adults must be 1 or more.");
            }
            if (int.TryParse(children, out childCount) == false)
            {
                return GenerateErrorArray("The value for the amount of children is not an integer.");
            }
            if (childCount < 0)
            {
                return GenerateErrorArray("The amount of children must be 0 or more.");
            }

            // Collect random hotel names around the provided latitude and longitude.
            string[] hotelNames = CollectRandomHotelNames(latitude, longitude);
            if (hotelNames.Length == 2 && hotelNames[0].Equals("-1"))  // checks for an error message
            {
                return GenerateErrorArray(hotelNames[1]);
            }

            // Initialize input parameters for a web request.
            string apiKey = "j8aa22uy72pv9ky9atspcjxp";
            string destination = latitude + "," + longitude;
            checkInDate = checkInTime.ToString("MM/dd/yyyy");
            checkOutDate = checkOutTime.ToString("MM/dd/yyyy");

            // Construct a URL to acquire data from http://api.hotwire.com/v1/search/hotel.
            Uri baseUri = new Uri("http://api.hotwire.com/v1");
            UriTemplate template = new UriTemplate("search/hotel?apikey={apiKey}&format=json&dest={destination}&rooms={rooms}&adults={adults}&children={children}&startdate={startDate}&enddate={endDate}");
            Uri completeUri = template.BindByPosition(baseUri, apiKey, destination, rooms, adults, children, checkInDate, checkOutDate);

            // Prepare for downloading from the above URL.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            WebClient channel = new WebClient();
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(HotelShoppingResponse));

            // Download data from the above URL and deserialize it into a HotelShoppingResponse instance.
            byte[] data;
            try
            {
                data = channel.DownloadData(completeUri);
            }
            catch (WebException webEx)
            {
                return GenerateErrorArray("The http://api.hotwire.com/v1/search/hotel API is currently offline or has been deactivated.");
            }
            catch (Exception ex)
            {
                return GenerateErrorArray(ex.Message);
            }
            Stream stream = new MemoryStream(data);
            HotelShoppingResponse hotelShoppingObj = (HotelShoppingResponse)deserializer.ReadObject(stream);

            // If the HotelShoppingResponse instance is null or has no hotel booking info, then return an error message.
            if (hotelShoppingObj == null || hotelShoppingObj.Result == null || hotelShoppingObj.Result.Count <= 0)
            {
                return GenerateErrorArray("There are no bookable hotels at this location.");
            }

            // Associate hotel amenities with unique IDs, if there are any.
            Dictionary<string, string> amenities = new Dictionary<string, string>();
            if (hotelShoppingObj.MetaData.HotelMetaData.Amenities != null && hotelShoppingObj.MetaData.HotelMetaData.Amenities.Count > 0)
            {
                foreach (Amenity amenity in hotelShoppingObj.MetaData.HotelMetaData.Amenities)
                {
                    amenities.Add(amenity.Code, amenity.Name);
                }
            }

            // Construct a list of hotel booking information and save it.
            List<List<string>> hotelBookings = new List<List<string>>();
            List<string> hotelBooking;
            int nameIndex = 0;
            foreach (Result hotelDescription in hotelShoppingObj.Result)
            {
               // Create a hotel booking.
                hotelBooking = new List<string>();

                // Associate the booking to a random hotel name gathered earlier.
                hotelBooking.Add("ID: " + (hotelDescription.ResultId != null ? hotelDescription.ResultId : "N/A"));
                hotelBooking.Add("Hotel Name: " + hotelNames[nameIndex]);
                if (nameIndex == hotelNames.Length - 1)
                {
                    nameIndex = 0;
                }
                else
                {
                    nameIndex++;
                }

                // Assign the booking a lodging type.
                if (hotelDescription.LodgingTypeCode != null)
                {
                    switch (hotelDescription.LodgingTypeCode)
                    {
                        case "H":
                            hotelBooking.Add("Lodging Type: Hotel");
                            break;
                        case "C":
                            hotelBooking.Add("Lodging Type: Condo");
                            break;
                        case "A":
                            hotelBooking.Add("Lodging Type: All-Inclusive Resort");
                            break;
                        default:
                            hotelBooking.Add("Lodging Type: Unknown");
                            break;
                    }
                }
                else
                {
                    hotelBooking.Add("Lodging Type: Unknown");
                }

                // Assign the booking other fields.
                hotelBooking.Add("Rating: " + (hotelDescription.StarRating != null ? hotelDescription.StarRating : "N/A"));
                hotelBooking.Add("Rooms: " + (hotelDescription.Rooms != null ? hotelDescription.Rooms : "N/A"));
                hotelBooking.Add("Average Price Per Night: $" + (hotelDescription.AveragePricePerNight != null ? hotelDescription.AveragePricePerNight : "N/A"));
                hotelBooking.Add("Total Price: $" + (hotelDescription.TotalPrice != null ? hotelDescription.TotalPrice : "N/A"));
                hotelBooking.Add("Check-In Date: " + (hotelDescription.CheckInDate != null ? hotelDescription.CheckInDate : "N/A"));
                hotelBooking.Add("Check-Out Date: " + (hotelDescription.CheckOutDate != null ? hotelDescription.CheckOutDate : "N/A"));

                // Assign the booking amenities.
                string hotelAmenities = "Amenities: ";
                if (hotelDescription.AmenityCodes != null || hotelDescription.AmenityCodes.Count > 0)
                {
                    for (int i = 0; i < hotelDescription.AmenityCodes.Count; i++)
                    {
                        if (amenities.ContainsKey(hotelDescription.AmenityCodes[i]))
                        {
                            if (i < hotelDescription.AmenityCodes.Count - 1)
                            {
                                hotelAmenities += amenities[hotelDescription.AmenityCodes[i]] + ", ";
                            }
                            else
                            {
                                hotelAmenities += amenities[hotelDescription.AmenityCodes[i]];
                            }
                        }
                    }
                }
                else
                {
                    hotelAmenities += "N/A";
                }
                hotelBooking.Add(hotelAmenities);

                // Add the booking to the list of hotel bookings.
                hotelBookings.Add(hotelBooking);
            }

            return hotelBookings.Select(x => x.ToArray()).ToArray();
        }

        /// <summary>
        /// Finds names of businesses near a location with a certain category based on a descriptive term.
        /// Data is retrieved from https://api.yelp.com/v3/businesses/search.
        /// </summary>
        /// <param name="latitude"> latitude to center search on </param>
        /// <param name="longitude"> longitude to center search on </param>
        /// <returns> string array of business names </returns>
        private string[] CollectRandomHotelNames(string latitude, string longitude)
        {
            // TEST INPUTS
            /*
            latitude = "33.3062";
            longitude = "-111.8413";
            */

            // Initialize input parameters for web request.
            string apiKey = "lhNoiAAZNAf-a1oClWbxb1xAfRvj8GM7TuMfql882mDo9bEiyIJYpnGEnOlXa_L4G5DVVr7JbEl94qzVY2PWR63hFZh0AdMmOepdqHtOBXldZccyL3vH8PhzLfVdYXYx";

            // Construct a URL to acquire data from https://api.yelp.com/v3/businesses/search.
            Uri baseUri = new Uri("https://api.yelp.com/v3");
            UriTemplate template = new UriTemplate("businesses/search?term=hotel&categories=hotels&latitude={latitude}&longitude={longitude}&limit=50");
            Uri completeUri = template.BindByPosition(baseUri, latitude, longitude);

            // Prepare for downloading from the URL.
            WebClient channel = new WebClient();
            channel.Headers.Add("Authorization", "Bearer " + apiKey);
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(BusinessSearchResponse));

            // Download data from URL and deserialize it into a BusinessSearchResponse instance.
            byte[] data;
            try
            {
                data = channel.DownloadData(completeUri);
            }
            catch (WebException webEx)
            {
                return new string[] { "-1", "The https://api.yelp.com/v3/businesses/search API is currently offline or has been deactivated." };
            }
            catch (Exception ex)
            {
                return new string[] { "-1", ex.Message };
            }
            Stream stream = new MemoryStream(data);
            BusinessSearchResponse businessSearchObj = (BusinessSearchResponse)deserializer.ReadObject(stream);

            // If the deserialized instance has no businesses listed, return an error message.
            if (businessSearchObj == null || businessSearchObj.businesses == null || businessSearchObj.businesses.Count < 1)
            {
                return new string[] { "-1", "There are no bookable hotels at this location." };
            }

            // Add hotel names to a list.
            List<string> hotelNames = new List<string>();
            foreach (Business business in businessSearchObj.businesses)
            {
                if (business.name != null)
                {
                    hotelNames.Add(business.name);
                }
            }

            return hotelNames.ToArray();
        }

        /// <summary>
        /// Generates a 1x2 jagged string array where the 1st column has "-1" and the 2nd column has the error message.
        /// </summary>
        /// <param name="errorMessage"> message to place in the 2nd column </param>
        /// <returns> 1x2 jagged string array </returns>
        private string[][] GenerateErrorArray(string errorMessage)
        {
            return new string[][] { new string[] { "-1", errorMessage } };
        }
    }

    #region GetFlightBookings Response Classes
    public class CountryFrom
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class CountryTo
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Duration
    {
        public string departure { get; set; }
        [DataMember(Name = "return", IsRequired = false)]
        public string @return { get; set; }
        public string total { get; set; }
    }

    public class Conversion
    {
        public string EUR { get; set; }
    }

    public class BagsPrice
    {
        [DataMember(Name = "1", IsRequired = false)]
        public string _1 { get; set; }
        [DataMember(Name = "2", IsRequired = false)]
        public string _2 { get; set; }
        public string hand { get; set; }
    }

    public class Baglimit
    {
        public int hand_height { get; set; }
        public int hand_length { get; set; }
        public int hand_weight { get; set; }
        public int hand_width { get; set; }
        public int hold_dimensions_sum { get; set; }
        public int hold_height { get; set; }
        public int hold_length { get; set; }
        public int hold_weight { get; set; }
        public int hold_width { get; set; }
    }

    public class Availability
    {
        public string seats { get; set; }
    }

    public class Route
    {
        public string id { get; set; }
        public string combination_id { get; set; }
        public string flyFrom { get; set; }
        public string flyTo { get; set; }
        public string cityFrom { get; set; }
        public string cityCodeFrom { get; set; }
        public string cityTo { get; set; }
        public string cityCodeTo { get; set; }
        public string airline { get; set; }
        public string flight_no { get; set; }
        public string operating_carrier { get; set; }
        public string operating_flight_no { get; set; }
        public string fare_basis { get; set; }
        public string fare_category { get; set; }
        public string fare_classes { get; set; }
        public string fare_family { get; set; }
        [DataMember(Name = "return" ,IsRequired = false)]
        public string @return { get; set; }
        public bool bags_recheck_required { get; set; }
        public bool guarantee { get; set; }
        public bool vi_connection { get; set; }
        public string last_seen { get; set; }
        public string refresh_timestamp { get; set; }
        public string equipment { get; set; }
        public string vehicle_type { get; set; }
        public string local_arrival { get; set; }
        public string utc_arrival { get; set; }
        public string local_departure { get; set; }
        public string utc_departure { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string flyFrom { get; set; }
        public string flyTo { get; set; }
        public string cityFrom { get; set; }
        public string cityCodeFrom { get; set; }
        public string cityTo { get; set; }
        public string cityCodeTo { get; set; }
        public CountryFrom countryFrom { get; set; }
        public CountryTo countryTo { get; set; }
        public List<string> type_flights { get; set; }
        public int nightsInDest { get; set; }
        public string quality { get; set; }
        public string distance { get; set; }
        public Duration duration { get; set; }
        public string price { get; set; }
        public Conversion conversion { get; set; }
        public BagsPrice bags_price { get; set; }
        public Baglimit baglimit { get; set; }
        public Availability availability { get; set; }
        public List<List<string>> routes { get; set; }
        public List<string> airlines { get; set; }
        public List<Route> route { get; set; }
        public string booking_token { get; set; }
        public string deep_link { get; set; }
        public string tracking_pixel { get; set; }
        public bool facilitated_booking_available { get; set; }
        public string pnr_count { get; set; }
        public bool has_airport_change { get; set; }
        public int technical_stops { get; set; }
        public bool throw_away_ticketing { get; set; }
        public bool hidden_city_ticketing { get; set; }
        public bool virtual_interlining { get; set; }
        public List<string> transfers { get; set; }
        public string local_arrival { get; set; }
        public string utc_arrival { get; set; }
        public string local_departure { get; set; }
        public string utc_departure { get; set; }
    }

    public class Seats
    {
        public string passengers { get; set; }
        public string adults { get; set; }
        public string children { get; set; }
        public string infants { get; set; }
    }

    public class SearchParams
    {
        public string flyFrom_type { get; set; }
        public string to_type { get; set; }
        public Seats seats { get; set; }
    }

    public class FlightSearchResponse
    {
        public string search_id { get; set; }
        public int time { get; set; }
        public string currency { get; set; }
        public string fx_rate { get; set; }
        public List<Datum> data { get; set; }
        public SearchParams search_params { get; set; }
        public List<string> all_airlines { get; set; }
        public List<string> all_stopover_airports { get; set; }
        public string del { get; set; }
        public string currency_rate { get; set; }
        public List<string> connections { get; set; }
        public List<string> refresh { get; set; }
        public List<string> ref_tasks { get; set; }
        public string sort_version { get; set; }
    }
    #endregion

    #region GetHotelBookings Response Classes
    /// <summary>
    /// The classes below are utilized to store deserialized JSON data from http://api.hotwire.com/v1/search/hotel.
    /// </summary>
    public class Amenity
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }

    public class Neighborhood
    {
        public string Centroid { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        //public List<object> Shape { get; set; }
        public string State { get; set; }
    }

    public class HotelMetaData
    {
        public List<Amenity> Amenities { get; set; }
        public List<Neighborhood> Neighborhoods { get; set; }
    }

    public class MetaData
    {
        public HotelMetaData HotelMetaData { get; set; }
    }

    public class Result
    {
        public string CurrencyCode { get; set; }
        public string DeepLink { get; set; }
        public string ResultId { get; set; }
        public string HWRefNumber { get; set; }
        public string SubTotal { get; set; }
        public string TaxesAndFees { get; set; }
        public string TotalPrice { get; set; }
        public List<string> AmenityCodes { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string NeighborhoodId { get; set; }
        public string LodgingTypeCode { get; set; }
        public string Nights { get; set; }
        public string AveragePricePerNight { get; set; }
        public string RecommendationPercentage { get; set; }
        public string Rooms { get; set; }
        //public List<object> SpecialTaxItems { get; set; }
        public string StarRating { get; set; }
    }

    public class HotwireError
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class HotelShoppingResponse
    {
        public List<HotwireError> Errors { get; set; }
        public MetaData MetaData { get; set; }
        public List<Result> Result { get; set; }
        public string StatusCode { get; set; }
        public string StatusDesc { get; set; }
    }
    #endregion

    #region CollectRandomHotelNames Response Classes
    /// <summary>
    /// The classes below are utilized to store deserialized JSON data from https://api.yelp.com/v3/businesses/search.
    /// </summary>
    public class BusinessSearchResponse
    {
        public int total { get; set; }
        public List<Business> businesses { get; set; }
        public YelpRegion region { get; set; }
        public YelpError error { get; set; }
    }

    public class Business
    {
        public List<Category> categories { get; set; }
        public Coordinates coordinates { get; set; }
        public string display_phone { get; set; }
        public decimal distance { get; set; }
        public string id { get; set; }
        public string alias { get; set; }
        public string image_url { get; set; }
        public bool is_closed { get; set; }
        public YelpLocation location { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string price { get; set; }
        public decimal rating { get; set; }
        public int review_count { get; set; }
        public string url { get; set; }
        public List<string> transactions { get; set; }
    }

    public class Category
    {
        public string alias { get; set; }
        public string title { get; set; }
    }

    public class Coordinates
    {
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
    }

    public class YelpLocation
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public List<string> display_address { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
    }

    public class YelpRegion
    {
        public Coordinates center { get; set; }
    }

    public class YelpError
    {
        public string code { get; set; }
        public string description { get; set; }
    }
    #endregion
}
