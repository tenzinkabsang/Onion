using System;
using System.Web.Caching;

namespace SportsStore.Infrastructure.Data.Caches
{
    public class ProductCache : HttpCache
    {
        public ProductCache(int duration)
            : base(duration)
        {
        }

        public override void Add<T>(string key, T value, int duration)
        {
            base.WebCache.Insert(key, value,
                new SqlCacheDependency("SportsStore2013", "Products"),
                Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(duration));
        }
    }
}