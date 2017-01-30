using Microsoft.AspNet.Identity;
using System; 
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Nebula.Domain;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using Nebula.Services.Dto;
using Nebula.Services.Services.Web.CS; 
using Nebula.Domain.ViewModels.Web;
using Nebula.Services.Utils;

namespace Nebula.Web.Controllers.CS
{ 
    public class CSExercisesController : BaseController
    {
        private readonly BaseRepository<CSQuestion> _questionRepo; 
        private readonly BaseRepository<User> _userRepo; 
        private readonly BaseRepository<FinishedTask> _finishedTaskRepo;
        private readonly BaseRepository<UserAnswer> _userAnswerRepo;

        private readonly CSExerciseService _exerciseService;

        public CSExercisesController(
            BaseRepository<Condition> conditionRepository,
            BaseRepository<Module> moduleRepository,  
            BaseRepository<CSQuestion> questionRpository, 
            BaseRepository<User> userRpository,  
            BaseRepository<FinishedTask> finishedTaskRpository, 
            BaseRepository<UserAnswer> userAnswerRpository, 
            BaseRepository<CSQuestion> cSQuestionRepo, 
            BaseRepository<Module> modulRepo, 
            BaseRepository<Answer> answerRepo)
        {
            _questionRepo = questionRpository; 
            _userRepo = userRpository; 
            _finishedTaskRepo = finishedTaskRpository;
            _userAnswerRepo = userAnswerRpository;


            _exerciseService = new CSExerciseService(new GeneralRepository
            {
                UserAnswer = _userAnswerRepo,
                CsQuestion = _questionRepo,
                Module = moduleRepository,
                Answer = answerRepo,
                FinishedTask  = _finishedTaskRepo,
                Condition = conditionRepository
            }); 
        }


        #region newCode
        public ActionResult Index(int type)
        {
            ViewBag.NeedsLicense = UserLicenseCheckerHelper.CheckGeoLicense(_userRepo, _currentUserId);

            return View(new ExerciseUrlViewModel
            {          
                Type = type
            });
        }
     
        public JsonResult GetNavigation(byte type)
        {
            var model = _currentUserId == null ?  _exerciseService.GetNavigationForGuest(type) : _exerciseService.GetNavigation(type);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetQuestions(CommonNavigationDto dto)
        {
            var model = _currentUserId == null ? _exerciseService.GetQuestionsForGuest(dto.Id) : _exerciseService.GetQuestions(dto.Id, _currentUserId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }  
        
        [HttpPost]
        public JsonResult SetAnswer(SetAnswerDto dto)
        {
            var model = _currentUserId == null ? _exerciseService.SetAnswerForGuest(dto) : _exerciseService.SetAnswer(dto, _currentUserId);
            return Json(model);
        } 

        [HttpPost]
        public JsonResult GetTask(CommonNavigationDto dto)
        {
            var model = _exerciseService.GetTask(dto.Id);
            return Json(model);
        }

        [HttpPost]
        public JsonResult SetTaskAnswer(SetTaskAnswerDto dto)
        {
            if (string.IsNullOrEmpty(_currentUserId))
                return null;

            var model = _exerciseService.SetTaskAnswer(dto);
            return Json(model);
        }

        [HttpPost]
        public JsonResult ResetCSExercise(int id)
        {
            if (string.IsNullOrEmpty(_currentUserId))
                return Json(new { reload = true });

            var userId = User.Identity.GetUserId();
                var userAnswers = _userAnswerRepo.Set()
                .Include(m => m.Answer.Question)
                .Where(m => m.UserId == userId && m.Answer.Question.ModuleId == id);

                if (id == 4 || id == 7)
                {
                    var pirobas = _finishedTaskRepo.Set()
                        .Include(m => m.Task)
                        .Where(m => m.UserId == userId && m.Task.ModuleId == id && m.IsFinished);

                    foreach (var item in pirobas)
                    {
                        _finishedTaskRepo._context.FinishedTasks.Remove(item);
                    }
                }

                foreach (var item in userAnswers)
                {
                    _userAnswerRepo._context.UserAnswers.Remove(item);
                }

                _userAnswerRepo._context.SaveChanges();

                return Json(new { success = true }); 
        }


        public JsonResult GeneretePirobaOrQuestions(int moduleId)
        { 
            var question = _questionRepo.Set().FirstOrDefault(c => c.ModuleId == moduleId);
             
            return Json(new
            {
                isPiroba = question?.ConditionId             
            }, JsonRequestBehavior.AllowGet);
        }
        
 

        [HttpPost]
        public JsonResult Reset(int moduleId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var userAnswers = _userAnswerRepo.Set()
                      .Include(m => m.Answer.Question)
                      .Where(m => m.UserId == userId && m.Answer.Question.ModuleId == moduleId);

                if (moduleId == 4 || moduleId == 7)
                {
                    var pirobas = _finishedTaskRepo.Set()
                        .Include(m => m.Task)
                        .Where(m => m.UserId == userId && m.Task.ModuleId == moduleId && m.IsFinished);

                    foreach (var item in pirobas)
                    {
                        _finishedTaskRepo._context.FinishedTasks.Remove(item);
                    } 
                }

                foreach (var item in userAnswers)
                {
                    _userAnswerRepo._context.UserAnswers.Remove(item);
                }

                _userAnswerRepo._context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        
        }
        #endregion
    }
}