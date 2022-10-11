using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Dealer
    {
        public Dealer()
        {
            DealerDeal = new HashSet<DealerDeal>();
            Dtri = new HashSet<Dtri>();
            Purchase = new HashSet<Purchase>();
        }

        public int DealerId { get; set; }
        public string Name { get; set; }
        public string Fname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime? RegDate { get; set; }

        public virtual ICollection<DealerDeal> DealerDeal { get; set; }
        public virtual ICollection<Dtri> Dtri { get; set; }
        public virtual ICollection<Purchase> Purchase { get; set; }
    }
}
