using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Currency
    {
        public Currency()
        {
            BenifitPayment = new HashSet<BenifitPayment>();
            CashInHand = new HashSet<CashInHand>();
            DealerDeal = new HashSet<DealerDeal>();
            Hdealler = new HashSet<Hdealler>();
            InvestMoney = new HashSet<InvestMoney>();
            Item2 = new HashSet<Item2>();
            Processed = new HashSet<Processed>();
            Sale = new HashSet<Sale>();
            SecondStockIncome = new HashSet<SecondStockIncome>();
            SecondStockItems = new HashSet<SecondStockItems>();
        }

        public int CurrencyId { get; set; }
        public string Currency1 { get; set; }

        public virtual ICollection<BenifitPayment> BenifitPayment { get; set; }
        public virtual ICollection<CashInHand> CashInHand { get; set; }
        public virtual ICollection<DealerDeal> DealerDeal { get; set; }
        public virtual ICollection<Hdealler> Hdealler { get; set; }
        public virtual ICollection<InvestMoney> InvestMoney { get; set; }
        public virtual ICollection<Item2> Item2 { get; set; }
        public virtual ICollection<Processed> Processed { get; set; }
        public virtual ICollection<Sale> Sale { get; set; }
        public virtual ICollection<SecondStockIncome> SecondStockIncome { get; set; }
        public virtual ICollection<SecondStockItems> SecondStockItems { get; set; }
    }
}
