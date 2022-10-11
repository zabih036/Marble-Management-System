using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Pkbelty
    {
        public int BeltyId { get; set; }
        public string DriverName { get; set; }
        public string Fname { get; set; }
        public int? CarNo { get; set; }
        public string Phone { get; set; }
        public string ContractName { get; set; }
        public string ComissionerNo { get; set; }
        public string ItemName { get; set; }
        public string FillPlace { get; set; }
        public string Eigency { get; set; }
        public double? Weight { get; set; }
        public double? RentPerTon { get; set; }
        public double? TotalExpense { get; set; }
        public double? TotalRent { get; set; }
        public string EmptyPlace { get; set; }
        public double? Paid { get; set; }
        public string Details { get; set; }
        public DateTime? Date { get; set; }
    }
}
