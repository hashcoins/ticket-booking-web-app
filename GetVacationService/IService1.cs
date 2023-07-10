using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Wcf_getvactionspotservice_
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        decimal[] Getvactionspot(decimal longitude, decimal langitude);

        [OperationContract]
        double Combine(double Relaxvalue, int Safetyvalue);
        // TODO: Add your service operations here
    }
}
