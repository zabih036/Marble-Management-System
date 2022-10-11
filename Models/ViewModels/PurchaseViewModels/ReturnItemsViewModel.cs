using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class ReturnItemsViewModel
    {
        public long PurchaseId { get; set; }
        public long Item2Id { get; set; }
        public int DealerId { get; set; }

        [Required(ErrorMessage = "EnterReturnQty")]
        public double ReturnQty { get; set; }

        public string Details { get; set; }
    }
}
