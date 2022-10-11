using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShawkanyDb.Models.Model;
using ShawkanyDb.Models.ViewModels;
using ShawkanyDb.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShawkanyDb.Controllers
{

    public class HomeController : Controller
    {

        ShawkanyDbContext db = new ShawkanyDbContext();

        private readonly SharedControllersResources localizer;

        public HomeController(SharedControllersResources localize)
        {
            localizer = localize;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult dashboard()
        {
            return View();
        }

        public IActionResult GeneralReport()
        {
            return View();
        }

        public IActionResult ManageCulture(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(3) });

            return LocalRedirect(returnUrl);
        }

        public IActionResult MonthlyExpenses()
        {
            var monthStart = DateTime.Now;
            var monthEnd = DateTime.Now;
            Dictionary<string, double> dictWeeklySum = new Dictionary<string, double>();
            var currentMonthNo = DateTime.Now.Month;
            var currentYearNo = DateTime.Now.Year;
            for (int i = 1; i < 13; i++)
            {
                var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-currentMonthNo + i).Month);
                monthStart = DateTime.Now.AddMonths((-currentMonthNo) + i).AddDays(-(DateTime.Now.Day - 1));
                monthEnd = DateTime.Now.AddMonths((-currentMonthNo) + i).AddDays(-(DateTime.Now.Day)).AddDays(last);


                double expense = db.Expence.Where
              (cat => cat.ExpenceDate >= monthStart && cat.ExpenceDate <= monthEnd)
              .Select(cat => cat.ExpenceAmount)
              .Sum();

                double salaries = (double)db.SalaryPayment.Where
              (cat => cat.PaidDate >= monthStart && cat.PaidDate <= monthEnd)
              .Select(cat => cat.PaidAmount)
              .Sum();

                if (expense == 0)
                {
                    expense = 0;
                }
                if (salaries == 0)
                {
                    salaries = 0;
                }
                double amount = expense + salaries;


                if (i == 1)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("Januray"), amount);
                }
                else if (i == 2)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("February"), amount);
                }
                else if (i == 3)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("March"), amount);
                }
                else if (i == 4)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("April"), amount);
                }
                else if (i == 5)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("May"), amount);
                }
                else if (i == 6)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("June"), amount);
                }
                else if (i == 7)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("July"), amount);
                }
                else if (i == 8)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("August"), amount);
                }
                else if (i == 9)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("September"), amount);
                }
                else if (i == 10)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("October"), amount);
                }
                else if (i == 11)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("November"), amount);
                }
                else if (i == 12)
                {
                    if (amount != 0)
                        dictWeeklySum.Add(localizer.Get("December"), amount);
                }


            }
            return new JsonResult(dictWeeklySum);
        }

        public JsonResult MonthlyIncomes()
        {

            var monthStart = DateTime.Now;
            var monthEnd = DateTime.Now;
            Dictionary<string, double> afghani = new Dictionary<string, double>();
            Dictionary<string, double> pk = new Dictionary<string, double>();
            Dictionary<string, double> dollar = new Dictionary<string, double>();

            var currentMonthNo = DateTime.Now.Month;
            var currentYearNo = DateTime.Now.Year;
            for (int i = 1; i < 13; i++)
            {

                var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-currentMonthNo + i).Month);
                monthStart = DateTime.Now.AddMonths((-currentMonthNo) + i).AddDays(-(DateTime.Now.Day - 1));
                monthEnd = DateTime.Now.AddMonths((-currentMonthNo) + i).AddDays(-(DateTime.Now.Day)).AddDays(last);

                var sale1 = 0.0;
                var purOnSaleTime = 0.0;
                var itemsale = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 1 && r.SaleState != "Returned").ToList();
                foreach (var item in itemsale)
                {
                    sale1 += (Convert.ToDouble(item.SalePrice) * (Convert.ToDouble(item.Quantity)));
                    purOnSaleTime += Convert.ToDouble(item.PurchasePrice) * (Convert.ToDouble(item.Quantity));
                }

                var income = sale1 - purOnSaleTime;


                if (i == 1)
                {

                    afghani.Add(localizer.Get("Januray"), income);
                }
                else if (i == 2)
                {

                    afghani.Add(localizer.Get("February"), income);
                }
                else if (i == 3)
                {

                    afghani.Add(localizer.Get("March"), income);
                }
                else if (i == 4)
                {

                    afghani.Add(localizer.Get("April"), income);
                }
                else if (i == 5)
                {

                    afghani.Add(localizer.Get("May"), income);
                }
                else if (i == 6)
                {

                    afghani.Add(localizer.Get("June"), income);
                }
                else if (i == 7)
                {

                    afghani.Add(localizer.Get("July"), income);
                }
                else if (i == 8)
                {

                    afghani.Add(localizer.Get("August"), income);
                }
                else if (i == 9)
                {

                    afghani.Add(localizer.Get("September"), income);
                }
                else if (i == 10)
                {

                    afghani.Add(localizer.Get("October"), income);
                }
                else if (i == 11)
                {

                    afghani.Add(localizer.Get("November"), income);
                }
                else if (i == 12)
                {

                    afghani.Add(localizer.Get("December"), income);
                }


            }

            for (int i = 1; i < 13; i++)
            {
                var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-currentMonthNo + i).Month);
                monthStart = DateTime.Now.AddMonths((-currentMonthNo) + i).AddDays(-(DateTime.Now.Day - 1));
                monthEnd = DateTime.Now.AddMonths((-currentMonthNo) + i).AddDays(-(DateTime.Now.Day)).AddDays(last);

                var sale1 = 0.0;
                var purOnSaleTime = 0.0;
                var itemsale = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 2 && r.SaleState != "Returned").ToList();

                foreach (var item in itemsale)
                {
                    sale1 += Convert.ToDouble(item.SalePrice) * Convert.ToDouble(item.Quantity);
                    purOnSaleTime += Convert.ToDouble(item.PurchasePrice) * (Convert.ToDouble(item.Quantity));
                }



                var income = sale1 - purOnSaleTime;



                if (i == 1)
                {

                    pk.Add(localizer.Get("Januray"), income);
                }
                else if (i == 2)
                {

                    pk.Add(localizer.Get("February"), income);
                }
                else if (i == 3)
                {

                    pk.Add(localizer.Get("March"), income);
                }
                else if (i == 4)
                {

                    pk.Add(localizer.Get("April"), income);
                }
                else if (i == 5)
                {

                    pk.Add(localizer.Get("May"), income);
                }
                else if (i == 6)
                {

                    pk.Add(localizer.Get("June"), income);
                }
                else if (i == 7)
                {

                    pk.Add(localizer.Get("July"), income);
                }
                else if (i == 8)
                {

                    pk.Add(localizer.Get("August"), income);
                }
                else if (i == 9)
                {

                    pk.Add(localizer.Get("September"), income);
                }
                else if (i == 10)
                {

                    pk.Add(localizer.Get("October"), income);
                }
                else if (i == 11)
                {

                    pk.Add(localizer.Get("November"), income);
                }
                else if (i == 12)
                {

                    pk.Add(localizer.Get("December"), income);
                }



            }

            for (int i = 1; i < 13; i++)
            {
                var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-currentMonthNo + i).Month);
                monthStart = DateTime.Now.AddMonths((-currentMonthNo) + i).AddDays(-(DateTime.Now.Day - 1));
                monthEnd = DateTime.Now.AddMonths((-currentMonthNo) + i).AddDays(-(DateTime.Now.Day)).AddDays(last);

                var sale1 = 0.0;
                var purOnSaleTime = 0.0;
                var itemsale = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 3 && r.SaleState != "Returned").ToList();
                foreach (var item in itemsale)
                {
                    sale1 += (Convert.ToDouble(item.SalePrice) * Convert.ToDouble(item.Quantity));
                    purOnSaleTime += Convert.ToDouble(item.PurchasePrice) * (Convert.ToDouble(item.Quantity));
                }



                var income = sale1 - purOnSaleTime;



                if (i == 1)
                {

                    dollar.Add(localizer.Get("Januray"), income);
                }
                else if (i == 2)
                {

                    dollar.Add(localizer.Get("February"), income);
                }
                else if (i == 3)
                {

                    dollar.Add(localizer.Get("March"), income);
                }
                else if (i == 4)
                {

                    dollar.Add(localizer.Get("April"), income);
                }
                else if (i == 5)
                {

                    dollar.Add(localizer.Get("May"), income);
                }
                else if (i == 6)
                {

                    dollar.Add(localizer.Get("June"), income);
                }
                else if (i == 7)
                {

                    dollar.Add(localizer.Get("July"), income);
                }
                else if (i == 8)
                {

                    dollar.Add(localizer.Get("August"), income);
                }
                else if (i == 9)
                {

                    dollar.Add(localizer.Get("September"), income);
                }
                else if (i == 10)
                {

                    dollar.Add(localizer.Get("October"), income);
                }
                else if (i == 11)
                {

                    dollar.Add(localizer.Get("November"), income);
                }
                else if (i == 12)
                {

                    dollar.Add(localizer.Get("December"), income);
                }



            }

            var allCurrency = new
            {
                afghani = afghani,
                pk = pk,
                dollar = dollar,
            };
            return new JsonResult(allCurrency);
        }

        public IActionResult ManualGeneralReprot(ManulReprotViewModel model)
        {
            if (ModelState.IsValid)
            {

                Dictionary<string, double> dictWeeklySum = new Dictionary<string, double>();


                var monthStart = model.FromDate.Date;
                var monthEnd = model.ToDate.Date;


                var expense = db.Expence.Where
                     (cat => cat.ExpenceDate >= monthStart && cat.ExpenceDate <= monthEnd)
                     .Select(cat => cat.ExpenceAmount)
                        .Sum();

                var salaries = db.SalaryPayment.Where
                     (cat => cat.PaidDate >= monthStart && cat.PaidDate <= monthEnd)
                     .Select(cat => cat.PaidAmount)
                     .Sum();


                var amount = Convert.ToDouble(expense + salaries);

                dictWeeklySum.Add(localizer.Get("Expense"), amount);



                var itemsaleAf = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 1 && r.SaleState != "Returned").ToList();
                var sale1Af = 0.0;
                foreach (var item in itemsaleAf)
                {
                    sale1Af += (Convert.ToDouble(item.Quantity * item.SalePrice));
                }



                dictWeeklySum.Add(localizer.Get("AFNSale"), Convert.ToDouble(sale1Af));

                var sale1Pk = 0.0;
                var itemsalePk = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 2 && r.SaleState != "Returned").ToList();
                foreach (var item in itemsalePk)
                {
                    sale1Pk += (Convert.ToDouble(item.Quantity * item.SalePrice));
                }

                dictWeeklySum.Add(localizer.Get("PKRSale"), Convert.ToDouble(sale1Pk));

                var sale1Us = 0.0;
                var itemsaleUs = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 3 && r.SaleState != "Returned").ToList();

                foreach (var item in itemsaleUs)
                {
                    sale1Us += (Convert.ToDouble(item.Quantity * item.SalePrice));
                }
                dictWeeklySum.Add(localizer.Get("USDSale"), Convert.ToDouble(sale1Us));

                var purchase1Af = 0.0;
                var itempurchaseAf = db.Purchase.Where(r => r.Date >= monthStart && r.Date <= monthEnd && r.CurrencyId == 1 && r.Status != "Returned").ToList();

                foreach (var item in itempurchaseAf)
                {
                    purchase1Af += Convert.ToDouble((item.SecondQty * item.SecondPrice));
                }

                dictWeeklySum.Add(localizer.Get("AFNPurchase"), Convert.ToDouble(purchase1Af));

                var purchase1Pk = 0.0;
                var itempurchasePk = db.Purchase.Where(r => r.Date >= monthStart && r.Date <= monthEnd && r.CurrencyId == 2 && r.Status != "Returned").ToList();

                foreach (var item in itempurchasePk)
                {
                    purchase1Pk += Convert.ToDouble((item.SecondQty * item.SecondPrice));
                }

                dictWeeklySum.Add(localizer.Get("PKRPurchase"), Convert.ToDouble(purchase1Pk));


                var purchase1Us = 0.0;
                var itempurchaseUs = db.Purchase.Where(r => r.Date >= monthStart && r.Date <= monthEnd && r.CurrencyId == 3 && r.Status != "Returned").ToList();

                foreach (var item in itempurchaseUs)
                {
                    purchase1Us += Convert.ToDouble((item.SecondQty * item.SecondPrice));
                }

                dictWeeklySum.Add(localizer.Get("USDPurchase"), Convert.ToDouble(purchase1Us));


                var paidAf = db.CustomerDeal.Where
                    (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 1)
                    .Select(cat => cat.Credit)
                    .Sum();

                dictWeeklySum.Add(localizer.Get("AFNCredit"), Convert.ToDouble(paidAf));

                var paidPk = db.CustomerDeal.Where
                     (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 2)
                     .Select(cat => cat.Credit)
                     .Sum();

                dictWeeklySum.Add(localizer.Get("PKRCredit"), Convert.ToDouble(paidPk));

                var paidUsd = db.CustomerDeal.Where
                        (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 3)
                        .Select(cat => cat.Credit)
                        .Sum();


                dictWeeklySum.Add(localizer.Get("USDCredit"), Convert.ToInt32(paidUsd));



                var debitAf = db.DealerDeal.Where
                     (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 1)
                    .Select(cat => cat.Debit)
                    .Sum();


                dictWeeklySum.Add(localizer.Get("AFNDebit"), Convert.ToDouble(debitAf));

                var debitPk = db.DealerDeal.Where
                       (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 2)
                        .Select(cat => cat.Debit)
                        .Sum();


                dictWeeklySum.Add(localizer.Get("PKRDebit"), Convert.ToDouble(debitPk));

                var debitUsd = db.DealerDeal.Where
                    (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 3)
                    .Select(cat => cat.Debit)
                    .Sum();


                dictWeeklySum.Add(localizer.Get("USDDebit"), Convert.ToDouble(debitUsd));

                return new JsonResult(dictWeeklySum);

            }
            return View("Error");


        }

        public JsonResult GetWeeklyGeneral()
        {

            var category = db.ExpenceType.ToList();
            Dictionary<string, double> dictWeeklySum = new Dictionary<string, double>();

            var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);

            var monthStart = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day - 1));
            var monthEnd = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day)).AddDays(last);


            var expense = db.Expence.Where
                 (cat => cat.ExpenceDate >= monthStart && cat.ExpenceDate <= monthEnd)
                 .Select(cat => cat.ExpenceAmount)
                    .Sum();

            var salaries = db.SalaryPayment.Where
                 (cat => cat.PaidDate >= monthStart && cat.PaidDate <= monthEnd)
                 .Select(cat => cat.PaidAmount)
                 .Sum();

           
            var amount = Convert.ToDouble(expense + salaries);

            dictWeeklySum.Add(localizer.Get("Expense"), amount);



            var itemsaleAf = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 1 && r.SaleState != "Returned").ToList();
            var sale1Af = 0.0;
            foreach (var item in itemsaleAf)
            {
                sale1Af += (Convert.ToDouble(item.Quantity * item.SalePrice));
            }



            dictWeeklySum.Add(localizer.Get("AFNSale"), Convert.ToDouble(sale1Af));

            var sale1Pk = 0.0;
            var itemsalePk = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 2 && r.SaleState != "Returned").ToList();
            foreach (var item in itemsalePk)
            {
                sale1Pk += (Convert.ToDouble(item.Quantity * item.SalePrice));
            }

            dictWeeklySum.Add(localizer.Get("PKRSale"), Convert.ToDouble(sale1Pk));

            var sale1Us = 0.0;
            var itemsaleUs = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 3 && r.SaleState != "Returned").ToList();

            foreach (var item in itemsaleUs)
            {
                sale1Us += (Convert.ToDouble(item.Quantity * item.SalePrice));
            }
            dictWeeklySum.Add(localizer.Get("USDSale"), Convert.ToDouble(sale1Us));

            var purchase1Af = 0.0;
            var itempurchaseAf = db.Purchase.Where(r => r.Date >= monthStart && r.Date <= monthEnd && r.CurrencyId == 1 && r.Status != "Returned").ToList();

            foreach (var item in itempurchaseAf)
            {
                purchase1Af += Convert.ToDouble((item.SecondQty * item.SecondPrice));
            }

            dictWeeklySum.Add(localizer.Get("AFNPurchase"), Convert.ToDouble(purchase1Af));

            var purchase1Pk = 0.0;
            var itempurchasePk = db.Purchase.Where(r => r.Date >= monthStart && r.Date <= monthEnd && r.CurrencyId == 2 && r.Status != "Returned").ToList();

            foreach (var item in itempurchasePk)
            {
                purchase1Pk += Convert.ToDouble((item.SecondQty * item.SecondPrice));
            }

            dictWeeklySum.Add(localizer.Get("PKRPurchase"), Convert.ToDouble(purchase1Pk));


            var purchase1Us = 0.0;
            var itempurchaseUs = db.Purchase.Where(r => r.Date >= monthStart && r.Date <= monthEnd && r.CurrencyId == 3 && r.Status != "Returned").ToList();

            foreach (var item in itempurchaseUs)
            {
                purchase1Us += Convert.ToDouble((item.SecondQty * item.SecondPrice));
            }

            dictWeeklySum.Add(localizer.Get("USDPurchase"), Convert.ToDouble(purchase1Us));


            var paidAf = db.CustomerDeal.Where
                (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 1)
                .Select(cat => cat.Credit)
                .Sum();

            dictWeeklySum.Add(localizer.Get("AFNCredit"), Convert.ToDouble(paidAf));

            var paidPk = db.CustomerDeal.Where
                 (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 2)
                 .Select(cat => cat.Credit)
                 .Sum();

            dictWeeklySum.Add(localizer.Get("PKRCredit"), Convert.ToDouble(paidPk));

            var paidUsd = db.CustomerDeal.Where
                    (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 3)
                    .Select(cat => cat.Credit)
                    .Sum();


            dictWeeklySum.Add(localizer.Get("USDCredit"), Convert.ToInt32(paidUsd));



            var debitAf = db.DealerDeal.Where
                 (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 1)
                .Select(cat => cat.Debit)
                .Sum();


            dictWeeklySum.Add(localizer.Get("AFNDebit"), Convert.ToDouble(debitAf));

            var debitPk = db.DealerDeal.Where
                   (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 2)
                    .Select(cat => cat.Debit)
                    .Sum();


            dictWeeklySum.Add(localizer.Get("PKRDebit"), Convert.ToDouble(debitPk));

            var debitUsd = db.DealerDeal.Where
                (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 3)
                .Select(cat => cat.Debit)
                .Sum();


            dictWeeklySum.Add(localizer.Get("USDDebit"), Convert.ToDouble(debitUsd));

            return new JsonResult(dictWeeklySum);
        }

        public JsonResult GetYearGeneral()
        {


            Dictionary<string, double> dictWeeklySum = new Dictionary<string, double>();

            var monthStart = DateTime.Now.Date.AddYears(-1).AddMonths(-(DateTime.Now.Date.Month - 1)).AddDays(-(DateTime.Now.Date.Day - 1));
            var monthEnd = DateTime.Now.Date.AddYears(-1).AddMonths(-(DateTime.Now.Date.Month - 1)).AddMonths(11).AddDays(-(DateTime.Now.Date.Day)).AddDays(31);




            var expense = db.Expence.Where
                  (cat => cat.ExpenceDate >= monthStart && cat.ExpenceDate <= monthEnd)
                    .Select(cat => cat.ExpenceAmount)
                    .Sum();

            var salaries = db.SalaryPayment.Where
                    (cat => cat.PaidDate >= monthStart && cat.PaidDate <= monthEnd)
                     .Select(cat => cat.PaidAmount)
                     .Sum();

            var amount = Convert.ToDouble(expense + salaries);

            dictWeeklySum.Add(localizer.Get("Expense"), amount);


            var itemsaleAf = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 1 && r.SaleState != "Returned").ToList();
            var sale1Af = 0.0;
            foreach (var item in itemsaleAf)
            {
                sale1Af += (Convert.ToDouble(item.Quantity * item.SalePrice));
            }

            dictWeeklySum.Add(localizer.Get("AFNSale"), Convert.ToDouble(sale1Af));

            var sale1Pk = 0.0;
            var itemsalePk = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 2 && r.SaleState != "Returned").ToList();
            foreach (var item in itemsalePk)
            {
                sale1Pk += (Convert.ToDouble(item.Quantity * item.SalePrice));
            }

            dictWeeklySum.Add(localizer.Get("PKRSale"), Convert.ToDouble(sale1Pk));

            var sale1Us = 0.0;
            var itemsaleUs = db.Sale.Where(r => r.SaleDate >= monthStart && r.SaleDate <= monthEnd && r.CurrencyId == 3 && r.SaleState != "Returned").ToList();
            foreach (var item in itemsaleUs)
            {
                sale1Us += (Convert.ToDouble(item.Quantity * item.SalePrice));
            }
            dictWeeklySum.Add(localizer.Get("USDSale"), Convert.ToDouble(sale1Us));

            var purchase1Af = 0.0;
            var itempurchaseAf = db.Purchase.Where(r => r.Date >= monthStart && r.Date <= monthEnd && r.CurrencyId == 1 && r.Status != "Returned").ToList();

            foreach (var item in itempurchaseAf)
            {
                purchase1Af += Convert.ToDouble((item.SecondQty * item.SecondPrice));
            }

            dictWeeklySum.Add(localizer.Get("AFNPurchase"), Convert.ToDouble(purchase1Af));

            var purchase1Pk = 0.0;
            var itempurchasePk = db.Purchase.Where(r => r.Date >= monthStart && r.Date <= monthEnd && r.CurrencyId == 2 && r.Status != "Returned").ToList();

            foreach (var item in itempurchasePk)
            {
                purchase1Pk += Convert.ToDouble((item.SecondQty * item.SecondPrice));
            }

            dictWeeklySum.Add(localizer.Get("PKRPurchase"), Convert.ToDouble(purchase1Pk));


            var purchase1Us = 0.0;
            var itempurchaseUs = db.Purchase.Where(r => r.Date >= monthStart && r.Date <= monthEnd && r.CurrencyId == 3 && r.Status != "Returned").ToList();

            foreach (var item in itempurchaseUs)
            {
                purchase1Us += Convert.ToDouble((item.SecondQty * item.SecondPrice));
            }

            dictWeeklySum.Add(localizer.Get("USDPurchase"), Convert.ToDouble(purchase1Us));

            var paidAf = db.CustomerDeal.Where
                (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 1 )
                .Select(cat => cat.Credit)
                .Sum();

            dictWeeklySum.Add(localizer.Get("AFNCredit"), Convert.ToDouble(paidAf));

            var paidPk = db.CustomerDeal.Where
                 (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 2)
                 .Select(cat => cat.Credit)
                 .Sum();

            dictWeeklySum.Add(localizer.Get("PKRCredit"), Convert.ToDouble(paidPk));

            var paidUsd = db.CustomerDeal.Where
                    (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 3 )
                    .Select(cat => cat.Credit)
                    .Sum();


            dictWeeklySum.Add(localizer.Get("USDCredit"), Convert.ToInt32(paidUsd));



            var debitAf = db.DealerDeal.Where
                 (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 1)
                .Select(cat => cat.Debit)
                .Sum();


            dictWeeklySum.Add(localizer.Get("AFNDebit"), Convert.ToDouble(debitAf));

            var debitPk = db.DealerDeal.Where
                   (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 2)
                    .Select(cat => cat.Debit)
                    .Sum();


            dictWeeklySum.Add(localizer.Get("PKRDebit"), Convert.ToDouble(debitPk));

            var debitUsd = db.DealerDeal.Where
                (cat => cat.Date >= monthStart && cat.Date <= monthEnd && cat.CurrencyId == 3)
                .Select(cat => cat.Debit)
                .Sum();


            dictWeeklySum.Add(localizer.Get("USDDebit"), Convert.ToDouble(debitUsd));

            return new JsonResult(dictWeeklySum);
        }

        public JsonResult GetDailyGeneral()
        {

            Dictionary<string, double> dictWeeklySum = new Dictionary<string, double>();


            var expense = db.Expence.Where
         (cat => cat.ExpenceDate == DateTime.Now.Date)
         .Select(cat => cat.ExpenceAmount)
         .Sum();

            var salaries = db.SalaryPayment.Where
          (cat => cat.PaidDate == DateTime.Now.Date)
          .Select(cat => cat.PaidAmount)
          .Sum();


            var amount = Convert.ToDouble(expense + salaries);

            dictWeeklySum.Add(localizer.Get("Expense"), amount);


            var itemsaleAf = db.Sale.Where(r => r.SaleDate == DateTime.Now.Date && r.CurrencyId == 1 && r.SaleState != "Returned").ToList();
            var sale1Af = 0.0;

            foreach (var item in itemsaleAf)
            {
                sale1Af += Convert.ToDouble(item.Quantity * item.SalePrice);
            }

            dictWeeklySum.Add(localizer.Get("AFNSale"), Convert.ToDouble(sale1Af));

            var sale1Pk = 0.0;
            var itemsalePk = db.Sale.Where(r => r.SaleDate == DateTime.Now.Date && r.CurrencyId == 2 && r.SaleState != "Returned").ToList();

            foreach (var item in itemsalePk)
            {
                sale1Pk += Convert.ToDouble(item.Quantity * item.SalePrice);
            }

            dictWeeklySum.Add(localizer.Get("PKRSale"), Convert.ToDouble(sale1Pk));


            var sale1Us = 0.0;
            var itemsaleUs = db.Sale.Where(r => r.SaleDate == DateTime.Now.Date && r.CurrencyId == 3 && r.SaleState != "Returned").ToList();

            foreach (var item in itemsaleUs)
            {
                sale1Us += Convert.ToDouble(item.Quantity * item.SalePrice);
            }
            dictWeeklySum.Add(localizer.Get("USDSale"), Convert.ToDouble(sale1Us));

            var purchase1Af = 0.0;

            var itempurchaseAf = db.Purchase.Where(r => r.Date == DateTime.Now.Date && r.CurrencyId == 1 && r.Status != "Returned").ToList();

            foreach (var item in itempurchaseAf)
            {
                purchase1Af += Convert.ToDouble((item.SecondQty * item.SecondPrice));
            }

            dictWeeklySum.Add(localizer.Get("AFNPurchase"), Convert.ToDouble(purchase1Af));

            var purchase1Pk = 0.0;
            var itempurchasePk = db.Purchase.Where(r => r.Date == DateTime.Now.Date && r.CurrencyId == 2 && r.Status != "Returned").ToList();

            foreach (var item in itempurchasePk)
            {
                purchase1Pk += Convert.ToDouble((item.SecondQty * item.SecondPrice));
            }

            dictWeeklySum.Add(localizer.Get("PKRPurchase"), Convert.ToDouble(purchase1Pk));


            var purchase1Us = 0.0;
            var itempurchaseUs = db.Purchase.Where(r => r.Date == DateTime.Now.Date && r.CurrencyId == 3 && r.Status != "Returned").ToList();

            foreach (var item in itempurchaseUs)
            {
                purchase1Us += Convert.ToDouble((item.SecondQty * item.SecondPrice));
            }

            dictWeeklySum.Add(localizer.Get("USDPurchase"), Convert.ToDouble(purchase1Us));

            var paidAf = db.CustomerDeal.Where
                (cat => cat.Date == DateTime.Now.Date && cat.CurrencyId == 1 )
                .Select(cat => cat.Credit)
                .Sum();


            dictWeeklySum.Add(localizer.Get("AFNCredit"), Convert.ToDouble(paidAf));

            var paidPk = db.CustomerDeal.Where
                    (cat => cat.Date == DateTime.Now.Date && cat.CurrencyId == 2 )
                    .Select(cat => cat.Credit)
                    .Sum();


            dictWeeklySum.Add(localizer.Get("PKRCredit"), Convert.ToDouble(paidPk));

            var paidUsd = db.CustomerDeal.Where
                (cat => cat.Date == DateTime.Now.Date && cat.CurrencyId == 3 )
                .Select(cat => cat.Credit)
                .Sum();


            dictWeeklySum.Add(localizer.Get("USDCredit"), Convert.ToDouble(paidUsd));


            var debitAf = db.DealerDeal.Where
               (cat => cat.Date == DateTime.Now.Date && cat.CurrencyId == 1)
               .Select(cat => cat.Debit)
               .Sum();


            dictWeeklySum.Add(localizer.Get("AFNDebit"), Convert.ToDouble(debitAf));

            var debitPk = db.DealerDeal.Where
                    (cat => cat.Date == DateTime.Now.Date && cat.CurrencyId == 2)
                    .Select(cat => cat.Debit)
                    .Sum();


            dictWeeklySum.Add(localizer.Get("PKRDebit"), Convert.ToDouble(debitPk));

            var debitUsd = db.DealerDeal.Where
                (cat => cat.Date == DateTime.Now.Date && cat.CurrencyId == 3)
                .Select(cat => cat.Debit)
                .Sum();


            dictWeeklySum.Add(localizer.Get("USDDebit"), Convert.ToDouble(debitUsd));

            return new JsonResult(dictWeeklySum);
        }

        public IActionResult PurchasedItems()
        {
            return View();
        }

        public IActionResult ManualReprotP(ManulReprotViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = db.Category.ToList();
                Dictionary<string, int> dictMonthlySum = new Dictionary<string, int>();
                foreach (var item in category)
                {
                    var amount = db.Sale.Where
                   (cat => cat.SaleDate >= model.FromDate && cat.SaleDate <= model.ToDate && cat.SaleState != "Returned"/* && cat.ExpenceTypeId == item.ExpenceTypeId*/)
                   .Select(cat => cat.SalePrice)
                   .Sum();

                    dictMonthlySum.Add(item.Category1, 1);
                }
                return new JsonResult(dictMonthlySum);
            }
            return View("Error");


        }

        public JsonResult GetWeeklyExpenseP()
        {
            // List<ExpenseReport> lstEmployee = new List<ExpenseReport>();\
            var category = db.ExpenceType.ToList();
            Dictionary<string, int> dictWeeklySum = new Dictionary<string, int>();
            var last = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);

            var monthStart = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day - 1));
            var monthEnd = DateTime.Now.Date.AddMonths(-1).AddDays(-(DateTime.Now.Date.Day)).AddDays(last);

            foreach (var item in category)
            {
                var amount = db.Expence.Where
               (cat => cat.ExpenceDate >= monthStart && monthEnd <= cat.ExpenceDate && cat.ExpenceTypeId == item.ExpenceTypeId)
               .Select(cat => cat.ExpenceAmount)
               .Sum();

                dictWeeklySum.Add(item.ExpenceType1, Convert.ToInt32(amount));
            }

            return new JsonResult(dictWeeklySum);
        }

        public JsonResult GetYearExpenseP()
        {
            var monthStart = DateTime.Now;
            var monthEnd = DateTime.Now;
            var category = db.ExpenceType.ToList();
            Dictionary<string, int> yearDate = new Dictionary<string, int>();
            foreach (var item in category)
            {
                monthStart = DateTime.Now.AddYears(-1).AddMonths(-(DateTime.Now.Month - 1)).AddDays(-(DateTime.Now.Day - 1));
                monthEnd = DateTime.Now.AddYears(-1).AddMonths(-(DateTime.Now.Month)).AddMonths(12).AddDays(+(DateTime.Now.Day + 1));
                var amount = db.Expence.Where
               (cat => cat.ExpenceDate >= monthStart && cat.ExpenceDate <= monthEnd && cat.ExpenceTypeId == item.ExpenceTypeId)
               .Select(cat => cat.ExpenceAmount)
               .Sum();

                yearDate.Add(item.ExpenceType1, Convert.ToInt32(amount));
            }

            return new JsonResult(yearDate);
        }
        [Authorize(Roles = ("Admin Department"))]
        public JsonResult GetDailyExpenseP()
        {

            var category = db.ExpenceType.ToList();
            Dictionary<string, int> dictMonthlySum = new Dictionary<string, int>();
            foreach (var item in category)
            {
                var amount = db.Expence.Where
               (cat => cat.ExpenceDate == DateTime.Now.Date && cat.ExpenceTypeId == item.ExpenceTypeId)
               .Select(cat => cat.ExpenceAmount)
               .Sum();

                dictMonthlySum.Add(item.ExpenceType1, Convert.ToInt32(amount));
            }


            return new JsonResult(dictMonthlySum);
        }
        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Benefit()
        {
            var currency = (from d in db.Currency
                            select new SelectListItem()
                            {
                                Text = d.Currency1,
                                Value = d.CurrencyId.ToString()
                            }).ToList();
            ViewBag.currency = currency;

            return View();
        }

        public JsonResult FetchFinanceReport(int reportType, int currencyId)
        {

            var sale = db.Sale.Where(x => x.SaleId > 0 && x.CurrencyId == currencyId && x.SaleState != "Returned");

            switch (reportType)
            {
                //////////////////////////// todays report ///////////////
                case 1:

                    sale = db.Sale.Where(x => x.SaleDate == DateTime.Now.Date && x.CurrencyId == currencyId && x.SaleState != "Returned");

                    break;
                ///////////////////////// current month's report ///////////
                case 2:
                    var currentMonth = DateTime.Now.Month;
                    sale = db.Sale.Where(x => x.SaleDate.Value.Month == currentMonth && x.CurrencyId == currencyId && x.SaleDate.Value.Year == DateTime.Now.Year && x.SaleState != "Returned");
                    break;
                /////////////////////////   last month report ////////////////////////
                case 3:
                    sale = db.Sale.Where(x => x.SaleDate.Value.Month == (DateTime.Now.Month - 1) && x.CurrencyId == currencyId && x.SaleDate.Value.Year == DateTime.Now.Year && x.SaleState != "Returned");

                    break;
                /////////////////////////  Current Year's report  ////////////
                case 4:
                    var currentYear = DateTime.Now.Year;

                    sale = db.Sale.Where(x => x.SaleDate.Value.Year == currentYear && x.CurrencyId == currencyId && x.SaleState != "Returned");
                    break;
                ////////////////////////   Last year's report ///////////////
                case 5:
                    sale = db.Sale.Where(x => x.SaleDate.Value.Year == (DateTime.Now.Year - 1) && x.CurrencyId == currencyId && x.SaleState != "Returned");
                    break;
            }




            return Json(sale);
        }

        [Authorize(Roles = "Admin Department")]
        public IActionResult ManualReprot(AllViewModel model, string BillNo ,int currencyId)
        {
            var sale = db.Sale.Where(x => x.SaleDate >= model.manulReport.FromDate.Date && x.CurrencyId == currencyId && x.SaleDate <=
                                      model.manulReport.ToDate.Date && x.SaleState != "Returned");

            return Json(sale);

        }
    }
}
