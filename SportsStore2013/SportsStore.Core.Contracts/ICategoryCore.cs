using System.Collections.Generic;
using SportsStore.Core.Contracts.Models;

namespace SportsStore.Core.Contracts
{
    public interface ICategoryCore
    {
        IList<string> GetCategoryNames();

        IList<Category> GetAllCategories();
    }
}