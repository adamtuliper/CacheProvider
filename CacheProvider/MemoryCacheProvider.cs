using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CacheProvider
{
    /// <summary>
    /// Since then 
    /// </summary>
    public class MemoryCacheProvider : ICacheProvider
    {
        private static ObjectCache Cache { get { return MemoryCache.Default; } }

        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data">The object to store in the cache</param>
        /// <param name="cacheTime">The time in seconds to cache</param>
        public void Add<T>(string key, T data, int cacheTime)
        {
            var itemPolicy = new CacheItemPolicy {AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime)};

            Cache.Add(new CacheItem(key, data), itemPolicy);
        }

        public bool Exists(string key)
        {
            return (Cache[key] != null);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }
    }
}
