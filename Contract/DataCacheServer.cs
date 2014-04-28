using System;
namespace Guardian.Menta.Interfaces.Cache
{
    public interface IDataCacheServer
    {
        string Host { get; set; }
        int Port { get; set; }
    }
}
