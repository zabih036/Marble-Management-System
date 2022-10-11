using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "EnterOldPasswrd")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "EnterNewPassword")]
        [StringLength(100, ErrorMessage = "PasswordCheck", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "ConfirmNotMatch")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
