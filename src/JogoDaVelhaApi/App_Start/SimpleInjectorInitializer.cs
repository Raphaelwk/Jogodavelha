[assembly: WebActivator.PostApplicationStartMethod(typeof(JogoDaVelhaApi.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace JogoDaVelhaApi.App_Start
{
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Mvc;
    using JogoDaVelha.CrossCutting.IoC;
    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using SimpleInjector.Integration.WebApi;

    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            //container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            InitializeContainer(container);

            //container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);


            container.Verify();

            //DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
     
        private static void InitializeContainer(Container container)
        {
            BootStrapper.Register(container);
        }
    }
}