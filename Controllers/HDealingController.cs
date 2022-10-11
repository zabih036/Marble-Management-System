using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShawkanyDb.Models;
using ShawkanyDb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShawkanyDb.Models.Model;

namespace ShawkanyDb.Controllers
{
    [Authorize(Roles = "Admin Department")]
    public class HDealingController : Controller
    {
        ShawkanyDbContext db = new ShawkanyDbContext();
        private readonly UserManager<ApplicationUser> _userManager;
        int depId = 0;
        public HDealingController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        public IActionResult Dealing()
        {

            return View();
        }
        public IActionResult NewHdealer(AllViewModel allv)
        {

            if (allv.hdealer.HDealerID == 0)
            {
                Hdealer hd = new Hdealer();
                hd.Hdealer1 = allv.hdealer.HDealer;
                hd.Mobile = allv.hdealer.Mobile;

                db.Hdealer.Add(hd);
                db.SaveChanges();
                return Json(" معلومات سیستم کې ذخیره شول ");

            }
            else
            {
                var recored = db.Hdealer.Where(x => x.HdealerId == allv.hdealer.HDealerID).FirstOrDefault();
                recored.Hdealer1 = allv.hdealer.HDealer;
                recored.Mobile = allv.hdealer.Mobile;
                db.Entry(recored).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                return Json("رېکارد تغیر شول");
            }

        }
        public JsonResult FetchHDealers()
        {

            var allHDealer = db.Hdealer.ToList();

            return Json(allHDealer);
        }

        public JsonResult GetAllDeals(int DealerID)
        {

            var DealV = (from d in db.Hdealler 
                         join dealer in db.Hdealer on d.HdealerId equals dealer.HdealerId
                         join cu in db.Currency on d.CurrencyId equals cu.CurrencyId
                         join e in db.Employee on d.EmployeeId equals e.EmployeeId
                         select new
                         {
                             dealid = d.HdealId,
                             debit = d.Debit,
                             credit = d.Credit,
                             currencyid = d.CurrencyId,
                             currency = cu.Currency1,

                             dealerid = d.HdealerId,
                             phone = dealer.Mobile,

                             dealer = dealer.Hdealer1,
                             employeeid = d.EmployeeId,
                             employee = e.Name,
                             date = Convert.ToDateTime(d.Date).ToShortDateString(),
                             detail = d.Detail
                         }).Where(x => x.dealerid == DealerID).OrderByDescending(r => r.dealid);

            return Json(DealV);
        }

        public async Task<IActionResult> NewDeal(DealViewModelBank deal,string cash)
        {


            var email = _userManager.GetUserAsync(User).Result.Email;
            var empid = db.Employee.Where(d => d.Email == email ).Select(r => r.EmployeeId).FirstOrDefault();

            Hdealler de = new Hdealler
            {
                Credit = deal.Credit,
                Debit = deal.Debit,
                HdealerId = deal.DealerID,
                CurrencyId = deal.CurrencyID,
                EmployeeId = empid,
                Date = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                Detail = deal.Detail,
            };
            db.Hdealler.Add(de);
            await db.SaveChangesAsync();

            if (cash == "on")
            {
                CashInHand ch = new CashInHand
                {
                    Debit = deal.Debit,
                    Credit = deal.Credit,
                    CurrencyId = deal.CurrencyID,
                    Date = DateTime.Now.Date,
                    Description = deal.Detail,
                    HdealId = de.HdealId,
                };
                db.CashInHand.Add(ch);
                await db.SaveChangesAsync();
            }

            return Json("ریکارډ ذخیره شو");
        }


        public async Task<IActionResult> DeleteRecord(int recordId)
        {
            var rec = await db.Hdealler.FindAsync(recordId);
            var cash = db.CashInHand.Where(x => x.HdealId == recordId).FirstOrDefault();

            if (rec != null)
            {
                try
                {
                    if (cash != null)
                    {
                    db.CashInHand.Remove(cash);
                    }
                    db.Hdealler.Remove(rec);

                    await db.SaveChangesAsync();

                    return Json("ریکارډ خذف شو");
                }
                catch (Exception e)
                {

                    return Json("error", e);
                }
            }
            return Json("error");
        }

        public JsonResult HasanaLoanReport()
        {

            var allLoan = db.Hdealler.ToList();
            var afghaniDebit = Convert.ToDecimal(0.00);
            var afghaniCredit = Convert.ToDecimal(0.00);
            var rupeDebit = Convert.ToDecimal(0.00);
            var rupeCredit = Convert.ToDecimal(0.00);
            var dollerDebit = Convert.ToDecimal(0.00);
            var dollerCredit = Convert.ToDecimal(0.00);
            foreach (var item in allLoan)
            {
                switch (item.CurrencyId)
                {
                    case 1:
                        afghaniDebit += Convert.ToDecimal(item.Debit);
                        afghaniCredit += Convert.ToDecimal(item.Credit);
                        break;
                    case 2:
                        rupeDebit += Convert.ToDecimal(item.Debit);
                        rupeCredit += Convert.ToDecimal(item.Credit);
                        break;
                    case 3:
                        dollerDebit += Convert.ToDecimal(item.Debit);
                        dollerCredit += Convert.ToDecimal(item.Credit);
                        break;
                }
            }
            var result = new
            {
                afghaniLoan = afghaniCredit- afghaniDebit ,
                rupeLoan = rupeCredit- rupeDebit ,
                dollerLoan =  dollerCredit- dollerDebit,
            };
            return Json(result);
        }
    }
}
