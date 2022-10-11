using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class DealerDeal
    {
        public long DealId { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public DateTime? Date { get; set; }
        public int? DealTypeId { get; set; }
        public int? CurrencyId { get; set; }
        public int? EmployeeId { get; set; }
        public int? DealerId { get; set; }
        public string Loan { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
        public string Image { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual DealType DealType { get; set; }
        public virtual Dealer Dealer { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
