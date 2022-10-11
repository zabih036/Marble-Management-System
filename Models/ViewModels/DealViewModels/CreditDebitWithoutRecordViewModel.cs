namespace ShawkanyDb.Models.ViewModels
{
    public class CreditDebitWithoutRecordViewModel
    {
        public int id { get; set; }
        public double afghaniCredit { get; set; }
        public double afghaniDebit { get; set; }
        public double rupeCredit { get; set; }
        public double rupeDebit { get; set; }
        public double dollerCredit { get; set; }
        public double dollerDebit { get; set; }
        public double selling { get; set; }
        public string tempCustomer { get; set; }
    }
}
