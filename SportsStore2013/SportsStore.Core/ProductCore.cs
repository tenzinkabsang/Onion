using System;
using System.Collections.Generic;
using System.Linq;
using SportsStore.Core.Contracts;
using SportsStore.Core.Contracts.Models;
using SportsStore.Core.Contracts.Repositories;

namespace SportsStore.Core
{
    public class ProductCore : IProductCore
    {
        private readonly IProductRepository _products;
        private readonly IOrderProcessor _orderProcessor;

        public ProductCore(IProductRepository products, IOrderProcessor orderProcessor)
        {
            _products = products;
            _orderProcessor = orderProcessor;
        }

        public IList<Product> GetAllProducts()
        {
            var products = _products.GetAll();
            return products;
        }

        public IList<Product> GetProductsForCategoryName(string categoryName)
        { 
            return _products.GetAllIncluding(p => p.Category)
                            .Where(p => categoryName == null || string.Equals(p.Category.Name, categoryName, StringComparison.OrdinalIgnoreCase))
                            .ToList();
        }

        public Product GetProductById(int productId)
        {
            return _products.GetByIdIncluding(productId, p => p.Category);
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            _orderProcessor.ProcessOrder(cart, shippingDetails);
        }

        //public void Save(Product product)
        //{
        //    if (product.Id == 0)
        //        _products.Add(product);
        //    else
        //        _products.Update(product);

        //    // commit these changes
        //    _products.Commit();
        //}

        public IProductCore Save(Product product)
        {
            if (product.Id == 0)
                _products.Add(product);
            else
                _products.Update(product);

            return this;
        }

        public IProductCore Delete(int id)
        {
            _products.Delete(id);

            return this;
        }

        public void Commit()
        {
            _products.Commit();
        }

        public void RefreshProductCache()
        {
            _products.ClearProductsCache();
        }
    }
}