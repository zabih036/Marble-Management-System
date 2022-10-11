using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.ViewModels
{
    public class ExpenseTypeViewModel
    {
        public int ExpenseTypeId { get; set; }

        [Required(ErrorMessage = "EnterExenseType")]
        [DataType(DataType.Text)]
        [Remote("IsExpenseTypeExist", "Expense", AdditionalFields = "ExpenseTypeId", ErrorMessage = "ExpenseTypeExist")]

        public string ExpenseType { get; set; }
    }
}
