//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using ShawkanyDb.Models;
//using ShawkanyDb.Models.Model;
//using ShawkanyDb.Models.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ShawkanyDb.Controllers
//{
//    [Authorize(Roles = ("HR Department,Admin Department"))]
//    public class SaleController : Controller
//    {
//        private readonly UserManager<ApplicationUser> _userManager;

//        public SaleController(UserManager<ApplicationUser> userManager)
//        {
//            _userManager = userManager;

//        }
//        ShawkanyDbContext db = new ShawkanyDbContext();
//        [HttpGet]
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public IActionResult Sale()
//        {
//            var i = (from d in db.Item2
//                     where d.TotalQuantity > 0
//                     select new SelectListItem()
//                     {
//                         Text = d.Item.Name,
//                         Value = d.ItemId.ToString()
//                     }).ToList();
//            ViewBag.item = i;
//            var unit = (from u in db.Unit
//                        select new SelectListItem()
//                        {
//                            Text = u.Unit1,
//                            Value = u.UnitId.ToString()
//                        }).ToList();
//            ViewBag.unit = unit;
//            var customer = (from d in db.Dealler.Where(x => x.DealerTypeId == 2)
//                            select new SelectListItem()
//                            {
//                                Text = d.DealerName,
//                                Value = d.DealerId.ToString()
//                            }).ToList();
//            ViewBag.customer = customer;

//            //var company = (from d in db.Company
//            //               select new SelectListItem()
//            //               {
//            //                   Text = d.Company1,
//            //                   Value = d.CompanyId.ToString()
//            //               }).ToList();
//            //ViewBag.company = company;

//            var country = (from d in db.Country
//                           select new SelectListItem()
//                           {
//                               Text = d.Country1,
//                               Value = d.CountryId.ToString()
//                           }).ToList();
//            ViewBag.country = country;

//            var currency = (from d in db.Currency
//                            select new SelectListItem()
//                            {
//                                Text = d.Currency1,
//                                Value = d.CurrencyId.ToString()
//                            }).ToList();
//            ViewBag.currency = currency;

//            var stock = (from d in db.Stock
//                         select new SelectListItem()
//                         {
//                             Text = d.Stock1,
//                             Value = d.StockId.ToString()
//                         }).ToList();
//            ViewBag.stock = stock;

//            // ViewBag.billno = db.Sale.Max(x => Convert.ToInt32(x.BillNo) + 1).ToString();

//            return View();
//        }
//        [HttpGet]
//        public JsonResult FetchItemInfo(int itemId)
//        {
//            var result = (from i in db.Item
//                          join i2 in db.Item2 on i.ItemId equals i2.ItemId
//                          //join ca in db.Category on i.CategoryId equals ca.CategoryId
//                          join cu in db.Currency on i2.CurrencyId equals cu.CurrencyId
//                          join cou in db.Country on i2.CountryId equals cou.CountryId
//                          join unit in db.Unit on i2.UnitId equals unit.UnitId
//                          where i2.ItemId == itemId && i2.TotalQuantity>0
//                          select new
//                          {
//                              itemid = i.ItemId,
//                              itemname = i.Name,
//                              //categoryid = ca.CategoryId,
//                              category = i.Category.Category1,
//                              type = i2.Item.Category.Category1,
//                              quantity = i2.TotalQuantity,
//                              countryid = cou.CountryId,
//                              country = cou.Country1,
//                              item2Id = i2.Item2Id,
//                              itemInStock = i2.TotalQuantity,
//                              purchaseprice = i2.PurchasePrice,
//                              saleprice = i2.SalePrice,
//                              currencyid = i2.CurrencyId,
//                              currency = cu.Currency1,
//                              amountinpackage = i2.AmountInPackage,
//                              unitid = i2.UnitId,
//                              unit = unit.Unit1,
//                              shortage = i2.QuantityForShortage,
//                              manDates = i2.ManufactureDate,
//                              expDates = i2.ExpireDate,
//                              manDate = Convert.ToDateTime(i2.ManufactureDate).ToShortDateString() == "1/1/0001" ? "" : Convert.ToDateTime(i2.ManufactureDate).ToShortDateString(),
//                              expDate = Convert.ToDateTime(i2.ExpireDate).ToShortDateString() == "1/1/0001" ? "" : Convert.ToDateTime(i2.ExpireDate).ToShortDateString(),
//                          }).ToList();

//            return Json(result);
//        }
//        [HttpPost]
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public async Task<JsonResult> Sale(AllViewModel pModel)
//        {
//            var email = _userManager.GetUserAsync(User).Result.Email;
//            var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

//            var dealerid = 0;
//            double totalquantity = 0;
//            var amountinpackage = 0;
//            var saletype = "";
//            if (pModel.sale.parchonsale == "عمده")
//            {
//                totalquantity = pModel.purchase.Quantity * pModel.purchase.AmountInPackage;
//                amountinpackage = pModel.purchase.AmountInPackage;
//                saletype = "عمده";
//            }
//            else
//            {
//                totalquantity = pModel.purchase.Quantity;
//                amountinpackage = 1;
//                saletype = "پرچون";
//            }
//            if (pModel.sale.DealerID == 0)
//            {
//                dealerid = 1;
//            }
//            else
//            {
//                dealerid = pModel.sale.DealerID;
//            }
//            var Itemrecored = db.Item2.Where(x => x.Item2Id == pModel.purchase.Item2ID && x.StockId == pModel.purchase.StockID).FirstOrDefault();

//            var discount = (((pModel.purchase.Quantity * amountinpackage) * pModel.purchase.SalePrice) * pModel.sale.Discount) / 100;

//            if (Itemrecored == null)
//            {
//                return Json("جنس به نوموړي ګودام کې نشته، بل ګودام انتخاب کړئ");
//            }
//            if (Itemrecored.TotalQuantity < totalquantity)
//            {
//                return Json(" نوموړي جنس په کافي اندازه وجود نلري،ګودام کې د جنس تعداد دی " + (Itemrecored.TotalQuantity));
//            }
//            else
//            {
//                try
//                {
//                    Sale selling = new Sale()
//                    {
//                        DealerId = dealerid,
//                        Item2Id = pModel.purchase.Item2ID,
//                        TempCustomerName = pModel.sale.tempCustomerName,
//                        BillNo = pModel.purchase.Bill_No,
//                        ItemId = pModel.purchase.ItemID,
//                        Quantity = totalquantity,
//                        SalePrice = pModel.purchase.SalePrice,
//                        PurchasePrice = pModel.purchase.PurchasePrice,
//                        CurrencyId = pModel.purchase.CurrencyID,
//                        Discount = pModel.sale.Discount,
//                        UnitId = pModel.purchase.UnitID,
//                        StockId = pModel.purchase.StockID,
//                        SaleType = saletype,
//                        SaleDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
//                        ManufactureDate = Itemrecored.ManufactureDate,
//                        ExpireDate = Itemrecored.ExpireDate,
//                        Type = pModel.sale.Type,
//                        AmountInPackage = amountinpackage,
//                        CountryId = pModel.purchase.CountryID,
//                        EmployeeId = empId,
//                        Note = pModel.sale.Note,
//                    };
//                    db.Sale.Add(selling);
//                    await db.SaveChangesAsync();
//                }
//                catch (Exception e)
//                {

