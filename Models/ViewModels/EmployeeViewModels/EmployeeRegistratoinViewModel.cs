using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.ViewModels
{
    public class EmployeeRegistratoinViewModel
    {
        public int EmployeeId { get; set; }
        public string defalut { get; set; }

        [Required(ErrorMessage = "EnterEmployeeName")]
        [Display(Name = "EmployeeName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "EnterFatherName")]
        [Display(Name = "FName")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "EnterAddress")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "EnterPhone")]
        [Display(Name = "Phone")]
        public int Phone { get; set; }

        [Required(ErrorMessage = "EnterEmail")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "EmailIsNotCorrect")]
        [Remote("IsEmployeeEmailExist", "Employee", AdditionalFields = "Email,EmployeeId", ErrorMessage = "EmailAlreadyUsed")]
        public string Email { get; set; }


        [Required(ErrorMessage = "EnterHireDate")]
        [Display(Name = "HireDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; } = DateTime.Now.Date;

        [Required(ErrorMessage = "EnterSalary")]
        [Display(Name = "Salary")]
        public int Salary { get; set; }

        public string oldEmail { get; set; }



    }
}
