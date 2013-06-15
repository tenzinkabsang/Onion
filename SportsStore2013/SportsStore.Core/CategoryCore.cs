using System.Collections.Generic;
using SportsStore.Core.Contracts;
using SportsStore.Core.Contracts.Models;
using SportsStore.Core.Contracts.Repositories;

namespace SportsStore.Core
{
    public class CategoryCore : ICategoryCore
    {
        private readonly ICategoryRepository _categories;

        public CategoryCore(ICategoryRepository categories)
        {
            _categories = categories;
        }

        public IList<string> GetCategoryNames()
        {
            IList<string> names = _categories.GetCategoryNames();
            return names;
        }

        public IList<Category> GetAllCategories()
        {
            return _categories.GetAll();
        }
    }
}