using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transactions.Data.Entities
{
    public class Transaction
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime TransactedOn { get; set; }
        public string Description { get; set; }
        public string Merchant { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}