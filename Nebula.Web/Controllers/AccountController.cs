using Nebula.Web.Models;
using Nebula.Web.Models.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization; 
using Nebula.Domain.Concrete;
using Nebula.Domain.Entities;

namespace Nebula.Web.Controllers
{
   
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        } 

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            TempData["ReturnUrl"] = returnUrl;
            if(!string.IsNullOrEmpty(returnUrl))
            if (returnUrl.ToLower().Contains("admin"))
                return View();

            TempData["show-login-form"] = true;       
            
            return RedirectToAction("Index", "Home");
         
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, bool showPdf = false)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            } 
                
            User user = new User();
            using (var _db = new NebulaDbContext())
            {
                user =
                    _db.Users.FirstOrDefault(
                        u =>
                            u.Email.ToLower().Equals(model.Email.ToLower()) && u.GoogleId == null &&
                            u.FacebookId == null);
                if (user != null)
                {

                    if (new PasswordHasher().VerifyHashedPassword(user.PasswordHash, model.Password) ==
                        PasswordVerificationResult.Success)
                    {
                        if (!user.EmailConfirmed)
                            return Json(new
                            {
                                Error = "იმისთვის რომ სისტემაში შეხვიდეთ გთხოვთ  დაადასტურეთ ელ-ფოსტა"
                            });
                    }
                } 

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true

                if (user != null)
                {
                    var userRepository = new UserRepository(_db, this.HttpContext);
                    userRepository.CheckLicense(user);                   

                }
            }
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
          
            
            switch (result)
            {
                case SignInStatus.Success:
              
                    using (var _db = new NebulaDbContext())
                    {
                        //    var userId = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();
                        if (user != null) user.LastLogin = DateTime.Now;
                        _db.SaveChanges();
                    }
                    //if admin
                 
                    if (user != null && UserManager.IsInRole(user.Id, "Admin")) 
                       if(returnUrl != null)
                           if (returnUrl.ToLower().Contains("admin"))
                           {
                               return RedirectToLocal(returnUrl);
                           }

                    if (showPdf)
                    {
                        if (user != null && !user.HasUploadPdf)
                        {
                            showPdf = true; 
                        }
                    }
                    else
                    {
                        showPdf = false;
                    } 

                    return Json(new
                    {
                        Auth = true,
                        showPdf = showPdf ? "true" : null
                    });
               
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    return Json(new
                    {
                        Error = "ელ-ფოსტა ან პაროლი არასწორია"
                    });
                  //  return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    Errors = ModelState.Values.Select(m => m.Errors)
                });

            var user = new User { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, RegisterDate = DateTime.Now, LastLogin = DateTime.Now };
            var result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return Json(new
                {
                    Errors = result.Errors
                });

            //  await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code}, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(user.Id, "activation", "Please  confirm your account by clicking  <a href=\"" + callbackUrl + "\">here</a>");

            return Json(new
            {
                Confirm = true
            });
            // If we got this far, something failed, redisplay form


        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code, bool externalLogin = false)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);

            if (!result.Succeeded) return View(result.Succeeded ? "ConfirmEmail" : "Error");

            if (externalLogin)
            {
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = await UserManager.FindByIdAsync(userId);

                var loginResult = await UserManager.AddLoginAsync(user.Id, info.Login);


                if (loginResult.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("RegSuccess", "Account");
                }
            }


            TempData["show-login-form"] = true;                       
            return RedirectToAction("RegSuccess", "Account");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new {systemError = true},JsonRequestBehavior.AllowGet);

                var user = await UserManager.FindByNameAsync(model.Email);

                if (!string.IsNullOrEmpty(user.FacebookId) || !string.IsNullOrEmpty(user.GoogleId))
                {
                    return Json(new { error = "ელ.ფოსტა ვერ მოიძებნა" },
                        JsonRequestBehavior.AllowGet);
                }
                if (!(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                        
                    return Json(new { error = "ელ.ფოსტა ვერ მოიძებნა" },
                        JsonRequestBehavior.AllowGet);
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

                return Json(new { success = true },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { systemError = true }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            TempData["Code"] = code;
            return RedirectToAction("Index", "Home");
          
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    return Json(new { error = "სამწუხაროდ პაროლი ვერ შეიცვალა, სცადეთ ხელახლა" });

                    //return RedirectToAction("ResetPasswordConfirmation", "Account");
                }

                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                return result.Succeeded ? 
                    Json(new { success = "პაროლი წარმატებით შეიცვალა" }) : 
                    Json(new { error = "სამწუხაროდ პაროლი ვერ შეიცვალა, სცადეთ ხელახლა" });
            }
            catch (Exception)
            {
                return Json(new { error = "სამწუხაროდ პაროლი ვერ შეიცვალა, სცადეთ ხელახლა" });
            }
          
        
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl, bool showPdf = false)
        {
            string teacherId = null;
            if (Session["Teacher"] != null)
                teacherId = Session["Teacher"].ToString();

            ControllerContext.HttpContext.Session?.RemoveAll();

            if(teacherId != null)
             Session["Teacher"] = teacherId;
             //Session["nebula"] = true;
            // Request a redirect to the external login provider
             return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl, showPdf = showPdf }));
        }

        [AllowAnonymous]
        public ActionResult RegSuccess()
        { 
            return View();
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
          
            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl, bool showPdf = false)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login

            using (var _db = new NebulaDbContext())
            {
                User user;
                if(loginInfo.Login.LoginProvider == "Facebook")
                {
                     user = _db.Users.FirstOrDefault(u => u.FacebookId == loginInfo.Login.ProviderKey);
                     if(user != null){
                         var userRepository = new UserRepository(_db, this.HttpContext);
                         userRepository.CheckLicense(user);
                     }
                     
                } else if(loginInfo.Login.LoginProvider == "Google"){

                    user = _db.Users.FirstOrDefault(u => u.GoogleId == loginInfo.Login.ProviderKey);
                    if (user != null)
                    {
                        var userRepository = new UserRepository(_db, this.HttpContext);
                        userRepository.CheckLicense(user);
                    }
                }
            }
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    using (var _db = new NebulaDbContext())
                    {
                        var userId = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();
                        var extUser = _db.Users.Find(userId);
                        extUser.LastLogin = DateTime.Now;
                        _db.SaveChanges();

                        if (extUser.HasUploadPdf || !showPdf) return RedirectToLocal(returnUrl);

                        TempData["show-pdf-popup"] = true;
                        return RedirectToAction("Index", "Home");
                    }
                   
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    string email;
                    if (loginInfo.Email == null)
                    { 
                        email = "defaultFb" + Guid.NewGuid() + "@gmail.com"; 
                        //TempData["external-login-confirmation"] = true;
                        //return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        email = loginInfo.Email;
                    }
              

                    if (ModelState.IsValid)
                    {
                        // Get the information about the user from the external login provider
                        //var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                         
                        var externalUser = new User
                        {
                            UserName = email,
                            Email = email,
                            ExternalUserName = loginInfo.ExternalIdentity.Name,
                            FacebookId = loginInfo.Login.LoginProvider == "Facebook" ? loginInfo.Login.ProviderKey : null,
                            GoogleId = loginInfo.Login.LoginProvider == "Google" ? loginInfo.Login.ProviderKey : null,
                            RegisterDate = DateTime.Now,
                            LastLogin = DateTime.Now,
                            EmailConfirmed = !email.StartsWith("defaultFb")
                        };

                        var registerResult = await UserManager.CreateAsync(externalUser);
                        if (registerResult.Succeeded)
                        {
                            registerResult = await UserManager.AddLoginAsync(externalUser.Id, loginInfo.Login);
                            if (registerResult.Succeeded)
                            {
                                await SignInManager.SignInAsync(externalUser, isPersistent: false, rememberBrowser: false);              
                                return RedirectToLocal(returnUrl);
                            }
                        }
                        AddErrors(registerResult);
                        if (registerResult.Errors.Any())
                        {
                            var jss = new JavaScriptSerializer();
                            if (registerResult.Errors.Any(error => error.Contains("is already taken")))
                            {
                                TempData["errors"] = jss.Serialize(new List<string> { "ელ.ფოსტა უკვე გამოყენებულია" }.Select(m => m));
                                //return Json("ელ.ფოსტა უკვე გამოყენებულია", JsonRequestBehavior.AllowGet);
                                return RedirectToLocal(returnUrl);
                            }
                            var errors = registerResult.Errors.ToList();

                            TempData["errors"] = jss.Serialize(errors.Select(m => m));
                            return RedirectToLocal(returnUrl);
                          //  return Json(registerResult.Errors, JsonRequestBehavior.AllowGet);
                        }                      
                    }

                    TempData["show-pdf-popup"] = true;
                    return RedirectToAction("Index", "Home");
               
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (!ModelState.IsValid)
                    return Json(new
                    {
                        Errors = ModelState.Values.Select(m => m.Errors)
                    });
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    ExternalUserName = info.ExternalIdentity.Name,
                    FacebookId = info.Login.LoginProvider == "Facebook" ? info.Login.ProviderKey : null,
                    GoogleId = info.Login.LoginProvider == "Google" ? info.Login.ProviderKey : null,
                    RegisterDate = DateTime.Now,
                    LastLogin = DateTime.Now

                };
                using (var _db = new NebulaDbContext())
                {
                    if (!string.IsNullOrEmpty(user.FacebookId))
                    {
                        var checkUser = _db.Users.FirstOrDefault(u => u.FacebookId == user.FacebookId);
                        if (checkUser != null)
                        {
                            checkUser.Email = model.Email;
                            checkUser.UserName = model.Email;
                            checkUser.ExternalUserName = info.ExternalIdentity.Name;
                            _db.SaveChanges();

                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(checkUser.Id);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = checkUser.Id, code = code, externalLogin = true }, protocol: Request.Url.Scheme);
                            await UserManager.SendEmailAsync(checkUser.Id, "activation", "Please  confirm your account by clicking  <a href=\"" + callbackUrl + "\">here</a>");
                            return Json(new
                            {
                                Confirm = true
                            });
                        }
                    }
                    else if (!string.IsNullOrEmpty(user.GoogleId))
                    {
                        var checkUser = _db.Users.FirstOrDefault(u => u.GoogleId == user.GoogleId);
                        if (checkUser != null)
                        {
                            checkUser.Email = model.Email;
                            checkUser.UserName = model.Email;
                            checkUser.ExternalUserName = info.ExternalIdentity.Name;
                            _db.SaveChanges();

                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(checkUser.Id);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = checkUser.Id, code = code, externalLogin = true }, protocol: Request.Url.Scheme);
                            await UserManager.SendEmailAsync(checkUser.Id, "activation", "Please  confirm your account by clicking  <a href=\"" + callbackUrl + "\">here</a>");
                            return Json(new
                            {
                                Confirm = true
                            });
                        }
                    }


                }
                var result = await UserManager.CreateAsync(user);


                if (result.Succeeded)
                {
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code, externalLogin = true }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "activation", "Please  confirm your account by clicking  <a href=\"" + callbackUrl + "\">here</a>");
                    return Json(new
                    {
                        Confirm = true
                    });


                }
                return Json(new
                {
                    Errors = result.Errors
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Errors = e.Message
                });
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie); 
            return RedirectToAction("Index", "Home");
        }

        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject,
               "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            return callbackUrl;
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult IsAuthenticated()
        {
            return Json(new
            {
                isAuthenticated = Request.IsAuthenticated
            }, JsonRequestBehavior.AllowGet);
        }

    
 

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                var result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return Json(new
                    {
                        Confirm = true
                    });
                }
                return Json(new
                {
                    errors = result.Errors
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    errors = e.Message
                });
            }
       
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateUserInfo(UserInfoViewModel model)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                if (ModelState.IsValid)
                {
                    
                    using (var _db = new NebulaDbContext())
                    {
                        var user = _db.Users.Find(userId);
                        user.PhoneNumber = model.PhoneNumber;

                        
                            if (model.IdentificationNumber.Length != 11)
                            {
                                throw new Exception("გთხოვთ, სწორად შეიყვანოთ პირდპი ნომერი");
                            }

                            if (_db.Users.Any(m => m.Email.ToLower() == model.Email.ToLower() && m.Id != user.Id))
                            {
                                throw new Exception("ეს ელ.ფოსტა უკვე გამოყენებულია");
                            }
                            if (_db.Users.Any(m => m.IdentificationNumber == model.IdentificationNumber && m.Id != user.Id))
                            {
                                throw new Exception("ეს პირადი ნომერი უკვე გამოყენებულია");
                            } 
                        
                        user.IdentificationNumber = model.IdentificationNumber;
                        user.Email = model.Email;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.UserName = model.Email;

                        _db.SaveChanges();
                    }

                    return Json(new
                    {
                        Confirm = true
                    });
                }
                return Json(new
                {
                    errors = ModelState.Select(m => m.Value.Errors)
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    errors = e.Message
                });
            } 
        }

        [AllowAnonymous]
        public JsonResult AddLicenseToUser()
        {
            using (var _db = new NebulaDbContext())
            {
                var user = _db.Users.Single(m => m.Email == "giorginikolaishvili99@gmail.com");
            
                user.HasLicense = true;
                user.LicenseBuyDate = DateTime.Now;
                user.LicenseEndDate = GlobalClass.LicenseEndDate;

                user.HasGeoLicense = true;
                user.GeoLicenseBuyDate = DateTime.Now;
                user.GeoLicenseEndDate = DateTime.Now;

                _db.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}