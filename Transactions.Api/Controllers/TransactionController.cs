using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Transactions.Data.Repositories;
using Transactions.Data.Entities;
using System.Web.Hosting;
using Transactions.Api.Models;
using Transactions.Data.Interfaces;

namespace Transactions.Api.Controllers
{
    public class TransactionsController : ApiController
    {
        private ITransactionRepository _repo;
        private TransactionModeller _transactionModeller;

        public TransactionsController(ITransactionRepository transactionRepository)
        {
            _repo = transactionRepository;
        }

        private TransactionModeller TransactionModeller
        {
            get
            {
                if (_transactionModeller == null)
                {
                    _transactionModeller = new TransactionModeller(this.Request);
                }
                return _transactionModeller;
            }
        }

        /// <summary>
        /// Gets transactions in the system.
        /// </summary>
        /// <returns> List of top 50 transactions.</returns>
        public HttpResponseMessage GetAll()
        {
            var transactions = _repo.GetAll()
                .Take(50)
                .ToList();

            return Request.CreateResponse(HttpStatusCode.OK,
                transactions.Select(t => TransactionModeller.Create(t))
                    .ToList());
        }

        /// <summary>
        /// Gets a single transaction by Id and retursn it with status OK.
        /// </summary>
        /// <param name="id"> Identifier of the transaction.</param>
        /// <returns>
        /// OK response message wrapping the queried transaction.
        /// NotFound response if input id does not exist in persistance store.
        /// </returns>
        public HttpResponseMessage Get(long id)
        {
            var transaction = _repo.GetAll().Where(t => t.Id == id).FirstOrDefault();

            if (transaction == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, TransactionModeller.Create(transaction));
        }

        /// <summary>
        /// Creates a new transaction based on the input model.
        /// </summary>
        /// <param name="model"> Input model from the post request's body. Can be formatted as Json or Xml.</param>
        /// <returns>
        /// Created response message wrapping the newly created transaction.
        /// BadRequest response if provided model is dodgy.
        /// InternalServerError response if persistance fails.
        /// </returns>
        public HttpResponseMessage Post([FromBody]TransactionModel model)
        {
            if (TransactionModeller.IsNullOrEmpty(model))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Model is empty or could not be parsed.");

            var transaction = TransactionModeller.Parse(model);

            return transaction != null && _repo.SaveOrUpdate(transaction)
                ? Request.CreateResponse(HttpStatusCode.Created, TransactionModeller.CreateNew(transaction))
                : Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An issue occured during saving new transaction.");
        }

        /// <summary>
        /// Updates a transaction.
        /// </summary>
        /// <param name="id">Id of the transaction to be updated</param>
        /// <param name="model">Input model from the put request's body. Can be formatted as Json or Xml.</param>
        /// <returns>
        /// Ok if persistance succeeds.
        /// BadRequest if provided model is dodgy.
        /// NotFound if no transaction is found based on the input Id.
        /// NotModified if provided model is the same as what is in persistance store.
        /// InternalServerError if persisatnce fails.
        /// </returns>
        public HttpResponseMessage Put(long id, [FromBody]TransactionModel model)
        {
            if (TransactionModeller.IsNullOrEmpty(model))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Model is empty or could not be parsed.");

            var transactionToUpdate = _repo.GetAll().Where(t => t.Id == id).FirstOrDefault();
            if (transactionToUpdate == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "No matching record found to update.");

            var modified = TransactionModeller.Update(transactionToUpdate, model);

            if (modified == false)
                return Request.CreateResponse(HttpStatusCode.NotModified, "Nothing to change.");

            return _repo.SaveOrUpdate(transactionToUpdate)
                ? Request.CreateResponse(HttpStatusCode.OK, TransactionModeller.Create(transactionToUpdate))
                : Request.CreateResponse(HttpStatusCode.InternalServerError, "An issue occured during updating transaction.");
        }

        /// <summary>
        /// Deletes a transaction.
        /// </summary>
        /// <param name="id">Id of the transaction to be deleted.</param>
        /// <returns>
        /// Ok if persistance succeeds.
        /// NotFound if no transaction is found based on the input Id.
        /// InternalServerError if persisatnce fails.
        /// </returns>
        public HttpResponseMessage Delete(long id)
        {
            //TODO:with persistence store, get all is unnecessary
            var transactionToDelete = _repo.GetAll().Where(t => t.Id == id).FirstOrDefault();
            if (transactionToDelete == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "No matching record found to delete.");

            return _repo.Delete(id)
                ? Request.CreateResponse(HttpStatusCode.OK)
                : Request.CreateResponse(HttpStatusCode.InternalServerError, "An issue occured during deleting transaction.");
        }
    }
}
