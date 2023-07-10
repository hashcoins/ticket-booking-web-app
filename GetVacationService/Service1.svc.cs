using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net;
using System.Xml;

namespace Wcf_getvactionspotservice_
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public decimal[] Getvactionspot(decimal longitude, decimal latitude)
        {
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead("https://www.latlong.net/latest-places.html");
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                String text = reader.ReadToEnd();
                int thestart = text.IndexOf("<table>");
                int thend = text.IndexOf("</table>");
                text = text.Substring(thestart, thend - thestart + 8);
                text = text.Remove(0,70);
                int count = 0;
                String isittr = "";
                //at position 0 is latitude 1 longitude 2 latitude and so on
                List<decimal> Alllatitudesandslongitude = new List<decimal>();
                for (int i = 0; i < text.Length; i++)
                {
                    if (count < 4)
                    {
                        count = count + 1;
                        isittr = isittr + text[i];
                    }
                    else
                    {
                        count = 0;
                        if (isittr == "<tr>")
                        {
                            int nextthing = 0;
                            String isittdslash = "";
                            int counttosopgettingdata  = 0;
                            for (int n = i + 1; n < text.Length; n++)
                            {
                                if (nextthing < 5)
                                {
                                    nextthing = nextthing + 1;
                                    isittdslash = isittdslash + text[n];
                                }
                                else
                                {
                                    nextthing = 0;
                                    if (isittdslash == "</td>")
                                    {
                                        int anotherthing = 0;
                                        String isittd = "";
                                        for (int v = n; v < text.Length; v++)
                                        {
                                            if (anotherthing < 4)
                                            {
                                                anotherthing = anotherthing + 1;
                                                isittd = isittd + text[v];
                                            }
                                            else
                                            {
                                                anotherthing = 0;
                                                if (isittd == "<td>")
                                                {
                                                    isittd = "";
                                                    String thingtoaddtoarray = "";
                                                    while (text[v] != '<')
                                                    {
                                                        thingtoaddtoarray = thingtoaddtoarray + text[v];
                                                        v = v + 1;
                                                    }
                                                    decimal thingtoaddtoarrayintform = System.Convert.ToDecimal(thingtoaddtoarray);
                                                    Alllatitudesandslongitude.Add(thingtoaddtoarrayintform);
                                                    n = v + 4;
                                                    counttosopgettingdata = counttosopgettingdata + 1;
                                                    if (counttosopgettingdata >= 2)
                                                    {
                                                        i = n + 3;
                                                        n = text.Length;
                                                    }
                                                    v = text.Length + 4;
                                                }
                                                isittd = "";
                                                v = v - 4;
                                            }
                                        }
                                    }
                                    isittdslash = "";
                                    n = n - 5;
                                }
                            }
                        }
                        isittr = "";
                        i = i - 4;
                    }
                }
                int savedposition = 0;
                decimal positionreltaivetostarting = 0;
                decimal howcloseitistostartingplac = latitude + longitude;
                for (int i = 0; i < Alllatitudesandslongitude.Count; i = i + 2)
                {
                    decimal howcloseitis = Alllatitudesandslongitude[i] + Alllatitudesandslongitude[i + 1];
                    decimal comparetothis = Math.Abs(howcloseitis - howcloseitistostartingplac);
                    if (positionreltaivetostarting == 0)
                    {
                        positionreltaivetostarting = comparetothis;
                        savedposition = i;
                    }
                    else if (comparetothis < positionreltaivetostarting)
                    {
                        positionreltaivetostarting = comparetothis;
                        savedposition = i;
                    }
                }
                decimal[] returnthis = new decimal[2];
                returnthis[0] = Alllatitudesandslongitude[savedposition];
                returnthis[1] = Alllatitudesandslongitude[savedposition + 1];
                return returnthis;
            }
        }
        //
        public double Combine(double Relaxevalue, int Safetyvalue)
        {
            double returnthis = (Relaxevalue + Safetyvalue) / 2;
            return returnthis;
        }
    }
}