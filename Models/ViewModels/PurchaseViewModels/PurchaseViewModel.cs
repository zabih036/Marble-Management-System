using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class PurchaseViewModel
    {
        public long PurchaseId { get; set; }
        public long Item2Id { get; set; }

      

        [Required(ErrorMessage = "EnterBill")]
        public string PurchaseBill { get; set; }

        [Required(ErrorMessage = "SelectItem")]
        [ForeignKey("ItemId")]
        public int ItemId { get; set; }

        //////////////// <Dealer>
        [Required(ErrorMessage = "SelectDealer")]
        public int DealerId { get; set; }


        [Required(ErrorMessage = "EnterFirstQty")]
        public double FirstQty { get; set; }

        [Required(ErrorMessage = "EnterSecondQty")]
        public double SecondQty { get; set; }

        [Required(ErrorMessage = "EnterUnit")]
        public string Unit { get; set; }

        [Required(ErrorMessage = "EnterCarNo")]
        public int CarNumber { get; set; }

        [Required(ErrorMessage = "EnterPurchasePrice")]
        public double PurchasePrice { get; set; }

        [Required(ErrorMessage = "SelectCurrency")]
        public int CurrencyId { get; set; }


        [Required(ErrorMessage = "SelectStock")]
        public int StockId { get; set; }


        [Required(ErrorMessage = "EnterExpense")]
        public double TotalExpense2 { get; set; }

        [Required(ErrorMessage = "SelectDate")]
        public DateTime PurDate { get; set; }

        public double Wastage { get; set; }

        //// [Required(ErrorMessage = "د جنس د کمبودی تعداد ورسوئ!!")]
        //[Display(Name = "د کمبودی تعداد:")]
        //[ForeignKey("UnitID")]
        //public int shortageQuantity { get; set; }

    }
}
