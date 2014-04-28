using System;
namespace Guardian.Menta.Interfaces.Cache
{
    public interface IConfigurationCache{

        System.Collections.Generic.List<IDataCacheServer> DataCacheServers { get; set; }
    }
}
