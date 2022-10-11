using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class DealType
    {
        public DealType()
        {
            DealerDeal = new HashSet<DealerDeal>();
        }

        public int DealTypeId { get; set; }
        public string DealType1 { get; set; }

        public virtual ICollection<DealerDeal> DealerDeal { get; set; }
    }
}
