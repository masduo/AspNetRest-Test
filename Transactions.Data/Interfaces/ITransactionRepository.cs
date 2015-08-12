using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Data.Entities;

namespace Transactions.Data.Interfaces
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Gets all transactions from the persistance store.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Transaction> GetAll();

        /// <summary>
        /// Saves (creates or updates) a transaction to persistance store.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        bool SaveOrUpdate(Transaction transaction);

        /// <summary>
        /// Deletes a transaction from the persistance store.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(long id);
    }
}