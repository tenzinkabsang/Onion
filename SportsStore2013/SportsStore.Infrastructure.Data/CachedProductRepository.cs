using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Linq;
using System.Web;
using SportsStore.Core.Contracts.Models;
using SportsStore.Core.Contracts.Repositories;
using SportsStore.Infrastructure.Data.Interfaces;

namespace SportsStore.Infrastructure.Data
{
    public class CachedProductRepository : IRepository<Product>
    {
        private const string CACHE_KEY = "all_products";
        private readonly IRepository<Product> _productRepository;
        private readonly ICache _productsCache;
        private static readonly object _lockObject = new object();

        public CachedProductRepository(IRepository<Product> productRepository, ICache productsCache)
        {
            _productRepository = productRepository;
            _productsCache = productsCache;
        }

        public IList<Product> GetAll()
        {
            // If not in cache, simply get it from the database
            // We don't want to put this cache. That will be taken care of by GetAllIncluding method
            // which will also include all the related data as well.
            return _productsCache.Get<IList<Product>>(CACHE_KEY) ?? _productRepository.GetAll();
        }

        public IList<Product> GetAllIncluding(params Expression<Func<Product, object>>[] includeProperties)
        {
            var products = GetProductsInCache();

            if (products == null)
            {
                lock (_lockObject)
                {
                    products = GetProductsInCache();
                    if (products == null)
                    {
                        products = _productRepository.GetAllIncluding(x =>x.Category); // add all dependencies here!

                        _productsCache.Add(CACHE_KEY, products);
                    }
                }
            }
            return products;
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetByIdIncluding(int id, params Expression<Func<Product, object>>[] includeProperties)
        {
            Product product = GetProductsInCache().FirstOrDefault(p => p.Id == id);

            return product ?? _productRepository.GetByIdIncluding(id, includeProperties);
        }

        public void Add(Product entity)
        {
            _productRepository.Add(entity);
        }

        public void Update(Product entity)
        {
            _productRepository.Update(entity);
        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public void Delete(int id)
        {
            _productRepository.Delete(id);
        }


        // helper
        private IList<Product> GetProductsInCache()
        {
            return _productsCache.Get<IList<Product>>(CACHE_KEY);
        }
    }
}
