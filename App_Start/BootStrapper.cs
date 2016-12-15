using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using DataCachingApp.Repository.Interfaces;
using DataCachingApp.Repository.Implementation;

namespace DataCachingApp.App_Start
{
    public class BootStrapper
    {
        public static void Initialise()
        {
            BuildUnityContainer();
            // BuildAutoFacContainer();            
        }

        private static void BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IProductRepository, ProductRepository>
                (new ContainerControlledLifetimeManager()); // only one instance

            container.RegisterType<IProductCacheRepository, ProductCacheRepository>
                (new ContainerControlledLifetimeManager()); // only one instance

           // container.RegisterType<ICacheRepository, WebCacheRepository>(new ContainerControlledLifetimeManager());
             
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static void BuildAutoFacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ProductRepository>().As<IProductRepository>().SingleInstance(); // only one instance
            builder.RegisterType<ProductCacheRepository>().As<IProductCacheRepository>().SingleInstance(); // only one instance
         //   builder.RegisterType<WebCacheRepository>().As<ICacheRepository>().InstancePerLifetimeScope();
          
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}