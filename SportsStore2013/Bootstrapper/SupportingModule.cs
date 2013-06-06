using System.Configuration;
using Ninject.Modules;
using SportsStore.Core;
using SportsStore.Core.Contracts;
using SportsStore.Core.Contracts.Models;
using SportsStore.Core.Contracts.Repositories;
using SportsStore.Infrastructure.Data;
using SportsStore.Infrastructure.Data.Caches;
using SportsStore.Infrastructure.Data.Interfaces;

namespace Bootstrapper
{
    public class SupportingModule :NinjectModule 
    {
        public override void Load()
        {
            ConfigureCore();

            ConfigureInfrastructure();
        }


        private void ConfigureCore()
        {
            Bind<IProductCore>().To<ProductCore>();
            Bind<ICategoryCore>().To<CategoryCore>();

            EmailSettings emailSettings = new EmailSettings
                                              {
                                                  WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
                                              };
            Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                                   .WithConstructorArgument("settings", emailSettings);
        }

        private void ConfigureInfrastructure()
        {
            // This should handle all the generic repositories
            Bind(typeof(IRepository<>)).To(typeof(EFRepository<>));

            Bind<IRepository<Product>>().To<EFRepository<Product>>()
                                        .WhenInjectedInto<CachedProductRepository>();

            Bind<IRepository<Product>>().To<CachedProductRepository>();
                

            Bind<ICategoryRepository>().To<CategoryRepository>();

            Bind<ICache>().To<ProductCache>().WithConstructorArgument("duration", 20);
        }
    }
}