//                    return Json("Saler error " + e);
//                }
//                //////////////////////////////////////
//                /////////////////////Updating Item in Item2 Table //////////
//                if (Itemrecored.TotalQuantity == totalquantity)
//                {
//                    Itemrecored.TotalQuantity = 0;
//                    db.Entry(Itemrecored).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//                    db.Entry(Itemrecored).Property(x => x.TotalQuantity).IsModified = true;
//                    await db.SaveChangesAsync();
//                }
//                else
//                {
//                    Itemrecored.TotalQuantity -= totalquantity;
//                    db.Entry(Itemrecored).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//                    db.Entry(Itemrecored).Property(x => x.TotalQuantity).IsModified = true;
//                    await db.SaveChangesAsync();
//                }
//                return Json("جنس وپلورل شو");
//            }

//        }
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public async Task<JsonResult> SaleReturn(AllViewModel pModel)
//        {
//            var email = _userManager.GetUserAsync(User).Result.Email;
//            var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

//            var dealerid = 0;
//            double totalquantity = 0;
//            var amountinpackage = 0;
//            var saletype = "";
//            if (pModel.sale.parchonsale == "عمده")
//            {
//                totalquantity = pModel.purchase.Quantity * pModel.purchase.AmountInPackage;
//                amountinpackage = pModel.purchase.AmountInPackage;
//                saletype = "عمده";
//            }
//            else
//            {
//                totalquantity = pModel.purchase.Quantity;
//                amountinpackage = 1;
//                saletype = "پرچون";
//            }
//            if (pModel.sale.DealerID == 0)
//            {
//                dealerid = 1;
//            }
//            else
//            {
//                dealerid = pModel.sale.DealerID;
//            }
//            {
//                Sale returedSale = db.Sale.Where(x => x.SaleId == pModel.sale.SaleId).FirstOrDefault();

//                ///////////////////////// if returned item's quantity is greater //////// 
//                if (returedSale.Quantity < totalquantity)
//                {
//                    return Json("د واپس شوي جنس تعداد مو ډیر رسولی");
//                }
//                /////////////////////////////////////////////////////////////////////////
//                var item2 = db.Item2.Where(x => x.Item2Id == pModel.purchase.Item2ID
//                      //&& x.PurchasePrice == returedSale.PurchasePrice
//                      //&& x.CountryId == returedSale.CountryId
//                      //&& x.CurrencyId == returedSale.CurrencyId
//                      //&& x.StockId == returedSale.StockId
//                      //&& x.ManufactureDate == returedSale.ManufactureDate
//                      //&& x.ExpireDate == returedSale.ExpireDate
//                      //&& x.UnitId == returedSale.UnitId
//                      ).FirstOrDefault();

//                if (returedSale.Quantity == totalquantity)
//                {
//                    //if (item2 == null)
//                    //{

//                    //    //Item2 newItem = new Item2
//                    //    //{
//                    //    //    ItemId = pModel.purchase.ItemID,
//                    //    //    CurrencyId = pModel.purchase.CurrencyID,
//                    //    //    CountryId = pModel.purchase.CountryID,
//                    //    //    SalePrice = pModel.purchase.SalePrice,
//                    //    //    AmountInPackage = pModel.purchase.AmountInPackage,
//                    //    //    PurchasePrice = returedSale.PurchasePrice,
//                    //    //    UnitId = pModel.purchase.UnitID,
//                    //    //    TotalQuantity = totalquantity,
//                    //    //    ManufactureDate = pModel.purchase.ManuficturDate,
//                    //    //    ExpireDate = pModel.purchase.ExpireDate,
//                    //    //    StockId = pModel.purchase.StockID,
                            
//                    //    //};
//                    //    //db.Item2.Add(newItem);
//                    //    //await db.SaveChangesAsync();
//                    //}
//                    //else
//                    //{

                        
//                    //}

//                    item2.TotalQuantity += totalquantity;
//                    db.Entry(item2).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//                    db.Entry(item2).Property(x => x.TotalQuantity).IsModified = true;
//                    await db.SaveChangesAsync();

//                    ///////////////////////////// Returing purchase ///////////////////////
//                    db.Sale.Attach(returedSale);
//                    db.Entry(returedSale).Property(x => x.SaleState).IsModified = true;
//                    returedSale.SaleState = "Returned";
//                    db.Entry(returedSale).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//                    await db.SaveChangesAsync();

//                    return Json("پلورل شوی جنس په مکمله توګه واپس شو");
//                }
//                else
//                {


//                    db.Sale.Attach(returedSale);
//                    returedSale.Quantity -= totalquantity;
//                    returedSale.SaleId = pModel.sale.SaleId;
//                    db.Entry(returedSale).Property(x => x.Quantity).IsModified = true;
//                    db.Entry(returedSale).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//                    await db.SaveChangesAsync();
//                    ////////////////////////// Updating Quantity of Item in stock  ////////////
//                    //if (item2 == null)
//                    //{
//                    //    Item2 newItem = new Item2
//                    //    {
//                    //        ItemId = pModel.purchase.ItemID,
//                    //        CurrencyId = pModel.purchase.CurrencyID,
//                    //        CountryId = pModel.purchase.CountryID,
//                    //        SalePrice = pModel.purchase.SalePrice,
//                    //        AmountInPackage = pModel.purchase.AmountInPackage,
//                    //        PurchasePrice = returedSale.PurchasePrice,
//                    //        UnitId = pModel.purchase.UnitID,
//                    //        TotalQuantity = totalquantity,
//                    //        ManufactureDate = pModel.purchase.ManuficturDate,
//                    //        ExpireDate = pModel.purchase.ExpireDate,
//                    //        StockId = pModel.purchase.StockID,

//                    //    };
//                    //    db.Item2.Add(newItem);
//                    //    await db.SaveChangesAsync();
//                    //}
//                    //else
//                    //{

