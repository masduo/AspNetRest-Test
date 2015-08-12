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
    public class When_http_post_is_called : BaseTransactionsApiTests
    {
        [Test]
        public void When_good_data_is_submitted_should_return_http_staus_created()
        {
            //Arrange
            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.SaveOrUpdate(Arg<Transaction>.Is.Anything)).Return(true);

            //Act
            var response = _client.PostAsJsonAsync(_transactionsResourceUri, _dummyTransaction).Result;

            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.Created,
                String.Format("Actual {0}", response.StatusCode));
        }

        [Test]
        public void When_bad_data_is_submitted_should_return_http_staus_badrequest()
        {
            //Act
            var response = _client.PostAsJsonAsync(_transactionsResourceUri,
                new StringContent("{representing : some bad request}", Encoding.UTF8, "application/json")).Result;

            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.BadRequest,
                String.Format("Actual {0}", response.StatusCode));
        }

        [Test]
        public void When_save_fails_should_return_http_staus_internalservererror()
        {
            //Arrange
            var transactionRepoMock = NinjectBootstrap.KernelInstance.Get<ITransactionRepository>();
            transactionRepoMock.Stub(_ => _.SaveOrUpdate(Arg<Transaction>.Is.Anything)).Return(false);

            //Act
            var response = _client.PostAsJsonAsync(_transactionsResourceUri, _dummyTransaction).Result;

            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.InternalServerError,
                String.Format("Actual {0}", response.StatusCode));
        }
    }
}