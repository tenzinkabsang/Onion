using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Bootstrapper;
using Ninject;
using Ninject.Modules;

namespace SportsStore.Web.UI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _ninjectKernel;

        public NinjectControllerFactory()
        {
            INinjectModule[] allConfiguredModules = GetAllModules();
            _ninjectKernel = new StandardKernel(allConfiguredModules);
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)_ninjectKernel.Get(controllerType);
        }

        private static INinjectModule[] GetAllModules()
        {
            //var ninjectModules = AppDomain.CurrentDomain.GetAssemblies()
            //                              .SelectMany(a => a.GetTypes())
            //                              .Where(t => t.BaseType == typeof(NinjectModule) && !t.IsAbstract)
            //                              .Select(module => (NinjectModule)Activator.CreateInstance(module))
            //                              .ToArray();

            return new INinjectModule[] { new WebModule(), new SupportingModule() };
        }
    }
}