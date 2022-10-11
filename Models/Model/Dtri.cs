using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Dtri
    {
        public long DtriId { get; set; }
        public int? DealerId { get; set; }
        public int? ItemId { get; set; }
        public int? CarNumber { get; set; }
        public double? ItemCredit { get; set; }
        public double? ItemDebit { get; set; }
        public double? Price { get; set; }
        public double? Amount { get; set; }
        public DateTime? Date { get; set; }
        public string Details { get; set; }
        public long? Item2Id { get; set; }
        public int? EmpId { get; set; }
        public double? QtyMoney { get; set; }
        public int? CurrencyId { get; set; }

        public virtual Dealer Dealer { get; set; }
    }
}
