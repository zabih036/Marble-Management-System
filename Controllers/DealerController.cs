using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShawkanyDb.Models.Model;
using ShawkanyDb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShokanayDb.Controllers
{
    public class DealerController : Controller
    {
        ShawkanyDbContext db = new ShawkanyDbContext();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin Department")]
        public async Task<IActionResult> DealerRegistration(DealerViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DealerId != 0)
                {
                    var rec = await db.Dealer.FindAsync(model.DealerId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.DealerId = model.DealerId;
                    rec.Name = model.Name;
                    rec.Fname = model.FName;
                    rec.Address = model.Address;
                    rec.Phone = model.Phone.ToString();

                    db.Dealer.Update(rec);
                    await db.SaveChangesAsync();

                    return Json("کهاتدار معلومات تغیر شو");
                }
                else
                {
                    try
                    {
                        Dealer dealer = new Dealer()
                        {
                            Name = model.Name,
                            Fname = model.FName,
                            Address = model.Address,
                            Phone = model.Phone.ToString(),
                            RegDate = DateTime.Now.Date,
                        };
                        db.Dealer.Add(dealer);
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    return Json("کهاتدار معلومات ذخیره شو");
                }
            }
            return View("Error");
        }

        public JsonResult FetchDealers()
        {
            var dealer = (from d in db.Dealer
                                       select new
                                       {
                                           dealerId = d.DealerId,
                                           name = d.Name,
                                           fName = d.Fname,
                                           address = d.Address,
                                           phone = d.Phone,
                                       }).ToList().OrderByDescending(r => r.dealerId);
            return Json(dealer);
        }

        [Authorize(Roles = "Admin Department")]
        public async Task<IActionResult> DeleteDealer(DealerViewModel model)
        {
            if (model.DealerId != 0)
            {
                var dealer = await db.Dealer.FindAsync(model.DealerId);
                if (dealer == null)
                {
                    return View("Error");
                }

                try
                {
                    db.Dealer.Remove(dealer);
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

        public async Task<IActionResult> CustomerRegistration(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CustomerId != 0)
                {
                    var rec = await db.Customer.FindAsync(model.CustomerId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.CustomerId = model.CustomerId;
                    rec.Name = model.CusName;
                    rec.Fname = model.CusFName;
                    rec.Address = model.CusAddress;
                    rec.Phone = model.CusPhone.ToString();

                    db.Customer.Update(rec);
                    await db.SaveChangesAsync();

                    return Json("مشترې معلومات تغیر شو");
                }
                else
                {
                    try
                    {
                        Customer Customer = new Customer()
                        {
                            Name = model.CusName,
                            Fname = model.CusFName,
                            Address = model.CusAddress,
                            Phone = model.CusPhone.ToString(),
                            RegDate = DateTime.Now.Date,
                        };
                        db.Customer.Add(Customer);
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    return Json("مشترې معلومات ذخیره شو");
                }
            }
            return View("Error");
        }

        public JsonResult FetchCustomers()
        {
            var Customer = (from d in db.Customer
                          select new
                          {
                              customerId = d.CustomerId,
                              name = d.Name,
                              fName = d.Fname,
                              address = d.Address,
                              phone = d.Phone,
                          }).ToList().OrderByDescending(r => r.customerId);
            return Json(Customer);
        }

        [Authorize(Roles = "Admin Department")]
        public async Task<IActionResult> DeleteCustomer(CustomerViewModel model)
        {
            if (model.CustomerId != 0)
            {
                var Customer = await db.Customer.FindAsync(model.CustomerId);
                if (Customer == null)
                {
                    return View("Error");
                }

                try
                {
                    db.Customer.Remove(Customer);
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
    }
}
