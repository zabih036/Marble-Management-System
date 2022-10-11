using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class BeltyItems
    {
        public int BeltyItemsId { get; set; }
        public int? ItemId { get; set; }
        public int? CategoryId { get; set; }
        public double? Weight { get; set; }
        public double? SeekNo { get; set; }
        public int? PkbeltyId { get; set; }
        public DateTime? Date { get; set; }
        public string FromMine { get; set; }
        public string Details { get; set; }
    }
}
