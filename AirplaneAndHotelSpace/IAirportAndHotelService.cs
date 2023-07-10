using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AirplaneAndHotelSpace
{
    [ServiceContract]
    public interface IAirportAndHotelService
    {
        [OperationContract]
        string[][] GetFlightBookings(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude, string departDate, string returnDate, string adults, string children);

        [OperationContract]
        string[][] GetHotelBookings(string latitude, string longitude, string checkInDate, string checkOutDate, string rooms, string adults, string children);
    }
}
