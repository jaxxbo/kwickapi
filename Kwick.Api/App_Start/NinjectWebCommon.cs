using System.Web.Http;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Instances;
using Hack.Common;
using Hack.Common.Fetchers;
using Hack.Common.Framework;
using Hack.Common.Helpers;
using Kwick.Data;
using log4net;
using NHibernate;
using NHibernate.Context;
using Ninject.Activation;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Kwick.Api.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Kwick.Api.App_Start.NinjectWebCommon), "Stop")]

namespace Kwick.Api.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var containerConfigurator = new NinjectConfigurator();
            Configure(kernel);
        }

        public static void Configure(IKernel container)
        {
            //Add all bindings/dependencies
            AddBindings(container);

            //Use the container and our NinjectDependencyResolver as
            //application's resolver
            var resolver = new NinjectDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

        }


        /// <summary>
        /// Add all bindings/dependencies to the container
        /// </summary>
        private static void AddBindings(IKernel container)
        {
            ConfigureNHibernate(container);
            ConfigureLog4net(container);

            container.Bind<ICreateKUser>().To<CreateKUser>();
            container.Bind<IBodyParser>().To<BodyParser>();
            container.Bind<IActionExceptionHandler>().To<ActionExceptionHandler>();
            container.Bind<IExceptionManager>().To<ExceptionManager>();
            container.Bind<IActionLogHelper>().To<ActionLogHelper>();
            container.Bind<IActionTransactionHelper>().To<ActionTransactionHelper>();
            container.Bind<IExceptionMessageFormatter>().To<ExceptionMessageFormatter>();
            container.Bind<IKUserFetcher>().To<KUserFetcher>();
            container.Bind<IKService>().To<KwuickService>();

        }

        /// <summary>
        /// Set up log4net for this application, including putting it in the 
        /// given container.
        /// </summary>
        private static void ConfigureLog4net(IKernel container)
        {
            log4net.Config.XmlConfigurator.Configure();
            var loggerForWebSite = LogManager.GetLogger("hackweb");
            container.Bind<ILog>().ToConstant(loggerForWebSite);
        }

        /// <summary>
        /// Used to fetch the current thread's principal as 
        /// an <see cref="IUserSession"/> object.
        /// </summary>
        //public IUserSession CreateUserSession(IContext arg)
        //{
        //    return new UserSession(Thread.CurrentPrincipal as GenericPrincipal);
        //}


        /// <summary>
        /// Used to fetch the current thread's principal as 
        /// an <see cref="IUserSession"/> object.
        /// </summary>
        //public IUserSession CreateUserSessionAccessToken(IContext arg)
        //{
        //    //var authenticatedUser = HttpContext.Current.User.Identity.Name;

        //    return new UserSession(HttpContext.Current.User as GenericPrincipal);
        //}

        /// <summary>
        /// Sets up NHibernate, and adds an ISessionFactory to the given
        /// container.
        /// </summary>
        public static void ConfigureNHibernate(IKernel container)
        {
            var sessionFactory = FluentNHibernate
                .Cfg.Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(
                        c => c.FromConnectionStringWithKey("hackdb")))
                .CurrentSessionContext("web")
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Kwick.MySql.KUserMap>())
                .BuildSessionFactory();

            // Add the ISessionFactory instance to the container
            container.Bind<ISessionFactory>().ToConstant(sessionFactory);

            // Configure a resolver method to be used for creating ISession objects
            container.Bind<ISession>().ToMethod(CreateSession);

            container.Bind<ICurrentSessionContextAdapter>().To<CurrentSessionContextAdapter>();
        }

        /// <summary>
        /// Method used to create instances of ISession objects
        /// and bind them to the HTTP context.
        /// </summary>
        public static ISession CreateSession(IContext context)
        {
            var sessionFactory = context.Kernel.Get<ISessionFactory>();
            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                // Open new ISession and bind it to the current session context
                var session = sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }

            return sessionFactory.GetCurrentSession();
        }

    }
}
