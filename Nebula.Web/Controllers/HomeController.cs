using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using PagedList;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity.Owin;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using Nebula.Domain.ViewModels.Web;
using Nebula.Services.Services;
using Nebula.Web.Models.BusinessLogic;

namespace Nebula.Web.Controllers
{
    //  [Filters.Authorize]
    public class HomeController : Controller
    {
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
        private readonly BaseRepository<Competition> _competitionRepo;
        private readonly BaseRepository<AboutNebula> _aboutNebulaRepo;
        private readonly BaseRepository<ExplantationVideo> _explantationVideoRepo;
        private readonly BaseRepository<CSQuestion> _questionRepo;
        private readonly BaseRepository<Notification> _notificationRepo;
        private readonly BaseRepository<News> _newsRepo;

        public HomeController(
            BaseRepository<User> userRpository,
            BaseRepository<Competition> competitionRpository,
            BaseRepository<AboutNebula> aboutNebulaRepository,
            BaseRepository<ExplantationVideo> explantationVideoRepository,
            BaseRepository<CSQuestion> questionRepository,
            BaseRepository<Notification> notificationRepository,
            BaseRepository<News> newsRepo)
        {
            _userRepo = userRpository;
            _competitionRepo = competitionRpository;
            _aboutNebulaRepo = aboutNebulaRepository;
            _explantationVideoRepo = explantationVideoRepository;
            _questionRepo = questionRepository;
            _notificationRepo = notificationRepository;
            _newsRepo = newsRepo;
        }

        //  [Nebula.Web.Filters.Authorize]
        public ActionResult Index(int? videoId, int? competitionId, bool showPdf = false)
        { 
            //  int price = _db.Prices.First(m => m.Id == 1).Price;
            if (Request.Browser.IsMobileDevice)
                ViewBag.IsMobile = true;

            var newsViewModel = new HomePageNewsViewModel
            {
                Tweets =
                    _newsRepo.Set()
                        .Where(a => a.ShowOnHomePage && !a.IsBlog)
                        .OrderByDescending(a => a.Id)
                        .Take(4)
                        .ToList(),
                Blogs =
                    _newsRepo.Set()
                        .Where(a => a.IsBlog && a.ShowOnHomePage)
                        .OrderByDescending(a => a.Id)
                        .Take(3)
                        .ToList()
            };

            bool hasLicense = false;

            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userRepo.Fetch(userId);

                if (showPdf)
                {
                    if (!user.HasUploadPdf)
                        TempData["show-pdf-popup"] = true;
                }

                if (user.HasLicense)
                {
                    switch (UserLicenseManager.UserLicenseStatus(user))
                    {
                        case LicenseStatus.Ok:
                            // nothing happens
                            hasLicense = true;
                            break;
                    }
                }
            }

            if (videoId != null)
            {
                var explantationVideo = _explantationVideoRepo.Set().FirstOrDefault(m => m.Id == videoId);

                if (explantationVideo != null)
                    ViewBag.Video = explantationVideo.FileName;
            }

            if (competitionId != null)
            {
                var competition = _competitionRepo.Set().FirstOrDefault(m => m.Id == competitionId);

                if (competition != null)
                    ViewBag.Competition = competition.Id;
            }

            ViewBag.HasLicense = hasLicense;

            return View(newsViewModel);
        }

        [Authorize]
        public ActionResult AllQuestions(int? page)
        {
            if (Request.Browser.IsMobileDevice)
                ViewBag.IsMobile = true;

            var userId = User.Identity.GetUserId();
            var user = _userRepo.Fetch(userId);

            if (!user.HasUploadPdf)
            {
                TempData["show-pdf-popup"] = true;
                return RedirectToAction("Index");
            }

            var model = _questionRepo.Set().Include(a => a.Answers);

            return View(model.ToList()
                .ToPagedList(page ?? 1, 80));
        }

        public ActionResult SampleQuestions()
        {
            return View();
        }


