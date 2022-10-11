using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Item
    {
        public Item()
        {
            Item2 = new HashSet<Item2>();
            Processed = new HashSet<Processed>();
            Purchase = new HashSet<Purchase>();
            Sale = new HashSet<Sale>();
            SecondStockIncome = new HashSet<SecondStockIncome>();
            SecondStockItems = new HashSet<SecondStockItems>();
        }

        public int ItemId { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Item2> Item2 { get; set; }
        public virtual ICollection<Processed> Processed { get; set; }
        public virtual ICollection<Purchase> Purchase { get; set; }
        public virtual ICollection<Sale> Sale { get; set; }
        public virtual ICollection<SecondStockIncome> SecondStockIncome { get; set; }
        public virtual ICollection<SecondStockItems> SecondStockItems { get; set; }
    }
}
