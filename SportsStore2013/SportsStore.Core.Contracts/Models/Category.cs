using System.Collections.Generic;

namespace SportsStore.Core.Contracts.Models
{
    public class Category : TEntity
    {
        public string Name { get; set; }
        public IList<Product> Products { get; set; }
    }
}