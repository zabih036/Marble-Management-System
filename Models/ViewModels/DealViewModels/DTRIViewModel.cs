using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class DTRIViewModel
    {
       
        public string Paid { get; set; }

        [Required(ErrorMessage = "SelectDealer")]
        public int DealersId { get; set; }

        [Required(ErrorMessage = "SelectItem")]
        public int ItemId { get; set; }


        [Required(ErrorMessage = "EnterItemQty")]
        public double Qty { get; set; }

        [Required(ErrorMessage = "EnterItemPrice")]
        public double Price { get; set; }

        [Required(ErrorMessage = "SelectCurrency")]
        public int DCurrencyId { get; set; }

        [Required(ErrorMessage = "SelectStock")]
        public int StockId { get; set; }

        [Required(ErrorMessage = "EnterAmount")]
        public double DtriAmount { get; set; }

        [Required(ErrorMessage = "SelectDebitCredit")]
        public string DtriType { get; set; }

        [Required(ErrorMessage = "EnterCarNumber")]
        public int CarNumber { get; set; }

        [Required(ErrorMessage = "SelectDate")]
        public DateTime DtDate { get; set; }

        public string Detail { get; set; }


    }
}
