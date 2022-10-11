using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShawkanyDb.Models;
using ShawkanyDb.Models.Model;
using ShawkanyDb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShawkanyDb.Controllers
{
    public class InvestmentController : Controller
    {

        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        ShawkanyDbContext db = new ShawkanyDbContext();

        public InvestmentController(UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]  
        public IActionResult Investor()
        {
            if (TempData["updated"] != null)
            {
                ViewBag.Alert = TempData["updated"];
            }
            else
            {
                ViewBag.Alert = "empty";
            }
            return View();
        }

        public JsonResult LoadInvestor()
        {
            var rec = db.Investor.ToList().OrderByDescending(d => d.InvestorId);
            return Json(rec);
        }

        [HttpPost]
        public async Task<IActionResult> InvestorRegistration(AllViewModel model, [FromForm] IFormFile img)
        {
            if (ModelState.IsValid)
            {


                string uploadPath = "";
                string upload = "";
                if (img == null)
                {
                    if (model.investor.InvestorId == 0)
                        upload = "/images/StaticImages/browse.png";
                }
                else
                {


                    var fileName = model.investor.Email + Guid.NewGuid().ToString().Replace("_", "") + Path.GetExtension(img.FileName);
                    upload = Path.Combine("Images/InvestorImage/", fileName);
                    uploadPath = Path.Combine(hostingEnvironment.WebRootPath, upload);
                    img.CopyTo(new FileStream(uploadPath, FileMode.Create));
                    upload = "/" + upload;

                }


                if (model.investor.InvestorId != 0)
                {
              
                    var rec = await db.Investor.FindAsync(model.investor.InvestorId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.Name = model.investor.Name;
                    rec.Phone = model.investor.Phone.ToString();
                    rec.Email = model.investor.Email;
                    rec.RegistrationDate = model.investor.RegistrationDate;
                    rec.Description = model.investor.Description;

                    db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    if (img != null && model.investor.defalut == "0")
                    {
                        db.Entry(rec).Property(d => d.Image).IsModified = true;
                        
                    }
                    else if (model.investor.defalut != "0")
                    {
                        db.Entry(rec).Property(d => d.Image).IsModified = true;
                        
                    }
                    
                    await db.SaveChangesAsync();
                    TempData["updated"] = "2";
                    return RedirectToAction("Investor", "Investment");

                }
                else
                {
                    try
                    {
                        Investor it = new Investor
                        {
                            Name = model.investor.Name,
                            Phone = model.investor.Phone.ToString(),
                            Email = model.investor.Email,
                            RegistrationDate = Convert.ToDateTime(model.investor.RegistrationDate.ToShortDateString()).Date,
                            Description = model.investor.Description,
                            Image = upload
                        };
                        db.Investor.Add(it);
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.Message);
                    }
                    TempData["updated"] = "1";
                    return RedirectToAction("Investor", "Investment");
                }

            }
            return View("Error");

        }

        public async Task<IActionResult> Deleteinvestor(AllViewModel model)
        {
            if (model.investor.InvestorId != 0)
            {
                var comp = await db.Investor.FindAsync(model.investor.InvestorId);
                if (comp == null)
                {
                    return View("Error");
                }

                try
                {                             
                    db.Investor.Remove(comp);
                    await db.SaveChangesAsync();
                    return Json("1");
                }
                catch (Exception)
                {

                    return Json("2");
                }
            }
            return NotFound();

        }

        public async Task<IActionResult> AddinvestorSalary(AllViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.salary.SalaryId != 0)
                {
                    var rec = await db.SalaryPayment.FindAsync(model.salary.SalaryId);
                    if (rec == null)
                    {
                        return View("Error");
                    }

                    rec.PaidAmount = model.salary.PaidAmount;

                    db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Entry(rec).Property(d => d.PaidAmount).IsModified = true;

                    var id = model.salary.SalaryId + "," + "salary";
                    var rec4 = db.CashInHand.Where(x => x.Description == id).FirstOrDefault();
                    rec4.Debit = model.salary.PaidAmount;
                    rec4.Date = DateTime.Now.Date;
                    db.Entry(rec4).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Entry(rec4).Property(x => x.Debit).IsModified = true;
                    db.Entry(rec4).Property(x => x.Date).IsModified = true;


                    await db.SaveChangesAsync();
                    return Json("2");

                }
                else
                {
                    try
                    {

                        var email = _userManager.GetUserAsync(User).Result.Email;
                        var empId = db.Investor.Where(e => e.Email == email).Select(r => r.InvestorId).FirstOrDefault();

                        SalaryPayment it = new SalaryPayment()
                        {
                            PaidAmount = model.salary.PaidAmount,
                            PaidDate = DateTime.Now.Date,
                            PaidBy = empId
                        };
                        db.SalaryPayment.Add(it);
                        await db.SaveChangesAsync();

                        var exId = db.SalaryPayment.OrderByDescending(x => x.SalaryId).FirstOrDefault();
                        CashInHand ch = new CashInHand()
                        {
                            Debit = model.salary.PaidAmount,
                            Credit = 0,
                            Date = DateTime.Now.Date,
                            CurrencyId = 1,
                            Description = exId.SalaryId.ToString() + "," + "salary"
                        };
                        db.CashInHand.Add(ch);
                        await db.SaveChangesAsync();

                        return Json("نوموړی شراکتې ته پیسې ورسول شوی");
                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.StackTrace);
                    }
                }

            }
            return View("Error");

        }

        public IActionResult LoadinvestorAllPayments(AllViewModel model)
        {
            if (model.investor.InvestorId != 0)
            {
                var comp = (from s in db.SalaryPayment
                            where s.EmployeeId ==1 
                            select new
                            {
                                paidDate = Convert.ToDateTime(s.PaidDate).Date.ToShortDateString(),
                                paidAmount = s.PaidAmount
                            }).ToList();
                if (comp == null)
                {
                    return View("Error");
                }

                try
                {

                    return Json(comp);
                }
                catch (Exception)
                {

                    return Json("نوموړی شراکتې ته پیسې نه دی ورکړل شوی");
                }
            }
            return NotFound();

        }

        public IActionResult AddInvestment()
        {
            return View();
        }

        public JsonResult GetAllDeals(int DealerID)
        {
            var DealV = (from i in db.InvestMoney
                         join iv in db.Investor on i.InvestorId equals iv.InvestorId
                         join cu in db.Currency on i.CurrencyId equals cu.CurrencyId
                         select new
                         {
                             dealid = i.InvestMoneyId,
                             debit = i.Debit,
                             credit = i.Credit,
                             currencyid = cu.CurrencyId,
                             currency = cu.Currency1,
                             dealerid = iv.InvestorId,
                             phone = iv.Phone,
                             dealer = iv.Name,
                             employeeid = i.Employee.EmployeeId,
                             employee = i.Employee.Name,
                             date = Convert.ToDateTime(i.Date).ToShortDateString(),
                             detail = i.Description
                         }).Where(x => x.dealerid == DealerID).OrderByDescending(r => r.dealid);

            return Json(DealV);
        }
        public async Task<IActionResult> NewDeal(InvDealViewModel deal)
        {
            var email = _userManager.GetUserAsync(User).Result.Email;
            var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                InvestMoney de = new InvestMoney
                {
                    Credit = deal.Credit,
                    Debit = deal.Debit,
                    InvestorId = deal.DealerID,
                    CurrencyId = deal.CurrencyID,
                    EmployeeId = empId,
                    Date = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                    Description = deal.Detail,
                    Type = deal.Type,
                };
                db.InvestMoney.Add(de);
                await db.SaveChangesAsync();

                    CashInHand ch = new CashInHand()
                    {
                        CurrencyId = deal.CurrencyID,
                        Credit = deal.Credit,
                        Debit = deal.Debit,
                        Date = DateTime.Now.Date,
                        Description = " پانګونه"
                    };
                    db.CashInHand.Add(ch);
                    await db.SaveChangesAsync();

            }
           
            return Json("1");
        }
        public JsonResult FetchDealers()
        {
            var SelectedItemRecored = (from i in db.Investor                                      
                                       select new
                                       {
                                           dealerid = i.InvestorId,
                                           name = i.Name,
                                           phone = i.Phone,
                                           email = i.Email,
                                       }).ToList();
            return Json(SelectedItemRecored);
        }

        public IActionResult Benifit()
        {
            var i = (from d in db.Item
                     select new SelectListItem()
                     {
                         Text = d.Name,
                         Value = d.ItemId.ToString()
                     }).ToList();
            ViewBag.item = i;

            var Currency = (from d in db.Currency
                            select new SelectListItem()
                            {
                                Text = d.Currency1,
                                Value = d.CurrencyId.ToString()
                            }).ToList();
            ViewBag.Currency = Currency;
            return View();             
        }

        public JsonResult FetchFinanceReport(int reportType)
        {
            var pur = db.Purchase.Where(x => x.PurchaseId > 0 && x.Status != "Returned").ToList();
            var C_D = db.CustomerDeal.ToList();
            var sale = db.Sale.Where(x => x.SaleId > 0 && x.SaleState != "Returned");
            var expence = db.Expence.Where(x => x.ExpenceId > 0);
            var salary = db.SalaryPayment.Where(x => x.SalaryId > 0);
            //var purchseExpense = db.PurchaseExpense.Where(x => x.PurchaseExpenseId > 0);
            var qarzhasana = db.Hdealler.Where(x => x.HdealId > 0);
            var paidB = db.BenifitPayment.Where(x => x.BenifitId > 0);
            var year = 0;
            var month = 0;
            var day = 0;
            switch (reportType)
            {
                //////////////////////////// todays report ///////////////
                case 1:
                    pur = db.Purchase.Where(x => x.Date == DateTime.Today && x.Status != "Returned").ToList();
                    C_D = db.CustomerDeal.Where(x => x.Date == DateTime.Today).ToList();
                    sale = db.Sale.Where(x => x.SaleDate == DateTime.Today && x.SaleState != "Returned");
                    expence = db.Expence.Where(x => x.ExpenceDate == DateTime.Today);
                    salary = db.SalaryPayment.Where(x => x.PaidDate == DateTime.Today);
                    //purchseExpense = db.PurchaseExpense.Where(x => x.Date == DateTime.Today);
                    qarzhasana = db.Hdealler.Where(x => x.Date == DateTime.Today);
                    paidB = db.BenifitPayment.Where(x => x.MonthDate == DateTime.Today);
                    year = DateTime.Now.Year;
                    month = DateTime.Now.Month;
                    day = DateTime.Now.Day;
                    break;
                ///////////////////////// current month's report ///////////
                case 2:
                    var currentMonth = DateTime.Now.Month;
                    pur = db.Purchase.Where(x => x.Date.Value.Month == currentMonth && x.Date.Value.Year == DateTime.Now.Year && x.Status != "Returned").ToList();
                    C_D = db.CustomerDeal.Where(x =>  x.Date.Value.Month == currentMonth && x.Date.Value.Year == DateTime.Now.Year).ToList();
                    sale = db.Sale.Where(x => x.SaleDate.Value.Month == currentMonth && x.SaleDate.Value.Year == DateTime.Now.Year && x.SaleState != "Returned");
                    expence = db.Expence.Where(x => x.ExpenceDate.Value.Month == currentMonth && x.ExpenceDate.Value.Year == DateTime.Now.Year);
                    salary = db.SalaryPayment.Where(x => x.PaidDate.Value.Month == currentMonth && x.PaidDate.Value.Year == DateTime.Now.Year);
                    //purchseExpense = db.PurchaseExpense.Where(x => x.Date.Value.Month == currentMonth && x.Date.Value.Year == DateTime.Now.Year);
                    qarzhasana = db.Hdealler.Where(x => x.Date.Value.Month == currentMonth && x.Date.Value.Year == DateTime.Now.Year);
                    paidB = db.BenifitPayment.Where(x => x.MonthDate.Value.Month == currentMonth && x.MonthDate.Value.Year == DateTime.Now.Year);
                    year = DateTime.Now.Year;
                    month = DateTime.Now.Month;
                    break;
                /////////////////////////   last month report ////////////////////////
                case 3:
                    {
                        Dictionary<string, int> dictWeeklySum = new Dictionary<string, int>();
                        pur = db.Purchase.Where(x => x.Date.Value.Month == (DateTime.Now.Month - 1) && x.Date.Value.Year == DateTime.Now.Year && x.Status != "Returned").ToList();
                        C_D = db.CustomerDeal.Where(x => x.Date.Value.Month == (DateTime.Now.Month - 1) && x.Date.Value.Year == DateTime.Now.Year).ToList();
                        sale = db.Sale.Where(x => x.SaleDate.Value.Month == (DateTime.Now.Month - 1) && x.SaleDate.Value.Year == DateTime.Now.Year && x.SaleState != "Returned");
                        expence = db.Expence.Where(x => x.ExpenceDate.Value.Month == (DateTime.Now.Month - 1) && x.ExpenceDate.Value.Year == DateTime.Now.Year);
                        salary = db.SalaryPayment.Where(x => x.PaidDate.Value.Month == (DateTime.Now.Month - 1) && x.PaidDate.Value.Year == DateTime.Now.Year);
                        //purchseExpense = db.PurchaseExpense.Where(x => x.Date.Value.Month == (DateTime.Now.Month - 1) && x.Date.Value.Year == DateTime.Now.Year);
                        qarzhasana = db.Hdealler.Where(x => x.Date.Value.Month == (DateTime.Now.Month - 1) && x.Date.Value.Year == DateTime.Now.Year);
                        paidB = db.BenifitPayment.Where(x => x.PaidDate.Value.Month == (DateTime.Now.Month - 1) && x.PaidDate.Value.Year == DateTime.Now.Year);
                        year = DateTime.Now.Year;
                        month = DateTime.Now.Month - 1;
                    }
                    break;
                /////////////////////////  Current Year's report  ////////////
                case 4:
                    var currentYear = DateTime.Now.Year;

                    pur = db.Purchase.Where(x => x.Date.Value.Year == currentYear && x.Status != "Returned").ToList();
                    C_D = db.CustomerDeal.Where(x => x.Date.Value.Year == currentYear).ToList();
                    sale = db.Sale.Where(x => x.SaleDate.Value.Year == currentYear && x.SaleState != "Returned");
                    expence = db.Expence.Where(x => x.ExpenceDate.Value.Year == currentYear);
                    salary = db.SalaryPayment.Where(x => x.PaidDate.Value.Year == currentYear);
                    //purchseExpense = db.PurchaseExpense.Where(x => x.Date.Value.Year == currentYear);
                    qarzhasana = db.Hdealler.Where(x => x.Date.Value.Year == currentYear);
                    paidB = db.BenifitPayment.Where(x => x.MonthDate.Value.Year == currentYear);
                    year = DateTime.Now.Year;
                    break;
                ////////////////////////   Last year's report ///////////////
                case 5:
                    pur = db.Purchase.Where(x => x.Date.Value.Year == (DateTime.Now.Year - 1) && x.Status != "Returned").ToList();
                    C_D = db.CustomerDeal.Where(x =>  x.Date.Value.Year == (DateTime.Now.Year - 1)).ToList();
                    sale = db.Sale.Where(x => x.SaleDate.Value.Year == (DateTime.Now.Year - 1) && x.SaleState != "Returned");
                    expence = db.Expence.Where(x => x.ExpenceDate.Value.Year == (DateTime.Now.Year - 1));
                    salary = db.SalaryPayment.Where(x => x.PaidDate.Value.Year == (DateTime.Now.Year - 1));
                    //purchseExpense = db.PurchaseExpense.Where(x => x.Date.Value.Year == (DateTime.Now.Year - 1));
                    qarzhasana = db.Hdealler.Where(x => x.Date.Value.Year == (DateTime.Now.Year - 1));
                    paidB = db.BenifitPayment.Where(x => x.MonthDate.Value.Year == (DateTime.Now.Year - 1));
                    year = DateTime.Now.Year - 1;
                    break;
            }
            double afghanipur = 0, rupepur = 0, dollerpur = 0;
            double afghanisale = 0, rupesale = 0, dollersale = 0, afghanisalePur = 0, rupesalePur = 0, dollersalePur = 0;
            double afghanicredit = 0, rupecredit = 0, dollercredit = 0;
            double afghanidebit = 0, rupedebit = 0, dollerdebit = 0;
            double afghaniexpense = 0, rupeexpence = 0, dollerexpence = 0;

            ///////////////////////////// All purchase ////////////////
            foreach (var item in pur)
            {
                switch (item.CurrencyId)
                {
                    case 1:
                        afghanipur += Convert.ToDouble(item.SecondQty * item.SecondQty );
                        break;
                    case 2:
                        rupepur += Convert.ToDouble(item.SecondQty * item.SecondQty);
                        break;
                    case 3:
                        dollerpur += Convert.ToDouble(item.SecondQty * item.SecondQty);
                        break;
                }
            }
            ////////////////////////////////////////////////////////
            ///////////////////////////// All Debit and credit ////// ////////////////
            var dealers = db.Dealer;
            int[] dealerid = new int[dealers.Count() + 1];
            int index = 0;

            //foreach (var item in C_D)
            //{

            //    var ID = item.DealerId;
            //    var idExist = true;

            //    double dAfghanicredit = 0.0, dAfghanidebit = 0.0;
            //    double dRupecredit = 0.0, dRupedebit = 0.0;
            //    double dDollercredit = 0.0, dDollerdebit = 0.0;

            //    foreach (var deal in C_D)
            //    {
            //        ////////// checking if a dealer's dealing already checked ////////// 
            //        foreach (var array in dealerid)
            //        {
            //            if (array == ID)
            //            {
            //                idExist = false;
            //            }

            //        }
            //        if (idExist)
            //        {

            //            if (item.DealerId == deal.DealerId)
            //            {
            //                switch (deal.CurrencyId)
            //                {
            //                    case 1:
            //                        dAfghanidebit += Convert.ToDouble(deal.Debit);
            //                        dAfghanicredit += Convert.ToDouble(deal.Credit);
            //                        break;
            //                    case 2:
            //                        dRupedebit += Convert.ToDouble(deal.Debit);
            //                        dRupecredit += Convert.ToDouble(deal.Credit);
            //                        break;
            //                    case 3:
            //                        dDollerdebit += Convert.ToDouble(deal.Debit);
            //                        dDollercredit += Convert.ToDouble(deal.Credit);
            //                        break;
            //                }
            //            }
            //        }

            //    }

            //    ///////////////  assigning afghani credit and debit //////////
            //    if ((dAfghanidebit - dAfghanicredit) >= 0)
            //    {
            //        afghanidebit += dAfghanidebit - dAfghanicredit;
            //    }
            //    else
            //    {
            //        afghanicredit += dAfghanicredit - dAfghanidebit;
            //    }
            //    //////////////////////////////////////////////////////
            //    ///////////////// assigning rupe credit and debit/////
            //    if ((dRupedebit - dRupecredit) >= 0)
            //    {
            //        rupedebit += dRupedebit - dRupecredit;
            //    }
            //    else
            //    {
            //        rupecredit += dRupecredit - dRupedebit;
            //    }
            //    ///////////////////////////////////////////////////////////
            //    /////////////// assigning doller credit and debit /////////
            //    if ((dDollerdebit - dDollercredit) >= 0)
            //    {
            //        dollerdebit += dDollerdebit - dDollercredit;
            //    }
            //    else
            //    {
            //        dollercredit += dDollercredit - dDollerdebit;
            //    }
            //    ///////////////////////////////////////////////////////////
            //    if (idExist)
            //    {
            //        dealerid[index] = Convert.ToInt32(item.DealerId);
            //        index++;
            //    }
            //}
            foreach (var qarz in qarzhasana)
            {
                if (qarz.CurrencyId == 1)
                {
                    afghanicredit += qarz.Credit;
                    afghanidebit += qarz.Debit;
                }
                if (qarz.CurrencyId == 2)
                {
                    rupecredit += qarz.Credit;
                    rupedebit += qarz.Debit;
                }
                if (qarz.CurrencyId == 3)
                {
                    dollercredit += qarz.Credit;
                    dollerdebit += qarz.Debit;
                }
            }

            var discount = 0.0;
            var afcredit = afghanicredit - afghanidebit >= 0 ? afghanicredit - afghanidebit : 0;
            var afdebit = afghanidebit - afghanicredit >= 0 ? afghanidebit - afghanicredit : 0;

            var pkcredit = rupecredit - rupedebit >= 0 ? rupecredit - rupedebit : 0;
            var pkdebit = rupedebit - rupecredit >= 0 ? rupedebit - rupecredit : 0;


            var uscredit = dollercredit - dollerdebit >= 0 ? dollercredit - dollerdebit : 0;
            var usdebit = dollerdebit - dollercredit >= 0 ? dollerdebit - dollercredit : 0;

            //////////////////////////////////////////////////////////////////
            /////////////////////////// All Sales /////////////////
            foreach (var item in sale)
            {
                switch (item.CurrencyId)
                {
                    case 1:
                        afghanisale += (Convert.ToDouble((item.Quantity ) * item.SalePrice)) ;
                        afghanisalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity ));
                        break;
                    case 2:
                        rupesale += Convert.ToDouble((item.Quantity ) * item.SalePrice) ;
                        rupesalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity));
                        break;
                    case 3:
                        dollersale += Convert.ToDouble((item.Quantity ) * item.SalePrice) ;
                        dollersalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity ));
                        break;
                }
            }
            /////////////////////////////////////////////////////////////////
            //////////////////////  All expence ////////////////////////////
            foreach (var item in expence)
            {

                afghaniexpense += Convert.ToDouble(item.ExpenceAmount);

            }
            foreach (var item in salary)
            {

                afghaniexpense += Convert.ToDouble(item.PaidAmount);
            }
            //foreach (var item in purchseExpense.Where(x => x.CurrencyId == 1))
            //{

            //    afghaniexpense += Convert.ToDouble(item.Car + item.Custom + item.Municipalty + item.Commission + item.Cring + item.BorderExp);
            //}
            //foreach (var item in purchseExpense.Where(x => x.CurrencyId == 2))
            //{

            //    rupeexpence += Convert.ToDouble(item.Car + item.Custom + item.Municipalty + item.Commission + item.Cring + item.BorderExp);
            //}
            //foreach (var item in purchseExpense)
            //{

            //    dollerexpence += Convert.ToDouble(item.Car + item.Custom + item.Municipalty + item.Commission + item.Cring + item.BorderExp + item.Blowing);
            //}
            var investmnt = (from i in db.InvestMoney
                             select new
                             {
                                 debit = i.Debit,
                                 credit = i.Credit,
                                 currencyId = i.CurrencyId,
                                 investorId = i.InvestorId,
                             }).ToList();
            var investr = (from i in db.Investor
                           select new
                           {
                               image = i.Image,
                               name = i.Name,
                               investorId = i.InvestorId,

                           }).ToList();
            double afinvcr = 0;
            double afinvde = 0;
            double pkinvcr = 0;
            double pkinvde = 0;
            double usinvcr = 0;
            double usinvde = 0;
            foreach (var item in investmnt)
            {
                if (item.currencyId == 1)
                {
                    afinvcr += item.credit;
                    afinvde += item.debit;
                }
                if (item.currencyId == 2)
                {
                    pkinvcr += item.credit;
                    pkinvde += item.debit;
                }
                if (item.currencyId == 3)
                {
                    usinvcr += item.credit;
                    usinvde += item.debit;
                }
            }

            var afinvcredit = afinvcr - afinvde;

            var pkinvcredit = pkinvcr - pkinvde;

            var usinvcredit = usinvcr - usinvde;



            var afBenifit = (afdebit - afcredit - afghaniexpense) + (afghanisale - afghanisalePur);
            var pkBenifit = (pkdebit - pkcredit - rupeexpence) + (rupesale - rupesalePur);
            var usBenifit = (usdebit - uscredit - dollerexpence) + (dollersale - dollersalePur);
            var result = new
            {
                investment = investmnt,
                investor = investr,
                afb = afBenifit,
                pkb = pkBenifit,
                usb = usBenifit,
                afinvest = afinvcredit,
                pkinvest = pkinvcredit,
                usinvest = usinvcredit,
                year = year,
                month = month,
                day = day,
                paid = paidB,
            };

            return Json(result);
        }
        public IActionResult ManualReprot(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                Dictionary<string, int> dictMonthlySum = new Dictionary<string, int>();
                var year = model.manulReport.ToDate.Year;
                var month = model.manulReport.ToDate.Month;
                var day = model.manulReport.ToDate.Day;

                var pur = db.Purchase.Where(x => x.Date >= model.manulReport.FromDate.Date && x.Date <=
                                             model.manulReport.ToDate.Date && x.Status != "Returned").ToList();
                var C_D = db.CustomerDeal.Where(x =>  x.Date >= model.manulReport.FromDate.Date && x.Date <=
                                             model.manulReport.ToDate.Date).ToList();
                var sale = db.Sale.Where(x => x.SaleDate >= model.manulReport.FromDate.Date && x.SaleDate <=
                                             model.manulReport.ToDate.Date && x.SaleState != "Returned");
                var expence = db.Expence.Where(x => x.ExpenceDate >= model.manulReport.FromDate.Date && x.ExpenceDate <=
                                             model.manulReport.ToDate.Date);
                var salary = db.SalaryPayment.Where(x => x.PaidDate >= model.manulReport.FromDate.Date && x.PaidDate <=
                                     model.manulReport.ToDate.Date);
                //var purchaseExpense = db.PurchaseExpense.Where(x => x.Date >= model.manulReport.FromDate.Date && x.Date <=
                //                    model.manulReport.ToDate.Date);
                var qarzhasana = db.Hdealler.Where(x => x.Date >= model.manulReport.FromDate.Date && x.Date <=
                                  model.manulReport.ToDate.Date);
                var paidB = db.BenifitPayment.Where(x => x.PaidDate >= model.manulReport.FromDate.Date && x.PaidDate <=
                                  model.manulReport.ToDate.Date);

                double afghanipur = 0, rupepur = 0, dollerpur = 0;
                double afghanisale = 0, rupesale = 0, dollersale = 0, afghanisalePur = 0, rupesalePur = 0, dollersalePur = 0;
                double afghanicredit = 0, rupecredit = 0, dollercredit = 0;
                double afghanidebit = 0, rupedebit = 0, dollerdebit = 0;
                double afghaniexpence = 0, rupeexpence = 0, dollerexpence = 0;

                ///////////////////////////// All purchase ////////////////
                foreach (var item in pur)
                {
                    switch (item.CurrencyId)
                    {
                        case 1:
                            afghanipur += Convert.ToDouble(item.SecondPrice * (item.SecondQty ));
                            break;
                        case 2:
                            rupepur += Convert.ToDouble(item.SecondPrice * (item.SecondQty));
                            break;
                        case 3:
                            dollerpur += Convert.ToDouble(item.SecondPrice * (item.SecondQty));
                            break;
                    }
                }
                ////////////////////////////////////////////////////////
                ///////////////////////////// All Debit and credit ////// ////////////////
                var dealers = db.Dealer;
                int[] dealerid = new int[dealers.Count() + 1];

                //foreach (var item in C_D)
                //{

                //    var ID = item.DealerId;
                //    var idExist = true;

                //    double dAfghanicredit = 0.0, dAfghanidebit = 0.0;
                //    double dRupecredit = 0.0, dRupedebit = 0.0;
                //    double dDollercredit = 0.0, dDollerdebit = 0.0;

                //    foreach (var deal in C_D)
                //    {
                //        ////////// checking if a dealer's dealing already checked ////////// 
                //        foreach (var array in dealerid)
                //        {
                //            if (array == ID)
                //            {
                //                idExist = false;
                //            }

                //        }
                //        if (idExist)
                //        {

                //            if (item.DealerId == deal.DealerId)
                //            {
                //                switch (deal.CurrencyId)
                //                {
                //                    case 1:
                //                        dAfghanidebit += Convert.ToDouble(deal.Debit);
                //                        dAfghanicredit += Convert.ToDouble(deal.Credit);
                //                        break;
                //                    case 2:
                //                        dRupedebit += Convert.ToDouble(deal.Debit);
                //                        dRupecredit += Convert.ToDouble(deal.Credit);
                //                        break;
                //                    case 3:
                //                        dDollerdebit += Convert.ToDouble(deal.Debit);
                //                        dDollercredit += Convert.ToDouble(deal.Credit);
                //                        break;
                //                }
                //            }
                //        }

                //    }

                //    ///////////////  assigning afghani credit and debit //////////
                //    if ((dAfghanidebit - dAfghanicredit) >= 0)
                //    {
                //        afghanidebit += dAfghanidebit - dAfghanicredit;
                //    }
                //    else
                //    {
                //        afghanicredit += dAfghanicredit - dAfghanidebit;
                //    }
                //    //////////////////////////////////////////////////////
                //    ///////////////// assigning rupe credit and debit/////
                //    if ((dRupedebit - dRupecredit) >= 0)
                //    {
                //        rupedebit += dRupedebit - dRupecredit;
                //    }
                //    else
                //    {
                //        rupecredit += dRupecredit - dRupedebit;
                //    }
                //    ///////////////////////////////////////////////////////////
                //    /////////////// assigning doller credit and debit /////////
                //    if ((dDollerdebit - dDollercredit) >= 0)
                //    {
                //        dollerdebit += dDollerdebit - dDollercredit;
                //    }
                //    else
                //    {
                //        dollercredit += dDollercredit - dDollerdebit;
                //    }
                //    ///////////////////////////////////////////////////////////
                //    if (idExist)
                //    {
                //        dealerid[index] = Convert.ToInt32(item.DealerId);
                //        index++;
                //    }
                //}

                //////////////////////////////////////////////////////////////////
                /////////////////////////// All Sales /////////////////
                foreach (var item in sale)
                {
                    switch (item.CurrencyId)
                    {
                        case 1:
                            afghanisale += Convert.ToDouble(item.SalePrice * (item.Quantity ));
                            afghanisalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity ));
                            break;
                        case 2:
                            rupesale += Convert.ToDouble(item.SalePrice * (item.Quantity ));
                            rupesalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity));
                            break;
                        case 3:
                            dollersale += Convert.ToDouble(item.SalePrice * (item.Quantity));
                            dollersalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity ));
                            break;
                    }
                }
                /////////////////////////////////////////////////////////////////
                //////////////////////  All expence ////////////////////////////
                foreach (var item in expence)
                {

                    afghaniexpence += Convert.ToDouble(item.ExpenceAmount);

                }
                foreach (var item in salary)
                {

                    afghaniexpence += Convert.ToDouble(item.PaidAmount);

                }
                //foreach (var item in purchaseExpense.Where(x => x.CurrencyId == 1))
                //{

                //    afghaniexpence += Convert.ToDouble(item.Car + item.Custom + item.Municipalty + item.Commission + item.Cring + item.BorderExp);
                //}
                //foreach (var item in purchaseExpense.Where(x => x.CurrencyId == 2))
                //{

                //    rupeexpence += Convert.ToDouble(item.Car + item.Custom + item.Municipalty + item.Commission + item.Cring + item.BorderExp);
                //}
                //foreach (var item in purchaseExpense)
                //{

                //    dollerexpence += Convert.ToDouble(item.Car + item.Custom + item.Municipalty + item.Commission + item.Cring + item.BorderExp +item.Blowing);
                //}
                foreach (var qarz in qarzhasana)
                {
                    if (qarz.CurrencyId == 1)
                    {
                        afghanicredit += qarz.Credit;
                        afghanidebit += qarz.Debit;
                    }
                    if (qarz.CurrencyId == 2)
                    {
                        rupecredit += qarz.Credit;
                        rupedebit += qarz.Debit;
                    }
                    if (qarz.CurrencyId == 3)
                    {
                        dollercredit += qarz.Credit;
                        dollerdebit += qarz.Debit;
                    }
                }

                var discount = 0.0;
                var afcredit = afghanicredit - afghanidebit >= 0 ? afghanicredit - afghanidebit : 0;
                var afdebit = afghanidebit - afghanicredit >= 0 ? afghanidebit - afghanicredit : 0;

                var pkcredit = rupecredit - rupedebit >= 0 ? rupecredit - rupedebit : 0;
                var pkdebit = rupedebit - rupecredit >= 0 ? rupedebit - rupecredit : 0;


                var uscredit = dollercredit - dollerdebit >= 0 ? dollercredit - dollerdebit : 0;
                var usdebit = dollerdebit - dollercredit >= 0 ? dollerdebit - dollercredit : 0;
                var investmnt = (from i in db.InvestMoney
                                 select new
                                 {
                                     debit = i.Debit,
                                     credit = i.Credit,
                                     currencyId = i.CurrencyId,
                                     investorId = i.InvestorId,
                                 }).ToList();
                var investr = (from i in db.Investor
                               select new
                               {
                                   image = i.Image,
                                   name = i.Name,
                                   investorId = i.InvestorId,

                               }).ToList();
                double afinvcr = 0;
                double afinvde = 0;
                double pkinvcr = 0;
                double pkinvde = 0;
                double usinvcr = 0;
                double usinvde = 0;
                foreach (var item in investmnt)
                {
                    if (item.currencyId == 1)
                    {
                        afinvcr += item.credit;
                        afinvde += item.debit;
                    }
                    if (item.currencyId == 2)
                    {
                        pkinvcr += item.credit;
                        pkinvde += item.debit;
                    }
                    if (item.currencyId == 3)
                    {
                        usinvcr += item.credit;
                        usinvde += item.debit;
                    }
                }

                var afinvcredit = afinvcr - afinvde;

                var pkinvcredit = pkinvcr - pkinvde;

                var usinvcredit = usinvcr - usinvde;



                var afBenifit = (afdebit - afcredit - afghaniexpence) + (afghanisale - afghanisalePur);
                var pkBenifit = (pkdebit - pkcredit - rupeexpence) + (rupesale - rupesalePur);
                var usBenifit = (usdebit - uscredit - dollerexpence) + (dollersale - dollersalePur);
                var result = new
                {
                    investment = investmnt,
                    investor = investr,
                    afb = afBenifit,
                    pkb = pkBenifit,
                    usb = usBenifit,
                    afinvest = afinvcredit,
                    pkinvest = pkinvcredit,
                    usinvest = usinvcredit,
                    year = year,
                    month = month,
                    day = day,
                    paid = paidB,
                   
                };

                return Json(result);
            }
            return View("Error");
        }
        public async Task<IActionResult> AddBenifit(AllViewModel model)
        {
            var date = new DateTime(model.Benifit.year, model.Benifit.month, model.Benifit.day);
            BenifitPayment bp = new BenifitPayment
            {
                CurrencyId = model.Benifit.CurrencyId,
                InvestorId = model.Benifit.InvestorId,
                Description = model.Benifit.Description,
                MonthDate = date,
                Amount = model.Benifit.Amount,
                PaidDate = DateTime.Now.Date

            };
            db.BenifitPayment.Add(bp);
            await db.SaveChangesAsync();

            CashInHand ch = new CashInHand
            {
                Debit = model.Benifit.Amount,
                Credit = 0,
                Date = DateTime.Now.Date,
                Description = "د ګټې ورکړه",
                CurrencyId = model.Benifit.CurrencyId,
            };
            db.CashInHand.Add(ch);
            await db.SaveChangesAsync();

            return Json("1");
        }
        public JsonResult LoadPaidMoney(int invid)
        {
            var rec = (from p in db.BenifitPayment
                       where p.InvestorId == invid
                       select new
                       {
                           amount = p.Amount,
                           date = Convert.ToDateTime(p.PaidDate).ToShortDateString(),
                           currency = p.Currency.Currency1
                       }).ToList();
            return Json(rec);
        }
    }
}
