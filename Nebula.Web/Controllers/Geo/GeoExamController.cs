using Nebula.Domain;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using Nebula.Domain.ViewModels.Web.Geo;
using Nebula.Services.Services.Web.Geo;
using Nebula.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nebula.Web.Controllers.Geo
{
 
    [Authorize]
    public class GeoExamController : BaseController
    {
        #region servises
        private readonly GeoExamService _geoExamService;
        #endregion

        #region repositories
        private readonly BaseRepository<GeoEssay> _geoEssayRepo;
        private readonly BaseRepository<GeoTextEditing> _geoTextEditingRepo;
        private readonly BaseRepository<GeoUserExam> _geoUserExamRepo;
        private readonly BaseRepository<GeoFictionText> _geoFixtionRepo;
        private readonly BaseRepository<GeoUserAnswer> _geoUserAnswerRepo;
        private readonly BaseRepository<User> _userRepo;
        private readonly GeneralRepository _generalRepository;
        #endregion

        public GeoExamController(
            BaseRepository<GeoEssay> geoEssay,
            BaseRepository<GeoTextEditing> geoTextEditing,
            BaseRepository<GeoUserExam> geoUserExam,
            BaseRepository<GeoFictionText> geoFictionText,
            BaseRepository<GeoUserAnswer> geoUserAnswer,
            BaseRepository<User> user
            )
        {
            _geoEssayRepo = geoEssay;
            _geoTextEditingRepo = geoTextEditing;
            _geoUserExamRepo = geoUserExam;
            _geoFixtionRepo = geoFictionText;
            _geoUserAnswerRepo = geoUserAnswer;
            _userRepo = user;
            _generalRepository = new GeneralRepository
            {
                GeoEssay = _geoEssayRepo,
                GeoTextEditing = _geoTextEditingRepo,
                GeoUserExam = _geoUserExamRepo,
                GeoFictionText = _geoFixtionRepo,
                GeoUserAnswer = _geoUserAnswerRepo
            };

            _geoExamService = new GeoExamService(_generalRepository);
        }

        // GET: GeoExam
        public ActionResult Index()
        {
            var user = _userRepo.Fetch(_currentUserId);
            if (!user.HasGeoLicense)
                return RedirectToAction("Index", "Payment");
            return View();
        }

        [HttpPost]
        public ActionResult Init()
        {
            try
            {
                var user = _userRepo.Fetch(_currentUserId);
                if (!user.HasGeoLicense)
                    return NeedLicenseJson();

                bool continueExam;
                var exam = _geoExamService.Init(_currentUserId, out continueExam);
               // var userAnswers = _geoExamService.GetUserAnswerIds(exam.Id);
                return Json(new
                {
                    testNumber = _geoExamService.GetExamsCount(_currentUserId),
                    exam = new
                    {
                        id = exam.Id,
                        userTextEditingText = exam.TextEditing,
                        textEditingId = exam.GeoTextEditingId,
                        textEditingText = exam.GeoTextEditing.IncorrectText,
                        essayId = exam.GeoEssayId,
                        essayText = exam.GeoEssay.Topic,
                        userEssayText = exam.Essay,
                        userTheme = exam.UserTheme,
                        prose = new
                        {
                            id = exam.ProseId,
                            title = exam.Prose.Title,
                            text = exam.Prose.Text,
        
                            questions = exam.Prose.Questions.Select(m => new
                            {
                                id = m.Id,
                                text = m.Text,
                                userAnswerId = exam.User.GeoAnswers.FirstOrDefault(x => x.Answer.QuestionId == m.Id)?.AnswerId,
                                answers = m.Answers.Select(a => new
                                {
                                    id = a.Id,
                                    text = a.Text
                                })
                            }),

                            point1 = exam.Prose.PointOne,
                            point2 = exam.Prose.PointTwo,
                            point3 = exam.Prose.PointThree
                        },
                        poetry = new
                        {
                            id = exam.PoetryId,
                            title = exam.Poetry.Title,
                            text = exam.Poetry.Text,
                                                 
                            questions = exam.Poetry.Questions.Select(m => new
                            {
                                id = m.Id,
                                text = m.Text,
                                userAnswerId = exam.User.GeoAnswers.FirstOrDefault(x => x.Answer.QuestionId == m.Id)?.AnswerId,
                                answers = m.Answers.Select(a => new
                                {
                                    id = a.Id,
                                    text = a.Text
                                })
                            }),

                            point1 = exam.Poetry.PointOne,
                            point2 = exam.Poetry.PointTwo,
                            point3 = exam.Poetry.PointThree
                        }
                    },
                    continueExam = continueExam,
                    remainingSeconds = exam.GetRemainingSeconds(continueExam)
                });
            } catch(Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        [HttpPost]
        public JsonResult SetAnswer(int answerId, int questionId, int examId)
        {
            try
            {
                _geoExamService.SaveAnswer(answerId, questionId, examId);
                return SuccessResult();
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveTextEditing(int examId, string text)
        {
           try
            {
                _geoExamService.SaveTextEditing(examId, text);
                return SuccessResult();
            }
                   catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveEssay(int examId, string text)
        {
            try
            {
                _geoExamService.SaveEssey(examId, text);
                return SuccessResult();
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveUserTheme(int examId, string text)
        {
            try
            {
                _geoExamService.SaveTheme(examId, text);
                return SuccessResult();
            }
                   catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GameOver(GeoFinishedExamViewModel model)
        {
            try
            {
                _geoExamService.GameOver(model);
                return SuccessResult();

            } catch(Exception e)
            {
                return ErrorResult(e.Message);
            }
     
        }

        [AllowAnonymous]
        public void Report()
        {
            _geoExamService.Report();
        }


        public ActionResult Stats(int examId)
        {
            return View();
        }
    }
}