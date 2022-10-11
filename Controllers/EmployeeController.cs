using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShawkanyDb.Models;
using ShawkanyDb.Models.Model;
using ShawkanyDb.Models.ViewModels;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ShawkanyDb.Controllers
{

    public class EmployeeController : Controller
    {

        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration configuration;

        [Obsolete]
        public EmployeeController(IWebHostEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this.hostingEnvironment = hostingEnvironment;


            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            configuration = builder.Build();

        }


        ShawkanyDbContext db = new ShawkanyDbContext();
        [HttpGet]
        [Authorize(Roles = "Admin Department")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin Department")]
        public IActionResult Employee()
        {
            if (TempData["updated"] != null)
            {
                ViewBag.Alert = TempData["updated"];
            }
            else
            {
                ViewBag.Alert = "empty";
            }

            return View();
        }
        public JsonResult LoadEmployee()
        {
            var rec = db.Employee.ToList().OrderByDescending(d => d.EmployeeId);
            return Json(rec);
        }
        [Authorize(Roles = "Admin Department")]
        public IActionResult LoadEmployeeSalary()
        {
            try
            {
                var rec = (from e in db.Employee
                           select new
                           {
                               id = e.EmployeeId,
                               salaryId = db.SalaryPayment.Where(x => x.EmployeeId == e.EmployeeId).OrderByDescending(x => x.SalaryId).Select(r => r.SalaryId).FirstOrDefault(),
                               imagePath = e.Image,
                               name = e.Name,
                               overtime = db.OverTime.Where(x => x.EmployeeId == e.EmployeeId && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year).Sum(x => x.OverTimeHours) == 0 ? 0.0 : db.OverTime.Where(x => x.EmployeeId == e.EmployeeId && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year).Sum(x => x.OverTimeHours),
                               phone = e.Phone,
                               hireDate = Convert.ToDateTime(e.HireDate).ToShortDateString(),
                               email = e.Email,
                               address = e.Address,
                               salary = e.Salary,
                               paidAmount = Convert.ToDouble(db.SalaryPayment.Where(x => x.EmployeeId == e.EmployeeId).OrderByDescending(x => x.SalaryId).Select(r => r.PaidAmount).FirstOrDefault()),
                               totalMonths = Math.Floor(DateTime.Now.Date.Subtract(Convert.ToDateTime(e.HireDate)).TotalDays / 30),
                               totalPaid = Convert.ToDouble(db.SalaryPayment.Where(p => p.EmployeeId == e.EmployeeId).Sum(d => d.PaidAmount).ToString()),
                               totalSalary = e.Salary * Math.Floor(DateTime.Now.Date.Subtract(Convert.ToDateTime(e.HireDate)).TotalDays / 30),
                               totalRemain = (Convert.ToDouble(e.Salary * Math.Floor((DateTime.Now.Date.Subtract(Convert.ToDateTime(e.HireDate)).TotalDays)) / 30)) - db.SalaryPayment.Where(p => p.EmployeeId == e.EmployeeId).Sum(d => d.PaidAmount),
                               paidDate = db.SalaryPayment.Where(x => x.EmployeeId == e.EmployeeId).OrderByDescending(x => x.SalaryId).Select(r => r.PaidDate).FirstOrDefault()
                           }).ToList().GroupBy(x => x.id).Select(r => r.LastOrDefault());
                return Json(rec);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }

        }

        [HttpPost]
        [Authorize(Roles = "Admin Department")]
        public async Task<IActionResult> EmployeeRegistration(AllViewModel model, [FromForm] IFormFile img)
        {
            if (ModelState.IsValid)
            {
                string uploadPath = "";
                string upload = "";
                if (img == null)
                {
                    if (model.employee.EmployeeId == 0)
                        upload = "/images/StaticImages/browse.png";
                }
                else
                {


                    var fileName = model.employee.Email + Guid.NewGuid().ToString().Replace("_", "") + Path.GetExtension(img.FileName);
                    upload = Path.Combine("Images/EmployeeImage/", fileName);
                    uploadPath = Path.Combine(hostingEnvironment.WebRootPath, upload);
                    img.CopyTo(new FileStream(uploadPath, FileMode.Create));
                    upload = "/" + upload;

                }


                if (model.employee.EmployeeId != 0)
                {
                    var user1 = await _userManager.FindByEmailAsync(model.employee.Email);
                    string ro = "";
                    if (user1 != null)
                    {
                        var role = await _userManager.GetRolesAsync(user1);
                        ro = role[0].ToString();
                        var logeduser = await _userManager.GetUserAsync(User);
                        var rol = await _userManager.GetRolesAsync(logeduser);
                        string r = rol[0].ToString();
                        if (ro == "Admin Department")
                        {
                            if (r == "Admin Department")
                            {
                                var rec1 = await db.Employee.FindAsync(model.employee.EmployeeId);
                                if (rec1 == null)
                                {
                                    return View("Error");
                                }
                                rec1.Name = model.employee.Name;
                                rec1.FatherName = model.employee.FatherName;
                                rec1.Address = model.employee.Address;
                                rec1.Phone = model.employee.Phone.ToString();
                                rec1.Email = model.employee.Email;
                                rec1.HireDate = model.employee.HireDate;
                                rec1.Salary = model.employee.Salary;

                                var user2 = await _userManager.FindByEmailAsync(model.employee.oldEmail);

                                if (img == null && model.employee.defalut != null)
                                {
                                    rec1.Image = model.employee.defalut;
                                }
                                else { rec1.Image = upload; }


                                db.Entry(rec1).State = EntityState.Modified;
                                if (img != null && model.employee.defalut == "0")
                                {
                                    db.Entry(rec1).Property(d => d.Image).IsModified = true;
                                    if (user2 != null)
                                        user2.ImagePath = upload;
                                }
                                else if (model.employee.defalut != "0")
                                {
                                    db.Entry(rec1).Property(d => d.Image).IsModified = true;
                                    if (user2 != null)
                                        user2.ImagePath = model.employee.defalut;
                                }
                                if (user2 != null)
                                {


                                    var result = await _userManager.SetEmailAsync(user2, model.employee.Email);
                                    await _userManager.SetUserNameAsync(user2, model.employee.Email);
                                    if (result.Succeeded)
                                    {
                                        user2.EmployeeName = model.employee.Name;
                                        user2.EmailConfirmed = true;
                                        await _userManager.UpdateAsync(user2);
                                    }
                                    else
                                    {
                                        return View("Error");
                                    }

                                }

                                await db.SaveChangesAsync();



                                TempData["updated"] = "2";
                                // ViewBag.Alert = "";
                                return RedirectToAction("Employee", "Employee");
                            }
                            else
                            {
                                return View("Error");
                            }

                        }
                    }


                    var rec = await db.Employee.FindAsync(model.employee.EmployeeId);
                    if (rec == null)
                    {
                        return View("Error");
                    }
                    rec.Name = model.employee.Name;
                    rec.FatherName = model.employee.FatherName;
                    rec.Address = model.employee.Address;
                    rec.Phone = model.employee.Phone.ToString();
                    rec.Email = model.employee.Email;
                    rec.HireDate = model.employee.HireDate;
                    rec.Salary = model.employee.Salary;

                    var user = await _userManager.FindByEmailAsync(model.employee.oldEmail);

                    if (img == null && model.employee.defalut != null)
                    {
                        rec.Image = model.employee.defalut;
                    }
                    else { rec.Image = upload; }


                    db.Entry(rec).State = EntityState.Modified;
                    if (img != null && model.employee.defalut == "0")
                    {
                        db.Entry(rec).Property(d => d.Image).IsModified = true;
                        if (user != null)
                            user.ImagePath = upload;
                    }
                    else if (model.employee.defalut != "0")
                    {
                        db.Entry(rec).Property(d => d.Image).IsModified = true;
                        if (user != null)
                            user.ImagePath = model.employee.defalut;
                    }
                    if (user != null)
                    {


                        var result = await _userManager.SetEmailAsync(user, model.employee.Email);
                        await _userManager.SetUserNameAsync(user, model.employee.Email);
                        if (result.Succeeded)
                        {
                            user.EmployeeName = model.employee.Name;
                            user.EmailConfirmed = true;
                            await _userManager.UpdateAsync(user);
                        }
                        else
                        {
                            return View("Error");
                        }

                    }
                    await db.SaveChangesAsync();
                    TempData["updated"] = "1";
                    return RedirectToAction("Employee", "Employee");
                }
                else
                {
                    try
                    {


                        Employee it = new Employee()
                        {
                            Name = model.employee.Name,
                            FatherName = model.employee.FatherName,
                            Address = model.employee.Address,
                            Phone = model.employee.Phone.ToString(),
                            Email = model.employee.Email,
                            HireDate = Convert.ToDateTime(model.employee.HireDate.ToShortDateString()).Date,
                            Salary = model.employee.Salary,
                            Image = upload
                        };
                        db.Employee.Add(it);

                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.Message);
                    }
                    TempData["updated"] = " ریکارډ ذخیره شو";
                    return RedirectToAction("Employee", "Employee");
                }

            }
            return View("Error");

        }

        [Authorize(Roles = "Admin Department")]
        public async Task<IActionResult> DeleteEmployee(AllViewModel model)
        {
            if (model.employee.EmployeeId != 0)
            {
                var comp = await db.Employee.FindAsync(model.employee.EmployeeId);
                if (comp == null)
                {
                    return View("Error");
                }

                try
                {
                    var recs = db.Note.Where(d => d.UserId == model.employee.Email);
                    if (recs != null)
                        db.Note.RemoveRange(recs);
                    var account = await _userManager.FindByEmailAsync(model.employee.Email);
                    if (account != null)
                    {
                        var result = await _userManager.DeleteAsync(account);
                        if (!result.Succeeded)
                        {
                            return View("Error");
                        }
                    }

                    db.Employee.Remove(comp);
                    await db.SaveChangesAsync();
                    return Json("1");
                }
                catch (Exception)
                {

                    return Json("2");
                }
            }
            return NotFound();

        }

        public IActionResult IsEmployeeEmailExist(AllViewModel model)
        {
            if (model.employee.EmployeeId != 0)
            {
                var em = db.Employee.Where(d => d.Email == model.employee.Email && d.EmployeeId != model.employee.EmployeeId).ToList().Count();
                if (em > 0)
                {
                    return Json(false);

                }
                else
                {
                    return Json(true);
                }
            }
            var rec = db.Employee.Any(d => d.Email == model.employee.Email);

            if (!rec)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [Authorize(Roles = "Admin Department")]
        public async Task<IActionResult> AddEmployeeSalary(AllViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.salary.SalaryId != 0)
                {
                    var rec = await db.SalaryPayment.FindAsync(model.salary.SalaryId);
                    if (rec == null)
                    {
                        return View("Error");
                    }

                    rec.PaidAmount = model.salary.PaidAmount;

                    db.Entry(rec).State = EntityState.Modified;
                    db.Entry(rec).Property(d => d.PaidAmount).IsModified = true;

                    var id = model.salary.SalaryId + "," + "salary";
                    var rec4 = db.CashInHand.Where(x => x.Description == id).FirstOrDefault();
                    rec4.Debit = model.salary.PaidAmount;
                    rec4.Date = DateTime.Now.Date;
                    db.Entry(rec4).State = EntityState.Modified;
                    db.Entry(rec4).Property(x => x.Debit).IsModified = true;
                    db.Entry(rec4).Property(x => x.Date).IsModified = true;


                    await db.SaveChangesAsync();
                    return Json("2");

                }
                else
                {
                    try
                    {

                        var email = _userManager.GetUserAsync(User).Result.Email;
                        var empId = db.Employee.Where(e => e.Email == email).Select(r => r.EmployeeId).FirstOrDefault();

                        SalaryPayment it = new SalaryPayment()
                        {

                            PaidAmount = model.salary.PaidAmount,
                            PaidDate = DateTime.Now.Date,
                            EmployeeId = model.salary.EmployeeId,
                            PaidBy = empId
                        };
                        db.SalaryPayment.Add(it);
                        await db.SaveChangesAsync();

                        var exId = db.SalaryPayment.OrderByDescending(x => x.SalaryId).FirstOrDefault();
                        CashInHand ch = new CashInHand()
                        {
                            Debit = model.salary.PaidAmount,
                            Credit = 0,
                            Date = DateTime.Now.Date,
                            CurrencyId = 1,
                            Description = exId.SalaryId.ToString() + "," + "salary"
                        };
                        db.CashInHand.Add(ch);
                        await db.SaveChangesAsync();

                        return Json("1");
                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.StackTrace);
                    }
                    // TempData["updated"] = " ریکارډ ذخیره شو";

                }

            }
            return View("Error");

        }


        [Authorize(Roles = "Admin Department")]
        public IActionResult LoadEmployeeAllPayments(AllViewModel model)
        {
            if (model.employee.EmployeeId != 0)
            {
                var comp = (from s in db.SalaryPayment
                            where s.EmployeeId == model.employee.EmployeeId
                            select new
                            {
                                paidDate = Convert.ToDateTime(s.PaidDate).Date.ToShortDateString(),
                                paidAmount = s.PaidAmount
                            }).ToList();
                if (comp == null)
                {
                    return View("Error");
                }

                try
                {

                    return Json(comp);
                }
                catch (Exception)
                {

                    return Json("نوموړی کارمند ته معاش نه دی ورکړل شویږ");
                }
            }
            return NotFound();

        }
       
        [HttpGet]
        [Authorize(Roles = "Admin Department,Finance Department,HR Department")]
        public IActionResult Notes()
        {
            return View();
        }

        public async Task<JsonResult> AddNote(NoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = _userManager.GetUserAsync(User).Result.Email;


                if (model.NoteId == 0)
                {
                    Note nt = new Note()
                    {
                        Note1 = model.Note,
                        TargetDate = Convert.ToDateTime(model.TargetDate.ToShortDateString()),
                        RemindDate = Convert.ToDateTime(model.RemindDate.ToShortDateString()),
                        UserId = email,
                        NoteStatus = "Active"
                    };
                    db.Note.Add(nt);
                    await db.SaveChangesAsync();
                    return Json("1");
                }
                else
                {
                    Note targetedNote = db.Note.Where(x => x.NoteId == model.NoteId).FirstOrDefault();
                    targetedNote.NoteId = model.NoteId;
                    targetedNote.Note1 = model.Note;
                    targetedNote.TargetDate = Convert.ToDateTime(model.TargetDate.ToShortDateString());
                    targetedNote.RemindDate = Convert.ToDateTime(model.RemindDate.ToShortDateString());
                    db.Entry(targetedNote).Property(x => x.NoteStatus).IsModified = false;
                    db.Entry(targetedNote).Property(x => x.UserId).IsModified = false;
                    db.Entry(targetedNote).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json("2");
                }
            }


            return Json("");
        }

        public JsonResult FetchNotes()
        {
            var email = _userManager.GetUserAsync(User).Result.Email;

            var AcitveNotes = (from N in db.Note
                               where N.UserId == email && N.NoteStatus == "Active"

                               select new
                               {
                                   noteid = N.NoteId,
                                   note = N.Note1,
                                   targetdate = N.TargetDate,
                                   targetdateShow = Convert.ToDateTime(N.TargetDate).ToShortDateString(),
                                   reminddate = N.RemindDate,
                                   reminddateShow = Convert.ToDateTime(N.RemindDate).ToShortDateString(),
                                   notestatus = N.NoteStatus,

                               }).ToList();

            return Json(AcitveNotes);
        }

        public JsonResult DeleteNote(Note note)
        {
            var record = db.Note.Find(note.NoteId);
            db.Note.Remove(record);
            db.SaveChanges();
            return Json("3");
        }

        public JsonResult countActiveNotes()
        {
            var email = _userManager.GetUserAsync(User).Result.Email;

            var activeNotes = db.Note.Where(x => x.RemindDate <= DateTime.Now && x.TargetDate >= DateTime.Now && x.UserId == email).ToList();
            return Json(activeNotes.Count());
        }

        public JsonResult LoadActiveNotes()
        {
            var email = _userManager.GetUserAsync(User).Result.Email;

            var activeNotes = from note in db.Note.Where(x => x.RemindDate <= DateTime.Now && x.TargetDate >= DateTime.Now && x.UserId == email)
                              select new
                              {
                                  note = note.Note1,
                                  targetDate = Convert.ToDateTime(note.TargetDate).ToShortDateString()
                              };
            return Json(activeNotes);
        }
    }
}