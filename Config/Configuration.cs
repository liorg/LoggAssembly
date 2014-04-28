using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Guardian.Menta.Common.Configuration.JSON
{
    [DataContract]
    public class Configuration
    {
       
        [DataMember]
        public ConfigCaching ConfigCaching { get; set; }
       

    }
}