//                    //    item2.TotalQuantity += totalquantity;
//                    //    db.Entry(item2).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//                    //    db.Entry(item2).Property(x => x.TotalQuantity).IsModified = true;
//                    //    await db.SaveChangesAsync();
//                    //}
//                    item2.TotalQuantity += totalquantity;
//                    db.Entry(item2).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//                    db.Entry(item2).Property(x => x.TotalQuantity).IsModified = true;
//                    await db.SaveChangesAsync();
//                    try
//                    {
//                        Sale returned = new Sale
//                        {
//                            BillNo = pModel.purchase.Bill_No,
//                            Item2Id = pModel.purchase.Item2ID,
//                            DealerId = dealerid,
//                            TempCustomerName = pModel.sale.tempCustomerName,
//                            ItemId = pModel.purchase.ItemID,
//                            CurrencyId = pModel.purchase.CurrencyID,
//                            Quantity = totalquantity,
//                            Discount = pModel.sale.Discount,
//                            SaleDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
//                            EmployeeId = empId,
//                            Note = pModel.sale.Note,
//                            UnitId = pModel.purchase.UnitID,
//                            SalePrice = pModel.purchase.SalePrice,
//                            PurchasePrice = pModel.purchase.PurchasePrice,
//                            SaleState = "Returned",
//                            StockId = pModel.purchase.StockID,
//                            SaleType = saletype,
//                            CountryId = pModel.purchase.CountryID,
//                            AmountInPackage = amountinpackage,
//                            ManufactureDate = pModel.purchase.ManuficturDate,
//                            ExpireDate = pModel.purchase.ExpireDate,
//                            Type = pModel.sale.Type,
//                        };

//                        db.Sale.Add(returned);
//                        await db.SaveChangesAsync();
//                        return Json("د پلورل شوي یوه اندازه واپس شوه");
//                    }
//                    catch (Exception e)
//                    {

//                        return Json("adding half sale err" + e);
//                    }
//                }
//            }
//        }
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public JsonResult FetchBillNo()
//        {
//            var sale = db.Sale.ToList();
//            var billno = "1";
//            if (sale.Count() > 0)
//            {
//                billno = db.Sale.Max(x => Convert.ToInt32(x.BillNo) + 1).ToString();
//            }
//            return Json(billno);
//        }
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public JsonResult FetchSaledItems()
//        {
//            var result = (from S in db.Sale
//                          join item in db.Item on S.ItemId equals item.ItemId
//                          join de in db.Dealler on S.DealerId equals de.DealerId
//                          join cou in db.Country on S.CountryId equals cou.CountryId
//                          join currenc in db.Currency on S.CurrencyId equals currenc.CurrencyId
//                          join stoc in db.Stock on S.StockId equals stoc.StockId
//                          join emp in db.Employee on S.EmployeeId equals emp.EmployeeId
//                          join unit in db.Unit on S.UnitId equals unit.UnitId
//                          select new
//                          {
//                              id = S.SaleId,
//                              item2id = S.Item2Id,
//                              salid = S.SaleId,
//                              billno = S.BillNo,
//                              itemid = S.ItemId,
//                              item = item.Name,
//                              dealerid = S.DealerId,
//                              dealer = de.DealerName,
//                              tmpCusName = S.TempCustomerName,
//                              purchaseprice = S.PurchasePrice,
//                              saleprice = S.SalePrice,
//                              discount = S.Discount,
//                              quantity = S.Quantity,
//                              unitid = S.UnitId,
//                              unit = unit.Unit1,
//                              currencyid = S.CurrencyId,
//                              currency = currenc.Currency1,
//                              amountinpackage = S.AmountInPackage,
//                              manDates = S.ManufactureDate,
//                              manDate = Convert.ToDateTime(S.ManufactureDate).ToShortDateString() ,
//                              expDate = Convert.ToDateTime(S.ExpireDate).ToShortDateString(),
//                              saledate = Convert.ToDateTime(S.SaleDate).ToShortDateString(),
//                              expDates = S.ExpireDate,
//                              employee = emp.Name,
//                              salestatus = S.SaleState,
//                              note = S.Note,
//                              saletype = S.SaleType
//                          }).Where(x => x.salestatus == null).OrderByDescending(x=>x.id).ToList();
//            return Json(result);
//        }
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public async Task<JsonResult> AddDealAmount(AllViewModel model,string dealBillNo, string desc)
//        {
//            var email = _userManager.GetUserAsync(User).Result.Email;
//            var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

//            Deal afghani = new Deal();
//            Deal rupe = new Deal();
//            Deal doller = new Deal();
//            var dealerid = 0;

//            if (model.dealingform.id == -1 || model.dealingform.id > 0)
//            {
//                try
//                {
//                    if (model.dealingform.id == -1)
//                    {
//                        dealerid = 1;
//                    }
//                    else
//                    {
//                        dealerid = model.dealingform.id;
//                    }
//                }
//                catch (Exception)
//                {

//                    throw;
//                }
//                if (model.dealingform.afghaniCredit != 0 || model.dealingform.afghaniDebit != 0 || model.dealingform.rupeCredit != 0 || model.dealingform.rupeDebit != 0 || model.dealingform.dollerCredit != 0 || model.dealingform.dollerDebit != 0)
//                {
//                    CashInHand ch = new CashInHand();
//                    if (model.dealingform.afghaniCredit != 0 || model.dealingform.afghaniDebit != 0)
//                    {

//                        afghani.DealerId = dealerid;
//                        if (model.dealingform.selling == -1)
//                        {
//                            afghani.Credit = model.dealingform.afghaniCredit;
//                            afghani.Debit = model.dealingform.afghaniDebit;
//                            afghani.Type = "د پلور واپسي";


//                            ch.Debit = model.dealingform.afghaniDebit;
//                            ch.Credit = 0;
                         
//                        }
//                        else
//                        {
//                            afghani.Debit = model.dealingform.afghaniCredit;
//                            afghani.Credit = model.dealingform.afghaniDebit;
//                            afghani.Type = "پلور";

//                            ch.Credit = model.dealingform.afghaniDebit;
//                            ch.Debit = 0;
//                        }

//                        afghani.CurrencyId = 1;
//                        afghani.Detail = desc;
//                       // afghani.BillNo = dealBillNo;
//                        afghani.Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
//                        afghani.EmployeeId = empId;
//                        if (model.dealingform.afghaniCredit == model.dealingform.afghaniDebit)
//                        {
//                            afghani.Loan = "NO";
//                        }
//                        db.Deal.Add(afghani);
//                        await db.SaveChangesAsync();

//                        ch.CurrencyId = 1;
//                        ch.DealId = afghani.DealId;
//                        ch.Date = DateTime.Now.Date;
//                        db.CashInHand.Add(ch);
//                        await db.SaveChangesAsync();
//                    }
//                    if (model.dealingform.rupeCredit != 0 || model.dealingform.rupeDebit != 0)
//                    {
//                        rupe.DealerId = dealerid;
//                        if (model.dealingform.selling == -1)
//                        {
//                            rupe.Credit = model.dealingform.rupeCredit;
//                            rupe.Debit = model.dealingform.rupeDebit;
//                            rupe.Type = "د پلور واپسي";

//                            ch.Debit = model.dealingform.rupeDebit;
//                            ch.Credit = 0;
//                        }
//                        else
//                        {
//                            rupe.Debit = model.dealingform.rupeCredit;
//                            rupe.Credit = model.dealingform.rupeDebit;
//                            rupe.Type = "پلور";

//                            ch.Credit = model.dealingform.rupeDebit;
//                            ch.Debit = 0;
//                        }

