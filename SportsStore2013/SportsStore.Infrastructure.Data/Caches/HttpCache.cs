using System;
using System.Diagnostics.Contracts;
using System.Web;
using System.Web.Caching;
using SportsStore.Infrastructure.Data.Interfaces;

namespace SportsStore.Infrastructure.Data.Caches
{
    public class HttpCache : ICache
    {
        private readonly int _defaultDuration;
        protected readonly Cache WebCache;

        public HttpCache(int duration)
        {
            Contract.Requires<NullReferenceException>(HttpRuntime.Cache != null, "HttpRuntime does not exists");

            _defaultDuration = duration;
            //WebCache = HttpContext.Current.Cache;
            WebCache = HttpRuntime.Cache;
        }

        public T Get<T>(string key) where T : class
        {
            return WebCache[key] as T;
        }

        public void Add<T>(string key, T value)
        {
            Add(key, value, _defaultDuration);
        }

        // This is just the default implementation.
        // Override this for different caching needs
        public virtual void Add<T>(string key, T value, int duration)
        {
            WebCache.Insert(key, value, null,
                DateTime.UtcNow.AddMinutes(duration), Cache.NoSlidingExpiration);
        }

        public void Clear(string key)
        {
            WebCache.Remove(key);
        }
    }
}