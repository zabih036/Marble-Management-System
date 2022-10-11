using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "SelectEmployee")]
        [Remote("IsEmployeeAccountExist", "Account", AdditionalFields = "register_EmployeeId", ErrorMessage = "SelectedHasAccount")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(100, ErrorMessage = "PasswordCheck", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "ConfirmNotMatch")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "SelectDepartment")]
        public string id { get; set; }

        [Required(ErrorMessage = "SelectDepartment")]
        [ForeignKey("id2")]
        public string id2 { get; set; } = "jan";
        public string email { get; set; }
        public string email2 { get; set; }
        public string role { get; set; }
        public string trueOrfalse { get; set; }


    }
}
