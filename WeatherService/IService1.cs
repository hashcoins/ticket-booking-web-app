using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using System.Net;
using System.IO;

namespace _5DayForecast
{
    public class Service1 : IService1
    {
        // Author: Hunter Thompson
        // CSE 445 FALL 2021

        // Service obtains the weather conditions and location details using a ZIP code as parameter.
        // This program uses openweathermap API in it's calls.
        // Data returned: City, 5 day temp, 5 day weather with dates for each day.
        // This web service parses JSON responses from the API services using the JSON.NET library

        public string[] Weather5day(string zipcode)
        {
            string apiKey = "496b299ddd110607a494211b27733328"; // Api key from openweathermap.org/forecast5
            string url = "";

            if (zipcode.Length == 5)
            {
                url = $"https://api.openweathermap.org/data/2.5/forecast?zip=" + zipcode + "&units=imperial&appid=" + apiKey + "&lang=en.json"; //Inputs zipcode into API call
            }
            else
            {
                url = $"https://api.openweathermap.org/data/2.5/forecast?zip=85201&units=imperial&appid=" + apiKey + "&lang=en.json"; //Defaults to Tempe, Arizona, United States of America
            }

            List<string> details = new List<string>(); // List of strings to return later

            WeatherObject rt = new WeatherObject();// Creates object to deserialize JSON doc into

            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(url); // Retrieves JSON Document
                string data = json.ToString(); //saves string for debugging
                JObject o = JObject.Parse(data); //turns object into a JObject in order to use select token
                //this is needed because the json holds an array within it's contents.

                rt = JsonConvert.DeserializeObject<WeatherObject>(json); // Deserializes the JSON document                                    

                string weather = "";
                string temp = "";
                string date = "";

                // Adds various different details to the list
                details.Add((rt.city.name).ToString() + ", US"); // Adds "City, Country" to the list
                details.Add(url); // Adds URL for user to access more information

                //details.Add((rtlist[0].root.list[1].weather).ToString()) ; // Adds "City, Country" to the list

                //Adding the weather
                weather = (string)o.SelectToken("list[0].weather[0].description"); //Selects the nth element from the list and calls that weather description
                details.Add(weather); //Adds
                weather = (string)o.SelectToken("list[8].weather[0].description");
                details.Add(weather);
                weather = (string)o.SelectToken("list[16].weather[0].description");
                details.Add(weather);
                weather = (string)o.SelectToken("list[24].weather[0].description");
                details.Add(weather);
                weather = (string)o.SelectToken("list[32].weather[0].description");
                details.Add(weather);

                //Adding the temperature
                temp = (string)o.SelectToken("list[0].main.temp"); //Selects the nth element from the list and calls that temp
                details.Add(temp + " F"); //Adds
                temp = (string)o.SelectToken("list[8].main.temp");
                details.Add(temp + " F");
                temp = (string)o.SelectToken("list[16].main.temp");
                details.Add(temp + " F");
                temp = (string)o.SelectToken("list[24].main.temp");
                details.Add(temp + " F");
                temp = (string)o.SelectToken("list[32].main.temp");
                details.Add(temp + " F");


                //Adding the date
                date = (string)o.SelectToken("list[0].dt_txt"); //Selects the nth element from the list and calls that date
                details.Add(date); //Adds
                date = (string)o.SelectToken("list[8].dt_txt");
                details.Add(date);
                date = (string)o.SelectToken("list[16].dt_txt");
                details.Add(date);
                date = (string)o.SelectToken("list[24].dt_txt");
                details.Add(date);
                date = (string)o.SelectToken("list[32].dt_txt");
                details.Add(date);



                //details.Add(rt.root.list[0].weather[1].description);
                //details.Add("Current Temperature: " + rt.l.temp);
                //details.Add("Feels Like: " + rt.list.feelslike);
                //details.Add("Humidity: " + rt.list.humidity);

            }

            return details.ToArray(); //Returns data array
        }


        // Class definitions for the JSON documents to be loaded for the service invocation:
        public class WeatherObject
        {
            public Main main { get; set; }
            public class Main
            {
                public double temp { get; set; }
                public double feels_like { get; set; }
                public double temp_min { get; set; }
                public double temp_max { get; set; }
                public int pressure { get; set; }
                public int sea_level { get; set; }
                public int grnd_level { get; set; }
                public int humidity { get; set; }
                public double temp_kf { get; set; }
            }

            public Weather weather { get; set; }
            public class Weather
            {
                public int id { get; set; }
                public string main { get; set; }
                public string description { get; set; }
                public string icon { get; set; }
            }

            public class Clouds
            {
                public int all { get; set; }
            }

            public class Wind
            {
                public double speed { get; set; }
                public int deg { get; set; }
                public double gust { get; set; }
            }

            public class Sys
            {
                public string pod { get; set; }
            }
            public class List
            {
                public int dt { get; set; }
                public Main main { get; set; }
                public List<Weather> weather { get; set; }
                public Clouds clouds { get; set; }
                public Wind wind { get; set; }
                public int visibility { get; set; }
                public int pop { get; set; }
                public Sys sys { get; set; }
                public string dt_txt { get; set; }
            }

