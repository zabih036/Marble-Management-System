using System;
using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.ViewModels
{
    public class SalaryViewModel
    {
        public int EmployeeId { get; set; }
        public int SalaryId { get; set; }

        [Required(ErrorMessage = "PleaseEnterAmount")]
        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        public double PaidAmount { get; set; }

        [Required(ErrorMessage = "PleaseSalectDate")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime PaidDate { get; set; }



    }
}
