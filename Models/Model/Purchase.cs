using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Purchase
    {
        public long PurchaseId { get; set; }
        public string BillNo { get; set; }
        public int? DealerId { get; set; }
        public int? CurrencyId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ItemId { get; set; }
        public string Unit { get; set; }
        public double FirstQty { get; set; }
        public double FirstPrice { get; set; }
        public double? SecondQty { get; set; }
        public double? SecondPrice { get; set; }
        public int? CarNumber { get; set; }
        public int? StockId { get; set; }
        public DateTime? Date { get; set; }
        public long? Item2Id { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }
        public DateTime? ReturnedDate { get; set; }

        public virtual Dealer Dealer { get; set; }
        public virtual Item Item { get; set; }
    }
}
