using System.Data.Entity;
using SportsStore.Core.Contracts.Models;

namespace SportsStore.Infrastructure.Data
{
    public class EFDbContext :DbContext 
    {
        public EFDbContext()
            : base(nameOrConnectionString: "SportsStore")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
