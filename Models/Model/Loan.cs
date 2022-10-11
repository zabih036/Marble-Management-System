using ShawkanyDb.Models.Model;
using System;

namespace ShawkanyDb
{
    public partial class Loan
    {
        public int LoanId { get; set; }
        public int? CurrencyId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? LoanDate { get; set; }
        public int? Quantity { get; set; }

        public Currency Currency { get; set; }
        public Customer Customer { get; set; }
    }
}
