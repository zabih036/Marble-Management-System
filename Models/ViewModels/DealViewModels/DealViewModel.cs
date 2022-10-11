using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class DealViewModel
    {
        public long DealersId { get; set; }

        [Required(ErrorMessage = "SelectDealer")]
        public int DealerId { get; set; }

        [Required(ErrorMessage = "SelectCustomer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "SelectCurrency")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "SelectDebitCredit")]
        public string Type { get; set; }

        [Required(ErrorMessage = "EnterAmount")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "SelectDate")]
        public DateTime Date { get; set; }

        public IFormFile Image { get; set; }

        public string camera { get; set; }

        public string Detail { get; set; }


    }
}
