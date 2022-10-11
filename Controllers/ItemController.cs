using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ShawkanyDb.Models;
using ShawkanyDb.Models.Model;
using ShawkanyDb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShawkanyDb.Controllers
{

    public class ItemController : Controller
    {
        ShawkanyDbContext db = new ShawkanyDbContext();

        private readonly UserManager<ApplicationUser> _userManager;
        public ItemController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var role = _userManager.GetUsersInRoleAsync("Admin Department").Result;
            var user = role.FirstOrDefault();
            var email = user.Email;
            var employeeId = db.Employee.Where(d => d.Email == email).Select(r => r.EmployeeId).FirstOrDefault();
            var employees = (from d in db.Employee
                             where d.EmployeeId != employeeId
                             select new SelectListItem()
                             {
                                 Text = d.Name,
                                 Value = d.EmployeeId.ToString()
                             }).ToList();
            ViewBag.EmployeeId = employees;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin Department,Finance Department")]
        public async Task<IActionResult> ItemSave(ItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ItemId != 0)
                {
                    var rec = await db.Item.FindAsync(model.ItemId);
                    if (rec == null)
                    {
                        return PartialView("Error");
                    }


                    rec.ItemId = model.ItemId;
                    rec.Name = model.Name;
                    rec.Type = model.ItemType;

                    db.Item.Update(rec);

                    await db.SaveChangesAsync();

                    return Json("ریکارد تغیر شو");

                }
                else
                {
                    try
                    {
                        Item it = new Item()
                        {
                            Name = model.Name,
                            Type = model.ItemType,
                        };
                        db.Item.Add(it);

                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.Message);
                    }

                    return Json("ریکارډ ذخیره شو");
                }

            }
            return View("Error");

        }

        public IActionResult IsItemExist(ItemViewModel model)
        {

            if (model.ItemId != 0)
                return Json(true);
            var rec = db.Item.Any(d => d.Name == model.Name);

            if (!rec)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public JsonResult LoadItem()
        {
            var rec = (from i in db.Item
                       select new
                       {
                           i.ItemId,
                           i.Name,
                           typeId = i.Type,
                           type = i.Type == 1 ? "خام" : "پراسس",
                       }).ToList().OrderByDescending(d => d.ItemId);
            return new JsonResult(rec);
        }

        public async Task<IActionResult> DeleteItem(ItemViewModel model)
        {
            if (model.ItemId != 0)
            {
                var item = await db.Item.FindAsync(model.ItemId);
                if (item == null)
                {
                    return View("Error");
                }
                try
                {
                    db.Item.Remove(item);
                    await db.SaveChangesAsync();
                    return Json("ریکارډ حذف شو");
                }
                catch (Exception)
                {
                    return Json("تاسو نه شی کولی نوموړی ریکارد حذف کړی");
                }
            }
            return NotFound();
        }

        [Authorize(Roles = "Admin Department,Finance Department")]
        public async Task<IActionResult> SaveItemCategory(ItemCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CategoryId != 0)
                {
                    var rec = await db.Category.FindAsync(model.CategoryId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.Category1 = model.Category;


                    db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Entry(rec).Property(d => d.Category1).IsModified = true;

                    await db.SaveChangesAsync();

                    return Json("ریکارد تغیر شو");
                }
                else
                {
                    Category it = new Category()
                    {
                        Category1 = model.Category,

                    };
                    db.Category.Add(it);
                    await db.SaveChangesAsync();

                    return Json("ریکارد ذخیره شو");
                }

            }
            return View("Error");

        }

        [Authorize(Roles = "Admin Department,Finance Department")]
        public JsonResult LoadCategory()
        {
            var rec = db.Category.ToList().OrderByDescending(d => d.CategoryId);
            return Json(rec);
        }

        [Authorize(Roles = "Admin Department,Finance Department")]
        public async Task<IActionResult> DeleteCategory(ItemCategoryViewModel model)
        {
            if (model.CategoryId != 0)
            {
                var comp = await db.Category.FindAsync(model.CategoryId);
                if (comp == null)
                {
                    return View("Error");
                }
                try
                {
                    db.Category.Remove(comp);
                    await db.SaveChangesAsync();
                    return Json("ریکارډ حذف شو");
                }
                catch (Exception)
                {

                    return Json("تاسو نه شی کولی نوموړی ریکارد حذف کړی");
                }
            }
            return NotFound();
        }

        public IActionResult IsItemCategoryExist(ItemCategoryViewModel model)
        {

            if (model.CategoryId != 0)
                return Json(true);
            var rec = db.Category.Any(d => d.Category1 == model.Category);

            if (!rec)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public JsonResult LoadStock()
        {
            var rec = (from d in db.Stock
                       join c in db.Employee on d.Responsible equals c.EmployeeId
                       select new
                       {
                           stockId = d.StockId,
                           stock = d.Stock1,
                           employee = c.Name,
                           employeeId = c.EmployeeId,
                           location = d.Location,
                           details = d.Details,
                       }).ToList().OrderByDescending(d => d.stockId);
            return Json(rec);
        }

        public IActionResult IsStockExist(StockViewModel model)
        {
            if (model.StockId != 0)
            {
                return Json(true);
            }
            var rec = db.Stock.Any(d => d.Stock1 == model.Stock);

            if (!rec)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public async Task<IActionResult> SaveStock(StockViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.StockId != 0)
                {
                    var rec = await db.Stock.FindAsync(model.StockId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.Stock1 = model.Stock;
                    rec.Responsible = model.EmployeeId;
                    rec.Location = model.Location;
                    rec.Details = model.Details;


                    db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Entry(rec).Property(d => d.Stock1).IsModified = true;

                    await db.SaveChangesAsync();

                    return Json("د ګودام نوم تغیر شو.");
                }
                else
                {
                    Stock c = new Stock()
                    {
                        Stock1 = model.Stock,
                        Responsible = model.EmployeeId,
                        Location = model.Location,
                        Details = model.Details,

                    };
                    db.Stock.Add(c);
                    await db.SaveChangesAsync();
                    return Json("نوی ګودام ذخیره شو.");
                }
            }
            return View("Error");
        }

        [Authorize(Policy = ("DeleteRolePolicy"))]
        public async Task<IActionResult> DeleteStock(StockViewModel model)
        {
            if (model.StockId != 0)
            {
                var comp = await db.Stock.FindAsync(model.StockId);
                if (comp == null)
                {
                    return View("Error");
                }

                try
                {
                    db.Stock.Remove(comp);
                    await db.SaveChangesAsync();
                    return Json("ریکارډ حذف شو");
                }
                catch (Exception)
                {

                    return Json("تاسو نه شی کولی نوموړی ریکارد حذف کړی");
                }
            }
            return NotFound();

        }

        public IActionResult RawStock()
        {
            var stock = (from d in db.Stock
                         select new SelectListItem()
                         {
                             Text = d.Stock1,
                             Value = d.StockId.ToString()
                         }).ToList();
            ViewBag.stock = stock;
            return View();
        }

        public IActionResult ProcessedStock()
        {
            var stock = (from d in db.Stock
                         select new SelectListItem()
                         {
                             Text = d.Stock1,
                             Value = d.StockId.ToString()
                         }).ToList();
            ViewBag.stock = stock;
            return View();
        }

        public JsonResult FetchStockItems(int stockid)
        {
            var stock = (from i in db.Item
                         join i2 in db.Item2 on i.ItemId equals i2.ItemId
                         join curr in db.Currency on i2.CurrencyId equals curr.CurrencyId
                         where i2.Quantity > 0 && i2.StockId ==stockid
                         select new
                         {
                             itemid = i.ItemId,
                             itemname = i.Name,
                             currency = curr.Currency1,
                             item2id = i2.Item2Id,
                             purchaseprice = i2.PurchasePrice,
                             quantity = i2.Quantity,
                             currencyid = i2.CurrencyId,
                         }).ToList();

            return Json(stock);
        }

        public JsonResult FetchStockProcessedItems(int stockid)
        {
            var stock = (from i in db.Item
                         join i2 in db.Processed on i.ItemId equals i2.ItemId
                         join curr in db.Currency on i2.CurrencyId equals curr.CurrencyId
                         where i2.NewQty > 0 && i2.StockId == stockid
                         select new
                         {
                             itemid = i.ItemId,
                             itemname = i.Name,
                             category = i2.Category.Category1,
                             currency = curr.Currency1,
                             item2id = i2.Item2Id,
                             purchaseprice = i2.Price,
                             quantity = i2.NewQty,
                             currencyid = i2.CurrencyId,
                         }).ToList();

            return Json(stock);
        }

        public JsonResult FetchStockRawItems()
        {
            var stock = (from i in db.Item
                         join i2 in db.Item2 on i.ItemId equals i2.ItemId
                         join curr in db.Currency on i2.CurrencyId equals curr.CurrencyId
                         where i2.Quantity > 0 
                         select new
                         {
                             itemid = i.ItemId,
                             itemname = i.Name,
                             currency = curr.Currency1,
                             item2id = i2.Item2Id,
                             purchaseprice = i2.PurchasePrice,
                             quantity = i2.Quantity,
                             currencyid = i2.CurrencyId,
                         }).ToList();

            return Json(stock);
        }

        public JsonResult FetchStockProItems()
        {
            var stock = (from i in db.Item
                         join i2 in db.Processed on i.ItemId equals i2.ItemId
                         join curr in db.Currency on i2.CurrencyId equals curr.CurrencyId
                         where i2.NewQty > 0
                         select new
                         {
                             itemid = i.ItemId,
                             itemname = i.Name,
                             currency = curr.Currency1,
                             item2id = i2.Item2Id,
                             purchaseprice = i2.Price,
                             quantity = i2.NewQty,
                             currencyid = i2.CurrencyId,
                         }).ToList();

            return Json(stock);
        }

        public IActionResult FinancialReport()
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

        public JsonResult FetchCashInHandReport()
        {
            var rec = (from c in db.CashInHand
                       select new
                       {
                           debit = c.Debit,
                           credit = c.Credit,
                           currency = c.CurrencyId
                       }).ToList();
            return Json(rec);

        }

        [Authorize(Roles = "Admin Department")]
        public JsonResult FetchFinanceReport(int reportType)
        {
            var pur = db.Purchase.Where(x => x.PurchaseId > 0 && x.Status != "Returned").ToList();
            var customers = db.CustomerDeal.ToList();
            var C_D = db.CustomerDeal.ToList();
            var sherikat = db.DealerDeal.ToList();
            var dealers = db.DealerDeal.ToList();
            var sale = db.Sale.Where(x => x.SaleId > 0 && x.SaleState != "Returned");
            var expence = db.Expence.Where(x => x.ExpenceId > 0);
            var salary = db.SalaryPayment.Where(x => x.SalaryId > 0);
            switch (reportType)
            {
                //////////////////////////// todays report ///////////////
                case 1:
                    pur = db.Purchase.Where(x => x.Date == DateTime.Now.Date && x.Status != "Returned").ToList();
                    C_D = db.CustomerDeal.Where(x =>  x.Date == DateTime.Now.Date).ToList();
                    sherikat = db.DealerDeal.Where(x => x.Date == DateTime.Now.Date).ToList();
                    sale = db.Sale.Where(x => x.SaleDate == DateTime.Now.Date && x.SaleState != "Returned");
                    expence = db.Expence.Where(x => x.ExpenceDate == DateTime.Now.Date);
                    salary = db.SalaryPayment.Where(x => x.PaidDate == DateTime.Now.Date);
                    break;
                ///////////////////////// current month's report ///////////
                case 2:
                    var currentMonth = DateTime.Now.Month;
                    pur = db.Purchase.Where(x => x.Date.Value.Month == currentMonth && x.Date.Value.Year == DateTime.Now.Year && x.Status != "Returned").ToList();
                    C_D = db.CustomerDeal.Where(x => x.Date.Value.Month == currentMonth && x.Date.Value.Year == DateTime.Now.Year).ToList();
                    sherikat = db.DealerDeal.Where(x =>x.Date.Value.Month == currentMonth && x.Date.Value.Year == DateTime.Now.Year).ToList();
                    sale = db.Sale.Where(x => x.SaleDate.Value.Month == currentMonth && x.SaleDate.Value.Year == DateTime.Now.Year && x.SaleState != "Returned");
                    expence = db.Expence.Where(x => x.ExpenceDate.Value.Month == currentMonth && x.ExpenceDate.Value.Year == DateTime.Now.Year);
                    salary = db.SalaryPayment.Where(x => x.PaidDate.Value.Month == currentMonth && x.PaidDate.Value.Year == DateTime.Now.Year);
                    break;
                /////////////////////////   last month report ////////////////////////
                case 3:
                    Dictionary<string, int> dictWeeklySum = new Dictionary<string, int>();
                    pur = db.Purchase.Where(x => x.Date.Value.Month == (DateTime.Now.Month - 1) && x.Date.Value.Year == DateTime.Now.Year && x.Status != "Returned").ToList();
                    C_D = db.CustomerDeal.Where(x => x.Date.Value.Month == (DateTime.Now.Month - 1) && x.Date.Value.Year == DateTime.Now.Year).ToList();
                    sherikat = db.DealerDeal.Where(x =>  x.Date.Value.Month == (DateTime.Now.Month - 1) && x.Date.Value.Year == DateTime.Now.Year).ToList();
                    sale = db.Sale.Where(x => x.SaleDate.Value.Month == (DateTime.Now.Month - 1) && x.SaleDate.Value.Year == DateTime.Now.Year && x.SaleState != "Returned");
                    expence = db.Expence.Where(x => x.ExpenceDate.Value.Month == (DateTime.Now.Month - 1) && x.ExpenceDate.Value.Year == DateTime.Now.Year);
                    salary = db.SalaryPayment.Where(x => x.PaidDate.Value.Month == (DateTime.Now.Month - 1) && x.PaidDate.Value.Year == DateTime.Now.Year);
                    break;
                /////////////////////////  Current Year's report  ////////////
                case 4:
                    var currentYear = DateTime.Now.Year;

                    pur = db.Purchase.Where(x => x.Date.Value.Year == currentYear && x.Status != "Returned").ToList();
                    C_D = db.CustomerDeal.Where(x => x.Date.Value.Year == currentYear).ToList();
                    sherikat = db.DealerDeal.Where(x => x.Date.Value.Year == currentYear).ToList();
                    sale = db.Sale.Where(x => x.SaleDate.Value.Year == currentYear && x.SaleState != "Returned");
                    expence = db.Expence.Where(x => x.ExpenceDate.Value.Year == currentYear);
                    salary = db.SalaryPayment.Where(x => x.PaidDate.Value.Year == currentYear);
                    break;
                ////////////////////////   Last year's report ///////////////
                case 5:
                    pur = db.Purchase.Where(x => x.Date.Value.Year == (DateTime.Now.Year - 1) && x.Status != "Returned").ToList();
                    C_D = db.CustomerDeal.Where(x => x.Loan != "NO" && x.Date.Value.Year == (DateTime.Now.Year - 1)).ToList();
                    sherikat = db.DealerDeal.Where(x => x.Loan != "NO" && x.Date.Value.Year == (DateTime.Now.Year - 1)).ToList();
                    sale = db.Sale.Where(x => x.SaleDate.Value.Year == (DateTime.Now.Year - 1) && x.SaleState != "Returned");
                    expence = db.Expence.Where(x => x.ExpenceDate.Value.Year == (DateTime.Now.Year - 1));
                    salary = db.SalaryPayment.Where(x => x.PaidDate.Value.Year == (DateTime.Now.Year - 1));
                    break;
            }
            double afghanipur = 0, rupepur = 0, dollerpur = 0;
            double afghanisale = 0, rupesale = 0, dollersale = 0, afghanisalePur = 0, rupesalePur = 0, dollersalePur = 0;
            double afghanicredit = 0, rupecredit = 0, dollercredit = 0;
            double afghanidebit = 0, rupedebit = 0, dollerdebit = 0;
            double afghaniexpense = 0, rupeexpence = 0, dollerexpence = 0;
            double purExp = 0.0;

            ///////////////////////////// All purchase ////////////////
            foreach (var item in pur)
            {
                switch (item.CurrencyId)
                {
                    case 1:
                        afghanipur += Convert.ToDouble(item.SecondPrice * item.SecondQty);
                        break;
                    case 2:
                        rupepur += Convert.ToDouble(item.SecondPrice * item.SecondQty);
                        break;
                    case 3:
                        dollerpur += Convert.ToDouble(item.SecondPrice * item.SecondQty);
                        break;
                }
            }
            /////////////////////////// All Sales /////////////////
            foreach (var item in sale)
            {
                switch (item.CurrencyId)
                {
                    case 1:
                        afghanisale += (Convert.ToDouble((item.Quantity) * item.SalePrice)) ;
                        afghanisalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity));
                        break;
                    case 2:
                        rupesale += Convert.ToDouble((item.Quantity ) * item.SalePrice);
                        rupesalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity ));
                        break;
                    case 3:
                        dollersale += Convert.ToDouble((item.Quantity ) * item.SalePrice) ;
                        dollersalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity ));
                        break;
                }
            }
            ///////////////////////////// All Debit and credit  of dealers////// ////////////////
            double afdebit = 0.0;
            double pkdebit = 0.0;
            double usdebit = 0.0;
            double afcredit = 0.0;
            double pkcredit = 0.0;
            double uscredit = 0.0;
            foreach (var item in customers)
            {
                if (item.CurrencyId == 1)
                {
                    afcredit += item.Credit;
                    afdebit += item.Debit;
                }
                if (item.CurrencyId == 2)
                {
                    pkcredit += item.Credit;
                    pkdebit += item.Debit;
                }
                if (item.CurrencyId == 3)
                {
                    uscredit += item.Credit;
                    usdebit += item.Debit;
                }
            }

            ///////////////////////////// All Debit and credit  of customer////// ////////////////
            double afdebitc = 0.0;
            double pkdebitc = 0.0;
            double usdebitc = 0.0;
            double afcreditc = 0.0;
            double pkcreditc = 0.0;
            double uscreditc = 0.0;
            foreach (var item in dealers)
            {
                if (item.CurrencyId == 1)
                {
                    afcreditc += item.Credit;
                    afdebitc += item.Debit;
                }
                if (item.CurrencyId == 2)
                {
                    pkcreditc += item.Credit;
                    pkdebitc += item.Debit;
                }
                if (item.CurrencyId == 3)
                {
                    uscreditc += item.Credit;
                    usdebitc += item.Debit;
                }
            }
            //////////////////////  All expence ////////////////////////////
            foreach (var item in expence)
            {
                afghaniexpense += Convert.ToDouble(item.ExpenceAmount);
            }
            foreach (var item in salary)
            {
                afghaniexpense += Convert.ToDouble(item.PaidAmount);
            }

            /////////////dealer remain///////////
            var afonus = afcreditc - afdebitc;
            var pkonus = pkcreditc - pkdebitc;
            var usonus = uscreditc - usdebitc;

            /////////////dealer remain///////////
            var afoncus = afdebit - afcredit;
            var pkoncus = pkdebit - pkcredit;
            var usoncus = usdebit - uscredit;

            /////////////customer paid///////////
            var afpaid = 0.0;
            var pkpaid = 0.0;
            var uspaid = 0.0;
            foreach (var item in C_D)
            {
                if (item.CurrencyId == 1)
                {
                    afpaid += item.Credit;
                }
                if (item.CurrencyId == 2)
                {
                    pkpaid += item.Credit;
                }
                if (item.CurrencyId == 3)
                {
                    uspaid += item.Credit;
                }
            }

            /////////////paid to dealer ///////////
            var afpaidd = 0.0;
            var pkpaidd = 0.0;
            var uspaidd = 0.0;
            foreach (var item in sherikat)
            {
                if (item.CurrencyId == 1)
                {
                    afpaidd += item.Debit;
                }
                if (item.CurrencyId == 2)
                {
                    pkpaidd += item.Debit;
                }
                if (item.CurrencyId == 3)
                {
                    uspaidd += item.Debit;
                }
            }
            var result = new
            {

                afpaid,
                pkpaid,
                uspaid,

                afpaidd,
                pkpaidd,
                uspaidd,

                afonus,
                pkonus,
                usonus,

                afoncus,
                pkoncus,
                usoncus,

                afghaniPur = afghanipur,
                rupePur = rupepur,
                dollerPur = dollerpur,
                afghaniSale = afghanisale,
                rupeSale = rupesale,
                dollerSale = dollersale,
                afghaniSalepur = afghanisalePur,
                rupeSalepur = rupesalePur,
                dollerSalepur = dollersalePur,
                afghaniCredit = afghanicredit,
                rupeCredit = rupecredit,
                dollerCredit = dollercredit,
                afghaniDebit = afghanidebit,
                rupeDebit = rupedebit,
                dollerDebit = dollerdebit,
                afghaniExpence = afghaniexpense,
                rupeExpence = rupeexpence,
                dollerExpence = dollerexpence,
                purExpense = purExp
            };

            return Json(result);
        }

        [Authorize(Roles = "Admin Department")]
        public IActionResult ManualReprot(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                Dictionary<string, int> dictMonthlySum = new Dictionary<string, int>();

                var pur = db.Purchase.Where(x => x.Date >= model.manulReport.FromDate.Date && x.Date <=
                                             model.manulReport.ToDate.Date && x.Status != "Returned").ToList();
                var C_D = db.CustomerDeal.Where(x => x.Date >= model.manulReport.FromDate.Date && x.Date <=
                                             model.manulReport.ToDate.Date).ToList();
                var sherikat = db.DealerDeal.Where(x => x.Date >= model.manulReport.FromDate.Date && x.Date <=
                                            model.manulReport.ToDate.Date).ToList();
                var sale = db.Sale.Where(x => x.SaleDate >= model.manulReport.FromDate.Date && x.SaleDate <=
                                             model.manulReport.ToDate.Date && x.SaleState != "Returned");
                var expence = db.Expence.Where(x => x.ExpenceDate >= model.manulReport.FromDate.Date && x.ExpenceDate <=
                                             model.manulReport.ToDate.Date);
                var salary = db.SalaryPayment.Where(x => x.PaidDate >= model.manulReport.FromDate.Date && x.PaidDate <=
                                     model.manulReport.ToDate.Date);

                var customers = db.CustomerDeal.ToList();
                var dealers = db.DealerDeal.ToList();


                double afghanipur = 0, rupepur = 0, dollerpur = 0;
                double afghanisale = 0, rupesale = 0, dollersale = 0, afghanisalePur = 0, rupesalePur = 0, dollersalePur = 0;
                double afghanicredit = 0, rupecredit = 0, dollercredit = 0;
                double afghanidebit = 0, rupedebit = 0, dollerdebit = 0;
                double afghaniexpence = 0, rupeexpence = 0, dollerexpence = 0;
                double purExp = 0.0;

                ///////////////////////////// All purchase ////////////////
                foreach (var item in pur)
                {
                    switch (item.CurrencyId)
                    {
                        case 1:
                            afghanipur += Convert.ToDouble(item.SecondPrice * item.SecondQty);
                            break;
                        case 2:
                            rupepur += Convert.ToDouble(item.SecondPrice * item.SecondQty);
                            break;
                        case 3:
                            dollerpur += Convert.ToDouble(item.SecondPrice * item.SecondQty);
                            break;
                    }
                }
                /////////////////////////// All Sales /////////////////
                foreach (var item in sale)
                {
                    switch (item.CurrencyId)
                    {
                        case 1:
                            afghanisale += (Convert.ToDouble((item.Quantity) * item.SalePrice));
                            afghanisalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity));
                            break;
                        case 2:
                            rupesale += Convert.ToDouble((item.Quantity) * item.SalePrice);
                            rupesalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity));
                            break;
                        case 3:
                            dollersale += Convert.ToDouble((item.Quantity) * item.SalePrice);
                            dollersalePur += Convert.ToDouble(item.PurchasePrice * (item.Quantity));
                            break;
                    }
                }
                ///////////////////////////// All Debit and credit  of dealers////// ////////////////
                double afdebit = 0.0;
                double pkdebit = 0.0;
                double usdebit = 0.0;
                double afcredit = 0.0;
                double pkcredit = 0.0;
                double uscredit = 0.0;
                foreach (var item in customers)
                {
                    if (item.CurrencyId == 1)
                    {
                        afcredit += item.Credit;
                        afdebit += item.Debit;
                    }
                    if (item.CurrencyId == 2)
                    {
                        pkcredit += item.Credit;
                        pkdebit += item.Debit;
                    }
                    if (item.CurrencyId == 3)
                    {
                        uscredit += item.Credit;
                        usdebit += item.Debit;
                    }
                }

                ///////////////////////////// All Debit and credit  of customer////// ////////////////
                double afdebitc = 0.0;
                double pkdebitc = 0.0;
                double usdebitc = 0.0;
                double afcreditc = 0.0;
                double pkcreditc = 0.0;
                double uscreditc = 0.0;
                foreach (var item in dealers)
                {
                    if (item.CurrencyId == 1)
                    {
                        afcreditc += item.Credit;
                        afdebitc += item.Debit;
                    }
                    if (item.CurrencyId == 2)
                    {
                        pkcreditc += item.Credit;
                        pkdebitc += item.Debit;
                    }
                    if (item.CurrencyId == 3)
                    {
                        uscreditc += item.Credit;
                        usdebitc += item.Debit;
                    }
                }
                //////////////////////  All expence ////////////////////////////
                foreach (var item in expence)
                {
                    afghaniexpence += Convert.ToDouble(item.ExpenceAmount);
                }
                foreach (var item in salary)
                {
                    afghaniexpence += Convert.ToDouble(item.PaidAmount);
                }
                
                /////////////dealer remain///////////
                var afonus = afcreditc - afdebitc;
                var pkonus = pkcreditc - pkdebitc;
                var usonus = uscreditc - usdebitc;

                /////////////dealer remain///////////
                var afoncus = afdebit - afcredit;
                var pkoncus = pkdebit - pkcredit;
                var usoncus = usdebit - uscredit;

                /////////////customer paid///////////
                var afpaid = 0.0;
                var pkpaid = 0.0;
                var uspaid = 0.0;
                foreach (var item in C_D)
                {
                    if (item.CurrencyId == 1)
                    {
                        afpaid += item.Credit;
                    }
                    if (item.CurrencyId == 2)
                    {
                        pkpaid += item.Credit;
                    }
                    if (item.CurrencyId == 3)
                    {
                        uspaid += item.Credit;
                    }
                }

                /////////////paid to dealer ///////////
                var afpaidd = 0.0;
                var pkpaidd = 0.0;
                var uspaidd = 0.0;
                foreach (var item in sherikat)
                {
                    if (item.CurrencyId == 1)
                    {
                        afpaidd += item.Debit;
                    }
                    if (item.CurrencyId == 2)
                    {
                        pkpaidd += item.Debit;
                    }
                    if (item.CurrencyId == 3)
                    {
                        uspaidd += item.Debit;
                    }
                }
                var result = new
                {

                    afpaid,
                    pkpaid,
                    uspaid,

                    afpaidd,
                    pkpaidd,
                    uspaidd,

                    afonus,
                    pkonus,
                    usonus,

                    afoncus,
                    pkoncus,
                    usoncus,

                    afghaniPur = afghanipur,
                    rupePur = rupepur,
                    dollerPur = dollerpur,
                    afghaniSale = afghanisale,
                    rupeSale = rupesale,
                    dollerSale = dollersale,
                    afghaniSalepur = afghanisalePur,
                    rupeSalepur = rupesalePur,
                    dollerSalepur = dollersalePur,
                    afghaniCredit = afghanicredit,
                    rupeCredit = rupecredit,
                    dollerCredit = dollercredit,
                    afghaniDebit = afghanidebit,
                    rupeDebit = rupedebit,
                    dollerDebit = dollerdebit,
                    rupeExpence = rupeexpence,
                    dollerExpence = dollerexpence,
                    purExpense = purExp
                };

                return Json(result);
            }
            return View("Error");
        }

       
        [Authorize(Roles = "Admin Department")]
        public IActionResult ExistItemsReport()
        {
            return View();
        }

        public async Task<IActionResult> MoveToAnotherStock(long item2idstock, int quantitystock, int oldstock, int newstock)
        {
            var item2 = db.Item2.Where(x => x.Item2Id == item2idstock && x.StockId == oldstock).FirstOrDefault();
            if (item2 != null)
            {
                Item2 it = new Item2
                {
                    ItemId = item2.ItemId,
                    CurrencyId = item2.CurrencyId,
                    PurchasePrice = item2.PurchasePrice,
                    Quantity =  quantitystock,
                    StockId = newstock,
                };
                db.Item2.Add(it);

                item2.Quantity -=  quantitystock;
                db.Item2.Update(item2);
                await db.SaveChangesAsync();
                return Json("جنس بل ګودام ته انتقال شو");
            }
            return Json("error");
        }


        public async Task<IActionResult> MoveToAnotherStockProcessed(long item2idstock, int quantitystock, int oldstock, int newstock)
        {
            var item2 = db.Processed.Where(x => x.Item2Id == item2idstock && x.StockId == oldstock).FirstOrDefault();
            if (item2 != null)
            {
                Processed it = new Processed
                {
                    ItemId = item2.ItemId,
                    CurrencyId = item2.CurrencyId,
                    Price = item2.Price,
                    NewQty = quantitystock,
                    StockId = newstock,
                    CategoryId = item2.CategoryId
                };
                db.Processed.Add(it);

                item2.NewQty -= quantitystock;
                db.Processed.Update(item2);
                await db.SaveChangesAsync();
                return Json("جنس بل ګودام ته انتقال شو");
            }
            return Json("error");
        }
    }

}