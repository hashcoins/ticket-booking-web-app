using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace _5DayForecast
{
    [ServiceContract]
    public interface IService1
    {
        // This operation returns a string of data about the user-given ZIP code
        [OperationContract]
        string[] Weather5day(string zipcode);

        [OperationContract]
        double AnnualSolar(decimal latitude, decimal longitude);
    }
}
