using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace MyBlog.Common.Cache
{
    public class RuntimeMemoryCache : ICache
    {
        private readonly IMemoryCache cacheItem;

        public RuntimeMemoryCache(IMemoryCache memoryCache)
        {
            this.cacheItem = memoryCache; 

        }


        /// <summary>
        /// 创建一个缓存实例 具有属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public ICacheEntry CreateCacheEntity(object key,MemoryCacheEntryOptions options)
        {
           var entity = cacheItem.CreateEntry(key);
           entity.SetOptions(options); 
           return entity;
        }

        public void Add(string key, object obj)
        {
            cacheItem.Set(key, obj);
        }

        public void Add(string key, object obj, TimeSpan slidingExpiration)
        {
            cacheItem.Set(key, obj, slidingExpiration);
        }

        public void Add(string key, object obj, int seconds)
        {
            cacheItem.Set(key, obj, DateTimeOffset.Now.AddSeconds(seconds));
        }

        public bool Exists(string key)
        {
            var value = new object();
            return cacheItem.TryGetValue(key, out value);
        }

        public T Get<T>(string key)
        {
            var obj = default(T);
            if (cacheItem.TryGetValue<T>(key, out obj))
            {
                return obj;
            }
            else
            {
                return default(T);
            }
        }

        public void Max(string key, object obj)
        {
            var cacheItemPolicy = new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTime.MaxValue.AddYears(-1)).SetPriority(CacheItemPriority.NeverRemove);
            cacheItem.Set(key, obj, cacheItemPolicy);
        }

        public void Remove(string key)
        {
            cacheItem.Remove(key);
        }
    }
}
