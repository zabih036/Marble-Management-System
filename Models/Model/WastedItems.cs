using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class WastedItems
    {
        public int WastedId { get; set; }
        public int? ItemId { get; set; }
        public double? Quantity { get; set; }
        public string Unit { get; set; }
        public double? Price { get; set; }
        public int? Currency { get; set; }
        public DateTime? Date { get; set; }
    }
}
