using System.Collections.Generic;
using SportsStore.Core.Contracts.Models;

namespace SportsStore.Core.Contracts
{
    public interface IProductCore
    {
        IList<Product> GetAllProducts();

        IList<Product> GetProductsForCategoryName(string categoryName);

        Product GetProductFor(int productId);

        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
