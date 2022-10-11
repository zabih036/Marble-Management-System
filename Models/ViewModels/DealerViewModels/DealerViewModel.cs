using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class DealerViewModel
    {
        public int DealerId { get; set; }

        [Required(ErrorMessage = "EnterDealerName")]
        public string Name { get; set; }


        [Required(ErrorMessage = "EnterFatherName")]
        public string FName { get; set; }


        [Required(ErrorMessage = "EnterAddress")]
        public string Address { get; set; }

        [Required(ErrorMessage = "EnterPhone")]
        public string Phone { get; set; }

    }
}
