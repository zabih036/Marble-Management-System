
using Microsoft.AspNetCore.Authorization;
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
using static System.Net.Mime.MediaTypeNames;

namespace ShawkanyDb.Controllers
{
    [Authorize(Roles = "Finance Department,Admin Department")]
    public class DealController : Controller
    {
        ShawkanyDbContext db = new ShawkanyDbContext();
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public DealController(UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {

            if (TempData["updated"] != null)
            {
                ViewBag.Alert = TempData["updated"];
            }
            else
            {
                ViewBag.Alert = "empty";
            }


            var dealers = (from d in db.Dealer
                           select new SelectListItem()
                           {
                               Text = d.Name + " / " + d.Fname,
                               Value = d.DealerId.ToString()
                           }).ToList();
            ViewBag.dealers = dealers;

            var customers = (from d in db.Customer
                             select new SelectListItem()
                             {
                                 Text = d.Name + " / " + d.Fname,
                                 Value = d.CustomerId.ToString()
                             }).ToList();
            ViewBag.customers = customers;


            var expenseType = (from d in db.ExpenceType
                               select new SelectListItem()
                               {
                                   Text = d.ExpenceType1,
                                   Value = d.ExpenceTypeId.ToString()
                               }).ToList();
            ViewBag.expenseType = expenseType;

            var items = (from d in db.Item
                         where d.Type == 1
                         select new SelectListItem()
                         {
                             Text = d.Name,
                             Value = d.ItemId.ToString()
                         }).ToList();
            ViewBag.items = items;

            var stocks = (from d in db.Stock
                          select new SelectListItem()
                          {
                              Text = d.Stock1,
                              Value = d.StockId.ToString()
                          }).ToList();
            ViewBag.stocks = stocks;

            long dealerdealid = 0;
            try
            {
                dealerdealid = db.DealerDeal.Max(x => x.DealId);
            }
            catch (Exception)
            {

                dealerdealid = 0;
            }

            ViewBag.dealerdealid = dealerdealid + 1;


            long customerdealid = 0;
            try
            {
                customerdealid = db.CustomerDeal.Max(x => x.DealId);
            }
            catch (Exception)
            {

                customerdealid = 0;
            }

            ViewBag.customerdealid = customerdealid + 1;

            return View();
        }

        public async Task<IActionResult> AddDealInfo(DealViewModel model, [FromForm] IFormFile image)
        {
            var empId = userManager.GetUserAsync(User).Result.EmpId;

            if (ModelState.IsValid)
            {
                try
                {
                    string upload = "";
                    string uploadPath = "";
                    string path = "";

                    double cr = 0.0;
                    double deb = 0.0;


                    if (model.CustomerId == 0)
                    {
                        path = "Images/DealerImage/";
                    }
                    else
                    {
                        path = "Images/CustomerImage/";
                    }

                    if ((model.camera == null || model.camera == "" || model.camera == "0") && image != null)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace("_", "") + Path.GetExtension(image.FileName);
                        upload = Path.Combine(path, fileName);
                        uploadPath = Path.Combine(hostingEnvironment.WebRootPath, upload);
                        var file = new FileStream(uploadPath, FileMode.Create);
                        image.CopyTo(file) ;
                        file.Close();
                        upload = "/" + upload;
                    }
                    else
                    {
                        byte[] bitmap = Convert.FromBase64String(model.camera.Split(',')[1]);
                        var fileName = Guid.NewGuid().ToString().Replace("_", "") + ".jpeg";
                        string uploads = Path.Combine(path, fileName);
                        uploadPath = Path.Combine(hostingEnvironment.WebRootPath, uploads);
                        System.IO.File.WriteAllBytes(uploadPath, bitmap);
                        upload = "/" + uploads;

                    }

                    if (model.Type == "Debit")
                    {
                        deb = model.Amount;
                    }
                    else
                    {
                        cr = model.Amount;
                    }

                    if (model.CustomerId == 0)
                    {
                        DealerDeal deal = new DealerDeal
                        {
                            Debit = deb,
                            Credit = cr,
                            CurrencyId = model.CurrencyId,
                            EmployeeId = empId,
                            DealerId = model.DealerId,
                            Date = model.Date,
                            Details = model.Detail,
                            Image = upload
                        };

                        db.DealerDeal.Add(deal);
                        await db.SaveChangesAsync();

                        CashInHand cash = new CashInHand
                        {
                            Debit = deb,
                            Credit = cr,
                            Date = model.Date,
                            CurrencyId = model.CurrencyId,
                            DelDealId = deal.DealId,
                        };

                        db.CashInHand.Add(cash);
                        await db.SaveChangesAsync();


                        TempData["updated"] = "1";
                        return RedirectToAction("Index", "Deal");
                    }
                    else
                    {
                        CustomerDeal deal = new CustomerDeal
                        {
                            Debit = deb,
                            Credit = cr,
                            CurrencyId = model.CurrencyId,
                            EmployeeId = empId,
                            CustomerId = model.CustomerId,
                            Date = model.Date,
                            Details = model.Detail,
                            Image = upload
                        };

                        db.CustomerDeal.Add(deal);
                        await db.SaveChangesAsync();

                        CashInHand cash = new CashInHand
                        {
                            Debit = deb,
                            Credit = cr,
                            Date = model.Date,
                            CurrencyId = model.CurrencyId,
                            CusDealId = deal.DealId,
                        };

                        db.CashInHand.Add(cash);
                        await db.SaveChangesAsync();


                        TempData["updated"] = "1";
                        return RedirectToAction("Index", "Deal");
                    }
                }
                catch (Exception)
                {

                    TempData["updated"] = "2";
                    return RedirectToAction("Index", "Deal");
                }

            }

            TempData["updated"] = "2";
            return RedirectToAction("Index", "Deal");

        }

