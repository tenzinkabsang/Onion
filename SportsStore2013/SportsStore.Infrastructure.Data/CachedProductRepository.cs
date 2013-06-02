using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SportsStore.Core.Contracts.Models;

namespace SportsStore.Infrastructure.Data
{
    public class CachedProductRepository :EFRepository<Product>
    {
        public CachedProductRepository(EFDbContext context)
            : base(context)
        {
        }

        public override IList<Product> GetAll()
        {
            if(true)
            // look in cache first
            return base.GetAll();
        }

        public override IList<Product> GetAllIncluding(params Expression<Func<Product, object>>[] includeProperties)
        {
            // look in cache
            return base.GetAllIncluding(includeProperties);
        }
    }
}
