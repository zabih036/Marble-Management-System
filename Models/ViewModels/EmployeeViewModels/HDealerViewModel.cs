using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.ViewModels
{
    public class HDealerViewModel
    {

        public int HDealerID { get; set; }

     
        [Required(ErrorMessage = "EnterName")]
        public string HDealer { get; set; }

       
        [Required(ErrorMessage = "EnterPhone")]
        public string Mobile { get; set; }
    }
}
