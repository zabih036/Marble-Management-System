using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Category
    {
        public Category()
        {
            Processed = new HashSet<Processed>();
            Sale = new HashSet<Sale>();
            SecondStockItems = new HashSet<SecondStockItems>();
        }

        public int CategoryId { get; set; }
        public string Category1 { get; set; }

        public virtual ICollection<Processed> Processed { get; set; }
        public virtual ICollection<Sale> Sale { get; set; }
        public virtual ICollection<SecondStockItems> SecondStockItems { get; set; }
    }
}
