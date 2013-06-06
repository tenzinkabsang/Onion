using System;
using System.Diagnostics;
using System.Web.Caching;
using SportsStore.Core.Contracts.Models;
using SportsStore.Core.Contracts.Repositories;

namespace SportsStore.Infrastructure.Data.Interfaces
{
    public interface ICacheDependencyFactory
    {
        AggregateCacheDependency Create(Type type);
    }


    public class CacheDependencyFactory : ICacheDependencyFactory
    {
        public AggregateCacheDependency Create(Type type)
        {
            var dependency = new AggregateCacheDependency();
            if (typeof(IRepository<Product>).IsAssignableFrom(type))
            {
                Debug.Print("Creating SqlCacheDependency on Products Table.");
                var sqlDependency = new SqlCacheDependency("SportsStore2013", "Products");
                dependency.Add(sqlDependency);
            }

            return dependency;
        }
    }

}