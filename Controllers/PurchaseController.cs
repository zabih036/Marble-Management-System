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
    public class PurchaseController : Controller
    {
        ShawkanyDbContext db = new ShawkanyDbContext();
        private readonly UserManager<ApplicationUser> _userManager;

        public PurchaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var items = (from d in db.Item
                     where d.Type ==1
                     select new SelectListItem()
                     {
                         Text = d.Name,
                         Value = d.ItemId.ToString()
                     }).ToList();
            ViewBag.items = items;

            var dealers = (from d in db.Dealer
                          select new SelectListItem()
                          {
                              Text = d.Name,
                              Value = d.DealerId.ToString()
                          }).ToList();
            ViewBag.dealers = dealers;

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

            long beltyid = 0;
            try
            {
                beltyid = db.MineBelty.Max(x => x.BeltyId);
            }
            catch (Exception)
            {

                beltyid = 0;
            }

            ViewBag.beltyid = beltyid + 1;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = ("HR Department,Admin Department"))]
        public async Task<IActionResult> AddPurchasedItems(string data, double credit, double debit, string details,DateTime purDate)
        {
            try
            {
                dynamic items = JsonConvert.DeserializeObject(data);

                var email = _userManager.GetUserAsync(User).Result.Email;
                var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

                var loan = "";
                if (debit == credit)
                {
                    loan = "NO";
                }
                else
                {
                    loan = null;
                }

                int dealerId = 0;
                int currencyId = 0;
                foreach (var item in items)
                {
                    int itemid = item.itemid;
                    currencyId = item.currencyId;
                    int stockId = item.stockId;
                    dealerId = item.dealerId;

                    double firstQty = item.firstQty;
                    double firstPrice = item.price;

                    double secondQty = item.secondQty;
                    double expense = item.expense;

                    double allPrice = firstPrice * firstQty;

                    double price = ((allPrice + expense) / secondQty);

                    string bill = item.bill;
                    int carNumber = item.carNumber;

                   

                    var foundItem = db.Item2.Where(x => x.ItemId == itemid && x.CurrencyId == currencyId && x.StockId == stockId && x.PurchasePrice == price).FirstOrDefault();

                    long item2id = 0;

                    if (foundItem != null)
                    {
                        foundItem.Quantity += secondQty;
                        db.Entry(foundItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.Entry(foundItem).Property(x => x.Quantity).IsModified = true;
                        item2id = foundItem.Item2Id;
                    }
                    else
                    {
                        Item2 newItem = new Item2
                        {
                            ItemId = itemid,
                            Unit = "ton",
                            CurrencyId = currencyId,
                            PurchasePrice = price,
                            Quantity = secondQty,
                            StockId = stockId
                        };
                        db.Item2.Add(newItem);

                        await db.SaveChangesAsync();

                        item2id = newItem.Item2Id;
                    }



                    Purchase purchase = new Purchase
                    {
                        BillNo = bill,
                        DealerId = dealerId,
                        CurrencyId = currencyId,
                        EmployeeId = empId,
                        ItemId = itemid,
                        Unit = "ton",
                        FirstQty = firstQty,
                        FirstPrice = firstPrice,
                        SecondQty = secondQty,
                        SecondPrice = price,
                        CarNumber = carNumber,
                        StockId = stockId,
                        Date =Convert.ToDateTime(purDate),
                        Status = null,
                        Item2Id = item2id
                    };
                    db.Purchase.Add(purchase);
                }

                DealerDeal deal = new DealerDeal
                {
                    Debit = 0,
                    Credit = credit,
                    DealerId = dealerId,
                    Date =Convert.ToDateTime(purDate),
                    CurrencyId = currencyId,
                    EmployeeId = empId,
                    Type = "اخستل",
                    Loan = loan,
                    Details = details,
                    Image = "/images/StaticImages/Admin.png"

                };
                db.DealerDeal.Add(deal);
                await db.SaveChangesAsync();

                CashInHand ch = new CashInHand()
                {
                    Credit = 0,
                    Debit = 0,
                    CurrencyId = currencyId,
                    Date =Convert.ToDateTime(purDate),
                    Description = "د اجناسو اخستل",
                    DelDealId = deal.DealId
                };
                db.CashInHand.Add(ch);

                await db.SaveChangesAsync();

                return Json("success");
            }
            catch (Exception e)
            {

                 return Json("error",e.Message);
            }
        }

        public JsonResult FetchPurchasedItems()
        {
            try
            {
                var result = (from p in db.Purchase
                              join item in db.Item on p.ItemId equals item.ItemId
                              join de in db.Dealer on p.DealerId equals de.DealerId
                              join currenc in db.Currency on p.CurrencyId equals currenc.CurrencyId
                              join stoc in db.Stock on p.StockId equals stoc.StockId
                              join emp in db.Employee on p.EmployeeId equals emp.EmployeeId
                              select new
                              {
                                  item2id = p.Item2Id,
                                  existqty = db.Item2.Where(x=>x.Item2Id == p.Item2Id).Select(x=>x.Quantity).FirstOrDefault(),
                                  purchaseid = p.PurchaseId,
                                  billno = p.BillNo,
                                  itemid = p.ItemId,
                                  item = item.Name,
                                  carNumber = p.CarNumber,
                                  dealerid = p.DealerId,
                                  dealer = de.Name,
                                  firstPrice = p.FirstPrice,
                                  firstQty = p.FirstQty,
                                  secondPrice = p.SecondPrice,
                                  secondQty = p.SecondQty,
                                  currencyid = p.CurrencyId,
                                  currency = currenc.Currency1,
                                  stockid = p.StockId,
                                  stock = stoc.Stock1,
                                  employeeid = p.EmployeeId,
                                  employee = emp.Name,
                                  p.Status,
                                  purchaseDate = Convert.ToDateTime(p.Date).ToShortDateString(),
                              }).Where(x => x.Status == null).ToList().OrderByDescending(x => x.purchaseid).Distinct();

                return Json(result);
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }

        }

        public async Task<IActionResult> ReturnItems(ReturnItemsViewModel model ,long purId, long Item2Id)
        {
            try
            {
                var email = _userManager.GetUserAsync(User).Result.Email;
                var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

                var purchased = db.Purchase.Find(purId);

                if(purchased.SecondQty == model.ReturnQty)
                {
                    purchased.Status = "Returned";
                    purchased.EmployeeId = empId;
                    purchased.ReturnedDate = DateTime.Now.Date;

                    db.Purchase.Update(purchased);
                }
                else
                {
                    Purchase purchase = new Purchase
                    {
                        BillNo = purchased.BillNo,
                        DealerId = purchased.DealerId,
                        CurrencyId = purchased.CurrencyId,
                        EmployeeId = empId,
                        ItemId = purchased.ItemId,
                        Unit = purchased.Unit,
                        SecondQty = model.ReturnQty,
                        SecondPrice = purchased.SecondPrice,
                        CarNumber = purchased.CarNumber,
                        StockId = purchased.StockId,
                        Status = "Returned",
                        ReturnedDate = DateTime.Now.Date,
                        Details = model.Details,
                    };

                    db.Purchase.Add(purchase);

                    purchased.EmployeeId = empId;
                    purchased.SecondQty  -= model.ReturnQty;
                    //purchased.FirstQty  -= model.ReturnQty;

                    db.Purchase.Update(purchased);
                }

                var item2 = db.Item2.Find(Item2Id);

                if (item2.Quantity == model.ReturnQty)
                {
                    item2.Quantity = 0;
                    db.Item2.Update(item2);
                }
                else
                {
                    item2.Quantity -= model.ReturnQty;
                    db.Item2.Update(item2);
                }
                await db.SaveChangesAsync();
                return Json("جنس واپس شو");
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }
        }

        [Authorize(Roles = "Admin Department")]
        public IActionResult ListOfReturnedPurchase()
        {
            return View();
        }

        [Authorize(Roles = "Admin Department")]
        public JsonResult FetchPurchaseReturned()
        {
            var List = (from p in db.Purchase.Where(x => x.Status == "Returned")
                        join item in db.Item on p.ItemId equals item.ItemId
                        join de in db.Dealer on p.DealerId equals de.DealerId
                        join currenc in db.Currency on p.CurrencyId equals currenc.CurrencyId
                        join stoc in db.Stock on p.StockId equals stoc.StockId
                        join emp in db.Employee on p.EmployeeId equals emp.EmployeeId
                        select new
                        {
                            item2id = p.Item2Id,
                            purchaseid = p.PurchaseId,
                            billno = p.BillNo,
                            itemid = p.ItemId,
                            item = item.Name,
                            carNumber = p.CarNumber,
                            dealerid = p.DealerId,
                            dealer = de.Name,
                            price = p.SecondPrice,
                            qty = p.SecondQty,
                            currencyid = p.CurrencyId,
                            currency = currenc.Currency1,
                            stockid = p.StockId,
                            stock = stoc.Stock1,
                            employeeid = p.EmployeeId,
                            employee = emp.Name,
                            p.Status,
                            desc = p.Details??"",
                            returnDate = Convert.ToDateTime(p.ReturnedDate).ToShortDateString(),
                        }).ToList().OrderByDescending(x => x.purchaseid).Distinct();
            return Json(List);
        }

        public JsonResult FetchContractedItems()
        {
            try
            {
                var result = (from p in db.Purchase
                              join item in db.Item on p.ItemId equals item.ItemId
                              join de in db.Dealer on p.DealerId equals de.DealerId
                              join currenc in db.Currency on p.CurrencyId equals currenc.CurrencyId
                              join stoc in db.Stock on p.StockId equals stoc.StockId
                              join emp in db.Employee on p.EmployeeId equals emp.EmployeeId
                              select new
                              {
                                  item2id = p.Item2Id,
                                  existqty = db.Item2.Where(x => x.Item2Id == p.Item2Id && x.PurchasePrice ==p.SecondPrice && x.CurrencyId == p.CurrencyId).Select(x => x.Quantity).FirstOrDefault(),
                                  purchaseid = p.PurchaseId,
                                  billno = p.BillNo,
                                  itemid = p.ItemId,
                                  item = item.Name,
                                  carNumber = p.CarNumber,
                                  dealerid = p.DealerId,
                                  dealer = de.Name,
                                  firstPrice = p.FirstPrice,
                                  firstQty = p.FirstQty,
                                  secondPrice = p.SecondPrice,
                                  secondQty = p.SecondQty,
                                  currencyid = p.CurrencyId,
                                  currency = currenc.Currency1,
                                  stockid = p.StockId,
                                  stock = stoc.Stock1,
                                  employeeid = p.EmployeeId,
                                  employee = emp.Name,
                                  p.Status,
                                  purchaseDate = Convert.ToDateTime(p.Date).ToShortDateString(),
                              }).Where(x => x.Status == null).ToList().OrderByDescending(x => x.purchaseid).Distinct();

                return Json(result);
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }

        }

        public async Task<IActionResult> AddBeltyInfo(BeltyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            if (model.BeltyId == 0)
            {
                MineBelty mine = new MineBelty
                {
                    DriverName = model.DriverName,
                    Fname = model.FName,
                    CarNo = model.CarNo,
                    Phone = model.Phone,
                    ContractName = model.ContractName,
                    MineName = model.MineName,
                    FillPlace = model.FillPlace,
                    EmptyPlace = model.EmptyPlace,
                    ItemOwner = model.ItemOwner,
                    Weight = model.Weight,
                    RentPerTon = model.RentPerTon,
                    TotalExpense = model.TotalExpense,
                    TotalRent = model.Weight * model.RentPerTon,
                    Paid = model.Paid,
                    Details = model.Details,
                    Date = model.Date.Date
                };

                db.MineBelty.Add(mine);

                await db.SaveChangesAsync();

                CashInHand cash = new CashInHand
                {
                    Debit = (model.Weight * model.RentPerTon) + model.TotalExpense,
                    Credit = 0,
                    CurrencyId = 1,
                    Date = model.Date.Date,
                    Description = "minebelty",
                    MineBeltyId = mine.BeltyId
                };

                db.CashInHand.Add(cash);
                await db.SaveChangesAsync();

                return Json("بیلټې معلومات ذخیره شو");
            }
            else
            {
                var rec = await db.MineBelty.FindAsync(model.BeltyId);
                var cash = db.CashInHand.Where(x => x.MineBeltyId == model.BeltyId).FirstOrDefault();

                rec.DriverName = model.DriverName;
                rec.Fname = model.FName;
                rec.Phone = model.Phone;
                rec.CarNo = model.CarNo;
                rec.ContractName = model.ContractName;
                rec.MineName = model.MineName;
                rec.FillPlace = model.FillPlace;
                rec.EmptyPlace = model.EmptyPlace;
                rec.ItemOwner = model.ItemOwner;
                rec.Weight = model.Weight;
                rec.RentPerTon = model.RentPerTon;
                rec.TotalExpense = model.TotalExpense;
                rec.Paid = model.Paid;
                rec.Details = model.Details;
                rec.Date = model.Date.Date;

                db.MineBelty.Update(rec);

                cash.Debit = (model.Weight * model.RentPerTon) + model.TotalExpense;
                db.CashInHand.Update(cash);

                await db.SaveChangesAsync();

                return Json("ریکارډ تغیر شو");

            }

        }

        public JsonResult LoadBelties()
        {
                var result = (from p in db.MineBelty
                              select new
                              {
                                  id = p.BeltyId,
                                  name = p.DriverName,
                                  fname = p.Fname,
                                  carno = p.CarNo,
                                  phone = p.Phone,
                                  contract = p.ContractName,
                                  mine = p.MineName,
                                  fill = p.FillPlace,
                                  empty = p.EmptyPlace,
                                  owner = p.ItemOwner,
                                  weight = p.Weight,
                                  rent = p.RentPerTon,
                                  expense = p.TotalExpense,
                                  paid = p.Paid,
                                  details = p.Details??"",
                                  date = Convert.ToDateTime(p.Date).ToShortDateString(),
                              }).ToList().OrderByDescending(x => x.id).Distinct();

                return Json(result);
        }


        public async Task<IActionResult> DeleteMineBelty(int recordId)
        {
            var rec = await db.MineBelty.FindAsync(recordId);

            var cash = db.CashInHand.Where(x => x.MineBeltyId == recordId).FirstOrDefault();

            if (rec != null && cash != null)
            {
                try
                {

                    db.MineBelty.Remove(rec);
                    db.CashInHand.Remove(cash);

                    await db.SaveChangesAsync();

                    return Json("Deleted");
                }
                catch (Exception e)
                {

                    return Json("error", e.Message);
                }
            }
            return Json("error");
        }

        public async Task<IActionResult> DriverPayement(int recordId, double newPayment)
        {
            var rec = await db.MineBelty.FindAsync(recordId);

            if (rec != null)
            {
                rec.Paid += newPayment;
                db.MineBelty.Update(rec);
                await db.SaveChangesAsync();
                return Json("Success");
            }
            return Json("Error");
        }

    }
}
