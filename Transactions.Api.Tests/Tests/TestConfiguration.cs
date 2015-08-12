using Ninject;
using Owin;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Transactions.Api.Controllers;
using Transactions.Data.Entities;
using Transactions.Data.Interfaces;
using Transactions.Data.Repositories;
using WebApiContrib.IoC.Ninject;

namespace Transactions.Api.Tests
{
    public class WebApiStartup
    {
        /// <summary>
        /// Configures Web API.
        /// </summary>
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            WebApiConfig.Register(config, registerAreas: false);

            //this overwrites the webapi config dependency resolver to enable using mocks in tests
            config.DependencyResolver = new NinjectResolver(NinjectBootstrap.CreateKernel());

            appBuilder.UseWebApi(config);
        }
    }

    public static class NinjectBootstrap
    {
        //bad practice, in order to make the mock accessible via tests
        public static IKernel KernelInstance;

        public static IKernel CreateKernel()
        {
            //var kernel = new Ninject.MockingKernel.RhinoMock.RhinoMocksMockingKernel();
            var kernel = new StandardKernel();

            //Setup IoC bindings
            kernel.Bind<ITransactionRepository>().ToConstant(MockRepository.GenerateMock<ITransactionRepository>());
            kernel.Bind<TransactionsController>().ToSelf();

            NinjectBootstrap.KernelInstance = kernel;

            return kernel;
        }
    }
}
