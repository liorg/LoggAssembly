using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guardian.Menta.Interfaces.Cache
{
    public interface ICacheItem
    {
        TResult GetCacheItem<TResult>(string key, string cacheName = "");

        TResult GetItem<TResult>(string key, Func<TResult> setCache, TimeSpan expiredTime, string cacheName = "");

        void SetItem<TResult>(string key, Func<TResult> setCache, TimeSpan expiredTime, string cacheName = "");
    }
}
