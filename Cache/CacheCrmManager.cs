using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guardian.Menta.Interfaces.Cache;
using Microsoft.ApplicationServer.Caching;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;

namespace Guardian.Menta.Cache
{
    public class CacheCrmManager : ICacheItem
    {

        IConfigurationCache ConfigurationCache;
        DataCacheFactory DataCacheFactoryApp;

        #region C'TORS

        public CacheCrmManager(IConfigurationCache configurationCache)
        {
            ConfigurationCache = configurationCache;

            if (configurationCache.DataCacheServers == null || !configurationCache.DataCacheServers.Any())
                throw new ArgumentNullException("must set at least one hostname and port");
            var servers = new List<DataCacheServerEndpoint>();

            foreach (var hostItem in configurationCache.DataCacheServers)
            {
                DataCacheServerEndpoint dataCache = new DataCacheServerEndpoint(hostItem.Host, hostItem.Port);
                servers.Add(dataCache);
            }
            DataCacheFactoryConfiguration factoryConfig = new DataCacheFactoryConfiguration();
            factoryConfig.Servers = servers;
            // Create a configured DataCacheFactory object.
            DataCacheFactoryApp = new DataCacheFactory(factoryConfig);
            // TODO: change to CONFIGURATION
            factoryConfig.ChannelOpenTimeout = TimeSpan.FromMinutes(10);
            factoryConfig.RequestTimeout = TimeSpan.FromMinutes(10);
            //  var t=mycacheFactory.GetCache("t");
            // DataCacheDefault = mycacheFactory.GetDefaultCache();
        }


        #endregion

        #region Factory

        public static ICacheItem CacheManagerFactory(IConfigurationCache configurationCache)
        {
            return new CacheCrmManager(configurationCache);
        }
        #endregion

        /// <summary>
        /// Get only Cache Item
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public TResult GetCacheItem<TResult>(string key, string cacheName = "")
        {
            var jsonType = "";
            DataCache defaultCache = DataCacheFactoryApp.GetDefaultCache();
            DataCache dataCache = String.IsNullOrEmpty(cacheName) ? defaultCache : DataCacheFactoryApp.GetCache(cacheName);
            if (dataCache.Get(key) != null)
            {
                jsonType = dataCache.Get(key) as string;
                var result = JsonDeserializeToObject<TResult>(jsonType);
                return result;
            }
            return default(TResult);


        }

        /// <summary>
        /// Get data from Cahce if not exsist add one
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="setCache"></param>
        /// <param name="expiredTime"></param>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public TResult GetItem<TResult>(string key, Func<TResult> setCache, TimeSpan expiredTime, string cacheName = "")
        {
            var jsonType = "";
            DataCache defaultCache = DataCacheFactoryApp.GetDefaultCache();
            DataCache dataCache = String.IsNullOrEmpty(cacheName) ? defaultCache : DataCacheFactoryApp.GetCache(cacheName);
            if (dataCache.Get(key) == null)
            {
                TResult value = setCache();
                jsonType = JsonSerializeObject(value);
                dataCache.Add(key, jsonType, expiredTime);
            }
            jsonType = dataCache.Get(key) as string;
            var result = JsonDeserializeToObject<TResult>(jsonType);
            return result;
        }

