using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShawkanyDb.Models.Model;
using ShawkanyDb.Models.ViewModels;
using ShawkanyDb.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShawkanyDb.Controllers
{
    [Authorize(Roles = "Admin Department")]
    public class ExpenseReportController : Controller
    {

        ShawkanyDbContext db = new ShawkanyDbContext();

        private readonly SharedControllersResources localizer;

        public ExpenseReportController(SharedControllersResources localize)
        {
            localizer = localize;
        }
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult ManualReprot(ManulReprotViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = db.ExpenceType.ToList();
                Dictionary<string, double> dictMonthlySum = new Dictionary<string, double>();
                foreach (var item in category)
                {
                    var amount = db.Expence.Where
                   (cat => cat.ExpenceDate >= model.FromDate.Date && cat.ExpenceDate <= model.ToDate.Date && cat.ExpenceTypeId == item.ExpenceTypeId)
                   .Select(cat => cat.ExpenceAmount)
                   .Sum();

                    dictMonthlySum.Add(item.ExpenceType1,Convert.ToDouble( amount));
                }
                var salaries = db.SalaryPayment.Where
                     (cat => cat.PaidDate >= model.FromDate.Date && cat.PaidDate <= model.ToDate.Date)
                        .Select(cat => cat.PaidAmount)
                        .Sum();

                dictMonthlySum.Add(localizer.Get("Expenses"), Convert.ToDouble(salaries));

                return new JsonResult(dictMonthlySum);


            }
            return View("Error");
        }

        public JsonResult GetWeeklyExpense()
        {
            var category = db.ExpenceType.ToList();
            Dictionary<string, double> dictWeeklySum = new Dictionary<string, double>();

            var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);

            var monthStart = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day - 1));
            var monthEnd = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day)).AddDays(last);
            foreach (var item in category)
            {
                var amount = db.Expence.Where
               (cat => cat.ExpenceDate >= monthStart && cat.ExpenceDate <= monthEnd && cat.ExpenceTypeId == item.ExpenceTypeId)
               .Select(cat => cat.ExpenceAmount)
               .Sum();

                dictWeeklySum.Add(item.ExpenceType1, Convert.ToDouble(amount));
            }
            var salaries = db.SalaryPayment.Where
                     (cat => cat.PaidDate >= monthStart && cat.PaidDate <= monthEnd)
                    .Select(cat => cat.PaidAmount)
                    .Sum();

            dictWeeklySum.Add(localizer.Get("Expenses"), Convert.ToDouble(salaries));

            return new JsonResult(dictWeeklySum);
        }

        public JsonResult GetYearExpense()
        {
            var monthStart = DateTime.Now;
            var monthEnd = DateTime.Now;
            var category = db.ExpenceType.ToList();
            Dictionary<string, double> yearDate = new Dictionary<string, double>();
            foreach (var item in category)
            {
                monthStart = DateTime.Now.Date.AddYears(-1).AddMonths(-(DateTime.Now.Date.Month - 1)).AddDays(-(DateTime.Now.Date.Day - 1));
                monthEnd = DateTime.Now.Date.AddYears(-1).AddMonths(-(DateTime.Now.Date.Month - 1)).AddMonths(11).AddDays(-(DateTime.Now.Date.Day)).AddDays(31);

                var amount = db.Expence.Where
               (cat => cat.ExpenceDate >= monthStart && cat.ExpenceDate <= monthEnd && cat.ExpenceTypeId == item.ExpenceTypeId)
               .Select(cat => cat.ExpenceAmount)
               .Sum();

                yearDate.Add(item.ExpenceType1, Convert.ToDouble(amount));
            }
            var salaries = db.SalaryPayment.Where
                 (cat => cat.PaidDate >= monthStart && cat.PaidDate <= monthEnd)
                 .Select(cat => cat.PaidAmount)
                 .Sum();

            yearDate.Add(localizer.Get("Expenses"), Convert.ToDouble(salaries));
            return new JsonResult(yearDate);
        }

        public JsonResult GetDailyExpense()
        {

            Dictionary<string, double> dictMonthlySum = new Dictionary<string, double>();
          
            var category = db.ExpenceType.ToList();
            foreach (var item in category)
            {
                var amount = db.Expence.Where
               (cat => cat.ExpenceDate == DateTime.Now.Date && cat.ExpenceTypeId == item.ExpenceTypeId)
               .Select(cat => cat.ExpenceAmount)
               .Sum();

                dictMonthlySum.Add(item.ExpenceType1, Convert.ToDouble(amount));
            }
            var salaries = db.SalaryPayment.Where
                     (cat => cat.PaidDate == DateTime.Now.Date)
                      .Select(cat => cat.PaidAmount)
                       .Sum();

            dictMonthlySum.Add(localizer.Get("Expenses"), Convert.ToDouble(salaries));

            return new JsonResult(dictMonthlySum);
        }

        public IActionResult Salaries()
        {
            return View();
        }

        public JsonResult LoadCurrentMonthsSalaries()
        {
            var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            var monthStart = DateTime.Now.Date.AddDays(-(DateTime.Now.Date.Day - 1));
            var monthEnd = DateTime.Now.Date.AddDays(-(DateTime.Now.Date.Day)).AddDays(last);
            var rec = (from s in db.SalaryPayment
                       join e in db.Employee on s.EmployeeId equals e.EmployeeId
                       where s.PaidDate >= monthStart && s.PaidDate <= monthEnd
                       select new
                       {
                           name = e.Name,
                           phone = e.Phone,
                           email = e.Email,
                           paidAmount = s.PaidAmount,
                           paidDate = Convert.ToDateTime(s.PaidDate).Date.ToShortDateString(),
                           paidBy = db.Employee.Where(d => d.EmployeeId == s.PaidBy).Select(r => r.Name).FirstOrDefault(),
                       }).ToList();
            return new JsonResult(rec);
        }

        public JsonResult LoadLastMonthsSalaries()
        {
            var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);

            var monthStart = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day - 1));
            var monthEnd = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day)).AddDays(last);
            var rec = (from s in db.SalaryPayment
                       join e in db.Employee on s.EmployeeId equals e.EmployeeId
                       where s.PaidDate >= monthStart && s.PaidDate <= monthEnd
                       select new
                       {
                           name = e.Name,
                           phone = e.Phone,
                           email = e.Email,
                           paidAmount = s.PaidAmount,
                           paidDate = Convert.ToDateTime(s.PaidDate).Date.ToShortDateString(),
                           paidBy = db.Employee.Where(d => d.EmployeeId == s.PaidBy).Select(r => r.Name).FirstOrDefault(),
                       }).ToList();
            return new JsonResult(rec);
        }

        public JsonResult LoadLastYearSalaries()
        {
            var monthStart = DateTime.Now.Date.AddYears(-1).AddMonths(-(DateTime.Now.Date.Month - 1)).AddDays(-(DateTime.Now.Date.Day - 1));
            var monthEnd = DateTime.Now.Date.AddYears(-1).AddMonths(-(DateTime.Now.Date.Month - 1)).AddMonths(11).AddDays(-(DateTime.Now.Date.Day)).AddDays(31);

            var rec = (from s in db.SalaryPayment
                       join e in db.Employee on s.EmployeeId equals e.EmployeeId
                       where s.PaidDate >= monthStart && s.PaidDate <= monthEnd
                       select new
                       {
                           name = e.Name,
                           phone = e.Phone,
                           email = e.Email,
                           paidAmount = s.PaidAmount,
                           paidDate = Convert.ToDateTime(s.PaidDate).Date.ToShortDateString(),
                           paidBy = db.Employee.Where(d => d.EmployeeId == s.PaidBy).Select(r => r.Name).FirstOrDefault(),
                       }).ToList();
            return new JsonResult(rec);
        }

        public IActionResult ManualSalaryReprot(ManulReprotViewModel model)
        {
            if (ModelState.IsValid)
            {
                var monthStart = model.FromDate.Date;
                var monthEnd = model.ToDate.Date;

                var rec = (from s in db.SalaryPayment
                           join e in db.Employee on s.EmployeeId equals e.EmployeeId
                           where s.PaidDate >= monthStart && s.PaidDate <= monthEnd
                           select new
                           {
                               name = e.Name,
                               phone = e.Phone,
                               email = e.Email,
                               paidAmount = s.PaidAmount,
                               paidDate = Convert.ToDateTime(s.PaidDate).Date.ToShortDateString(),
                               paidBy = db.Employee.Where(d => d.EmployeeId == s.PaidBy).Select(r => r.Name).FirstOrDefault(),
                           }).ToList();
                return new JsonResult(rec);
            }
            return View("Error");
        }
    }
}