        public async Task<IActionResult> SaveExpense(ExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                Expence c = new Expence()
                {
                    ExpenceAmount = model.Expense,
                    ExpenceTypeId = model.ExpenseTypeId,
                    ExpenceDate = model.ExpenseDate,
                    Detail = model.Detail
                };

                db.Expence.Add(c);
                await db.SaveChangesAsync();

                CashInHand ch = new CashInHand()
                {
                    Debit = model.Expense,
                    Credit = 0,
                    Date = model.ExpenseDate,
                    CurrencyId = 1,
                    Description = "expense",
                    ExpenseId = c.ExpenceId
                };
                db.CashInHand.Add(ch);

                await db.SaveChangesAsync();
                return Json(" مصرف ریکارډ ذخیره شو.");
            }

            return View("Error");
        }

        public JsonResult LoadExpense()
        {
            var rec = (from i in db.Expence
                       select new
                       {
                           expenceId = i.ExpenceId,
                           expenceAmount = i.ExpenceAmount,
                           expenceType = i.ExpenceType.ExpenceType1,
                           expenceDateShow = Convert.ToDateTime(i.ExpenceDate).ToShortDateString(),
                           detail = i.Detail ?? "",
                           expenceTypeId = i.ExpenceTypeId
                       }).ToList().OrderByDescending(d => d.expenceId);
            return Json(rec);
        }

        /// dealer /// 
        public IActionResult DealerDeal()
        {
            return View();
        }

        public JsonResult FetchDealers()
        {
            var dealers = (from cus in db.Dealer
                           select new
                           {
                               dealerid = cus.DealerId,
                               name = cus.Name,
                               fname = cus.Fname,
                               address = cus.Address,
                               phone = cus.Phone,
                           }).ToList().OrderByDescending(x => x.dealerid);
            return Json(dealers);
        }

        public JsonResult DealerDealInfo(int dealerId)
        {
            var DealInfo = (from d in db.DealerDeal
                            where d.DealerId == dealerId
                            join cu in db.Currency on d.CurrencyId equals cu.CurrencyId
                            join e in db.Employee on d.EmployeeId equals e.EmployeeId
                            select new
                            {
                                dealerid = dealerId,
                                dealid = d.DealId,
                                name = d.Dealer.Name,
                                fname = d.Dealer.Fname,
                                phone = d.Dealer.Phone,
                                image = d.Image,
                                debit = d.Debit,
                                credit = d.Credit,
                                currencyid = d.CurrencyId,
                                currency = cu.Currency1,
                                employeeid = d.EmployeeId,
                                employee = e.Name,
                                date = Convert.ToDateTime(d.Date).ToShortDateString(),
                                details = d.Details ?? "",
                            }).ToList().OrderByDescending(r => r.dealid);

            return Json(DealInfo);
        }