            public class Coord
            {
                public double lat { get; set; }
                public double lon { get; set; }
            }

            public City city { get; set; }
            public class City
            {
                public int id { get; set; }
                public string name { get; set; }
                public Coord coord { get; set; }
                public string country { get; set; }
                public int population { get; set; }
                public int timezone { get; set; }
                public int sunrise { get; set; }
                public int sunset { get; set; }
            }
            public Root root { get; set; }
            public class Root
            {
                public string cod { get; set; }
                public int message { get; set; }
                public int cnt { get; set; }
                public List<List> list { get; set; }
                public City city { get; set; }
            }
        }
        public class WeatherListObject
        {
            public Main main { get; set; }
            public class Main
            {
                public double temp { get; set; }
                public double feels_like { get; set; }
                public double temp_min { get; set; }
                public double temp_max { get; set; }
                public int pressure { get; set; }
                public int sea_level { get; set; }
                public int grnd_level { get; set; }
                public int humidity { get; set; }
                public double temp_kf { get; set; }
            }

            public Weather weather { get; set; }
            public class Weather
            {
                public int id { get; set; }
                public string main { get; set; }
                public string description { get; set; }
                public string icon { get; set; }
            }

            public class Clouds
            {
                public int all { get; set; }
            }

            public class Wind
            {
                public double speed { get; set; }
                public int deg { get; set; }
                public double gust { get; set; }
            }

            public class Sys
            {
                public string pod { get; set; }
            }
            public class List
            {
                public int dt { get; set; }
                public Main main { get; set; }
                public List<Weather> weather { get; set; }
                public Clouds clouds { get; set; }
                public Wind wind { get; set; }
                public int visibility { get; set; }
                public int pop { get; set; }
                public Sys sys { get; set; }
                public string dt_txt { get; set; }
            }

            public class Coord
            {
                public double lat { get; set; }
                public double lon { get; set; }
            }

            public City city { get; set; }
            public class City
            {
                public int id { get; set; }
                public string name { get; set; }
                public Coord coord { get; set; }
                public string country { get; set; }
                public int population { get; set; }
                public int timezone { get; set; }
                public int sunrise { get; set; }
                public int sunset { get; set; }
            }

            public Root root { get; set; }
            public class Root
            {
                public string cod { get; set; }
                public int message { get; set; }
                public int cnt { get; set; }
                public List<List> list { get; set; }
                public City city { get; set; }
            }
        }

        public string Get(string uri) //Used to establish a get request from the api server
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public double AnnualSolar(decimal latitude, decimal longitude) //Function to find the solar intensity of a longitude or laititude
        {
            string apiKey = "97YMsPyjf0aikPYp6yVPg2vPXkoOz12VSm8F6drS"; //API Key from developer.nrel.gov

            string uri = $"https://developer.nrel.gov/api/solar/solar_resource/v1.json?api_key=" + apiKey + "&lat=" + latitude + "&lon=" + longitude; //Uri

            //string url = Get(uri);

            double score = 0; //default score

            SolarObject rt = new SolarObject();// Creates object to deserialize JSON doc into

            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(uri); // Retrieves JSON Document
                string data = json.ToString(); //saves string for debugging
                JObject o = JObject.Parse(data); //turns object into a JObject in order to use select token
                                                 //this is needed because the json holds an array within it's contents.

                rt = JsonConvert.DeserializeObject<SolarObject>(json); // Deserializes the JSON document 

                score = rt.outputs.avg_dni.annual; //retrieves out of annual dni or Direct Normal Irradiance 

            }

            return score;
        }

        //Object root class to delineate each category of the json for ease of access
        public class SolarObject
        {
            public class Metadata
            {
                public List<string> sources { get; set; }
            }

            public class Inputs
            {
                public string api_key { get; set; }
                public string lat { get; set; }
                public string lon { get; set; }
            }

            public class Monthly
            {
                public double jan { get; set; }
                public double feb { get; set; }
                public double mar { get; set; }
                public double apr { get; set; }
                public double may { get; set; }
                public double jun { get; set; }
                public double jul { get; set; }
                public double aug { get; set; }
                public double sep { get; set; }
                public double oct { get; set; }
                public double nov { get; set; }
                public double dec { get; set; }
            }

            public AvgDni avgDni { get; set; }
            public class AvgDni
            {
                public double annual { get; set; }
                public Monthly monthly { get; set; }
            }

            public class AvgGhi
            {
                public double annual { get; set; }
                public Monthly monthly { get; set; }
            }

            public class AvgLatTilt
            {
                public double annual { get; set; }
                public Monthly monthly { get; set; }
            }

            public Outputs outputs { get; set; }
            public class Outputs
            {
                public AvgDni avg_dni { get; set; }
                public AvgGhi avg_ghi { get; set; }
                public AvgLatTilt avg_lat_tilt { get; set; }
            }

            public Root root { get; set; }
            public class Root
            {
                public string version { get; set; }
                public List<object> warnings { get; set; }
                public List<object> errors { get; set; }
                public Metadata metadata { get; set; }
                public Inputs inputs { get; set; }
                public Outputs outputs { get; set; }
            }
        }
    }
}

