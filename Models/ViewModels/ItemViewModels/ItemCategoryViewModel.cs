using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.ViewModels
{
    public class ItemCategoryViewModel
    {

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "EnterCategoryName")]
        [Remote("IsItemCategoryExist", "Item", AdditionalFields = "CategoryId", ErrorMessage = "CategoryAlreadySaved")]
        public string Category { get; set; }
    }
}
