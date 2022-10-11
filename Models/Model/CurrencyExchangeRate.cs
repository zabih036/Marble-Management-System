using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class CurrencyExchangeRate
    {
        public int CurrencyExchangeId { get; set; }
        public double? AfghaniRateToRupe { get; set; }
        public double? AfhaniRateToDoller { get; set; }
    }
}
