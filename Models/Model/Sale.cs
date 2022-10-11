using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Sale
    {
        public long SaleId { get; set; }
        public string BillNo { get; set; }
        public int? CustomerId { get; set; }
        public int? ItemId { get; set; }
        public int? CurrencyId { get; set; }
        public double SalePrice { get; set; }
        public double Quantity { get; set; }
        public DateTime? SaleDate { get; set; }
        public int? EmployeeId { get; set; }
        public string Note { get; set; }
        public double PurchasePrice { get; set; }
        public int? StockId { get; set; }
        public string Unit { get; set; }
        public long? ProcessedId { get; set; }
        public string Type { get; set; }
        public string SaleState { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Item Item { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
