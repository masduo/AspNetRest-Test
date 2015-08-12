using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml;

namespace Transactions.Api.Models
{
    /// <summary>
    /// Represents a Transaction for the API to the outside world
    /// </summary>
    public class TransactionModel
    {
        public string Url { get; set; }

        public long TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CurrencyCode { get; set; }
        public string Merchant { get; set; }
    }
}