using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class SalaryPayment
    {
        public int SalaryId { get; set; }
        public double? PaidAmount { get; set; }
        public DateTime? PaidDate { get; set; }
        public int? EmployeeId { get; set; }
        public int? PaidBy { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Employee PaidByNavigation { get; set; }
    }
}