        /// <summary>
        /// Set only To Cache item
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="setCache"></param>
        /// <param name="expiredTime"></param>
        /// <param name="cacheName"></param>
        public void SetItem<TResult>(string key, Func<TResult> setCache, TimeSpan expiredTime, string cacheName = "")
        {
            var jsonType = "";
            DataCache dataCache = String.IsNullOrEmpty(cacheName) ? DataCacheFactoryApp.GetDefaultCache() : DataCacheFactoryApp.GetCache(cacheName);
            if (dataCache.Get(key) != null)
            {
                TResult valueUpdate = setCache();
                jsonType = JsonSerializeObject(valueUpdate);
                dataCache.Put(key, jsonType, expiredTime);
            }
            else
            {
                TResult valueAdd = setCache();
                jsonType = JsonSerializeObject(valueAdd);
                dataCache.Add(key, jsonType, expiredTime);
            }
        }
      /*
        #region more options  remove
        TResult GetItemAppFabric<TResult>(string key, Func<TResult> setCache, TimeSpan expiredTime, string cacheName = "")
        {
            DataCache dataCache = String.IsNullOrEmpty(cacheName) ? DataCacheFactoryApp.GetDefaultCache() : DataCacheFactoryApp.GetCache(cacheName);
            if (dataCache.Get(key) == null)
            {
                TResult value = setCache();
                dataCache.Add(key, value, expiredTime);
            }

            return (TResult)dataCache.Get(key);
        }

        void SetItemAppFabric<TResult>(string key, Func<TResult> setCache, TimeSpan expiredTime, string cacheName = "")
        {
            DataCache dataCache = String.IsNullOrEmpty(cacheName) ? DataCacheFactoryApp.GetDefaultCache() : DataCacheFactoryApp.GetCache(cacheName);
            if (dataCache.Get(key) != null)
            {
                TResult value = setCache();
                dataCache.Put(key, value, expiredTime);
            }
            else
            {
                TResult valueAdd = setCache();
                dataCache.Add(key, valueAdd, expiredTime);
            }
        }

        TResult GetSerilizerItemBinary<TResult>(string key, Func<TResult> setCache, TimeSpan expiredTime, string cacheName = "")
        {
            Byte[] byteType = null;
            DataCache dataCache = String.IsNullOrEmpty(cacheName) ? DataCacheFactoryApp.GetDefaultCache() : DataCacheFactoryApp.GetCache(cacheName);
            if (dataCache.Get(key) == null)
            {
                TResult value = setCache();
                byteType = SerializeToBinary<TResult>(value);
                dataCache.Add(key, byteType, expiredTime);
            }
            byteType = dataCache.Get(key) as Byte[];
            var result = DeserializeFromBinary<TResult>(byteType);
            return result;
        }

        TResult GetSerilizerItemXml<TResult>(string key, Func<TResult> setCache, TimeSpan expiredTime, string cacheName = "")
        {
            var xmlType = "";
            DataCache dataCache = String.IsNullOrEmpty(cacheName) ? DataCacheFactoryApp.GetDefaultCache() : DataCacheFactoryApp.GetCache(cacheName);
            if (dataCache.Get(key) == null)
            {
                TResult value = setCache();
                xmlType = SerializeToXml(value);
                dataCache.Add(key, xmlType, expiredTime);
            }
            xmlType = dataCache.Get(key) as string;
            var result = DeserializeFromXml<TResult>(xmlType);
            return result;
        }

        #endregion
        */

        #region Serialize/Deserialize

        /// <summary>
        /// Another option Serialize it's prefer then binary option because diffrent type objects
        /// (in ilmerge problem the types is the some but assembly are not) can not be Deserialize 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        static string SerializeToXml<T>(T obj)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                var ser = new XmlSerializer(typeof(T));
                ser.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }

        static T DeserializeFromXml<T>(string xml)
        {
            T result;
            var ser = new XmlSerializer(typeof(T));
            using (var tr = new StringReader(xml))
            {
                result = (T)ser.Deserialize(tr);
            }
            return result;
        }
     
        /// <summary>
        ///Preffer option Serialize over XML option because is light way string option on memory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        static string JsonSerializeObject<T>(T obj)
        {
            //Create a stream to serialize the object to.
            using (var ms = new MemoryStream())
            {
                // Serializer the User object to the stream.
                var ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(ms, obj);
                byte[] json = ms.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }

        }
        
        // Deserialize a JSON stream to a User object.
        static T JsonDeserializeToObject<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                T deserialized = (T)ser.ReadObject(ms);
                return deserialized;
            }
        }

        /// <summary>
        /// Bad option because has problem with ilMerge that's has some types but diffrent assembly    
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        static Byte[] SerializeToBinary<T>(T obj)
        {
            Byte[] arr;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter BF = new BinaryFormatter();
                BF.Serialize(memoryStream, obj);
                arr = memoryStream.ToArray();
            }
            return arr;
        }

        static T DeserializeFromBinary<T>(Byte[] data)
        {
            T o;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                o = (T)binaryFormatter.Deserialize(memoryStream);
            }
            return o;
        }

        #endregion

       
       
    }
}
