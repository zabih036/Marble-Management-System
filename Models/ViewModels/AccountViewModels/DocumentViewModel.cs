using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.ViewModels
{
    public class DocumentViewModel
    {
        public int DocumentID { get; set; }

        [Required(ErrorMessage = "EnterDocumentDetails")]
        [DataType(DataType.Text)]
        public string DocumentDetails { get; set; }

        [Required(ErrorMessage = "SelectImage")]
        [DataType(DataType.Upload)]
        public string DocumentPicture { get; set; }


    }
}