        public ActionResult About()
        {
            if (Request.Browser.IsMobileDevice)
            {
                ViewBag.IsMobile = true;
            }
            var jss = new JavaScriptSerializer();

            var model = _aboutNebulaRepo.Set().Where(m => m.Visible).Select(m => new
            {
                NavTitle = m.NavTitle,
                Title = m.Title,
                Text = m.Text
            }).ToList();

            ViewBag.Model = jss.Serialize(model);
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
        public ActionResult ParseUtf()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ParseUtf(string myStr)
        {
            if (string.IsNullOrEmpty(myStr)) return View();

            byte[] bytes = Encoding.Default.GetBytes(myStr);
            myStr = Encoding.UTF8.GetString(bytes);
            ViewBag.Result = myStr;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SendEmail(string email, string text)
        {
            try
            {
                _notificationRepo.Save(new Notification
                {
                    UserId = "4c642e1a-815d-4136-ae3f-5189bd7346a8",
                    Text = text,
                    SendDate = DateTime.Now
                });

                string message = text;

                var msg = new MailMessage { From = new MailAddress("no-reply@leavingstone.net", "nebula") };

                msg.To.Add(GlobalClass.DestionationEmail);
                msg.Subject = "from :" + email;
                msg.Body = text;

                new SmtpClient("smtp.mandrillapp.com")
                {
                    Port = 25,
                    Credentials = ((ICredentialsByHost)new NetworkCredential("no-reply@leavingstone.net", "UXbVgBVC5HC5WIbLTWi8ig"))
                }.Send(msg);


                return Json(new { send = true });
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }

        public JsonResult asd()
        {
            return Json(true,
                JsonRequestBehavior.AllowGet);
        }


        public ActionResult SocialProgram()
        {
            var userId = User.Identity.GetUserId();

            var model = new SocialViewModel();

            if (string.IsNullOrEmpty(userId))
            {
                model.Social = new SocialProgramViewModel();
                ViewBag.Action = "SocialProgramForNotRegisteredUser";
            }
            else
            {
                var user = _userRepo.Fetch(userId);
                model.User = user;
                ViewBag.Action = "SocialProgram";
            }

            return View(model);
        }

        public ActionResult Social()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult SocialProgram(User model, HttpPostedFileBase UserFile)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var user = _userRepo.Fetch(userId);

                if (_userRepo.Set().Any(m => m.IdentificationNumber == model.IdentificationNumber && m.Id != userId ))
                    throw new Exception("ეს პირადი ნომერი უკვე გამოყენებულია");

                if (string.IsNullOrEmpty(model.PhoneNumber))
                    throw new Exception("გთხოვთ შეავსოთ მობილურის ველი");

                if (string.IsNullOrEmpty(model.SocialText))
                    throw new Exception("გთხოვთ შეავსოთ განაცხადის ველი");

                SocialRestrictions(model.IdentificationNumber, UserFile);

                string fileName = Guid.NewGuid() + UserFile.FileName;

                GlobalClass.AddFile(UserFile, fileName, GlobalClass.UserFilesPath);

                if (!string.IsNullOrEmpty(user.FileName))
                    GlobalClass.DeleteFile(user.FileName, GlobalClass.UserFilesPath);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;

                user.IdentificationNumber = model.IdentificationNumber;
                user.SocialText = model.SocialText;
                user.FileName = fileName;
                user.SocialTextSendDate = DateTime.Now;
                _userRepo.Save(user);

                ViewBag.Succes = "succesReg";
                return View("SocialProgram", new SocialViewModel { User = model });    
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                ViewBag.Action = "SocialProgram";
                return View("SocialProgram", new SocialViewModel() { User = model });
            }
        }

        public ActionResult SocialProgramForNotRegisteredUser(SocialProgramViewModel model, HttpPostedFileBase UserFile)
        {
            ViewBag.Action = "SocialProgramForNotRegisteredUser";

            if (!ModelState.IsValid)
            {
                return View("SocialProgram", new SocialViewModel() { Social = model });
            }

            try
            {
                if (_userRepo.Set().Any(m => m.IdentificationNumber == model.IdentificationNumber))
                    throw new Exception("ეს პირადი ნომერი უკვე გამოყენებულია");

                if (_userRepo.Set().Any(m => string.Equals(m.Email.ToLower(), model.Email.ToLower())))
                    throw new Exception("ეს ელ.ფოსტა უკვე გამოყენებულია");

                SocialRestrictions(model.IdentificationNumber, UserFile);

                var fileName = Guid.NewGuid() + UserFile.FileName;

                GlobalClass.AddFile(UserFile, fileName, GlobalClass.UserFilesPath);

                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    SocialText = model.SocialText,
                    IdentificationNumber = model.IdentificationNumber,
                    PhoneNumber = model.PhoneNumber,
                    FileName = fileName,
                    RegisterDate = DateTime.Now,
                    LastLogin = DateTime.Now,
                    SocialTextSendDate = DateTime.Now
                };

                var result = UserManager.Create(user, model.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error);

                    return View("SocialProgram", new SocialViewModel() { Social = model });
                }

                var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                UserManager.SendEmail(user.Id, "activation", "Please  confirm your account by clicking  <a href=\"" + callbackUrl + "\">here</a>");

                ViewBag.Succes = "succes";
                return View("SocialProgram", new SocialViewModel { Social = new SocialProgramViewModel() });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View("SocialProgram", new SocialViewModel { Social = model });
            }
        }

        private static void SocialRestrictions(string identificationNumber, HttpPostedFileBase userFile)
        {
            if (identificationNumber.Length != 11)
                throw new Exception("გთხოვთ, სწორად შეიყვანოთ პირადი ნომერი");

            if (userFile == null || !userFile.IsValidFile())
                throw new Exception("გთხოვთ ატვირთეთ სურათი, word ან pdf  - ი");
        }

        public ActionResult GenerateRandomQuestions()
        {
            var questions = _questionRepo.Set().Include(a => a.Answers)
                .Where(m => m.QuestionType == CSQuestionType.ExerciseQuestion && m.ModuleId == 8).OrderBy(m => Guid.NewGuid()).Take(10).ToList();
            return View(questions);
        }

        public ActionResult News(int id = 0)
        {
            var model = new NewsViewModel
            {
                CurrentNews = id != 0 ? _newsRepo.Fetch(id) : _newsRepo.Set().OrderByDescending(a => a.Id).First()
            };

            model.NavigationNews =
                _newsRepo.Set()
                    .Where(a => a.Id != model.CurrentNews.Id)
                    .OrderByDescending(a => a.Id)
                    .Select(a => new NewsNavigationViewModel()
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Date = a.Date
                    }).ToList();

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult BuyBtn(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = _userRepo.Fetch(userId);
            ViewBag.Id = id;
            return View(user);
        }


        //public void RemoveFinishedTasks()
        //{
        //    RestoreDatabase service = new RestoreDatabase();
        //    service.MakeCorrectionInFinishedTasks();
        //}


        //public void RemoveDuplicateUserAnswers()
        //{
        //    RestoreDatabase service = new RestoreDatabase();
        //    service.RemoveDuplicateUserAnswers();
        //}


      

        public ActionResult NovaTest()
        {
            var user = _userRepo.Set().SingleOrDefault(m => m.IdentificationNumber == "12345654321");
            return Json("");
        }
    }
}