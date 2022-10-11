using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class BenifitPayment
    {
        public int BenifitId { get; set; }
        public int? InvestorId { get; set; }
        public double? Amount { get; set; }
        public int? CurrencyId { get; set; }
        public DateTime? MonthDate { get; set; }
        public string Description { get; set; }
        public DateTime? PaidDate { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual Investor Investor { get; set; }
    }
}
