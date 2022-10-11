using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class ExpenseViewModel
    {

        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "EnterExpense")]
        public double Expense { get; set; }

        [Required(ErrorMessage = "SelectExpenseType")]
        [ForeignKey("ExpenseTypeId")]
        public int ExpenseTypeId { get; set; }

        [Required(ErrorMessage = "SelectExpenseDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpenseDate { get; set; }

        public string Detail { get; set; }
    }
}
