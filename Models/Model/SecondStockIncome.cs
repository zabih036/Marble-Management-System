using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class SecondStockIncome
    {
        public long SecondStockId { get; set; }
        public string BillNo { get; set; }
        public int? DealerId { get; set; }
        public int? CurrencyId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ItemId { get; set; }
        public string Unit { get; set; }
        public double FirstQty { get; set; }
        public double FirstPrice { get; set; }
        public double? SecondQty { get; set; }
        public double? NetQty { get; set; }
        public double? NetPrice { get; set; }
        public int? CarNumber { get; set; }
        public int? StockId { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public long? SecondStockItemsId { get; set; }
        public string Details { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Item Item { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
