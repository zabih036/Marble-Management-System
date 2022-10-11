using System;
using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.ViewModels
{
    public class BenifitViewModel
    {
        public int BenifitId { get; set; }
        public int InvestorId { get; set; }
        public int CurrencyId { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }

        public double TotalAmount { get; set; }

        [Required(ErrorMessage = "EnterAmount")]
        public double Amount { get; set; }

        public string Description { get; set; }



    }
}
