//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Guardian.Menta.Interfaces.Cache
//{
//    public interface ICache
//    {
//        /// <summary>
//        /// Function Get item from cache        
//        /// </summary>
//        /// <param name="key">cache key</param>        
//        /// <returns>object from cache</returns>
//        object GetItem(string key);

//        /// <summary>
//        /// Function Get item from cache
//        /// If item not exist -> it will be created by dlgGetItem and then inserted to the cache
//        /// </summary>
//        /// <param name="key">cache key</param>
//        /// <param name="dlgGetItem">Delgete that will be called to create item, if its not exist</param>
//        /// <returns>object from cache</returns>
//        object GetItem(string key, Func<object> dlgGetItem);
        
//        /// <summary>
//        /// Insert item to cache
//        /// </summary>
//        /// <param name="key">cache key</param>
//        /// <param name="item">item to insert</param>        
//        bool Insert(string key, object item);

//        /// <summary>
//        /// Update item in the cache
//        /// </summary>
//        /// <param name="key">cache key</param>
//        /// <param name="item">item to update</param>        
//        bool Update(string key, object item);

//        /// <summary>
//        /// Remove item from cache
//        /// </summary>
//        /// <param name="key">cache key to remove</param>        
//        bool Remove(string key);
//    }
//}