//                        rupe.CurrencyId = 2;
//                        rupe.Detail = desc;
//                        //rupe.BillNo = dealBillNo;
//                        rupe.Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
//                        rupe.EmployeeId = empId;
//                        if (model.dealingform.rupeCredit == model.dealingform.rupeDebit)
//                        {
//                            rupe.Loan = "NO";
//                        }
//                        db.Deal.Add(rupe);
//                        await db.SaveChangesAsync();

//                        ch.CurrencyId = 2;
//                        ch.DealId = rupe.DealId;
//                        ch.Date = DateTime.Now.Date;
//                        db.CashInHand.Add(ch);
//                        await db.SaveChangesAsync();
//                    }
//                    if (model.dealingform.dollerCredit != 0 || model.dealingform.dollerDebit != 0)
//                    {

//                        doller.DealerId = dealerid;
//                        if (model.dealingform.selling == -1)
//                        {
//                            doller.Credit = model.dealingform.dollerCredit;
//                            doller.Debit = model.dealingform.dollerDebit;
//                            doller.Type = "د پلور واپسي";

//                            ch.Debit = model.dealingform.dollerDebit;
//                            ch.Credit = 0;
//                        }
//                        else
//                        {
//                            doller.Debit = model.dealingform.dollerCredit;
//                            doller.Credit = model.dealingform.dollerDebit;
//                            doller.Type = "پلور";

//                            ch.Credit = model.dealingform.dollerDebit;
//                            ch.Debit = 0;
//                        }

//                        doller.CurrencyId = 3;
//                        doller.Detail = desc;
//                      //  doller.BillNo = dealBillNo;
//                        doller.Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
//                        doller.EmployeeId = empId;
//                        if (model.dealingform.dollerCredit == model.dealingform.dollerDebit)
//                        {
//                            doller.Loan = "NO";
//                        }
//                        db.Deal.Add(doller);
//                        await db.SaveChangesAsync();

//                        ch.CurrencyId = 3;
//                        ch.DealId = doller.DealId;
//                        ch.Date = DateTime.Now.Date;
//                        db.CashInHand.Add(ch);
//                        await db.SaveChangesAsync();
//                    }
//                    return Json("کهاته سیستم کې ذخیره شول");
//                }

//            }

//            return Json("لطفاً لومړی پلور تر سره کړئ");
//        }
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public async Task<JsonResult> RemoveSinlgeSaled(string type,int item2id, string billno, DateTime mandate, DateTime expdate, int stockid, int currencyid, int countryid, double purchaseprice, double saleprice, int dealerid, int itemid, int unitid, double quantity, int amountinPackage)
//        {
//            var aminpackage = 1;
//            if (type == "عمده")
//            {
//                aminpackage = amountinPackage;
//            }
//            else
//            {
//                aminpackage = 1;
//            }
//            if (dealerid == 0)
//            {
//                dealerid = 1;
//            }
//            var totalquantity = quantity * aminpackage;
//            var record = db.Sale.Where(x => x.BillNo == billno
//                            && x.ItemId == itemid
//                            && x.AmountInPackage == aminpackage
//                            && x.DealerId == dealerid
//                            && x.UnitId == unitid
//                            && x.CountryId == countryid
//                            //&& x.SalePrice == saleprice
//                            && x.CurrencyId == currencyid
//                            && x.ManufactureDate == mandate.Date
//                            && x.ExpireDate == expdate.Date
//                            && x.PurchasePrice == purchaseprice
//                            && x.Quantity == totalquantity).FirstOrDefault();

//            var record2 = db.Item2.Where(x => x.Item2Id == item2id
//                           //// && x.AmountInPackage == amountinPackage 
//                           //&& x.UnitId == unitid
//                           //&& x.CountryId == countryid
//                           //&& x.PurchasePrice == purchaseprice
//                           //&& x.CurrencyId == currencyid
//                           ////&& x.SalePrice == saleprice
//                           //&& x.ManufactureDate == mandate
//                           //&& x.ExpireDate == expdate
//                           //&& x.StockId == stockid
//                           ).FirstOrDefault();

//            if (record == null)
//            {
//                return Json("جنس حذف نه شو");
//            }

//            record2.TotalQuantity += totalquantity;
//            db.Entry(record2).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//            db.Entry(record2).Property(x => x.TotalQuantity).IsModified = true;
         

//            db.Sale.Remove(record);

//            await db.SaveChangesAsync();
//            return Json("جنس د بیل څخه حذف شو");
//        }
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public IActionResult returnedSales()
//        {
//            return View();
//        }
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public JsonResult fetchReturnedSales()
//        {
//            try
//            {
//                var result = (from S in db.Sale.Where(x => x.SaleState == "Returned")
//                              join item in db.Item on S.ItemId equals item.ItemId
//                              join de in db.Dealler on S.DealerId equals de.DealerId
//                              join cou in db.Country on S.CountryId equals cou.CountryId
//                              join currenc in db.Currency on S.CurrencyId equals currenc.CurrencyId
//                              join stoc in db.Stock on S.StockId equals stoc.StockId
//                              join emp in db.Employee on S.EmployeeId equals emp.EmployeeId
//                              join unit in db.Unit on S.UnitId equals unit.UnitId
//                              select new
//                              {
//                                  salid = S.SaleId,
//                                  billno = S.BillNo,
//                                  itemid = S.ItemId,
//                                  item = item.Name,
//                                  dealerid = S.DealerId,
//                                  dealer = de.DealerName,
//                                  dealertypeid = de.DealerTypeId,
//                                  tmpCusName = S.TempCustomerName,
//                                  countryid = S.CountryId,
//                                  country = cou.Country1,
//                                  saleprice = S.SalePrice,
//                                  discount = S.Discount,
//                                  quantity = S.Quantity,
//                                  unitid = S.UnitId,
//                                  unit = unit.Unit1,
//                                  currencyid = S.CurrencyId,
//                                  currency = currenc.Currency1,
//                                  returndate = Convert.ToDateTime(S.SaleDate).ToShortDateString(),
//                                  stockid = S.StockId,
//                                  stock = stoc.Stock1,
//                                  amountinpackage = S.AmountInPackage,
//                                  employeeid = S.EmployeeId,
//                                  employee = emp.Name,
//                                  salestatus = S.SaleState,
//                                  note = S.Note,
//                                  type = S.Item.Category.Category1,
//                                  saletype = S.SaleType
//                              }).ToList();
//                return Json(result);
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public async Task<IActionResult> DeleteBill(AllViewModel model)
//        {
//            var saledItems = db.Sale.Where(x => x.BillNo == model.changeBill.billNo && x.SaleState != "Returned" && x.SaleDate == model.changeBill.SaleDate).ToList();
//            var deal = db.Deal.Where(x => x.DealerId == model.changeBill.dealerid && x.Type == "پلور" && x.Date == model.changeBill.SaleDate).FirstOrDefault();
//            if (saledItems == null)
//            {
//                return Json("مهربانئ له مخې بیل نمبر او نیټه چیک کړی");
//            }
//            if (deal == null)
//            {
//                return Json("مهربانئ له مخې مشترې نوم او نیټه چیک کړی");
//            }
//            foreach (var returedSale in saledItems)
//            {
//                var item2 = db.Item2.Where(x => x.ItemId == returedSale.ItemId
//                      && x.PurchasePrice == returedSale.PurchasePrice
//                      && x.CountryId == returedSale.CountryId
//                      && x.CurrencyId == returedSale.CurrencyId
//                      && x.StockId == returedSale.StockId
//                      && x.ManufactureDate == returedSale.ManufactureDate
//                      && x.ExpireDate == returedSale.ExpireDate
//                      && x.UnitId == returedSale.UnitId).FirstOrDefault();
//                if (item2 == null)
//                {
//                    Item2 newItem = new Item2
//                    {
//                        ItemId = returedSale.ItemId,
//                        CurrencyId = returedSale.CurrencyId,
//                        CountryId = returedSale.CountryId,
//                        SalePrice = returedSale.SalePrice,
//                        AmountInPackage = returedSale.AmountInPackage,
//                        PurchasePrice = returedSale.PurchasePrice,
//                        UnitId = returedSale.UnitId,
//                        TotalQuantity = returedSale.Quantity,
//                        StockId = returedSale.StockId,
//                        ManufactureDate = returedSale.ManufactureDate,
//                        ExpireDate = returedSale.ExpireDate,

