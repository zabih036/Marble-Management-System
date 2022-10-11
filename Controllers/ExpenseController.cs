using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShawkanyDb.Models.Model;
using ShawkanyDb.Models.ViewModels;
using ShawkanyDb.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShawkanyDb.Controllers
{

    [Authorize(Roles = "Admin Department")]
    public class ExpenseController : Controller
    {

        ShawkanyDbContext db = new ShawkanyDbContext();


        public IActionResult Expense()
        {
            var c = (from d in db.ExpenceType    
                     select new SelectListItem()
                     {
                         Text = d.ExpenceType1,
                         Value = d.ExpenceTypeId.ToString()
                     }).ToList();

            ViewBag.ExpenseType = c;
            return View();
        }
        public JsonResult LoadExpenseType()
        {
            var rec = db.ExpenceType.OrderByDescending(d => d.ExpenceTypeId);
            return Json(rec);
        }
        public JsonResult LoadExpense()
        {


            var rec = (from i in db.Expence
                       join c in db.ExpenceType on i.ExpenceTypeId equals c.ExpenceTypeId
                       select new
                       {
                           expenceId = i.ExpenceId,
                           expenceAmount = i.ExpenceAmount,
                           expenceType = c.ExpenceType1,
                           expenceDate = Convert.ToDateTime(i.ExpenceDate),
                           expenceDateShow = Convert.ToDateTime(i.ExpenceDate).ToShortDateString(),
                           detail = i.Detail??"",
                           expenceTypeId = i.ExpenceTypeId

                       }).ToList().OrderByDescending(d => d.expenceId);
            return Json(rec);
        }

        public async Task<IActionResult> SaveExpense(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.expense.ExpenseId != 0)
                {
                    var rec = await db.Expence.FindAsync(model.expense.ExpenseId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.ExpenceAmount = model.expense.Expense;
                    rec.ExpenceTypeId = model.expense.ExpenseTypeId;
                    rec.ExpenceDate = model.expense.ExpenseDate;
                    rec.Detail = model.expense.Detail;


                    db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Entry(rec).Property(d => d.ExpenceAmount).IsModified = true;
                    db.Entry(rec).Property(d => d.ExpenceTypeId).IsModified = true;
                    db.Entry(rec).Property(d => d.ExpenceDate).IsModified = true;
                    db.Entry(rec).Property(d => d.Detail).IsModified = true;

                    var rec4 = db.CashInHand.Where(x => x.Description == model.expense.ExpenseId.ToString()).FirstOrDefault();
                    rec4.Debit = model.expense.Expense;
                    rec4.Date = model.expense.ExpenseDate;
                    db.Entry(rec4).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Entry(rec4).Property(x => x.Debit).IsModified = true;
                    db.Entry(rec4).Property(x => x.Date).IsModified = true;

                    await db.SaveChangesAsync();

                    return Json("Updated");
                }
                else
                {
                    Expence c = new Expence()
                    {
                        ExpenceAmount = model.expense.Expense,
                        ExpenceTypeId = model.expense.ExpenseTypeId,
                        ExpenceDate = model.expense.ExpenseDate,
                        Detail = model.expense.Detail
                    };

                    db.Expence.Add(c);
                    await db.SaveChangesAsync();
                    var exId = db.Expence.OrderByDescending(x => x.ExpenceId).FirstOrDefault();
                    CashInHand ch = new CashInHand()
                    {
                        Debit = model.expense.Expense,
                        Credit = 0,
                        Date = model.expense.ExpenseDate,
                        CurrencyId = 1,
                        Description = exId.ExpenceId.ToString()
                    };
                    db.CashInHand.Add(ch);
                    await db.SaveChangesAsync();
                    return Json("Saved");
                }

            }
            return View("Error");
        }

        public IActionResult IsExpenseTypeExist(AllViewModel model)
        {
            if (model.expenseType.ExpenseTypeId != 0)
            {
                return Json(true);
            }
            var rec = db.ExpenceType.Any(d => d.ExpenceType1 == model.expenseType.ExpenseType);

            if (!rec)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public async Task<IActionResult> SaveExpenseType(AllViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.expenseType.ExpenseTypeId != 0)
                {
                    var rec = await db.ExpenceType.FindAsync(model.expenseType.ExpenseTypeId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.ExpenceType1 = model.expenseType.ExpenseType;


                    db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.Entry(rec).Property(d => d.ExpenceType1).IsModified = true;

                    await db.SaveChangesAsync();

                    return Json("Updated");
                }
                else
                {
                    ExpenceType c = new ExpenceType()
                    {
                        ExpenceType1 = model.expenseType.ExpenseType
                    };
                    db.ExpenceType.Add(c);
                    await db.SaveChangesAsync();
                    return Json("Saved");
                }

            }
            return View("Error");
        }

     
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteExpenseType(AllViewModel model)
        {
            if (model.expenseType.ExpenseTypeId != 0)
            {
                var comp = await db.ExpenceType.FindAsync(model.expenseType.ExpenseTypeId);
                if (comp == null)
                {
                    return View("Error");
                }

                try
                {
                    db.ExpenceType.Remove(comp);
                    await db.SaveChangesAsync();
                    return Json("Deleted");

                }
                catch (Exception)
                {

                    return Json("NotDeleted");
                }
            }
            return NotFound();

        }
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteExpense(int cId)
        {
            if (cId != 0)
            {
                var comp = await db.Expence.FindAsync(cId);
                if (comp == null)
                {
                    return View("Error");
                }
                db.Expence.Remove(comp);
                await db.SaveChangesAsync();
                return Json("d");
            }
            return NotFound();

        }
        public IActionResult BillExpenses()
        {
            return View();
        }
    }
}