using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Transactions.Data.Repositories;
using WebApiContrib.IoC.Ninject;
using Ninject;
using System.Web;
using System.Web.Mvc;
using Transactions.Api.Areas.HelpPage;
using Transactions.Data.Entities;
using Transactions.Data.Interfaces;

namespace Transactions.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Register(config, true);
        }

        public static void Register(HttpConfiguration config, bool registerAreas = true)
        {
            if (registerAreas)
            {
                //registers areas - help page mvc app
                AreaRegistration.RegisterAllAreas();

                //help page documentation
                config.SetDocumentationProvider(new XmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/App_Data/XmlDocument.xml")));
            }

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "TransactionsApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                    controller = "Transactions",
                    id = RouteParameter.Optional
                }
            );

            //replacing dependecy resolver to enable ioc
            config.DependencyResolver = new NinjectResolver(NinjectBootstrap.CreateKernel());
        }

        public static class NinjectBootstrap
        {
            public static IKernel CreateKernel()
            {
                var kernel = new StandardKernel();

                RegisterBindings(kernel);

                return kernel;
            }

            private static void RegisterBindings(IKernel kernel)
            {
                //Setup IoC bindings
                kernel.Bind<ITransactionRepository>().To<TransactionsRepository>();
            }
        }
    }
}