//                    };
//                    db.Item2.Add(newItem);

//                    db.Sale.Remove(returedSale);

//                    await db.SaveChangesAsync();
//                }
//                else
//                {
//                    item2.TotalQuantity += returedSale.Quantity;
//                    db.Entry(item2).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//                    db.Entry(item2).Property(x => x.TotalQuantity).IsModified = true;

//                    db.Sale.Remove(returedSale);

//                    await db.SaveChangesAsync();
//                }
//            }
//            db.Deal.Remove(deal);
          
//            CashInHand ch = new CashInHand
//            {
//                Debit = deal.Credit,
//                Credit = 0,
//                CurrencyId = deal.CurrencyId,
//                Date = DateTime.Now.Date,
//                Description = "د حذف شوي بیل پیسې"
//            };
//            db.CashInHand.Add(ch);

//            await db.SaveChangesAsync();
//            return Json("خرڅ شوی اجناس او کهاته حذف شو");

//            //var discount = 0.0;
//            //var totalDiscount = 0.0;
//            //var saleprice = 0.0;
//            //var oldDealerID = 0;
//            //var loop = true;
//            //var currencyid = 0;
//            //foreach (var item in saled)
//            //{
//            //    discount = (double)(item.SalePrice * item.Quantity / item.AmountInPackage * item.Discount / 100);
//            //    totalDiscount += discount;
//            //    saleprice += (Convert.ToDouble(item.SalePrice) * (Convert.ToDouble(item.Quantity) / item.AmountInPackage)) - discount;
//            //    //////////////////////////////////
//            //    if (loop)
//            //    {
//            //        oldDealerID = Convert.ToInt32(item.DealerId);
//            //        currencyid = Convert.ToInt32(item.CurrencyId);
//            //        loop = false;
//            //    }
//            //    db.Sale.Attach(item);
//            //    db.Entry(item).Property(x => x.DealerId).IsModified = true;
//            //    item.DealerId = model.changeBill.dealerid;
//            //    item.SaleId = item.SaleId;
//            //    db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//            //}
//            //await db.SaveChangesAsync();
//            //var deal = db.Deal.Where(x => x.DealerId == oldDealerID && x.Debit == saleprice &&).FirstOrDefault();

//            //deal.DealerId = model.changeBill.dealerid;
//            //db.Entry(deal).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//            //db.Entry(deal).Property(x => x.DealerId).IsModified = true;
//            //await  db.SaveChangesAsync();


//            //var saleditems = (from S in db.Sale.Where(x => x.BillNo == model.changeBill.billNo)
//            //                  join item in db.Item on S.ItemId equals item.ItemId
//            //                  join de in db.Dealler on S.DealerId equals de.DealerId
//            //                  //  join dType in db.DealerType on de.DealerTypeId equals dType.DealerTypeId
//            //                  //  join comp in db.Company on S.CompanyId equals comp.CompanyId
//            //                  //  join cou in db.Country on S.CountryId equals cou.CountryId
//            //                  join currenc in db.Currency on S.CurrencyId equals currenc.CurrencyId
//            //                  //   join stoc in db.Stock on S.StockId equals stoc.StockId
//            //                  //   join emp in db.Employee on S.EmployeeId equals emp.EmployeeId
//            //                  join unit in db.Unit on S.UnitId equals unit.UnitId
//            //                  select new
//            //                  {
//            //                      salid = S.SaleId,
//            //                      billno = S.BillNo,
//            //                      itemid = S.ItemId,
//            //                      item = item.Name,
//            //                      dealerid = S.DealerId,
//            //                      dealer = de.DealerName,
//            //                      dealertypeid = de.DealerTypeId,
//            //                      tmpCusName = S.TempCustomerName,
//            //                      purchaseprice = S.PurchasePrice,
//            //                      saleprice = S.SalePrice,
//            //                      discount = S.Discount,
//            //                      quantity = S.Quantity,
//            //                      unitid = S.UnitId,
//            //                      unit = unit.Unit1,
//            //                      amountinpackage = S.AmountInPackage,
//            //                      currencyid = S.CurrencyId,
//            //                      currency = currenc.Currency1,
//            //                      salestatus = S.SaleState,
//            //                      note = S.Note,
//            //                      saletype = S.SaleType
//            //                  }).ToList();

//            //var dataForTotalRemain = db.Deal.Where(x => x.DealerId == model.changeBill.dealerid && x.CurrencyId == currencyid);
//            //var debit = 0.00;
//            //var credit = 0.00;
//            //foreach (var item in dataForTotalRemain)
//            //{
//            //    debit += Convert.ToDouble(item.Debit);
//            //    credit += Convert.ToDouble(item.Credit);
//            //}
//            //var loan = debit - credit;
//            //var result = new
//            //{
//            //    saledItems = saleditems,
//            //    totalprice = deal.Debit,
//            //    paid = deal.Credit,
//            //    discount = totalDiscount,
//            //    totalloan = loan
//            //};
           
