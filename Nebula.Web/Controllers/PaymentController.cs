using Nebula.Web.Models;
using Nebula.Web.Models.BusinessLogic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using Nebula.Domain.ViewModels.Web;  

namespace Nebula.Web.Controllers
{
   
    [Authorize]
    public class PaymentController : BaseController
    {
        //  private  const int LICENSE_PRICE = GlobalClass.Price;

        // GET: Payment
        private ApplicationUserManager _userManager;
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

        private readonly BaseRepository<User> _userRepo;
        private readonly BaseRepository<Notification> _notificationRepo;
        private readonly BaseRepository<UserCupon> _userCuponRepo;
        private readonly BaseRepository<Cupon> _cuponRepo;
        private readonly BaseRepository<UserLicense> _userLicenseRepo;
        private readonly BaseRepository<FailTransaction> _failTransactionRepo;
        private readonly BaseRepository<EndOfTheDay> _endOfTheDayRepo;
        private readonly BaseRepository<UserCuponTry> _userCuponTryRepo;

        public PaymentController(
            BaseRepository<User> userRepo,
            BaseRepository<Notification> notificationRepo,
            BaseRepository<UserCupon> userCuponRepo,
            BaseRepository<Cupon> cuponRepo,
            BaseRepository<UserLicense> userLicenseRepo,
            BaseRepository<FailTransaction> failTransactionRepo,
            BaseRepository<EndOfTheDay> endOfTheDayRepo,
            BaseRepository<UserCuponTry> userCuponTryRepo)
        {
            _userRepo = userRepo;
            _notificationRepo = notificationRepo;
            _userCuponRepo = userCuponRepo;
            _cuponRepo = cuponRepo;
            _userLicenseRepo = userLicenseRepo;
            _failTransactionRepo = failTransactionRepo;
            _endOfTheDayRepo = endOfTheDayRepo;
            _userCuponTryRepo = userCuponTryRepo;
        }

