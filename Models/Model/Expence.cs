using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Expence
    {
        public int ExpenceId { get; set; }
        public string Detail { get; set; }
        public int? EmployeeId { get; set; }
        public double ExpenceAmount { get; set; }
        public DateTime? ExpenceDate { get; set; }
        public int? ExpenceTypeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ExpenceType ExpenceType { get; set; }
    }
}
