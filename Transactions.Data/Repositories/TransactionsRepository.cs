using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Transactions.Data.Entities;
using Transactions.Data.Interfaces;

namespace Transactions.Data.Repositories
{
    public class TransactionsRepository : ITransactionRepository
    {
        private List<Transaction> _transactions;

        public IEnumerable<Transaction> Transactions
        {
            get
            {
                if (_transactions == null)
                {
                    _transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            Id = 1,
                            Amount = 100,
                            CurrencyCode = "GBP",
                            TransactedOn = DateTime.Now,
                            Description = "Some transaction 1",
                            Merchant = null,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.MinValue
                        },
                        new Transaction
                        {
                            Id = 2,
                            Amount = 150,
                            CurrencyCode = "USD",
                            TransactedOn = DateTime.Now,
                            Description = "Some transaction 2",
                            Merchant = "Some merchant",
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.MinValue
                        },
                        new Transaction
                        {
                            Id = 3,
                            Amount = 210,
                            CurrencyCode = "USD",
                            TransactedOn = DateTime.Now,
                            Description = "Some transaction 3",
                            Merchant = null,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.MinValue
                        }
                    };
                }
                return _transactions;
            }
        }

        public IEnumerable<Transaction> GetAll()
        {
            return Transactions;
        }

        public bool SaveOrUpdate(Transaction transaction)
        {
            try
            {
                if (transaction.Id == 0)
                {
                    //Id, CreatedOn, and ModifiedOn are owned by system only
                    transaction.Id = Transactions.Any() ? Transactions.Max(t => t.Id) + 1 : 1; //seed at one
                    transaction.CreatedOn = DateTime.Now;
                    transaction.ModifiedOn = DateTime.MinValue;

                    _transactions.Add(transaction);
                }
                else
                {
                    var transactionToUpdate = _transactions.Where(t => t.Id == transaction.Id).FirstOrDefault();
                    if (transactionToUpdate == null)
                        throw new ArgumentException("No transaction to update. Concurrency issue.");
                    _transactions.Remove(transactionToUpdate);

                    //Id, CreatedOn, and ModifiedOn are owned by system only
                    transaction.ModifiedOn = DateTime.Now;
                    _transactions.Add(transaction);
                }
            }
            catch (Exception)
            {
                //log error
                return false;
            }
            return true;
        }

        public bool Delete(long id)
        {
            try
            {
                var transactionToDelete = _transactions.Where(t => t.Id == id).FirstOrDefault();
                if (transactionToDelete == null)
                    throw new ArgumentException("No transaction to delete. Concurrency issue.");

                _transactions.Remove(transactionToDelete);
            }
            catch (Exception)
            {
                //log error
                return false;
            }
            return true;
        }
    }
}
