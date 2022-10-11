using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Item2
    {
        public long Item2Id { get; set; }
        public int? ItemId { get; set; }
        public string Unit { get; set; }
        public int? CurrencyId { get; set; }
        public double PurchasePrice { get; set; }
        public double Quantity { get; set; }
        public int? StockId { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual Item Item { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