        public JsonResult DealersDealInfo()
        {
            var DealInfo = (from d in db.DealerDeal
                            join cu in db.Currency on d.CurrencyId equals cu.CurrencyId
                            join e in db.Employee on d.EmployeeId equals e.EmployeeId
                            select new
                            {
                                dealerid = d.DealerId,
                                dealid = d.DealId,
                                name = d.Dealer.Name,
                                fname = d.Dealer.Fname,
                                phone = d.Dealer.Phone,
                                image = d.Image,
                                debit = d.Debit,
                                credit = d.Credit,
                                currencyid = d.CurrencyId,
                                currency = cu.Currency1,
                                employeeid = d.EmployeeId,
                                employee = e.Name,
                                date = Convert.ToDateTime(d.Date).ToShortDateString(),
                                details = d.Details ?? "",
                            }).ToList().OrderByDescending(r => r.dealid);

            return Json(DealInfo);
        }

        public async Task<IActionResult> DeleteDealerRecord(long recordId, string imagePath)
        {
            var rec = await db.DealerDeal.FindAsync(recordId);

            var cash = db.CashInHand.Where(x => x.DelDealId == recordId).FirstOrDefault();

            if (rec != null && cash != null)
            {
                try
                {
                    var filename = imagePath.Split('/');
                    var imgpath = Path.Combine(hostingEnvironment.WebRootPath, "images/DealerImage", filename[3]);

                    if (imagePath != "/images/StaticImages/Admin.png")
                    {
                        if (System.IO.File.Exists(imgpath))
                        {
                            using (FileStream file = System.IO.File.Open(imgpath, FileMode.Open, FileAccess.Read, FileShare.None))
                            {
                                file.Close();
                                await file.DisposeAsync();
                            };

                            System.IO.File.Delete(imgpath);
                        }
                    }

                    db.DealerDeal.Remove(rec);
                    db.CashInHand.Remove(cash);

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

        public IActionResult RemainFromDealerList()
        {
            return View();
        }

        public JsonResult TotalRemainOnUs()
        {
            var af = (from d in db.DealerDeal
                      where d.CurrencyId == 1
                      select new
                      {
                          name = d.Dealer.Name,
                          fname = d.Dealer.Fname,
                          province = d.Dealer.Address,
                          phone = d.Dealer.Phone,
                          debit = d.Debit,
                          credit = d.Credit,
                      }).ToList();
            var pk = (from d in db.DealerDeal
                      where d.CurrencyId == 2
                      select new
                      {
                          name = d.Dealer.Name,
                          fname = d.Dealer.Fname,
                          province = d.Dealer.Address,
                          phone = d.Dealer.Phone,
                          debit = d.Debit,
                          credit = d.Credit,
                      }).ToList();
            var us = (from d in db.DealerDeal
                      where d.CurrencyId == 3
                      select new
                      {
                          name = d.Dealer.Name,
                          fname = d.Dealer.Fname,
                          province = d.Dealer.Address,
                          phone = d.Dealer.Phone,
                          debit = d.Debit,
                          credit = d.Credit,
                      }).ToList();
            var rec = new
            {
                af,
                pk,
                us,
            };
            return Json(rec);
        }

        /// customers ///
        public IActionResult CustomerDeal()
        {
            return View();
        }

        public JsonResult FetchCustomers()
        {
            var customers = (from cus in db.Customer
                             select new
                             {
                                 customerid = cus.CustomerId,
                                 name = cus.Name,
                                 fname = cus.Fname,
                                 address = cus.Address,
                                 phone = cus.Phone,
                             }).ToList().OrderByDescending(x => x.customerid);
            return Json(customers);
        }

        public JsonResult CustomerDealInfo(int customerId)
        {
            var DealInfo = (from d in db.CustomerDeal
                            where d.CustomerId == customerId
                            join cu in db.Currency on d.CurrencyId equals cu.CurrencyId
                            join e in db.Employee on d.EmployeeId equals e.EmployeeId
                            select new
                            {
                                customerid = customerId,
                                dealid = d.DealId,
                                name = d.Customer.Name,
                                fname = d.Customer.Fname,
                                phone = d.Customer.Phone,
                                image = d.Image,
                                debit = d.Debit,
                                credit = d.Credit,
                                currencyid = d.CurrencyId,
                                currency = cu.Currency1,
                                dealerid = d.CustomerId,
                                employeeid = d.EmployeeId,
                                employee = e.Name,
                                date = Convert.ToDateTime(d.Date).ToShortDateString(),
                                details = d.Details ?? "",
                            }).ToList().OrderByDescending(r => r.dealid);

            return Json(DealInfo);
        }

        public JsonResult CustomersDealInfo()
        {
            var DealInfo = (from d in db.CustomerDeal
                            join cu in db.Currency on d.CurrencyId equals cu.CurrencyId
                            join e in db.Employee on d.EmployeeId equals e.EmployeeId
                            select new
                            {
                                customerid = d.CustomerId,
                                dealid = d.DealId,
                                name = d.Customer.Name,
                                fname = d.Customer.Fname,
                                phone = d.Customer.Phone,
                                image = d.Image,
                                debit = d.Debit,
                                credit = d.Credit,
                                currencyid = d.CurrencyId,
                                currency = cu.Currency1,
                                dealerid = d.CustomerId,
                                employeeid = d.EmployeeId,
                                employee = e.Name,
                                date = Convert.ToDateTime(d.Date).ToShortDateString(),
                                details = d.Details ?? "",
                            }).ToList().OrderByDescending(r => r.dealid);

            return Json(DealInfo);
        }

        public async Task<IActionResult> DeleteRecord(long recordId, string imagePath)
        {
            var rec = await db.CustomerDeal.FindAsync(recordId);
            var cash = db.CashInHand.Where(x => x.CusDealId == recordId).FirstOrDefault();

            if (rec != null && cash != null)
            {
                try
                {
                    var filename = imagePath.Split('/');
                    var imgpath = Path.Combine(hostingEnvironment.WebRootPath, "images/CustomerImage", filename[3]);

                    if (imagePath != "/images/StaticImages/Admin.png")
                    {
                        if (System.IO.File.Exists(imgpath))
                        {
                            using (FileStream file = System.IO.File.Open(imgpath, FileMode.Open, FileAccess.Read, FileShare.None))
                            {
                                file.Close();
                                await file.DisposeAsync();
                            };

                            System.IO.File.Delete(imgpath);
                        }
                    }
                    db.CustomerDeal.Remove(rec);
                    db.CashInHand.Remove(cash);

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

        public IActionResult RemainOnCustomerList()
        {
            return View();
        }

        public JsonResult TotalRemainOnCustomer()
        {
            var af = (from d in db.CustomerDeal
                      where d.CurrencyId == 1
                      select new
                      {
                          name = d.Customer.Name,
                          fname = d.Customer.Fname,
                          province = d.Customer.Address,
                          phone = d.Customer.Address,
                          debit = d.Debit,
                          credit = d.Credit,
                      }).ToList();
            var pk = (from d in db.CustomerDeal
                      where d.CurrencyId == 2
                      select new
                      {
                          name = d.Customer.Name,
                          fname = d.Customer.Fname,
                          province = d.Customer.Address,
                          phone = d.Customer.Address,
                          debit = d.Debit,
                          credit = d.Credit,
                      }).ToList();
            var us = (from d in db.CustomerDeal
                      where d.CurrencyId == 3
                      select new
                      {
                          name = d.Customer.Name,
                          fname = d.Customer.Fname,
                          province = d.Customer.Address,
                          phone = d.Customer.Address,
                          debit = d.Debit,
                          credit = d.Credit,
                      }).ToList();
            var rec = new
            {
                af,
                pk,
                us,
            };
            return Json(rec);
        }


        /// DTRI ///

        public async Task<IActionResult> AddDTRIDealData(DTRIViewModel model)
        {
            if (ModelState.IsValid)
            {
                double cr = 0.0;
                double deb = 0.0;
                var empId = userManager.GetUserAsync(User).Result.EmpId;

                if (model.Paid != "Paid")
                {

                    if (model.DtriType == "Credit")
                    {
                        cr = model.Qty;
                    }
                    else
                    {
                        deb = model.Qty;
                    }

                    var item2 = db.Item2.Where(x => x.ItemId == model.ItemId
                    && x.PurchasePrice == model.Price
                    && x.StockId == model.StockId
                    && x.CurrencyId == model.DCurrencyId).FirstOrDefault();

                    long item2id = 0;
                    if (item2 != null)
                    {
                        if (model.DtriType == "Debit")
                        {
                            item2.Quantity -= model.Qty;
                        }
                        else
                        {
                            item2.Quantity += model.Qty;
                        }

                        item2id = item2.Item2Id;

                        db.Item2.Update(item2);

                    }
                    else
                    {
                        Item2 item = new Item2
                        {
                            ItemId = model.ItemId,
                            Unit = "ton",
                            CurrencyId = model.DCurrencyId,
                            PurchasePrice = model.Price,
                            Quantity = model.Qty,
                            StockId = model.StockId
                        };

                        db.Item2.Add(item);

                        await db.SaveChangesAsync();

                        item2id = item.Item2Id;

                    }
                    Dtri dtri = new Dtri
                    {
                        DealerId = model.DealersId,
                        ItemId = model.ItemId,
                        CarNumber = model.CarNumber,
                        CurrencyId = model.DCurrencyId,
                        ItemCredit = cr,
                        ItemDebit = deb,
                        Price = model.Price,
                        Amount = model.DtriAmount,
                        Date = model.DtDate,
                        Details = model.Detail,
                        Item2Id = item2id,
                        EmpId = empId,
                    };

                    db.Dtri.Add(dtri);

                    await db.SaveChangesAsync();
                }
                else
                {
                    try
                    {

                        Dtri dtri = new Dtri
                        {
                            DealerId = model.DealersId,
                            ItemId = 0,
                            CarNumber = 0,
                            ItemCredit = cr,
                            ItemDebit = deb,
                            Price = 0,
                            CurrencyId = model.DCurrencyId,
                            Amount = model.DtriAmount,
                            Date = model.DtDate,
                            Details = model.Detail,
                            Item2Id = 0,
                            EmpId = empId,
                            QtyMoney = model.Qty,
                        };

                        db.Dtri.Add(dtri);
                        await db.SaveChangesAsync();

                        double credit = 0;
                        double debit = 0;
                        if (model.DtriType == "Debit")
                        {
                            debit = model.DtriAmount;
                        }
                        else
                        {
                            credit = model.DtriAmount;
                        }

                        CashInHand cash = new CashInHand
                        {
                            Credit = credit,
                            Debit = debit,
                            CurrencyId = model.DCurrencyId,
                            Date = model.DtDate,
                            DtriId = dtri.DtriId,
                        };

                        db.CashInHand.Add(cash);
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {

                        return Json("error", e);
                    }
                }




                return Json("معلومات ذخیره شو");

            }
            return View("error");
        }

        public JsonResult FetchDTRIData()
        {
            var dtri = (from d in db.Dtri
                        select new
                        {
                            dtriid = d.DtriId,
                            dealerid = d.DealerId,
                            name = d.Dealer.Name,
                            item = d.ItemId != 0 ? db.Item.Where(x => x.ItemId == d.ItemId).Select(x => x.Name).FirstOrDefault() : "",
                            itemid = d.ItemId,
                            car = d.CarNumber,
                            credit = d.ItemCredit,
                            debit = d.ItemDebit,
                            price = d.Price,
                            date = Convert.ToDateTime(d.Date).ToShortDateString(),
                            details = d.Details ?? "",
                            currency = d.CurrencyId != 0 ? db.Currency.Where(x => x.CurrencyId == d.CurrencyId).Select(x => x.Currency1).FirstOrDefault() : "",
                            currencyid = d.CurrencyId,
                            item2id = d.Item2Id == 0 ? 0 : d.Item2Id,
                            amount = d.Amount,
                        }).ToList().OrderByDescending(x => x.dtriid);
            return Json(dtri);
        }

        public JsonResult FetchItemInfo(int itemId)
        {
            var result = (from i in db.Item
                          join i2 in db.Item2 on i.ItemId equals i2.ItemId
                          where i2.ItemId == itemId && i2.Quantity > 0
                          select new
                          {
                              itemid = i.ItemId,
                              item2id = i2.Item2Id,
                              itemname = i.Name,
                              quantity = i2.Quantity,
                              price = i2.PurchasePrice,
                              currency = i2.Currency.Currency1,
                              currencyid = i2.CurrencyId,
                              stockid = i2.StockId,
                              stock = i2.Stock.Stock1,
                          }).ToList();

            return Json(result);
        }

        public async Task<IActionResult> DeleteDTRIRecord(long recordId, long item2Id, double amount)
        {
            try
            {

                var rec = db.Dtri.Where(x => x.DtriId == recordId).FirstOrDefault();
                if (amount > 0 && rec != null)
                {

                    var cash = db.CashInHand.Where(x => x.DtriId == rec.DtriId).FirstOrDefault();

                    if (cash != null)
                    {
                        db.Dtri.Remove(rec);
                        db.CashInHand.Remove(cash);

                        await db.SaveChangesAsync();

                        return Json("ریکارډ حذف شو");
                    }

                    return Json("error");
                }
                else
                {
                    var item2 = await db.Item2.FindAsync(item2Id);
                    if (rec != null && item2 != null)
                    {
                        double qty = 0;
                        if (rec.ItemCredit != 0)
                        {
                            qty = (double)rec.ItemCredit;
                        }
                        else
                        {
                            qty = (double)rec.ItemDebit;
                        }
                        item2.Quantity -= qty;

                        db.Item2.Update(item2);

                        db.Dtri.Remove(rec);

                        await db.SaveChangesAsync();

                        return Json("ریکارډ حذف شو");
                    }
                }

                return Json("error");
            }
            catch (Exception e)
            {

                return Json("error", e);
            }
        }

        public IActionResult DTRIDeal()
        {
            return View();
        }

        public JsonResult FetchDealerDTRIData(int dealerId)
        {
            var dtri = (from d in db.Dtri
                        where d.DealerId == dealerId
                        select new
                        {
                            dtriid = d.DtriId,
                            dealerid = d.DealerId,
                            name = d.Dealer.Name,
                            item = d.ItemId != 0 ? db.Item.Where(x => x.ItemId == d.ItemId).Select(x => x.Name).FirstOrDefault() : "",
                            itemid = d.ItemId,
                            car = d.CarNumber,
                            credit = d.ItemCredit,
                            debit = d.ItemDebit,
                            price = d.Price,
                            date = Convert.ToDateTime(d.Date).ToShortDateString(),
                            details = d.Details ?? "",
                            currency = d.CurrencyId != 0 ? db.Currency.Where(x => x.CurrencyId == d.CurrencyId).Select(x => x.Currency1).FirstOrDefault() : "",
                            currencyid = d.CurrencyId,
                            item2id = d.Item2Id == 0 ? 0 : d.Item2Id,
                            amount = d.Amount,
                            qtyMoney = d.QtyMoney,
                        }).ToList().OrderByDescending(x => x.dtriid);
            return Json(dtri);
        }


        //[Authorize(Roles = "Admin Department")]
        //public async Task<IActionResult> EditDeal(int dealidedit, double debite, double credite, string detailse)
        //{
        //    var email = userManager.GetUserAsync(User).Result.Email;
        //    var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

        //    var rec = db.Deal.Where(x => x.DealId == dealidedit).FirstOrDefault();
        //    var rec1 = db.CashInHand.Where(x => x.DealId == dealidedit).FirstOrDefault();
        //    if (rec == null || rec1 == null)
        //    {
        //        return Json("اشتباه");
        //    }
        //    if (rec != null)
        //    {
        //        rec.Debit = debite;
        //        rec.Credit = credite;
        //        rec.Detail = detailse;
        //        rec.EmployeeId = empId;
        //        db.Deal.Update(rec);
        //        await db.SaveChangesAsync();
        //    }

        //    if (rec1 != null)
        //    {
        //        rec1.Debit = debite;
        //        rec1.Credit = credite;
        //        rec1.Description = detailse;
        //        db.CashInHand.Update(rec1);
        //        await db.SaveChangesAsync();
        //    }
        //    return Json("done");

        //    //db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //    //db.Entry(rec).Property(x => x.Debit).IsModified = true;
        //    //db.Entry(rec).Property(x => x.Credit).IsModified = true;
        //    //db.Entry(rec).Property(x => x.Detail).IsModified = true;

        //    //var str1 = "کهاته" + "," + rec.DealId;
        //    //var rec1 = db.CashInHand.Where(x => x.Description == str1).FirstOrDefault();
        //    //if (rec1 != null)
        //    //{
        //    //    rec1.Debit = debite;
        //    //    rec1.Credit = credite;
        //    //    db.Entry(rec1).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //    //    db.Entry(rec1).Property(x => x.Debit).IsModified = true;
        //    //    db.Entry(rec1).Property(x => x.Credit).IsModified = true;
        //    //    db.SaveChanges();
        //    //    return Json("done");
        //    //}

        //    //var str2 = "د اجناسو پلور" + "," + rec.DealId;

        //    //var rec2 = db.CashInHand.Where(x => x.Description == str2).FirstOrDefault();
        //    //if (rec2 != null)
        //    //{
        //    //    rec2.Credit = credite;
        //    //    db.Entry(rec2).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //    //    db.Entry(rec2).Property(x => x.Credit).IsModified = true;
        //    //    db.SaveChanges();
        //    //    return Json("done");

        //    //}
        //    //var str3 = "پیرل" + "," + rec.DealId;
        //    //var rec3 = db.CashInHand.Where(x => x.Description == str3).FirstOrDefault();
        //    //if (rec3 != null)
        //    //{
        //    //    rec3.Debit = debite;
        //    //    db.Entry(rec3).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //    //    db.Entry(rec3).Property(x => x.Debit).IsModified = true;
        //    //    db.SaveChanges();
        //    //    return Json("done");
        //    //}

        //}

        //public JsonResult PurchaseDeal()
        //{
        //    var rec = db.Deal.Where(x => x.Dealer.DealerTypeId == 1 && x.Loan != "NO").ToList();
        //    return Json(rec);
        //}
        //public JsonResult CustomerDeal()
        //{
        //    var rec = db.Deal.Where(x => x.Dealer.DealerTypeId != 1 && x.Loan != "NO").ToList();
        //    return Json(rec);
        //}
        //public JsonResult Dealday()
        //{
        //    var day = DateTime.Now.DayOfWeek;
        //    var rec = db.Dealler.Where(x => x.PaymentDay == day.ToString() && x.DealerTypeId == 2).Count();
        //    return Json(rec);
        //}

        //public JsonResult TodaysDealsers()
        //{
        //    var day = DateTime.Now.DayOfWeek;
        //    var af = (from d in db.Dealler
        //              where d.PaymentDay == day.ToString() && d.DealerTypeId == 2
        //              select new
        //              {
        //                  name = d.DealerName,
        //                  fname = d.FatherName,
        //                  province = d.Province,
        //                  phone = d.Phone,
        //                  debit = db.Deal.Where(x => x.DealerId == d.DealerId && x.Loan != "NO" && x.CurrencyId == 1).Sum(x => x.Debit),
        //                  credit = db.Deal.Where(x => x.DealerId == d.DealerId && x.Loan != "NO" && x.CurrencyId == 1).Sum(x => x.Credit),
        //              }).ToList();
        //    var pk = (from d in db.Dealler
        //              where d.PaymentDay == day.ToString() && d.DealerTypeId == 2
        //              select new
        //              {
        //                  name = d.DealerName,
        //                  fname = d.FatherName,
        //                  province = d.Province,
        //                  phone = d.Phone,
        //                  debit = db.Deal.Where(x => x.DealerId == d.DealerId && x.Loan != "NO" && x.CurrencyId == 2).Sum(x => x.Debit),
        //                  credit = db.Deal.Where(x => x.DealerId == d.DealerId && x.Loan != "NO" && x.CurrencyId == 2).Sum(x => x.Credit),
        //              }).ToList();
        //    var us = (from d in db.Dealler
        //              where d.PaymentDay == day.ToString() && d.DealerTypeId == 2
        //              select new
        //              {
        //                  name = d.DealerName,
        //                  fname = d.FatherName,
        //                  province = d.Province,
        //                  phone = d.Phone,
        //                  debit = db.Deal.Where(x => x.DealerId == d.DealerId && x.Loan != "NO" && x.CurrencyId == 3).Sum(x => x.Debit),
        //                  credit = db.Deal.Where(x => x.DealerId == d.DealerId && x.Loan != "NO" && x.CurrencyId == 3).Sum(x => x.Credit),
        //              }).ToList();
        //    var rec = new
        //    {
        //        af,
        //        pk,
        //        us,
        //    };
        //    return Json(rec);
        //}


        public IActionResult DealersList()
        {
            return View();
        }

    }
}