using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Employee
    {
        public Employee()
        {
            DealerDeal = new HashSet<DealerDeal>();
            Expence = new HashSet<Expence>();
            Hdealler = new HashSet<Hdealler>();
            InvestMoney = new HashSet<InvestMoney>();
            OverTime = new HashSet<OverTime>();
            SalaryPaymentEmployee = new HashSet<SalaryPayment>();
            SalaryPaymentPaidByNavigation = new HashSet<SalaryPayment>();
            Sale = new HashSet<Sale>();
            SecondStockIncome = new HashSet<SecondStockIncome>();
            Stock = new HashSet<Stock>();
        }

        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? HireDate { get; set; }
        public double? Salary { get; set; }
        public string Image { get; set; }

        public virtual ICollection<DealerDeal> DealerDeal { get; set; }
        public virtual ICollection<Expence> Expence { get; set; }
        public virtual ICollection<Hdealler> Hdealler { get; set; }
        public virtual ICollection<InvestMoney> InvestMoney { get; set; }
        public virtual ICollection<OverTime> OverTime { get; set; }
        public virtual ICollection<SalaryPayment> SalaryPaymentEmployee { get; set; }
        public virtual ICollection<SalaryPayment> SalaryPaymentPaidByNavigation { get; set; }
        public virtual ICollection<Sale> Sale { get; set; }
        public virtual ICollection<SecondStockIncome> SecondStockIncome { get; set; }
        public virtual ICollection<Stock> Stock { get; set; }
    }
}
