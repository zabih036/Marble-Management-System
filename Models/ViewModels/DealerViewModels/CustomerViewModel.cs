using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "EnterCustomerName")]
        public string CusName { get; set; }


        [Required(ErrorMessage = "EnterFatherName")]
        public string CusFName { get; set; }


        [Required(ErrorMessage = "EnterAddress")]
        public string CusAddress { get; set; }

        [Required(ErrorMessage = "EnterPhone")]
        public string CusPhone { get; set; }

    }
}