//        }
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public IActionResult fetchTotalDeal(string IDFortotal, string currencyID)
//        {
//            var target = db.Deal.Where(x => x.DealerId == Convert.ToInt32(IDFortotal) && x.CurrencyId == Convert.ToInt32(currencyID));
//            var debit = 0.00;
//            var credit = 0.00;
//            foreach (var item in target)
//            {
//                debit += Convert.ToDouble(item.Debit);
//                credit += Convert.ToDouble(item.Credit);
//            }
//            var loan = debit - credit;
//            return Json(loan);
//        }
//        [Authorize(Roles = ("HR Department,Admin Department"))]
//        public async Task<JsonResult> saleFromOthersShop(AllViewModel model)
//        {
//            var email = _userManager.GetUserAsync(User).Result.Email;
//            var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();
//            var quantity = 0;
//            if (model.othersItem.saleType == "عمده")
//            {
//                quantity = model.othersItem.Quantity * model.othersItem.AmountInPackage;
//            }
//            else
//            {
//                quantity = model.othersItem.Quantity;
//            }
//            Sale sl = new Sale
//            {
//                BillNo = model.othersItem.Bill_No,
//                DealerId = model.othersItem.DealerID,
//                TempCustomerName = model.othersItem.tempCustomerName,
//                //ItemFormOther = model.othersItem.ItemName,
//                SalePrice = model.othersItem.SalePrice,
//                PurchasePrice = model.othersItem.purchaseprice,
//                Quantity = quantity,
//                AmountInPackage = model.othersItem.AmountInPackage,
//                SaleDate = DateTime.Now,
//                SaleType = model.othersItem.saleType,
//                CurrencyId = model.othersItem.CurrencyID,
//               // ShopKeeper = model.othersItem.shopKeeper,
//                EmployeeId = empId
//            };
//            db.Sale.Add(sl);
//            await db.SaveChangesAsync();
//            return Json("جنس وپلورل شو");
//        }
//        public IActionResult ItemFromOthers()
//        {
//            return View();
//        }
//        public JsonResult fetchSaledItemsFromOthers()
//        {
//            var result = (from S in db.Sale
//                          where S.SaleState != "Returned"
//                          select new
//                          {
//                              salid = S.SaleId,
//                              billno = S.BillNo,
//                              itemid = S.ItemId,
//                             // shopKeeper = S.ShopKeeper,
//                             // item = S.ItemFormOther,
//                              amountinpackage = S.AmountInPackage,

//                             saledate = Convert.ToDateTime(S.SaleDate).ToShortDateString(),
//                              dealerid = S.DealerId,
//                              dealer = db.Dealler.Where(r => r.DealerId == S.DealerId).Select(w => w.DealerName).FirstOrDefault(),
//                              tmpCusName = S.TempCustomerName,
//                              purchaseprice = S.PurchasePrice,
//                              saleprice = S.SalePrice,
//                              discount = S.Discount,
//                              quantity = S.Quantity,
//                              currencyid = S.CurrencyId,
//                              currency = db.Currency.Where(r => r.CurrencyId == S.CurrencyId).Select(w => w.Currency1).FirstOrDefault(),
//                              employeeid = S.EmployeeId,
//                              employee = db.Employee.Where(r => r.EmployeeId == S.EmployeeId).Select(w => w.Name).FirstOrDefault(),
//                              saletype = S.SaleType
//                          }).ToList();
//            return Json(result);
//        }
//        public IActionResult Benefit()
//        {
//            return View();
//        }
//        public JsonResult FetchFinanceReport(int reportType)
//        {
           
//            var sale = db.Sale.Where(x => x.SaleId > 0 && x.SaleState != "Returned");

//            switch (reportType)
//            {
//                //////////////////////////// todays report ///////////////
//                case 1:
                  
//                    sale = db.Sale.Where(x => x.SaleDate == DateTime.Now.Date && x.SaleState != "Returned");
                  
//                    break;
//                ///////////////////////// current month's report ///////////
//                case 2:
//                    var currentMonth = DateTime.Now.Month;
//                    sale = db.Sale.Where(x => x.SaleDate.Value.Month == currentMonth && x.SaleDate.Value.Year == DateTime.Now.Year && x.SaleState != "Returned");
//                     break;
//                /////////////////////////   last month report ////////////////////////
//                case 3:
//                        sale = db.Sale.Where(x => x.SaleDate.Value.Month == (DateTime.Now.Month - 1) && x.SaleDate.Value.Year == DateTime.Now.Year && x.SaleState != "Returned");
                       
//                    break;
//                /////////////////////////  Current Year's report  ////////////
//                case 4:
//                    var currentYear = DateTime.Now.Year;

//                     sale = db.Sale.Where(x => x.SaleDate.Value.Year == currentYear && x.SaleState != "Returned");
//                    break;
//                ////////////////////////   Last year's report ///////////////
//                case 5:
//                   sale = db.Sale.Where(x => x.SaleDate.Value.Year == (DateTime.Now.Year - 1) && x.SaleState != "Returned");
//                    break;
//            }
           

           

//            return Json(sale);
//        }
//        [Authorize(Roles = "Admin Department")]
//        public IActionResult ManualReprot(AllViewModel model ,string BillNo)
//        {
            
//                var sale = db.Sale.Where(x =>x.SaleId>0 &&  x.SaleState != "Returned");
//                if (BillNo != "")
//                {
//                    sale = db.Sale.Where(x => x.BillNo == BillNo && x.SaleState != "Returned");
//                }
//                else
//                {
//                     sale = db.Sale.Where(x => x.SaleDate >= model.manulReport.FromDate.Date && x.SaleDate <=
//                                          model.manulReport.ToDate.Date && x.SaleState != "Returned");
//                }

//                return Json(sale);
            
//        }
//        public JsonResult FetchFinanceReportSaled(int reportType)
//        {

//            var sale = (from S in db.Sale where S.SaleState!= "Returned"
//                        join item in db.Item on S.ItemId equals item.ItemId
//                          join de in db.Dealler on S.DealerId equals de.DealerId
//                          join cou in db.Country on S.CountryId equals cou.CountryId
//                          join currenc in db.Currency on S.CurrencyId equals currenc.CurrencyId
//                          join stoc in db.Stock on S.StockId equals stoc.StockId
//                          join emp in db.Employee on S.EmployeeId equals emp.EmployeeId
//                          join unit in db.Unit on S.UnitId equals unit.UnitId
//                          select new
//                          {
//                              id = S.SaleId,
//                              item2id = S.Item2Id,
//                              salid = S.SaleId,
//                              billno = S.BillNo,
//                              itemid = S.ItemId,
//                              item = item.Name,
//                              dealerid = S.DealerId,
//                              dealer = de.DealerName,
//                              tmpCusName = S.TempCustomerName,
//                              purchaseprice = S.PurchasePrice,
//                              saleprice = S.SalePrice,
//                              discount = S.Discount,
//                              quantity = S.Quantity,
//                              unitid = S.UnitId,
//                              unit = unit.Unit1,
//                              currencyid = S.CurrencyId,
//                              currency = currenc.Currency1,
//                              amountinpackage = S.AmountInPackage,
//                              manDates = S.ManufactureDate,
//                              manDate = Convert.ToDateTime(S.ManufactureDate).ToShortDateString(),
//                              expDate = Convert.ToDateTime(S.ExpireDate).ToShortDateString(),
//                              saledate = Convert.ToDateTime(S.SaleDate).ToShortDateString(),
//                              expDates = S.ExpireDate,
//                              employee = emp.Name,
//                              salestatus = S.SaleState,
//                              note = S.Note,
//                              saletype = S.SaleType,
//                              countryid = S.CountryId,
//                              stockid = S.StockId,
//                          }).Where(x => x.salestatus == null).OrderByDescending(x => x.id).ToList();

