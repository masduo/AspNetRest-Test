using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Web.Http.Routing;
using Transactions.Api.Models;
using Transactions.Data.Entities;

namespace Transactions.Api.Controllers
{
    internal class TransactionModeller
    {
        private UrlHelper _urlHelper;

        internal TransactionModeller(HttpRequestMessage httpRequestMessage)
        {
            _urlHelper = new UrlHelper(httpRequestMessage);
        }

        internal TransactionModel Create(Transaction transaction)
        {
            return new TransactionModel
            {
                Url = _urlHelper.Link("TransactionsApi", new { Id = transaction.Id }),

                TransactionId = transaction.Id,
                TransactionAmount = transaction.Amount,
                CurrencyCode = transaction.CurrencyCode,
                Description = transaction.Description,
                Merchant = transaction.Merchant,
                TransactionDate = transaction.TransactedOn,
                CreatedDate = transaction.CreatedOn,
                ModifiedDate = transaction.ModifiedOn
            };
        }

        internal TransactionModel CreateNew(Transaction transaction)
        {
            return new TransactionModel
            {
                Url = _urlHelper.Link("TransactionsApi", new { Id = transaction.Id }),

                TransactionId = transaction.Id,
                TransactionAmount = transaction.Amount,
                CurrencyCode = transaction.CurrencyCode,
                Description = transaction.Description,
                Merchant = transaction.Merchant,
                TransactionDate = transaction.TransactedOn,
                CreatedDate = transaction.CreatedOn,
                ModifiedDate = transaction.ModifiedOn
            };
        }

        internal Transaction Parse(TransactionModel model)
        {
            return new Transaction
            {
                Id = 0, //default for new tran, ignore model.Id
                Amount = model.TransactionAmount,
                CurrencyCode = model.CurrencyCode,
                Description = model.Description,
                TransactedOn = model.TransactionDate,
                Merchant = model.Merchant
            };
        }

        internal bool Update(Transaction transactionToUpdate, TransactionModel model)
        {
            var modified = false;
            //NOTE: Id, CreatedOn, and ModifiedOn modifications are owned by system only
            if (model.TransactionAmount != 0 && 
                model.TransactionAmount != transactionToUpdate.Amount)
            {
                transactionToUpdate.Amount = model.TransactionAmount;
                modified = true;
            }
            if (string.IsNullOrWhiteSpace(model.CurrencyCode) == false &&
                model.CurrencyCode != transactionToUpdate.CurrencyCode)
            {
                transactionToUpdate.CurrencyCode = model.CurrencyCode;
                modified = true;
            }
            if (model.TransactionDate != DateTime.MinValue &&
                model.TransactionDate != transactionToUpdate.TransactedOn)
            {
                transactionToUpdate.TransactedOn = model.TransactionDate;
                modified = true;
            }
            if (string.IsNullOrWhiteSpace(model.Description) == false &&
                model.Description != transactionToUpdate.Description)
            {
                transactionToUpdate.Description = model.Description;
                modified = true;
            }
            if (string.IsNullOrWhiteSpace(model.Merchant) == false &&
                model.Merchant != transactionToUpdate.Merchant)
            {
                transactionToUpdate.Merchant = model.Merchant;
                modified = true;
            }

            return modified;
        }

        internal bool IsNullOrEmpty(TransactionModel model)
        {
            return model == null || (model.TransactionAmount == 0 && string.IsNullOrWhiteSpace(model.CurrencyCode) && model.TransactionDate == DateTime.MinValue);
        }
    }
}
