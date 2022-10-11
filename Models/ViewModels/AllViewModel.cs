

using ShawkanyDb.Models.AccountViewModels;
using ShawkanyDb.Models.ViewModels;

namespace ShawkanyDb.Models.ViewModels
{
    public class AllViewModel
    {
        public ExpenseViewModel expense { get; set; }
        public ExpenseTypeViewModel expenseType { get; set; }
        public EmployeeRegistratoinViewModel employee { get; set; }
        public SalaryViewModel salary { get; set; }
        public RegisterViewModel register { get; set; }
        public ClaimViewModel claim { get; set; }
        public CreditDebitWithoutRecordViewModel dealingform { get; set; }
        public ForgotPasswordViewModel forgot { get; set; }
        public ForgotPasswordCodeViewModel code { get; set; }
        public ResetPasswordForgotViewModel reset { get; set; }
        public ManulReprotViewModel manulReport { get; set; }
        public HDealerViewModel hdealer { get; set; }
        public InvestorViewModel investor { get; set; }
        public BenifitViewModel Benifit { get; set; }
        public DealViewModelBank deal { get; set; }
    }
}