        #region new code
        public ActionResult Index()
        {
            var viewModel = new PaymentViewModel
            {
                GeoPrice = GlobalClass.GeoPrice,
                EngPrice = GlobalClass.EngPrice,
                CsPrice = GlobalClass.CSPrice
            };

            var userCoupons = _userCuponRepo.Set().Where(m => m.UserId == _currentUserId).ToList();
            var geoCoupon = userCoupons.FirstOrDefault(m => m.Cupon != null && m.Cupon.Type == CouponType.Geo);
            var engCoupon = userCoupons.FirstOrDefault(m => m.Cupon  != null && m.Cupon.Type == CouponType.Eng);
            var csCoupon = userCoupons.FirstOrDefault(m => m.Cupon  != null && m.Cupon.Type == CouponType.Cs);

            if (geoCoupon != null)
                viewModel.GeoPrice = GlobalClass.GeoPrice * (100 - geoCoupon.Cupon.Percent) / 100;

            if (engCoupon != null)
                viewModel.EngPrice = GlobalClass.EngPrice * (100 - engCoupon.Cupon.Percent) / 100;

            if (csCoupon != null)
                viewModel.CsPrice = GlobalClass.CSPrice * (100 - csCoupon.Cupon.Percent) / 100; 

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult GenerateTransactionId(int price)
        {
            try
            {
                var user = _userRepo.Fetch(_currentUserId);

                if (string.IsNullOrEmpty(user.PhoneNumber))
                    throw new Exception("თქვენი მობილურის ნომერი არ ვიცით, ასე ბარათიდან თანხას ვერ ჩარიცხავთ! პირველ რიგში, შეიყვანეთ მობილურის ნომერი");

                if (string.IsNullOrEmpty(user.IdentificationNumber))
                    throw new Exception("თქვენი პირადი ნომერი არ ვიცით, ასე ბარათიდან თანხას ვერ ჩარიცხავთ! პირველ რიგში, შეიყვანეთ პირადი ნომერი");

                price = price * 100;
                var tbcPay = new TBCPay();

                string transactionId = tbcPay.RegisterTransaction(price, "nebula license", Request.ServerVariables["REMOTE_ADDR"], 981, "ka");

                if (_userLicenseRepo.Set().Any(m => m.TransactionId == transactionId))
                {
                    throw new Exception("მიმდინარე ტრანზაქცია უკვე დაფიქსირებულია, სცადეთ ხელახლა");
                }

                _userLicenseRepo.Save(new UserLicense
                {
                    BuyDate = DateTime.Now,
                    LincenseAndDate = GlobalClass.LicenseEndDate,
                    Payed = false,
                    UserId = _currentUserId,
                    Result = "not payed yet",
                    TransactionId = transactionId,
                    Price = price,
                    PaymentType = PaymentType.Balance
                });

                return Json(new
                {
                    transactionId = transactionId
                });
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult Ok()
        {
            string transId = Request["trans_id"];
            string transactionError = "სამწუხაროდ ტრანზაქცია ვერ განხორციელდა, გთხოვთ მოგვწერეთ ელ.ფოსტაზე : info@nebula.ge ან  <a href=\"https://www.facebook.com/nebula.ge/\" target=\"_blank\">Facebook-ზე</a>";

            if (string.IsNullOrEmpty(transId))
                throw new Exception("ტრანზაქციის მნიშვნელობა არ შეიძლება იყოს ცარიელი");

            var tbcPay = new TBCPay();
            string result = tbcPay.GetTransactionResult(transId, Request.ServerVariables["REMOTE_ADDR"]);
            ViewBag.Result = tbcPay.LastResponse;

            try
            {
                string status = string.Empty;
                var license = _userLicenseRepo.Set().Single(m => m.TransactionId == transId);
                string currentUserId = license.UserId;
                var currentUser = _userRepo.Fetch(currentUserId);
                license.Result = tbcPay.LastResponse; 

                if (result == "OK")
                {
                    if (!license.Payed)
                    {
                        license.Payed = true;
                        license.BuyDate = DateTime.Now; 

                        currentUser.Balance = currentUser.Balance + license.Price;

                        _notificationRepo._context.Notifications.Add(new Notification
                        {
                            UserId = currentUserId,
                            Text = "გილოცავთ, ტრანზაქცია წარმატებით განხორციელდა!",
                            SendDate = DateTime.Now
                        });

                        status = "გილოცავთ, ტრანზაქცია წარმატებით განხორციელდა!";
                        TempData["transaction-success"] = true;

                    }
                    else
                    {
                        _notificationRepo._context.Notifications.Add(new Notification
                        {
                            UserId = currentUserId,
                            Text = transactionError,
                            SendDate = DateTime.Now

                        });

                        status = "მიმდინარე ტრანზაქცია უკვე დარეგისტრირებულია";
                        TempData["transaction-error"] = true;
                    }
                }
                else
                {
                    TempData["transaction-error"] = true;

                    _notificationRepo._context.Notifications.Add(new Notification
                    {
                        UserId = currentUserId,
                        Text = transactionError,
                        SendDate = DateTime.Now
                    });

                    status = transactionError;
                }

                // ViewBag.Status = status;                                      
                _notificationRepo._context.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            catch (Exception e)
            { 
                // ViewBag.Result = result[1];
                //  ViewBag.Error = e.Message;
                ViewBag.Status = GlobalClass.TransactionError;

                _failTransactionRepo._context.FailTransactions.Add(new FailTransaction
                {
                    Result = tbcPay.LastResponse,
                    TransactionDate = DateTime.Now,
                    TransactionId = transId,
                    UserId = User.Identity.GetUserId(),
                    Error = e.Message
                });

                _failTransactionRepo._context.SaveChanges();
                TempData["transaction-error"] = true;
                return RedirectToAction("Index", "Home");
            }

        }

        //[HttpPost]
   



   
       
        [AllowAnonymous]
        public JsonResult EndOfTheDay(string key)
        {
            try
            {
                if (key != "51e9e500-d975-4215-8914-234adcc74635")
                {

                    return Json("invalid key", JsonRequestBehavior.AllowGet);
                }

                var tbcPay = new TBCPay();
                string response = tbcPay.EndOfTheDay();
                _endOfTheDayRepo.Save(new EndOfTheDay
                {
                    Status = response,
                    TriggerTime = DateTime.Now
                });

                var date = DateTime.Now.AddDays(-7);
                var tries = _userCuponTryRepo.Set().Where(m => m.TryDate <= date);

                foreach (var item in tries)
                {
                    _userCuponTryRepo.Delete(item);
                }

                return Json("successfully closed", JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                _endOfTheDayRepo.Save(new EndOfTheDay
                {
                    Status = e.Message,
                    TriggerTime = DateTime.Now
                });

                return Json("something wrong", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}