using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.ViewModels
{
    public class InvestorViewModel
    {
        public int InvestorId { get; set; }
        public string defalut { get; set; }

        [Required(ErrorMessage = "EnterName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "EnterPhone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "EnterEmail")]
        [EmailAddress(ErrorMessage = "EmailIsNotCorrect")]
        public string Email { get; set; }


        [Required(ErrorMessage = "SelectDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now.Date;

        public string Description { get; set; }

        public IFormFile Image { get; set; }
        public string oldEmail { get; set; }



    }
}
