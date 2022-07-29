using System.Runtime.Serialization;


namespace MicronTMS.Helper
{
    [DataContract]
    public class ActionResult
    {

        [DataMember]
        public string TrxName { get; set; }

        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public string Result { get; set; }

        [DataMember]
        public string Data { get; set; }
    }
}
