using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class ExpiredItem
    {
        public int ExpireId { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Item { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public double? PurchasePrice { get; set; }
        public double? Quantity { get; set; }
        public double? SalePrice { get; set; }
        public int? AmountInPackage { get; set; }
        public string Unit { get; set; }
    }
}
