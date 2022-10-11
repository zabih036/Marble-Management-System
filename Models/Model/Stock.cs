using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Stock
    {
        public Stock()
        {
            Item2 = new HashSet<Item2>();
            Processed = new HashSet<Processed>();
            Sale = new HashSet<Sale>();
            SecondStockIncome = new HashSet<SecondStockIncome>();
            SecondStockItems = new HashSet<SecondStockItems>();
        }

        public int StockId { get; set; }
        public string Stock1 { get; set; }
        public int? Responsible { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }

        public virtual Employee ResponsibleNavigation { get; set; }
        public virtual ICollection<Item2> Item2 { get; set; }
        public virtual ICollection<Processed> Processed { get; set; }
        public virtual ICollection<Sale> Sale { get; set; }
        public virtual ICollection<SecondStockIncome> SecondStockIncome { get; set; }
        public virtual ICollection<SecondStockItems> SecondStockItems { get; set; }
    }
}
