using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class SaleViewModel
    {
        public long SaleId { get; set; }
        public long Item2Id { get; set; }


        [Required(ErrorMessage = "EnterBill")]
        public string SaleBill { get; set; }

        [Required(ErrorMessage = "SelectItem")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "SelectCategory")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "SelectCustomer")]
        public int CustomerId { get; set; }


        [Required(ErrorMessage = "EnterItemQuantity")]
        public double Qty { get; set; }

        public double Price { get; set; }

        public double ExistQty { get; set; }

        [Required(ErrorMessage = "EnterSalePrice")]
        public double SalePrice { get; set; }

        [Required(ErrorMessage = "SelectCurrency")]
        public int CurrencyId { get; set; }


        [Required(ErrorMessage = "SelectStock")]
        public int StockId { get; set; }

        [Required(ErrorMessage = "SelectDate")]
        public DateTime Date { get; set; }

    }
}
