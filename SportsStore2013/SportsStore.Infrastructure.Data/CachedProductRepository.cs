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
    public class CachedProductRepository : IProductRepository
    {
        private const string PRODUCTS_CACHE_KEY = "all_products";
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
            return _productsCache.Get<IList<Product>>(PRODUCTS_CACHE_KEY) ?? _productRepository.GetAll();
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

                        _productsCache.Add(PRODUCTS_CACHE_KEY, products);
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
            Product product = null;

            IList<Product> cachedProducts = GetProductsInCache();

            if(cachedProducts != null)
                product = cachedProducts.FirstOrDefault(p => p.Id == id);

            return product ?? _productRepository.GetByIdIncluding(id, includeProperties);
        }

        public void Add(Product product)
        {
            // add to database
            _productRepository.Add(product);
        }

        public void Update(Product product)
        {
            // update in database
            _productRepository.Update(product);
        }

        public void Delete(Product product)
        {
            // delete from database
            _productRepository.Delete(product);
        }

        public void Delete(int id)
        {
            _productRepository.Delete(id);
        }

        public void Commit()
        {
            _productRepository.Commit();
        }

        public void ClearProductsCache()
        {
            _productsCache.Clear(PRODUCTS_CACHE_KEY);
        }

        // helper
        private IList<Product> GetProductsInCache()
        {
            return _productsCache.Get<IList<Product>>(PRODUCTS_CACHE_KEY);
        }

        private void RemoveProductFromCache(int id)
        {
            // remove from cache
            IList<Product> cachedProducts = GetProductsInCache();
            if (cachedProducts == null)
                return;

            Product productToRemove = cachedProducts.FirstOrDefault(p => p.Id == id);
            if (productToRemove != null)
            {
                cachedProducts.Remove(productToRemove);
            } 
        }
    }
}
