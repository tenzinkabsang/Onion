using SportsStore.Core.Contracts.Models;

namespace SportsStore.Core.Contracts.Repositories
{
    public interface IProductRepository :IRepository<Product>
    {
        void ClearProductsCache();
    }
}