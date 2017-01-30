using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using Nebula.Services.Dto;
using Nebula.Services.Json;
using Nebula.Services.Services.Web.Geo;
using Nebula.Services.Utils;

namespace Nebula.Web.Controllers.Geo
{
    public class GeoExercisesController : BaseController
    {
        // GET: GeoExercises 
        private readonly GeoExercisesService _exercisesService;
        private readonly BaseRepository<GeoUserAnswer> _userAnswerRepo;
        private readonly BaseRepository<User> _userRepo;

        public GeoExercisesController(BaseRepository<GeoAnswer> answerRepo,
            BaseRepository<GeoQuestion> questionRepo,
            BaseRepository<GeoUserAnswer> userAnswerRepo,
            BaseRepository<GeoGrammarSubTag> subTagRepo,
            BaseRepository<GeoGrammarTag> tagRepo,
            BaseRepository<User> userRepo,
            BaseRepository<GeoAuthorWork> geoAuthorWork,
            BaseRepository<GeoAuthor> author)
        {
            _userAnswerRepo = userAnswerRepo;
            _userRepo = userRepo;

            _exercisesService = new GeoExercisesService(new Domain.GeneralRepository
            {
                GeoGrammarSubTag = subTagRepo,
                GeoAuthorWork = geoAuthorWork,
                GeoUserAnswer = userAnswerRepo,
                GeoQuestion = questionRepo,
                GeoAnswer = answerRepo,
                GeoAuthor = author
            });
        }


        public ActionResult Grammar()
        {
            ViewBag.NeedsLicense = UserLicenseCheckerHelper.CheckGeoLicense(_userRepo, _currentUserId);

            return View();
        }
        public ActionResult Literature()
        { 
            ViewBag.NeedsLicense = UserLicenseCheckerHelper.CheckGeoLicense(_userRepo, _currentUserId);

            return View();
        }

        public JsonResult GetGrammarNavigation()
        {
            var navigation = string.IsNullOrEmpty(_currentUserId) ? _exercisesService.GetNavigationForGuest() : _exercisesService.GetGrammarNavigation();

            return Json(navigation, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGrammarQuestions(CommonNavigationDto dto)
        {
            var model = string.IsNullOrEmpty(_currentUserId) ? _exercisesService.GetGrammarQuestionsForGuest() : _exercisesService.GetGrammarQuestions(dto.Id, _currentUserId);

            return Json(model);
        }

        public JsonResult GetLiteratureNavigation()
        {
            var navigation = string.IsNullOrEmpty(_currentUserId) ? _exercisesService.GetNavigationForGuest() : _exercisesService.GetLiteratureNavigation();
            return Json(navigation, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLiteratureQuestions(CommonNavigationDto dto)
        {
            var model = string.IsNullOrEmpty(_currentUserId) ? _exercisesService.GetLiteratureQuestionsForGuest() : _exercisesService.GetLiteratureQuestions(dto.Id, _currentUserId);
            return Json(model);
        }

        [HttpPost]
        public JsonResult SetAnswer(SetAnswerDto dto)
        {
            var model = string.IsNullOrEmpty(_currentUserId) ? _exercisesService.SetAnswerForGuest(dto) : _exercisesService.SetAnswer(dto, _currentUserId);
            return Json(model);
        }

        [HttpPost]
        public JsonResult ResetLiterature(int id)
        {
            if (string.IsNullOrEmpty(_currentUserId))
                return null;

            var userAnswers =
                _userAnswerRepo.Set()
                    .Include(a => a.Answer.Question.GeoAuthorWorkPart.AuthorWork)
                    .Where(a => a.UserId == _currentUserId &&
                    a.Answer.Question.GeoAuthorWorkPart.AuthorWork.AuthorId == id
                    && a.Answer.Question.QuestionType == GeoQuestionType.ExerciseQuestion).ToList();

            foreach (var item in userAnswers)
                _userAnswerRepo.Delete(item);

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult ResetGrammar(int id)
        {
            if (string.IsNullOrEmpty(_currentUserId))
                return null;

            var userAnswers =
                _userAnswerRepo.Set()
                    .Include(a => a.Answer.Question)
                    .Where(a => a.UserId == _currentUserId &&
                    a.Answer.Question.GeoGrammarSubTagId == id
                    && a.Answer.Question.QuestionType == GeoQuestionType.ExerciseQuestion).ToList();

            foreach (var item in userAnswers)
                _userAnswerRepo.Delete(item);

            return Json(new { success = true });
        }
    }
}