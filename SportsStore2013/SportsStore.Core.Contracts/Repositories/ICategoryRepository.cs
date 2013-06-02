using System.Collections.Generic;
using SportsStore.Core.Contracts.Models;

namespace SportsStore.Core.Contracts.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IList<string> GetCategoryNames();
    }
}