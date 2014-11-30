using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MartaService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "{stationName}")]
        [OperationContract]
        TrainSchedule GetTrainSchedule(string stationName);

       // [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "FivePoints")]
        //[OperationContract]
        //FivePointsSchedule GetFivePointsSchedule(string );

        // TODO: Add your service operations here
    }


    [DataContract]
    public class TrainSchedule
    {
        [DataMember]
        public string northBound { get; set; }

        [DataMember]
        public string southBound { get; set; }
    }
    /*

    public class FivePointsSchedule
    {
        [DataMember]
        public string northBound { get; set; }

        [DataMember]
        public string southBound { get; set; }
    }*/
}
