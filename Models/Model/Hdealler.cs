using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Hdealler
    {
        public int HdealId { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public DateTime? Date { get; set; }
        public string Detail { get; set; }
        public int? EmployeeId { get; set; }
        public int? HdealerId { get; set; }
        public int? CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Hdealer Hdealer { get; set; }
    }
}
