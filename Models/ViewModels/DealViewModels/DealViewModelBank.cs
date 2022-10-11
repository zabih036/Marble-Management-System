using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShawkanyDb.Models.ViewModels
{
    public class DealViewModelBank
    {
        public long DealId { get; set; }
        public string Type { get; set; }

        [Required(ErrorMessage = "EnterAmount")]
        public double Debit { get; set; }

        [Required(ErrorMessage = "EnterAmount")]
        public double Credit { get; set; }
        ///////////////////////////
        [Required(ErrorMessage = "جنس انتخاب کړئ!!")]
        public int DealerID { get; set; }
        ////////////////////
        [Required(ErrorMessage = "جنس انتخاب کړئ!!")]
        public int CurrencyID { get; set; }


        public double Token { get; set; }

        public string Detail { get; set; }
    }
}
