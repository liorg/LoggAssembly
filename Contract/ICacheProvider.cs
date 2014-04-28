using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guardian.Menta.Interfaces.Cache
{
    public interface ICacheProvider
    {
        /// <summary>
        /// Function Get item from cache        
        /// </summary>
        /// <param name="key">cache key</param>        
        /// <returns>object from cache</returns>
        object Get(string key);
        
        /// <summary>
        /// Insert item to cache
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="item">item to insert</param>        
        bool Insert(string key, object item);

        /// <summary>
        /// Update item in the cache
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="item">item to update</param>        
        bool Update(string key, object item);

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">cache key to remove</param>        
        bool Remove(string key);
    }
}
