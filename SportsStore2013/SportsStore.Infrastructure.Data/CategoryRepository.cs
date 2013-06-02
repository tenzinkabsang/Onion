using System.Collections.Generic;
using System.Linq;
using SportsStore.Core.Contracts.Models;
using SportsStore.Core.Contracts.Repositories;

namespace SportsStore.Infrastructure.Data
{
    public class CategoryRepository : EFRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(EFDbContext context)
            : base(context)
        {
        }

        public IList<string> GetCategoryNames()
        {
            IList<string> names = DbSet.Select(c => c.Name)
                                       .Distinct()
                                       .OrderBy(c => c)
                                       .ToList();

            return names;
        }
    }
}