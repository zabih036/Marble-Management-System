using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.ViewModels
{
    public class ForgotPasswordCodeViewModel
    {
        [Required(ErrorMessage = "کوډ ولیکی.")]
        [EmailAddress]
        [Display(Name = "کوډ")]
        public int Code { get; set; }
    }
}
