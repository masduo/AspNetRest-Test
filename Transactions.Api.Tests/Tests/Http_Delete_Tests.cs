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
    public class When_http_delete_is_called : BaseTransactionsApiTests
    {
        [Test]
        public void When_no_transactions_found_should_return_http_staus_notfound()
        {
            //Arrange
            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.GetAll()).Return(_dummyEmptyListOfTransactions);

            var response = _client.DeleteAsync(_transactionResourceUri).Result;

            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.NotFound,
                String.Format("Actual {0}", response.StatusCode));
        }

        [Test]
        public void When_delete_fails_should_return_http_staus_internalservererror()
        {
            //Arrange
            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.GetAll()).Return(new List<Transaction> { _dummyTransaction });
            transactionRepoMock.Stub(_ => _.Delete(Arg<long>.Is.Anything)).Return(false);

            var response = _client.DeleteAsync(_transactionResourceUri).Result;

            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.InternalServerError,
                String.Format("Actual {0}", response.StatusCode));
        }

        [Test]
        public void When_delete_succeeds_should_return_http_staus_ok()
        {
            //Arrange
            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.GetAll()).Return(new List<Transaction> { _dummyTransaction });
            transactionRepoMock.Stub(_ => _.Delete(Arg<long>.Is.Anything)).Return(true);

            var response = _client.DeleteAsync(_transactionResourceUri).Result;

            Assert.IsTrue(
                response.IsSuccessStatusCode,
                String.Format("Actual {0}", response.StatusCode));
        }
    }
}
