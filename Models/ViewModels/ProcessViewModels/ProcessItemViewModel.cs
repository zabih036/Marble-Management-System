using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class ProcessItemViewModel
    {
        public long Item2Id { get; set; }


        [Required(ErrorMessage = "EnterProcessedQty")]
        public double ProcessedQty { get; set; }

        [Required(ErrorMessage = "SelectItem")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "SelectCategory")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "SelectCurrency")]
        [ForeignKey("CurrencyId")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "EnterItemPrice")]
        public double Price { get; set; }

        [Required(ErrorMessage = "EnterWeight")]
        public double Quantity { get; set; }


        [Required(ErrorMessage = "SelectStock")]
        public int StockId { get; set; }

        [Required(ErrorMessage = "SelectDate")]
        public DateTime ProDate { get; set; }

        public double Wastage { get; set; }

    }
}
