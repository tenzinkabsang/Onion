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
        private readonly IRepository<Product> _products;
        private readonly IOrderProcessor _orderProcessor;

        public ProductCore(IRepository<Product> products, IOrderProcessor orderProcessor)
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

        public Product GetProductFor(int productId)
        {
            return _products.GetByIdIncluding(productId, p => p.Category);
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            _orderProcessor.ProcessOrder(cart, shippingDetails);
        }
    }
}