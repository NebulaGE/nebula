using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nebula.Web.Models.BusinessLogic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using Nebula.Web.Controllers.Geo;
using Nebula.Domain.ViewModels.Web.CS;

namespace Nebula.Web.Controllers
{
  
    public class CSMainController : BaseController
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

        private readonly BaseRepository<Module> _moduleRepo; 
        private readonly BaseRepository<User> _userRepo;
        private readonly BaseRepository<CSUserQuizzes> _userQuizRepo;
        private readonly BaseRepository<CSTag> _tagRepo;

        public CSMainController(
            BaseRepository<Module> moduleRpository,
            BaseRepository<User> userRpository,
            BaseRepository<CSUserQuizzes> userQuizRpository,
            BaseRepository<CSTag> tagRpository)
        {
            _moduleRepo = moduleRpository;
            _userRepo = userRpository;
            _userQuizRepo = userQuizRpository;
            _tagRepo = tagRpository; 
        }

        public ActionResult Index()
        {
            bool needLicense = false;
            int leftDays = 0;
            var user = _userRepo.Fetch(_currentUserId);

            if (user == null)
                return RedirectToAction("Guest");

            var userQuizzes = _userQuizRepo.Set().Where(u => u.UserId == _currentUserId)
                .OrderBy(q => q.Id).ToList();

            var lastQuizzes = userQuizzes.OrderByDescending(m => m.Id).ToList();
            var currentQuiz = lastQuizzes.FirstOrDefault();

            if (currentQuiz != null)
            {
                if (!currentQuiz.IsFinished && !currentQuiz.IsTimeOut)
                {
                    var remainingSeconds = GlobalClass.GeTExamTime -
                        DateTime.Now.Subtract(currentQuiz.VerbalQuizStartDate.GetValueOrDefault()).TotalSeconds;
                    if (remainingSeconds <= 0)
                    {
                        currentQuiz.IsFinished = true;
                        currentQuiz.IsTimeOut = true;
                        _userQuizRepo._context.SaveChanges();
                    }
                }
            }

            if (currentQuiz == null)
            {
                var quizHelper = QuizHelper.GetQuiz(_tagRepo._context);
                int quizIndex = 1 + userQuizzes.Count();

                if (quizIndex > 1)
                    leftDays = quizHelper.CalculateNextQuizLeftDays(quizIndex, _currentUserId);
            }
            else if (currentQuiz.IsFinished || currentQuiz.IsTimeOut)
            {
                var quizHelper = QuizHelper.GetQuiz(_tagRepo._context);
                int quizIndex = 1 + userQuizzes.Count();

                if (quizIndex > 1)
                    leftDays = quizHelper.CalculateNextQuizLeftDays(quizIndex, _currentUserId);
            }
            if (UserManager.IsInRole(user.Id, "admin"))
            {
                leftDays = 0;
            }
            var lastQuiz = currentQuiz != null ? !currentQuiz.IsFinished && !currentQuiz.IsTimeOut ? lastQuizzes.Skip(1).FirstOrDefault() : currentQuiz : null;

            if (!user.HasLicense && userQuizzes.Any())
            {
                if (currentQuiz != null)
                {
                    if (currentQuiz.IsFinished || currentQuiz.IsTimeOut)
                    {
                        needLicense = true;
                    }
                }
                else
                {
                    needLicense = true;
                }
            }
             
            var model = new CSMainPageViewModel
            {
                MathExercisesModuleId = _moduleRepo.Set().First(m => m.CSQuestionCategoryId == 2).Id,
                VerbalExercisesModuleId = _moduleRepo.Set().First(m => m.CSQuestionCategoryId == 1).Id,
                MathVideosTagId = user.HasLicense ? _tagRepo.Set().First(m => m.QuestionTypeId == 2).Id : 30,
                VerbalVideosTagId = user.HasLicense ? _tagRepo.Set().First(m => m.QuestionTypeId == 1).Id : 29,
                MathType = 2,
                VerbalType = 1,
                LeftDays = leftDays
            };
            if(lastQuiz != null)
            {
                model.PrevQuizId = lastQuiz.QuizId;
            }
            if (needLicense)
            {
                model.QuizStatus = CSQuizStatus.NeedLicense;

            } else if(currentQuiz != null && !currentQuiz.IsFinished && !currentQuiz.IsTimeOut)
            {
                model.QuizStatus = CSQuizStatus.Continue;
            }
            else if(leftDays > 0)
            {
                model.QuizStatus = CSQuizStatus.Locked;
            }
            else
            {
                model.QuizStatus = CSQuizStatus.Start;
            }   
            return View(model);
        }


