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
    public class ProcessController : Controller
    {
        ShawkanyDbContext db = new ShawkanyDbContext();
        private readonly UserManager<ApplicationUser> _userManager;

        public ProcessController(UserManager<ApplicationUser> userManager)
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
                beltyid = db.Pkbelty.Max(x => x.BeltyId);
            }
            catch (Exception)
            {

                beltyid = 0;
            }

            ViewBag.beltyid = beltyid + 1;

            return View();
        }

        public JsonResult FetchRawItems()
        {
            try
            {
                var result = (from i in db.Item2
                              join currenc in db.Currency on i.CurrencyId equals currenc.CurrencyId
                              join stoc in db.Stock on i.StockId equals stoc.StockId
                              where i.Quantity > 0
                              select new
                              {
                                  item2id = i.Item2Id,
                                  itemid = i.ItemId,
                                  item = i.Item.Name,
                                  qty = i.Quantity,
                                  price = i.PurchasePrice,
                                  currency = currenc.Currency1,
                                  stockid = i.StockId,
                                  stock = stoc.Stock1,
                              }).ToList().OrderByDescending(x => x.item2id).Distinct();

                return Json(result);
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }

        }

        [HttpPost]
        [Authorize(Roles = ("HR Department,Admin Department"))]
        public async Task<IActionResult> AddProcessedItems(string data, long item2id, double fromQuantity,DateTime date)
        {
            try
            {
                dynamic items = JsonConvert.DeserializeObject(data);

                var email = _userManager.GetUserAsync(User).Result.Email;
                var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

                var totalQuantity = 0.0;

                foreach (var item in items)
                {
                    int itemId = item.itemid;
                    int currencyId = item.currencyid;
                    int stockId = item.stockid;
                    int categoryId = item.categoryid;

                    double quantity = item.quantity;

                    double price = item.price;

                    string details = item.details;

                    totalQuantity += quantity;


                    Processed processed = new Processed
                    {
                        ItemId = itemId,
                        CategoryId = categoryId,
                        Unit = "ton",
                        CurrencyId = currencyId,
                        Price = price,
                        FromQty = fromQuantity,
                        StockId = stockId,
                        NewQty = quantity,
                        Date = date,
                        Item2Id = item2id,
                        Details = item.details
                    };

                    db.Processed.Add(processed);
                }

                var item2 = db.Item2.Find(item2id);

                item2.Quantity -= fromQuantity;

                db.Item2.Update(item2);

                double wastedItem = fromQuantity- totalQuantity ;

                var item2idwasted = item2.ItemId;

                var purPurchase = item2.PurchasePrice;

                var curr = item2.CurrencyId;

                if (wastedItem>0)
                {
                    WastedItems wastedItems = new WastedItems
                    {
                        ItemId = item2idwasted,
                        Quantity = wastedItem,
                        Unit = "ton",
                        Price = purPurchase,
                        Currency = curr,
                        Date = date,
                    };
                    db.WastedItems.Add(wastedItems);
                    await db.SaveChangesAsync();
                }

                await db.SaveChangesAsync();

                return Json("success");
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }
        }

        public JsonResult FetchProcessedItems()
        {
            try
            {
                var result = (from p in db.Processed
                              join item in db.Item on p.ItemId equals item.ItemId
                              join currenc in db.Currency on p.CurrencyId equals currenc.CurrencyId
                              join stoc in db.Stock on p.StockId equals stoc.StockId
                              select new
                              {
                                  processId = p.ProcessedId,
                                  item2id = p.Item2Id,
                                  itemid = p.ItemId,
                                  item = item.Name,
                                  category = p.Category.Category1,
                                  quantity = p.NewQty,
                                  fromqty = p.FromQty,
                                  price = p.Price,
                                  currency = currenc.Currency1,
                                  stock = stoc.Stock1,
                                  details = p.Details ?? "",
                                  date = Convert.ToDateTime(p.Date).ToShortDateString(),
                              }).ToList().OrderByDescending(x => x.processId).Distinct();

                return Json(result);
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }

        }

        public async Task<IActionResult> ReturnItems(ReturnItemsViewModel model, long ProcessedId, long Item2Idd)
        {
            try
            {
                var email = _userManager.GetUserAsync(User).Result.Email;
                var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

                var processed = db.Processed.Find(ProcessedId);

                if (processed.NewQty == model.ReturnQty)
                {
                    processed.NewQty = 0;

                    db.Processed.Update(processed);
                }
                else
                {
                    processed.NewQty -= model.ReturnQty;

                    db.Processed.Update(processed);
                }

                var item2 = db.Item2.Find(Item2Idd);

                item2.Quantity += model.ReturnQty;
                db.Item2.Update(item2);

                await db.SaveChangesAsync();
                return Json("تغیرات تر سره شو");
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }
        }

        public JsonResult FetchWastedItems()
        {
            try
            {
                var result = (from p in db.WastedItems
                              join item in db.Item on p.ItemId equals item.ItemId
                              select new
                              {
                                  id = p.WastedId,
                                  item = item.Name,
                                  quantity = p.Quantity,
                                  price = p.Price,
                                  currency =db.Currency.Where(x=>x.CurrencyId==p.Currency).Select(x=>x.Currency1).FirstOrDefault(),
                                  date = Convert.ToDateTime(p.Date).ToShortDateString(),
                              }).ToList().OrderByDescending(x => x.id).Distinct();

                return Json(result);
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }

        }

        public async Task<IActionResult> AddPKBelty(string data, string billinfo,DateTime date)
        {
            try
            {
                dynamic items = JsonConvert.DeserializeObject(data);

                dynamic info = JsonConvert.DeserializeObject(billinfo);

                var email = _userManager.GetUserAsync(User).Result.Email;
                var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

                var beltyid = 0;

                if (info != null && items!=null)
                {
                    var driver = info.driver;
                    var fname = info.fname;
                    var carno = info.carno;
                    var phone = info.phone;
                    var contract = info.contract;
                    var comission = info.comission;
                    var itemname = info.itemname;
                    var fill = info.fill;
                    var eigency = info.eigency;
                    var rent = info.rent;
                    var expense = info.expense;
                    var empty = info.empty;

                    Pkbelty belty = new Pkbelty
                    {
                        DriverName = driver,
                        Fname = fname,
                        CarNo = carno,
                        Phone = phone,
                        ContractName = contract,
                        ComissionerNo = comission,
                        ItemName = itemname,
                        FillPlace = fill,
                        Eigency = eigency,
                        RentPerTon = rent,
                        TotalExpense = expense,
                        EmptyPlace = empty,
                        Date = date
                    };

                    db.Pkbelty.Add(belty);
                    await db.SaveChangesAsync();

                    beltyid = belty.BeltyId;

                    foreach (var item in items)
                    {
                        int itemid = item.itemid;
                        int categoryid = item.categoryid;
                        double qty = item.qty;
                        double seek = item.seek;
                        string frommine = item.frommine;

                        BeltyItems bitems = new BeltyItems
                        {
                            ItemId = itemid,
                            CategoryId = categoryid,
                            Weight = qty,
                            SeekNo = seek,
                            PkbeltyId = beltyid,
                            FromMine = frommine,
                            Date = date,
                        };

                        db.BeltyItems.Add(bitems);
                        await db.SaveChangesAsync();
                    }

                    return Json("success");
                }
                return Json("Error");
            }
            catch (Exception e)
            {

                return Json("error", e.Message);
            }
        }

        public JsonResult LoadBelties()
        {
            var result = (from p in db.Pkbelty
                          select new
                          {
                              id = p.BeltyId,
                              name = p.DriverName,
                              fname = p.Fname,
                              carno = p.CarNo,
                              phone = p.Phone,
                              contract = p.ContractName,
                              comission = p.ComissionerNo,
                              itemname = p.ItemName,
                              fill = p.FillPlace,
                              empty = p.EmptyPlace,
                              eigency = p.Eigency,
                              rent = p.RentPerTon,
                              expense = p.TotalExpense,
                              date = Convert.ToDateTime(p.Date).ToShortDateString(),
                          }).ToList().OrderByDescending(x => x.id).Distinct();

            return Json(result);
        }

        public async Task<IActionResult> DeleteMineBelty(int recordId)
        {
            var rec = await db.Pkbelty.FindAsync(recordId);

            var items = db.BeltyItems.Where(x => x.PkbeltyId == recordId).ToList();

            if (rec != null && items != null)
            {
                try
                {

                    db.Pkbelty.Remove(rec);
                    db.BeltyItems.RemoveRange(items);

                    await db.SaveChangesAsync();

                    return Json("ریکارډ خذف شو");
                }
                catch (Exception e)
                {

                    return Json("error", e.Message);
                }
            }
            return Json("error");
        }

        public JsonResult BeltyItems (int id)
        {
            if (id != 0)
            {
                var result = (from p in db.BeltyItems where p.PkbeltyId == id
                              select new
                              {
                                  id = p.BeltyItemsId,
                                  mine = p.FromMine,
                                  item = db.Item.Where(x=>x.ItemId == p.ItemId).Select(x=>x.Name).FirstOrDefault(),
                                  category = db.Category.Where(x=>x.CategoryId == p.CategoryId).Select(x => x.Category1).FirstOrDefault(),
                                  qty = p.Weight,
                                  seek = p.SeekNo,
                                  date = Convert.ToDateTime(p.Date).ToShortDateString(),
                              }).ToList().OrderByDescending(x => x.id).Distinct();

                return Json(result);
            }
            return Json("Error");
        }
    }
}
