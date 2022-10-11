using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class OverTime
    {
        public int OverTimeId { get; set; }
        public double OverTimeHours { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
