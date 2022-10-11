using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShawkanyDb.Models;
using ShawkanyDb.Models.AccountViewModels;
using ShawkanyDb.Models.Model;
using ShawkanyDb.Models.ViewModels;
using ShawkanyDb.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShawkanyDb.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {

        ShawkanyDbContext db = new ShawkanyDbContext();

        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        public IConfiguration configuration;


        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
            this.configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        [TempData]
        public string ErrorMessage { get; set; }
        [Authorize(Roles = "Admin Department")]
        [HttpPost]
        public async Task<IActionResult> AddClaimToUser(AllViewModel model, [FromForm] string radio)
        {


            var user = await _userManager.FindByEmailAsync(model.claim.Email);


            if (user == null)
            {
                return NotFound();
            }

            var ClaimsOfUser = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, ClaimsOfUser);

            if (!result.Succeeded)
            {
                return View("Error");
            }

            if (radio == "None")
            {
                return Json("نوموړی کارمند کوم صلاحیت نه لرې");
            }
            if (radio == "View")
            {
                result = await _userManager.AddClaimAsync(user, new Claim("View Role", "View Role"));

            }
            if (radio == "Insert")
            {
                result = await _userManager.AddClaimsAsync(user, new List<Claim>() {
                    new Claim("View Role", "View Role"),
                    new Claim("Insert Role", "Insert Role")
                });
            }
            if (radio == "Edit")
            {
                result = await _userManager.AddClaimsAsync(user, new List<Claim>() {
                    new Claim("View Role", "View Role"),
                    new Claim("Insert Role", "Insert Role"),
                    new Claim("Edit Role", "Edit Role")
                });
            }
            if (radio == "Delete")
            {
                result = await _userManager.AddClaimsAsync(user, new List<Claim>() {
                    new Claim("View Role", "View Role"),
                    new Claim("Insert Role", "Insert Role"),
                    new Claim("Edit Role", "Edit Role"),
                     new Claim("Delete Role", "Delete Role")
                });
            }
            if (!result.Succeeded)
            {
                return View("Error");
            }

            return Json("کارمند ته انتخاب شوی صلاحیت ورکړل شو.");
        }
        [Authorize(Roles = "Admin Department")]
        public IActionResult LoadAccounts()
        {

            var rec = _userManager.Users.ToList();
            if (rec == null)
            {
                return NotFound();
            }
            return Json(rec);
        }
        [Authorize(Roles = "Admin Department")]
        public async Task<IActionResult> ChangeUserRole(AllViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.register.email2);
                if (user == null)
                {
                    return View("Error");
                }
                var role = await _userManager.RemoveFromRoleAsync(user, model.register.role);
                if (!role.Succeeded)
                {
                    return View("Error");
                }

                var assign = await _userManager.AddToRoleAsync(user, model.register.id2);


                return Json("د کارمند رول تغیر شو");

            }
            catch (Exception)
            {

                return Json("اشتباه");
            }

        }
        [Authorize(Roles = "Admin Department")]
        public async Task<IActionResult> LoadUserClaims(AllViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.claim.Email);

            var logs = (from l in db.UserLoginDetail
                        where l.UserId == model.claim.Email
                        select new
                        {
                            email = l.UserId,
                            loginDate = Convert.ToDateTime(l.LoginDate).ToString("yyyy/MM/dd hh:mm:ss"), /*Convert.ToDateTime(l.LoginDate).Date.ToUniversalTime()*/ /*+"-" + Convert.ToDateTime(l.LoginDate).ToLocalTime().ToString("HH:mm:ss")*/
                            logoutDate = Convert.ToDateTime(l.LogoutDate).ToString("yyyy/MM/dd hh:mm:ss"),
                        }).ToList();
            var role = await _userManager.GetRolesAsync(user);


            if (user == null)
                return NotFound();
            var Claims = await _userManager.GetClaimsAsync(user);
            if (Claims == null)
            {
                return Json("NoRoles");
            }
            var rec = new
            {
                claims = Claims.Count(),
                role = role,
                logs = logs
            };
            return Json(rec);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);


            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    UserLoginDetail log = new UserLoginDetail()
                    {
                        UserId = model.Email,
                        LoginDate = DateTime.Now.ToLocalTime(),
                    };
                    db.UserLoginDetail.Add(log);
                    //var role = "";
                    await db.SaveChangesAsync();
                    //if (user != null)
                    //{
                    //    role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                    //}
                    //if (role == "Finance Department")
                    //{
                    //    return RedirectToAction("Deal", "Deal");
                    //}
                    //if (role == "HR Department")
                    //{
                    //    return RedirectToAction("Sale", "Sale");
                    //}

                    return RedirectToLocal(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, " ستاسو اکاونټ د 15 دقیقو لپاره بلاک شو .");
                    return View(model);
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, " ستاسو اکاونټ د ریس لخوا بلاک شوی .");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "بریښنالیک او یا پټ نوم مو غلط دی");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin Department")]
        public async Task<IActionResult> LockAccount(AllViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.register.email);
            if (model.register.trueOrfalse == "lock")
            {
                user.EmailConfirmed = false;
                await _userManager.UpdateAsync(user);
                return Json("true");
            }
            else
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return Json("true");
            }


        }
        [Authorize(Roles = "Admin Department")]
        public async Task<IActionResult> DeleteAccount(AllViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.register.email);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json("اکاونت حذف شو");
            }
            else { return Json("اکاونت حذف نه شو"); }
        }
        [Authorize(Roles = "Admin Department")]
        public IActionResult Register(string returnUrl = null)
        {

            var Employee = (from d in db.Employee
                            select new SelectListItem()
                            {
                                Text = d.Name,
                                Value = d.EmployeeId.ToString()
                            }).ToList();
            ViewBag.Employee = Employee;

            var Roles = (from d in _roleManager.Roles
                         select new SelectListItem()
                         {
                             Text = d.Name,
                             Value = d.Name
                         }).ToList();
            ViewBag.Roles = Roles;
            ViewBag.Roles2 = Roles;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public IActionResult IsEmployeeAccountExist(AllViewModel model)
        {
            var rec = db.Employee.Where(d => d.EmployeeId == model.register.EmployeeId).FirstOrDefault();
            var Email = "";
            if (rec != null)
                Email = rec.Email;
            var user = _userManager.FindByEmailAsync(Email);

            if (user.Result == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin Department")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AllViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var rec = db.Employee.Where(e => e.EmployeeId == model.register.EmployeeId).FirstOrDefault();
                var user = new ApplicationUser { UserName = rec.Email, Email = rec.Email, EmployeeName = rec.Name, ImagePath = rec.Image ,EmpId = rec.EmployeeId};
                var result = await _userManager.CreateAsync(user, model.register.Password);
                if (result.Succeeded)
                {
                    var userToRole = await _userManager.FindByEmailAsync(rec.Email);
                    await _userManager.AddToRoleAsync(userToRole, model.register.id);

                    return Json("1");
                }
                return Json("2");
            }

            return View("Error");
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = _userManager.GetUserAsync(User).Result.Email;
                var rec1 = db.UserLoginDetail.Where(d => d.UserId == userId).ToList().OrderByDescending(r => r.DetailId);
                UserLoginDetail rec;
                if (rec1 != null)
                {
                    rec = rec1.First();
                    if (rec != null)
                    {
                        rec.LogoutDate = DateTime.Now.ToLocalTime();

                        db.Entry(rec).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.Entry(rec).Property(r => r.LogoutDate).IsModified = true;
                        await db.SaveChangesAsync();
                    }
                    await _signInManager.SignOutAsync();
                }

                return RedirectToAction("Login", "Account");

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Authorize(Roles = "Admin Department")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(AllViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(model.forgot.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Json(1);

                }
                if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return Json(2);

                }

                var code = SendCodeToUser(user.EmployeeName, user.Email);
                if (code == 500)
                {
                    return Json(3);
                }
                else
                {

                    return Json(code);
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   $"مهربانی له مخې په دې بټنې کلیک وکړی تر څو مو پټ نوم تغیر کړی.: <a href='{callbackUrl}'>link</a>");

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddNewPassword(AllViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.reset.userEmail);
                await _userManager.RemovePasswordAsync(user);
                var pass = await _userManager.AddPasswordAsync(user, model.reset.NewPassword);
                if (!pass.Succeeded)
                {
                    return Json("پټ نوم باید شپږ حروفه یو غټ حروف یو عدد او یو سیمبول ولرې");
                }
                await _signInManager.SignInAsync(user, isPersistent: false);

                UserLoginDetail log = new UserLoginDetail()
                {
                    UserId = model.reset.userEmail,
                    LoginDate = DateTime.Now.ToLocalTime(),
                };
                db.UserLoginDetail.Add(log);
                await db.SaveChangesAsync();

                return Json("done");


            }

            return RedirectToAction(nameof(ForgotPassword));
        }
        public int SendCodeToUser(string fName, string userEmailOrPhone)
        {
            var systemEmail = configuration["SystemEmail"];
            var key = configuration["key"];
            Random ran = new Random();
            var code = ran.Next(1, 999999);
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(userEmailOrPhone);
                mail.From = new MailAddress("smarthealthconsultation@gmail.com");
                mail.Subject = "Password Reset";
                mail.Body = string.Format("Dear " + fName + "  <br/>  <br/>" + code + "<br/> is your password reset code");
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(systemEmail, key);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return code;
            }
            catch (Exception)
            {

                return 500;
            }

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return View("Error");
                //throw new ApplicationException("کوډ نه بغیر پاسورډ نه شی بدلولی.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult SaveDocuments()
        {
            if (TempData["updated"] != null)
            {
                ViewBag.Alert = TempData["updated"];
            }
            else
            {
                ViewBag.Alert = "empty";
            }

            if (TempData["delete"] != null)
            {
                ViewBag.delete = TempData["delete"];
            }
            else
            {
                ViewBag.delete = "empty";
            }
            ViewBag.Documents = db.Document.ToList();
            return View();
        }

        [Authorize(Roles = "Admin Department")]
        [HttpPost]
        public async Task<IActionResult> SaveDocumentss(DocumentViewModel model, [FromForm] IFormFile img)
        {
            var upload = "";
            var uploadPath = "";

            if (img != null)
            {
                var fileName = Guid.NewGuid().ToString().Replace("_", "") + Path.GetExtension(img.FileName);
                upload = Path.Combine("Images/DocumentImage/", fileName);
                uploadPath = Path.Combine(hostingEnvironment.WebRootPath, upload);
                img.CopyTo(new FileStream(uploadPath, FileMode.Create));
                upload = "/" + upload;
                Document doc = new Document()
                {
                    DocumentDetails = model.DocumentDetails,
                    DocumentPicture = upload,
                };
                db.Document.Add(doc);
                await db.SaveChangesAsync();
                TempData["updated"] = "1";
                return RedirectToAction("SaveDocuments", "Account");
            }
            else { return View("Error"); }
        }
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var rec = db.Document.Find(id);
            if (rec == null)
            {
                return View("Error");
            }
            db.Document.Remove(rec);
            await db.SaveChangesAsync();
            TempData["delete"] = "2";
            return RedirectToAction("SaveDocuments", "Account");
        }
        [AllowAnonymous]
        public async Task<IActionResult> UpdateDatabase()
        {
            IdentityRole finance = new IdentityRole()
            {
                Name = "Finance Department"
            };
            IdentityRole Admin = new IdentityRole()
            {
                Name = "Admin Department"
            };
            IdentityRole Hr = new IdentityRole()
            {
                Name = "HR Department"
            };
            await _roleManager.CreateAsync(finance);
            await _roleManager.CreateAsync(Admin);
            await _roleManager.CreateAsync(Hr);

            //var process = db.ExpenceType.Where(d => d.ExpenceType1 == "د موادو پراسس").FirstOrDefault();
            //var bill = db.ExpenceType.Where(d => d.ExpenceType1 == "د بیل مصارف").FirstOrDefault();

            //if (process == null)
            //{
            //    ExpenceType e = new ExpenceType()
            //    {
            //        ExpenceType1 = "د موادو پراسس"
            //    };
            //    db.ExpenceType.Add(e);
            //    await db.SaveChangesAsync();
            //}

            //if (bill == null)
            //{
            //    ExpenceType e = new ExpenceType()
            //    {
            //        ExpenceType1 = "د بیل مصارف"
            //    };
            //    db.ExpenceType.Add(e);
            //    await db.SaveChangesAsync();
            //}

            var cur1 = db.Currency.Where(c => c.CurrencyId == 1).FirstOrDefault();
            var cur2 = db.Currency.Where(c => c.CurrencyId == 2).FirstOrDefault();
            var cur3 = db.Currency.Where(c => c.CurrencyId == 3).FirstOrDefault();
            if (cur1 == null)
            {
                Currency c1 = new Currency()
                {
                    CurrencyId = 1,
                    Currency1 = "افغاني"
                };
                db.Currency.Add(c1);
                await db.SaveChangesAsync();
            }
            if (cur2 == null)
            {
                Currency c1 = new Currency()
                {
                    CurrencyId = 2,
                    Currency1 = "کلدارې"
                };
                db.Currency.Add(c1);
                await db.SaveChangesAsync();
            }
            if (cur3 == null)
            {
                Currency c1 = new Currency()
                { CurrencyId = 3,
                    Currency1 = "ډالر"
                };
                db.Currency.Add(c1);
                await db.SaveChangesAsync();
            }

            //var dealer1 = db.DealerType.Where(r => r.DealerTypeId == 1).FirstOrDefault();
            //var dealer2 = db.DealerType.Where(r => r.DealerTypeId == 2).FirstOrDefault();
            //var dealer3 = db.DealerType.Where(r => r.DealerTypeId == 3).FirstOrDefault();
            //if (dealer1 == null)
            //{
            //    DealerType c1 = new DealerType()
            //    {
            //        Dealer = "شرکت"
            //    };
            //    db.DealerType.Add(c1);
            //    await db.SaveChangesAsync();
            //}
            //if (dealer2 == null)
            //{
            //    DealerType c1 = new DealerType()
            //    {
            //        Dealer = "مشتري"
            //    };
            //    db.DealerType.Add(c1);
            //    await db.SaveChangesAsync();
            //}
            //if (dealer3 == null)
            //{
            //    DealerType c1 = new DealerType()
            //    {
            //        Dealer = "موقت مشتري"
            //    };
            //    db.DealerType.Add(c1);
            //    await db.SaveChangesAsync();
            //}

            //var dlr = db.Dealler.Where(v => v.DealerId == 1).FirstOrDefault();
            //if (dlr == null)
            //{
            //    Dealler de = new Dealler()
            //    {
            //        DealerName = "موقت مشتري",
            //        DealerTypeId = 3,
            //    };
            //    db.Dealler.Add(de);
            //    await db.SaveChangesAsync();
            //}

            int empId = 1;

            var employee = db.Employee.Where(d => d.Email == "Admin@gmail.com").FirstOrDefault();
            if (employee == null)
            {
                Employee rec = new Employee()
                {
                    Email = "Admin@gmail.com",
                    Image = "/images/StaticImages/Admin.png",
                    Name = "AdminAccount",
                    Salary = 10000,
                };
                db.Employee.Add(rec);
                await db.SaveChangesAsync();
                empId = rec.EmployeeId;
            }

            var users = _userManager.FindByEmailAsync("Admin@gmail.com");
            if (users.Result==null)
            {
                var user = new ApplicationUser { UserName = "Admin@gmail.com", Email = "Admin@gmail.com", ImagePath = "/images/StaticImages/Admin.png", EmployeeName = "admin" ,EmpId = empId};
                var result = await _userManager.CreateAsync(user, "Khan1@");
                if (!result.Succeeded)
                {
                    return View("Error");
                }


                var us = await _userManager.FindByEmailAsync("Admin@gmail.com");


                us.EmailConfirmed = true;
                await _userManager.UpdateAsync(us);

                var res = await _userManager.AddToRoleAsync(us, "Admin Department");
                if (!res.Succeeded)
                {
                    return View("Error");

                }
            }


            return View("Login");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
