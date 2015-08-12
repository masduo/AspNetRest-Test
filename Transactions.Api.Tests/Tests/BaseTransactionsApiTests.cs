using System.Net;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using WebApiContrib.IoC.Ninject;
using Ninject;
using System.Web.Http;
using System.Text;
using Transactions.Data.Interfaces;
using NUnit.Framework;
using Transactions.Data.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System;

namespace Transactions.Api.Tests
{
    public class BaseTransactionsApiTests
    {
        private readonly string _baseAddress = "http://localhost:9000/";
        protected readonly string _transactionsResourceUri;
        protected readonly string _transactionResourceUri;
        protected IDisposable _server;
        protected HttpClient _client;

        protected readonly Transaction _dummyTransaction;
        protected readonly List<Transaction> _dummyEmptyListOfTransactions;
        protected readonly List<Transaction> _dummyListOfTransactionsWithADummyTransaction;

        public BaseTransactionsApiTests()
        {
            _transactionsResourceUri = _baseAddress + "api/transactions";
            _transactionResourceUri = _transactionsResourceUri + "/1";

            _dummyTransaction = new Transaction { Id = 1, Amount = 123, CurrencyCode = "JPY" };
            _dummyEmptyListOfTransactions = new List<Transaction>();
            _dummyListOfTransactionsWithADummyTransaction = new List<Transaction> { _dummyTransaction };
        }

        [SetUp]
        public void SetUp()
        {
            // Boostrap web api self hosting using owin
            _server = WebApp.Start<WebApiStartup>(url: _baseAddress);
            _client = new HttpClient { BaseAddress = new Uri(_baseAddress) };
        }

        [TearDown]
        public void TearDown()
        {
            _server.Dispose();
            _client.Dispose();
        }
    }
}