//            switch (reportType)
//            {
//                //////////////////////////// todays report ///////////////
//                case 1:

//                    sale = (from S in db.Sale.Where(x => x.SaleDate == DateTime.Now.Date && x.SaleState != "Returned")
//                            join item in db.Item on S.ItemId equals item.ItemId
//                            join de in db.Dealler on S.DealerId equals de.DealerId
//                            join cou in db.Country on S.CountryId equals cou.CountryId
//                            join currenc in db.Currency on S.CurrencyId equals currenc.CurrencyId
//                            join stoc in db.Stock on S.StockId equals stoc.StockId
//                            join emp in db.Employee on S.EmployeeId equals emp.EmployeeId
//                            join unit in db.Unit on S.UnitId equals unit.UnitId
//                            select new
//                            {
//                                id = S.SaleId,
//                                item2id = S.Item2Id,
//                                salid = S.SaleId,
//                                billno = S.BillNo,
//                                itemid = S.ItemId,
//                                item = item.Name,
//                                dealerid = S.DealerId,
//                                dealer = de.DealerName,
//                                tmpCusName = S.TempCustomerName,
//                                purchaseprice = S.PurchasePrice,
//                                saleprice = S.SalePrice,
//                                discount = S.Discount,
//                                quantity = S.Quantity,
//                                unitid = S.UnitId,
//                                unit = unit.Unit1,
//                                currencyid = S.CurrencyId,
//                                currency = currenc.Currency1,
//                                amountinpackage = S.AmountInPackage,
//                                manDates = S.ManufactureDate,
//                                manDate = Convert.ToDateTime(S.ManufactureDate).ToShortDateString(),
//                                expDate = Convert.ToDateTime(S.ExpireDate).ToShortDateString(),
//                                saledate = Convert.ToDateTime(S.SaleDate).ToShortDateString(),
//                                expDates = S.ExpireDate,
//                                employee = emp.Name,
//                                salestatus = S.SaleState,
//                                note = S.Note,
//                                saletype = S.SaleType,
//                                countryid = S.CountryId,
//                                stockid = S.StockId,
//                            }).Where(x => x.salestatus == null).OrderByDescending(x => x.id).ToList();
//                    break;
//                ///////////////////////// current month's report ///////////
//                case 2:
//                    var currentMonth = DateTime.Now.Month;
//                    sale = (from S in db.Sale.Where(x => x.SaleDate.Value.Month == currentMonth && x.SaleDate.Value.Year == DateTime.Now.Year && x.SaleState != "Returned")
//                            join item in db.Item on S.ItemId equals item.ItemId
//                            join de in db.Dealler on S.DealerId equals de.DealerId
//                            join cou in db.Country on S.CountryId equals cou.CountryId
//                            join currenc in db.Currency on S.CurrencyId equals currenc.CurrencyId
//                            join stoc in db.Stock on S.StockId equals stoc.StockId
//                            join emp in db.Employee on S.EmployeeId equals emp.EmployeeId
//                            join unit in db.Unit on S.UnitId equals unit.UnitId
//                            select new
//                            {
//                                id = S.SaleId,
//                                item2id = S.Item2Id,
//                                salid = S.SaleId,
//                                billno = S.BillNo,
//                                itemid = S.ItemId,
//                                item = item.Name,
//                                dealerid = S.DealerId,
//                                dealer = de.DealerName,
//                                tmpCusName = S.TempCustomerName,
//                                purchaseprice = S.PurchasePrice,
//                                saleprice = S.SalePrice,
//                                discount = S.Discount,
//                                quantity = S.Quantity,
//                                unitid = S.UnitId,
//                                unit = unit.Unit1,
//                                currencyid = S.CurrencyId,
//                                currency = currenc.Currency1,
//                                amountinpackage = S.AmountInPackage,
//                                manDates = S.ManufactureDate,
//                                manDate = Convert.ToDateTime(S.ManufactureDate).ToShortDateString(),
//                                expDate = Convert.ToDateTime(S.ExpireDate).ToShortDateString(),
//                                saledate = Convert.ToDateTime(S.SaleDate).ToShortDateString(),
//                                expDates = S.ExpireDate,
//                                employee = emp.Name,
//                                salestatus = S.SaleState,
//                                note = S.Note,
//                                saletype = S.SaleType,
//                                countryid = S.CountryId,
//                                stockid = S.StockId,
//                            }).Where(x => x.salestatus == null).OrderByDescending(x => x.id).ToList();
//                    break;
//                /////////////////////////   last month report ////////////////////////
//                case 3:
//                    {
//                        Dictionary<string, int> dictWeeklySum = new Dictionary<string, int>();

//                        sale = (from S in db.Sale.Where(x => x.SaleDate.Value.Month == (DateTime.Now.Month - 1) && x.SaleDate.Value.Year == DateTime.Now.Year && x.SaleState != "Returned")
//                                join item in db.Item on S.ItemId equals item.ItemId
//                                join de in db.Dealler on S.DealerId equals de.DealerId
//                                join cou in db.Country on S.CountryId equals cou.CountryId
//                                join currenc in db.Currency on S.CurrencyId equals currenc.CurrencyId
//                                join stoc in db.Stock on S.StockId equals stoc.StockId
//                                join emp in db.Employee on S.EmployeeId equals emp.EmployeeId
//                                join unit in db.Unit on S.UnitId equals unit.UnitId
//                                select new
//                                {
//                                    id = S.SaleId,
//                                    item2id = S.Item2Id,
//                                    salid = S.SaleId,
//                                    billno = S.BillNo,
//                                    itemid = S.ItemId,
//                                    item = item.Name,
//                                    dealerid = S.DealerId,
//                                    dealer = de.DealerName,
//                                    tmpCusName = S.TempCustomerName,
//                                    purchaseprice = S.PurchasePrice,
//                                    saleprice = S.SalePrice,
//                                    discount = S.Discount,
//                                    quantity = S.Quantity,
//                                    unitid = S.UnitId,
//                                    unit = unit.Unit1,
//                                    currencyid = S.CurrencyId,
//                                    currency = currenc.Currency1,
//                                    amountinpackage = S.AmountInPackage,
//                                    manDates = S.ManufactureDate,
//                                    manDate = Convert.ToDateTime(S.ManufactureDate).ToShortDateString(),
//                                    expDate = Convert.ToDateTime(S.ExpireDate).ToShortDateString(),
//                                    saledate = Convert.ToDateTime(S.SaleDate).ToShortDateString(),
//                                    expDates = S.ExpireDate,
//                                    employee = emp.Name,
//                                    salestatus = S.SaleState,
//                                    note = S.Note,
//                                    saletype = S.SaleType,
//                                    countryid = S.CountryId,
//                                    stockid = S.StockId,
//                                }).Where(x => x.salestatus == null).OrderByDescending(x => x.id).ToList();
//                    }
//                    break;
//                /////////////////////////  Current Year's report  ////////////
//                case 4:
//                    var currentYear = DateTime.Now.Year;

