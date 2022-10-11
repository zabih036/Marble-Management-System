using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class SaleReturnViewModel
    {
        public long SaleId { get; set; }
        public long ProcessedId { get; set; }
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "EnterReturnQty")]
        public double ReturnQty { get; set; }

      
        public string Details { get; set; }
    }
}
