using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace ShawkanyDb.Models.ViewModels
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }

        [Required(ErrorMessage = "EnterItemName") ]
        [Remote("IsItemExist", "Item", AdditionalFields = "ItemId", ErrorMessage = "ItemAlreadySaved")]
        public string Name { get; set; }

        [Required(ErrorMessage = "SelectItemType")]
        public int ItemType { get; set; }


    }


}
