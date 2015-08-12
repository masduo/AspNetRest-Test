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
    public class When_http_put_is_called : BaseTransactionsApiTests
    {
        [Test]
        public void When_bad_data_is_provided_should_return_http_staus_badrequest()
        {
            //Act
            var response = _client.PutAsJsonAsync(_transactionResourceUri,
                new StringContent("{some bad request}", Encoding.UTF8, "application/json")).Result;

            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.BadRequest,
                String.Format("Actual {0}", response.StatusCode));
        }

        [Test]
        public void When_no_matchig_record_found_should_return_http_staus_notfound()
        {
            //Arrange
            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.GetAll()).Return(_dummyEmptyListOfTransactions);

            var response = _client.PutAsJsonAsync(_transactionResourceUri, _dummyTransaction).Result;

            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.NotFound,
                String.Format("Actual {0}", response.StatusCode));
        }

        [Test]
        public void When_transaction_is_not_modified_should_return_http_staus_notmodified()
        {
            //Arrange
            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.GetAll()).Return(_dummyListOfTransactionsWithADummyTransaction);

            var response = _client.PutAsJsonAsync(_transactionResourceUri, _dummyTransaction).Result;

            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.NotModified,
                String.Format("Actual {0}", response.StatusCode));
        }

        [Test]
        public void When_save_succeeds_should_return_http_staus_ok()
        {
            //Arrange
            var transactionToUpdate = new Transaction { Id = 1, Amount = 123, CurrencyCode = "JPY" };
            var updatedTransaction = new Transaction { Id = 1, Amount = 321, CurrencyCode = "GBP" };

            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.GetAll()).Return(new List<Transaction>() { transactionToUpdate });
            transactionRepoMock.Stub(_ => _.SaveOrUpdate(Arg<Transaction>.Is.Anything)).Return(true);

            var response = _client.PutAsJsonAsync(_transactionResourceUri, updatedTransaction).Result;

            Assert.IsTrue(
                response.IsSuccessStatusCode,
                String.Format("Actual {0}", response.StatusCode));
        }

        [Test]
        public void When_save_fails_should_return_http_staus_internalservererror()
        {
            //Arrange
            var transactionToUpdate = new Transaction { Id = 1, Amount = 123, CurrencyCode = "JPY" };
            var updatedTransaction = new Transaction { Id = 1, Amount = 321, CurrencyCode = "GBP" };

            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.GetAll()).Return(new List<Transaction>() { transactionToUpdate });
            transactionRepoMock.Stub(_ => _.SaveOrUpdate(Arg<Transaction>.Is.Anything)).Return(false);

            var response = _client.PutAsJsonAsync(_transactionResourceUri, updatedTransaction).Result;

            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.InternalServerError,
                String.Format("Actual {0}", response.StatusCode));
        }
    }
}