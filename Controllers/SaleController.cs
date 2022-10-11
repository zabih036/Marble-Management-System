using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShawkanyDb.Models;
using ShawkanyDb.Models.Model;
using ShawkanyDb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShokanayDb.Controllers
{
    public class SaleController : Controller
    {
        ShawkanyDbContext db = new ShawkanyDbContext();
        private readonly UserManager<ApplicationUser> _userManager;

        public SaleController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var items = (from d in db.Item
                         where d.Type == 2
                         select new SelectListItem()
                         {
                             Text = d.Name,
                             Value = d.ItemId.ToString()
                         }).ToList();
            ViewBag.items = items;

            var category = (from d in db.Category
                         select new SelectListItem()
                         {
                             Text = d.Category1,
                             Value = d.CategoryId.ToString()
                         }).ToList();
            ViewBag.category = category;

            var customers = (from d in db.Customer
                             select new SelectListItem()
                             {
                                 Text = d.Name,
                                 Value = d.CustomerId.ToString()
                             }).ToList();
            ViewBag.customers = customers;

            var currency = (from d in db.Currency
                            select new SelectListItem()
                            {
                                Text = d.Currency1,
                                Value = d.CurrencyId.ToString()
                            }).ToList();
            ViewBag.currency = currency;

            var stock = (from d in db.Stock
                         select new SelectListItem()
                         {
                             Text = d.Stock1,
                             Value = d.StockId.ToString()
                         }).ToList();
            ViewBag.stock = stock;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = ("HR Department,Admin Department"))]
        public async Task<IActionResult> AddSaledItems(string data, double debit, double credit, string details,DateTime date)
        {
            try
            {
                dynamic items = JsonConvert.DeserializeObject(data);

                var email = _userManager.GetUserAsync(User).Result.Email;
                var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

                int customerId = 0;
                int currencyId = 0;
                foreach (var item in items)
                {
                    long ProcessedId = item.processedid;
                    int itemid = item.itemid;
                    int categoryid = item.categoryid;
                    double SalePrice = item.saleprice;
                    double PurchasePrice = item.purprice;
                    currencyId = item.currencyid;
                    customerId = item.customerid;

                    double qty = item.qty;

                    string bill = item.bill;


                    var foundItem = db.Processed.Where(x => x.ProcessedId == ProcessedId).FirstOrDefault();

                    long processedId = 0;

                    if (foundItem == null || foundItem.NewQty<qty)
                    {
                        return Json("error");

                    }

                    foundItem.NewQty -= qty;
                    db.Entry(foundItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Entry(foundItem).Property(x => x.NewQty).IsModified = true;
                    processedId = foundItem.ProcessedId;

                    Sale sale = new Sale
                    {
                        BillNo = bill,
                        CustomerId = customerId,
                        CurrencyId = currencyId,
                        EmployeeId = empId,
                        ItemId = itemid,
                        Unit = "ton",
                        Quantity = qty,
                        SalePrice = SalePrice,
                        PurchasePrice = PurchasePrice,
                        StockId = foundItem.StockId,
                        SaleDate = date,
                        SaleState = null,
                        ProcessedId = processedId,
                        CategoryId = categoryid
                    };
                    db.Sale.Add(sale);
                }

                CustomerDeal deal = new CustomerDeal
                {
                    Debit = debit,
                    Credit = 0,
                    CustomerId = customerId,
                    Date = date,
                    CurrencyId = currencyId,
                    EmployeeId = empId,
                    Type = "خرڅول",
                    Loan = null,
                    Details = details,
                    Image = "/images/StaticImages/Admin.png"

                };
                db.CustomerDeal.Add(deal);
                await db.SaveChangesAsync();

                CashInHand ch = new CashInHand()
                {
                    Credit = 0,
                    Debit = 0,
                    CurrencyId = currencyId,
                    Date = date,
                    Description = "د اجناسو خرڅول",
                    CusDealId = deal.DealId
                };
                db.CashInHand.Add(ch);

                await db.SaveChangesAsync();

                return Json("success");
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }
        }

        public JsonResult FetchSaledItems()
        {
            try
            {
                var result = (from p in db.Sale
                              join item in db.Item on p.ItemId equals item.ItemId
                              join de in db.Customer on p.CustomerId equals de.CustomerId
                              join currenc in db.Currency on p.CurrencyId equals currenc.CurrencyId
                              join stoc in db.Stock on p.StockId equals stoc.StockId
                              join emp in db.Employee on p.EmployeeId equals emp.EmployeeId
                              select new
                              {
                                  processedid = p.ProcessedId,
                                  saleid = p.SaleId,
                                  billno = p.BillNo,
                                  itemid = p.ItemId,
                                  item = item.Name,
                                  category = p.Category.Category1,
                                  cusomerid = p.CustomerId,
                                  customer = de.Name,
                                  qty = p.Quantity,
                                  price = p.SalePrice,
                                  currencyid = p.CurrencyId,
                                  currency = currenc.Currency1,
                                  stockid = p.StockId,
                                  stock = stoc.Stock1,
                                  employeeid = p.EmployeeId,
                                  employee = emp.Name,
                                  p.SaleState,
                                  date = Convert.ToDateTime(p.SaleDate).ToShortDateString(),
                              }).Where(x => x.SaleState == null).ToList().OrderByDescending(x => x.saleid).Distinct();

                return Json(result);
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }

        }

        public JsonResult FetchItemInfo(int itemId, int categoryid)
        {
            var result = (from i in db.Item
                          join p in db.Processed on i.ItemId equals p.ItemId
                          where p.ItemId == itemId && p.CategoryId == categoryid && p.NewQty > 0
                          select new
                          {
                              itemid = i.ItemId,
                              processedId = p.ProcessedId,
                              itemname = i.Name,
                              category = p.Category.Category1,
                              quantity = p.NewQty,
                              price = p.Price,
                              currency = p.Currency.Currency1,
                              currencyid = p.CurrencyId,
                              stockid = p.StockId,
                              stock = p.Stock.Stock1,
                          }).ToList();

            return Json(result);
        }

        public async Task<IActionResult> ReturnItems(ReturnItemsViewModel model, long saleId, long ProcessedId)
        {
            try
            {
                var email = _userManager.GetUserAsync(User).Result.Email;
                var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

                var saled = db.Sale.Find(saleId);

                if (saled.Quantity == model.ReturnQty)
                {
                    saled.SaleState = "Returned";
                    saled.EmployeeId = empId;
                    saled.ReturnDate = DateTime.Now.Date;

                    db.Sale.Update(saled);
                }
                else
                {
                    Sale sale = new Sale
                    {
                        BillNo = saled.BillNo,
                        CustomerId = saled.CustomerId,
                        CurrencyId = saled.CurrencyId,
                        EmployeeId = empId,
                        ItemId = saled.ItemId,
                        Unit = saled.Unit,
                        Quantity = model.ReturnQty,
                        SalePrice = saled.SalePrice,
                        PurchasePrice = saled.PurchasePrice,
                        StockId = saled.StockId,
                        SaleState = "Returned",
                        ReturnDate = DateTime.Now.Date,
                        CategoryId = saled.CategoryId
                    };

                    db.Sale.Add(sale);

                    saled.EmployeeId = empId;
                    saled.Quantity -= model.ReturnQty;

                    db.Sale.Update(saled);
                }

                var processed = db.Processed.Find(ProcessedId);

                processed.NewQty += model.ReturnQty;
                db.Processed.Update(processed);

                await db.SaveChangesAsync();
                return Json("جنس واپس شو");
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }
        }

        [Authorize(Roles = "Admin Department")]
        public IActionResult ListOfReturnedSaled()
        {
            return View();
        }

        [Authorize(Roles = "Admin Department")]
        public JsonResult FetchSaledReturned()
        {
            var List = (from p in db.Sale.Where(x => x.SaleState == "Returned")
                        join item in db.Item on p.ItemId equals item.ItemId
                        join de in db.Customer on p.CustomerId equals de.CustomerId
                        join currenc in db.Currency on p.CurrencyId equals currenc.CurrencyId
                        join stoc in db.Stock on p.StockId equals stoc.StockId
                        join emp in db.Employee on p.EmployeeId equals emp.EmployeeId
                        select new
                        {
                            saleid = p.SaleId,
                            billno = p.BillNo,
                            itemid = p.ItemId,
                            item = item.Name,
                            category = p.Category.Category1,
                            dealer = de.Name,
                            price = p.SalePrice,
                            qty = p.Quantity,
                            currencyid = p.CurrencyId,
                            currency = currenc.Currency1,
                            stockid = p.StockId,
                            stock = stoc.Stock1,
                            employeeid = p.EmployeeId,
                            employee = emp.Name,
                            p.SaleState,
                            returnDate = Convert.ToDateTime(p.ReturnDate).ToShortDateString(),
                        }).ToList().OrderByDescending(x => x.saleid).Distinct();
            return Json(List);
        }

    }
}
