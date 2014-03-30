using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using FluentNHibernate.Cfg.Db;
using log4net;
using NHibernate;
using NHibernate.Context;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Mvc;
using Hack.Common;
using NinjectDependencyResolver = Hack.Common.NinjectDependencyResolver;

namespace Kwick.Api.App_Start
{
    public class NinjectConfigurator 
    {

        public void Configure(IKernel container)
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
        private void AddBindings(IKernel container)
        {
            ConfigureNHibernate(container);
            ConfigureLog4net(container);

        }

        /// <summary>
        /// Set up log4net for this application, including putting it in the 
        /// given container.
        /// </summary>
        private void ConfigureLog4net(IKernel container)
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
        public void ConfigureNHibernate(IKernel container)
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
        public ISession CreateSession(IContext context)
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