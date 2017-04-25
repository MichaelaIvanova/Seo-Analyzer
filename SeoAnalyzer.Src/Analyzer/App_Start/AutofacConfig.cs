using Analyzer.BusinessLogic.Services.Contracts;
using Analyzer.Helpers;
using Analyzer.Helpers.Contracts;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.Web.Mvc;

namespace Analyzer.App_Start
{
    public  static class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var servicesAssembly = Assembly.GetAssembly(typeof(IWordCountService));
            builder.RegisterAssemblyTypes(servicesAssembly).AsImplementedInterfaces();

            builder.Register(x => new JsonResponseFormatHelper())
               .As<IJsonResponseFormatHelper>().SingleInstance();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}