//                    sale = (from S in db.Sale.Where(x => x.SaleDate.Value.Year == currentYear && x.SaleState != "Returned")
//                            join item in db.Item on S.ItemId equals item.ItemId
//                            join de in db.Dealler on S.DealerId equals de.DealerId
//                            join cou in db.Country on S.CountryId equals cou.CountryId
//                            join currenc in db.Currency on S.CurrencyId equals currenc.CurrencyId
//                            join stoc in db.Stock on S.StockId equals stoc.StockId
//                            join emp in db.Employee on S.EmployeeId equals emp.EmployeeId
//                            join unit in db.Unit on S.UnitId equals unit.UnitId
//                            select new
//                            {
//                                id = S.SaleId,
//                                item2id = S.Item2Id,
//                                salid = S.SaleId,
//                                billno = S.BillNo,
//                                itemid = S.ItemId,
//                                item = item.Name,
//                                dealerid = S.DealerId,
//                                dealer = de.DealerName,
//                                tmpCusName = S.TempCustomerName,
//                                purchaseprice = S.PurchasePrice,
//                                saleprice = S.SalePrice,
//                                discount = S.Discount,
//                                quantity = S.Quantity,
//                                unitid = S.UnitId,
//                                unit = unit.Unit1,
//                                currencyid = S.CurrencyId,
//                                currency = currenc.Currency1,
//                                amountinpackage = S.AmountInPackage,
//                                manDates = S.ManufactureDate,
//                                manDate = Convert.ToDateTime(S.ManufactureDate).ToShortDateString(),
//                                expDate = Convert.ToDateTime(S.ExpireDate).ToShortDateString(),
//                                saledate = Convert.ToDateTime(S.SaleDate).ToShortDateString(),
//                                expDates = S.ExpireDate,
//                                employee = emp.Name,
//                                salestatus = S.SaleState,
//                                note = S.Note,
//                                saletype = S.SaleType,
//                                countryid = S.CountryId,
//                                stockid = S.StockId,
//                            }).Where(x => x.salestatus == null).OrderByDescending(x => x.id).ToList();
//                    break;
//                ////////////////////////   Last year's report ///////////////
//                case 5:
//                    sale = (from S in db.Sale.Where(x => x.SaleDate.Value.Year == (DateTime.Now.Year - 1) && x.SaleState != "Returned")
//                            join item in db.Item on S.ItemId equals item.ItemId
//                            join de in db.Dealler on S.DealerId equals de.DealerId
//                            join cou in db.Country on S.CountryId equals cou.CountryId
//                            join currenc in db.Currency on S.CurrencyId equals currenc.CurrencyId
//                            join stoc in db.Stock on S.StockId equals stoc.StockId
//                            join emp in db.Employee on S.EmployeeId equals emp.EmployeeId
//                            join unit in db.Unit on S.UnitId equals unit.UnitId
//                            select new
//                            {
//                                id = S.SaleId,
//                                item2id = S.Item2Id,
//                                salid = S.SaleId,
//                                billno = S.BillNo,
//                                itemid = S.ItemId,
//                                item = item.Name,
//                                dealerid = S.DealerId,
//                                dealer = de.DealerName,
//                                tmpCusName = S.TempCustomerName,
//                                purchaseprice = S.PurchasePrice,
//                                saleprice = S.SalePrice,
//                                discount = S.Discount,
//                                quantity = S.Quantity,
//                                unitid = S.UnitId,
//                                unit = unit.Unit1,
//                                currencyid = S.CurrencyId,
//                                currency = currenc.Currency1,
//                                amountinpackage = S.AmountInPackage,
//                                manDates = S.ManufactureDate,
//                                manDate = Convert.ToDateTime(S.ManufactureDate).ToShortDateString(),
//                                expDate = Convert.ToDateTime(S.ExpireDate).ToShortDateString(),
//                                saledate = Convert.ToDateTime(S.SaleDate).ToShortDateString(),
//                                expDates = S.ExpireDate,
//                                employee = emp.Name,
//                                salestatus = S.SaleState,
//                                note = S.Note,
//                                saletype = S.SaleType,
//                                countryid = S.CountryId,
//                                stockid = S.StockId,
//                            }).Where(x => x.salestatus == null).OrderByDescending(x => x.id).ToList();
//                    break;


//            }
//            return Json(sale);
//        }
//        public IActionResult ManualReprotSaled(AllViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var sale = (from S in db.Sale.Where(x => x.SaleDate >= model.manulReport.FromDate.Date && x.SaleDate <=
//                                             model.manulReport.ToDate.Date && x.SaleState != "Returned")
//                            join item in db.Item on S.ItemId equals item.ItemId
//                            join de in db.Dealler on S.DealerId equals de.DealerId
//                            join cou in db.Country on S.CountryId equals cou.CountryId
//                            join currenc in db.Currency on S.CurrencyId equals currenc.CurrencyId
//                            join stoc in db.Stock on S.StockId equals stoc.StockId
//                            join emp in db.Employee on S.EmployeeId equals emp.EmployeeId
//                            join unit in db.Unit on S.UnitId equals unit.UnitId
//                            select new
//                            {
//                                id = S.SaleId,
//                                item2id = S.Item2Id,
//                                salid = S.SaleId,
//                                billno = S.BillNo,
//                                itemid = S.ItemId,
//                                item = item.Name,
//                                dealerid = S.DealerId,
//                                dealer = de.DealerName,
//                                tmpCusName = S.TempCustomerName,
//                                purchaseprice = S.PurchasePrice,
//                                saleprice = S.SalePrice,
//                                discount = S.Discount,
//                                quantity = S.Quantity,
//                                unitid = S.UnitId,
//                                unit = unit.Unit1,
//                                currencyid = S.CurrencyId,
//                                currency = currenc.Currency1,
//                                amountinpackage = S.AmountInPackage,
//                                manDates = S.ManufactureDate,
//                                manDate = Convert.ToDateTime(S.ManufactureDate).ToShortDateString(),
//                                expDate = Convert.ToDateTime(S.ExpireDate).ToShortDateString(),
//                                saledate = Convert.ToDateTime(S.SaleDate).ToShortDateString(),
//                                expDates = S.ExpireDate,
//                                employee = emp.Name,
//                                salestatus = S.SaleState,
//                                note = S.Note,
//                                saletype = S.SaleType,
//                                countryid = S.CountryId,
//                                stockid = S.StockId,
//                            }).Where(x => x.salestatus == null).OrderByDescending(x => x.id).ToList();
//                return Json(sale);
//            }
//            return Json("Error");
//        }
//    }
//}