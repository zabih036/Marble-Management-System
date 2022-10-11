using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class SecondStockItems
    {
        public long SecondStockItemsId { get; set; }
        public int? ItemId { get; set; }
        public int? CategoryId { get; set; }
        public string Unit { get; set; }
        public int? CurrencyId { get; set; }
        public double Price { get; set; }
        public double FromQty { get; set; }
        public int? StockId { get; set; }
        public double? NewQty { get; set; }
        public DateTime? Date { get; set; }

        public virtual Category Category { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Item Item { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
