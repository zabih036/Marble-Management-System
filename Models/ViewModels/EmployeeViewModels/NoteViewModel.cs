using System;
using System.ComponentModel.DataAnnotations;

namespace ShawkanyDb.Models.ViewModels
{
    public class NoteViewModel
    {
        public int NoteId { get; set; }

        [Required(ErrorMessage = "EnterNote")]
        public string Note { get; set; }

        /////////////////////////////////////////////////////////////
        [Required(ErrorMessage = "EnterDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime TargetDate { get; set; }
        ///////////////////////////////////////////////////////////
        [Required(ErrorMessage = "EnterRemainderDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime RemindDate { get; set; }
    }
}