        public ActionResult Guest()
        {
            var model = new CSMainPageViewModel()
            {
                QuizStatus=CSQuizStatus.NeedLicense,
                MathExercisesModuleId = 1,
                VerbalExercisesModuleId = 1,
                MathVideosTagId = 30,
                VerbalVideosTagId = 29,
                MathType = 2,
                VerbalType = 1,
            };

            return View("Index",model);
        }

        #region old
        [Authorize]
        //public ActionResult Index()
        //{
        //    if (Request.Browser.IsMobileDevice)
        //    {
        //        ViewBag.IsMobile = true;
        //    }

        //    var user = UserManager.FindById(_currentUserId);

        //    if (user.HasUploadPdf)
        //        return View();

        //    TempData["show-pdf-popup"] = true;
        //    return RedirectToAction("Index", "Home");
        //}

        public JsonResult GetAll()
        {
            bool hasLicense = false;
            bool lockedQuiz = false;
            int leftDays = 0; 
             
            var currentUser = _userRepo.Set().FirstOrDefault(u => u.Id == _currentUserId);

            if (currentUser != null)
            {
                switch (UserLicenseManager.UserLicenseStatus(currentUser))
                {
                    case LicenseStatus.Ok:
                        // nothing happens
                        hasLicense = true;
                        break;
                    case LicenseStatus.Fail:                   
                        break;
                    case LicenseStatus.TimeOut:
                        if (UserManager.IsInRole(currentUser.Id, GlobalClass.LicenseRoleWord))
                        {
                            UserManager.RemoveFromRole(currentUser.Id, GlobalClass.LicenseRoleWord);
                        } 
                        break;
                    default:
                        // system error 
                        break;
                }
            }

            var module = _moduleRepo.Set().ToList();
            var tags = _tagRepo.Set().ToList();

            if (currentUser == null)
            {
                #region user null
                object emptyVerbalResult = module.Where(et => et.CSQuestionCategoryId == 1).Select(et => new
                {
                    CSTag = et.Name,
                    Percent = 0
                });
                object emptyMathResult = module.Where(et => et.CSQuestionCategoryId == 2).Select(et => new
                {
                    CSTag = et.Name,
                    Percent = 0
                });

                return Json(new
                {
                    examsCount = GlobalClass.GetExamsCount,
                    verbalVideos = tags.Where(m => m.QuestionTypeId == 1).Select(m => new
                    {
                        id = m.Id
                    }).FirstOrDefault(),
                    mathVideos = tags.Where(m => m.QuestionTypeId == 2).Select(m => new
                    {
                        id = m.Id
                    }).FirstOrDefault(),

                    verbalExercises = module.Where(et => et.CSQuestionCategoryId == 1).Select(m => new
                    {
                        id = m.Id                      
                    }).FirstOrDefault(),
                    mathExercises = module.Where(et => et.CSQuestionCategoryId == 2).Select(m => new
                    {
                        id = m.Id                    
                    }).FirstOrDefault(),

                   
                    verbalResult =  emptyVerbalResult,
                    mathResult = emptyMathResult,
                    expectedScore = 0,
                    isAuth = User.Identity.IsAuthenticated,
                    hasLicense = hasLicense,
                    leftDays = leftDays
                }, JsonRequestBehavior.AllowGet);
            #endregion
            }

            var userQuizzes = _userQuizRepo.Set().Where(u => u.UserId == _currentUserId)                   
                .OrderBy(q => q.Id).ToList();

            var lastQuizzes = userQuizzes.OrderByDescending(m => m.Id).ToList();
            var currentQuiz = lastQuizzes.FirstOrDefault();

            if (currentQuiz != null)
            {
                if (!currentQuiz.IsFinished && !currentQuiz.IsTimeOut)
                { 
                    var remainingSeconds = GlobalClass.GeTExamTime - 
                        DateTime.Now.Subtract(currentQuiz.VerbalQuizStartDate.GetValueOrDefault()).TotalSeconds;
                    if (remainingSeconds <= 0)
                    {
                        currentQuiz.IsFinished = true;
                        currentQuiz.IsTimeOut = true;
                        _userQuizRepo._context.SaveChanges(); 
                    }
                }

            }

            if (currentQuiz == null)
            {
                var quizHelper = QuizHelper.GetQuiz(_tagRepo._context);
                int quizIndex = 1 + userQuizzes.Count();

                if(quizIndex > 1)
                    leftDays = quizHelper.CalculateNextQuizLeftDays(quizIndex, currentUser.Id);  
            }
            else if (currentQuiz.IsFinished || currentQuiz.IsTimeOut)
            {
                var quizHelper = QuizHelper.GetQuiz(_tagRepo._context);
                int quizIndex = 1 + userQuizzes.Count();

                if (quizIndex > 1)
                    leftDays = quizHelper.CalculateNextQuizLeftDays(quizIndex, currentUser.Id);
            }
             
            var lastQuiz = currentQuiz != null ? !currentQuiz.IsFinished && !currentQuiz.IsTimeOut? lastQuizzes.Skip(1).FirstOrDefault() : currentQuiz : null;
            if(!hasLicense && userQuizzes.Any()){                                 
                if (currentQuiz != null)
                {
                    if (currentQuiz.IsFinished || currentQuiz.IsTimeOut)
                    {                          
                        lockedQuiz = true;                              
                    } 
                }
                else
                {
                    lockedQuiz = true;
                } 
            }
         
            #region userscores 
            int takeCount = GlobalClass.ExamCoefficients.Count();

            var finishedQuizzes = _userQuizRepo.Set().Where(m => m.QuizConfirmed == true && m.UserId ==  _currentUserId)                  
                .OrderByDescending(m => m.VerbalQuizStartDate).Take(takeCount)
                .Include(m => m.Quiz.VerbalQuizzes.Select(q => q.Question.Module))
                .Include(m => m.Quiz.MathQuizzes.Select(q => q.Question.Module))
                .ToList();

            var result = QuizHelper.GetQuiz(_tagRepo._context).CalculateModulesPercent(finishedQuizzes,module);
            var verbalVideo = new CSTag();
            var mathVideo = new CSTag();

            if (!hasLicense)
            {
                verbalVideo = tags.FirstOrDefault(m => m.Id == 29);
                mathVideo = tags.FirstOrDefault(m => m.Id == 30);
            }
            else
            {
                verbalVideo = tags.FirstOrDefault(m => m.QuestionTypeId == 1);
                mathVideo = tags.FirstOrDefault(m => m.QuestionTypeId == 2);
            }
                 
            #endregion 
             
            return Json(new
            {
                examsCount = GlobalClass.GetExamsCount,

                verbalVideos = verbalVideo != null ? new
                {
                    id = verbalVideo.Id
                } : null,
                mathVideos = mathVideo != null ? new
                {
                    id = mathVideo.Id
                } : null,

                verbalExercises = module.Where(et => et.CSQuestionCategoryId == 1).Select(m => new
                {
                    id = m.Id,
                    name = m.Name
                }).FirstOrDefault(),

                mathExercises = module.Where(et => et.CSQuestionCategoryId == 2).Select(m => new
                {
                    id = m.Id,
                    name = m.Name
                }).FirstOrDefault(),

                lastQuiz = lastQuiz != null ? new
                {
                    id = lastQuiz.Id
                } : null,

                currentQuiz = currentQuiz == null ? null : currentQuiz.IsFinished || currentQuiz.IsTimeOut ? null : new
                {
                    id = currentQuiz.Id
                },
                lockedQuiz = lockedQuiz,
                verbalResult = result.modulePercents.Where(m => m.isVerbal).Select(m => new
                {
                    module = m.module,
                    percent = m.percent
                }),
                mathResult = result.modulePercents.Where(m => !m.isVerbal).Select(m => new
                {
                    module = m.module,
                    percent = m.percent
                }),
                expectedScore = result.excpectedScore,
                isAuth = User.Identity.IsAuthenticated,
                hasLicense = hasLicense,
                leftDays = leftDays
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}