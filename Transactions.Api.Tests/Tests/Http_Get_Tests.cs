using System;
using System.Collections.Generic;
using System.Net.Http;
using NUnit.Framework;
using Rhino.Mocks;
using Transactions.Api.Controllers;
using Transactions.Data.Entities;
using Transactions.Data.Repositories;
using System.Net;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using WebApiContrib.IoC.Ninject;
using Ninject;
using System.Web.Http;
using System.Text;
using Transactions.Data.Interfaces;

namespace Transactions.Api.Tests
{
    [TestFixture]
    public class When_http_get_is_called : BaseTransactionsApiTests
    {
        [Test]
        public void When_no_resource_parameter_is_provided_should_always_return_http_staus_ok()
        {
            //Arrange
            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.GetAll()).Return(_dummyListOfTransactionsWithADummyTransaction);

            //Act
            var response = _client.GetAsync(_transactionsResourceUri).Result;

            Assert.IsTrue(
                response.IsSuccessStatusCode,
                String.Format("Actual {0}", response.StatusCode));
        }

        [Test]
        public void When_a_resource_parameter_is_provided_and_a_transaction_exists_should_return_http_staus_ok()
        {
            //Arrange
            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.GetAll()).Return(_dummyListOfTransactionsWithADummyTransaction);

            //Act
            var response = _client.GetAsync(_transactionResourceUri).Result;

            Assert.IsTrue(
                response.IsSuccessStatusCode,
                String.Format("Actual {0}", response.StatusCode));
        }

        [Test]
        public void When_a_resource_parameter_is_provided_and_no_transaction_exists_get_should_return_http_staus_notfound()
        {
            //Arrange
            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.GetAll()).Return(_dummyEmptyListOfTransactions);

            //Act
            var response = _client.GetAsync(_transactionResourceUri).Result;

            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.NotFound,
                String.Format("Actual {0}", response.StatusCode));
        }
    }
}