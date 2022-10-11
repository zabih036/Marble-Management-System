using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class CashInHand
    {
        public int CashId { get; set; }
        public double? Credit { get; set; }
        public double? Debit { get; set; }
        public int? CurrencyId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public int? DealId { get; set; }
        public long? CusDealId { get; set; }
        public long? DelDealId { get; set; }
        public int? ExpenseId { get; set; }
        public int? SalaryId { get; set; }
        public long? DtriId { get; set; }
        public int? MineBeltyId { get; set; }
        public int? PkbeltyId { get; set; }
        public int? HdealId { get; set; }

        public virtual Currency Currency { get; set; }
    }
}
