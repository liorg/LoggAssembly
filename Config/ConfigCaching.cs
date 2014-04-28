using Guardian.Menta.Interfaces.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
 namespace Guardian.Menta.Common.Configuration.JSON
{
    [KnownType(typeof(DataCacheServer))]
    public class ConfigCaching : IConfigurationCache
    {
        [DataMember]
        public List<IDataCacheServer> DataCacheServers
        {
            get;
            set;
        }
    }
    public class DataCacheServer : IDataCacheServer
    {

        public string Host
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }
    }


}
