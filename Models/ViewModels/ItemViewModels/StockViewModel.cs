using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class StockViewModel
    {
        public int StockId { get; set; }

        [Required(ErrorMessage = "EnterStockName")]
        [Remote("IsStockExist", "Item", AdditionalFields = "StockId", ErrorMessage = "StockAlreadyExist")]
        public string Stock { get; set; }

        [Required(ErrorMessage = "SelectResponsible")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "EnterStockLocation")]
        public string Location { get; set; }

        public string Details { get; set; }
    }
}
