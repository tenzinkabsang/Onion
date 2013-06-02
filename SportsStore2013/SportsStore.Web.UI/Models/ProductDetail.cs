using System;
using System.Diagnostics.Contracts;
using SportsStore.Core.Contracts.Models;

namespace SportsStore.Web.UI.Models
{
    public class ProductDetail
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public ProductDetail(Product product)
        {
            Contract.Requires<ArgumentNullException>(product != null, "Product cannot be null");
            
            Product = product;
            Quantity = 1;   // Starts off with 1
        }
    }
}