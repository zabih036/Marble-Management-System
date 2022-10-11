using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class CustomerDeal
    {
        public long DealId { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public DateTime? Date { get; set; }
        public int? DealTypeId { get; set; }
        public int? CurrencyId { get; set; }
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public string Loan { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
        public string Image { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
