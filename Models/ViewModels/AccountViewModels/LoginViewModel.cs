using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "EnterEmail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
