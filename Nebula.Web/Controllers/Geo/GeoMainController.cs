using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using Nebula.Domain.ViewModels.Web.Geo; 
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace Nebula.Web.Controllers.Geo
{
     
    public class GeoMainController : BaseController
    {
        private readonly BaseRepository<GeoGrammarTag> _grammarTagRepo;
        private readonly BaseRepository<GeoAuthorWork> _authorWorkRepo;
        private readonly BaseRepository<User> _userRepo;
        private readonly BaseRepository<GeoUserExam> _userExamRepo;
        private readonly BaseRepository<GeoGrammarSubTag> _grammarSubTag;
        private readonly BaseRepository<GeoAuthor> _authorRepo;

        public GeoMainController(BaseRepository<GeoGrammarTag> grammarTagRepo, 
            BaseRepository<GeoAuthorWork> authorWorkRepo,
            BaseRepository<User> user,
            BaseRepository<GeoUserExam> userExam,
            BaseRepository<GeoGrammarSubTag> grammarSubTag,
            BaseRepository<GeoAuthor> author
            )
        {
            _grammarTagRepo = grammarTagRepo;
            _authorWorkRepo = authorWorkRepo;
            _userRepo = user;
            _userExamRepo = userExam;
            _grammarSubTag = grammarSubTag;
            _authorRepo = author;
        }

        // GET: GeoMain
        public ActionResult Index()
        {
            var user = _userRepo.Fetch(_currentUserId);

            if (user == null) 
                return RedirectToAction("Guest");

            var model = new GeoMainPageViewModel
            {
                VideoTagId =  _grammarTagRepo.Set().First(m => !m.IsPayed).Id,
                VideoAuthorWorkId = _authorRepo.Set().First(m => !m.IsPayed).Id,
                ExerciseAuthorWorkId = _authorRepo.Set().First().Id,
                ExerciseSubTagId = _grammarSubTag.Set().Include(m => m.Questions).First(a => a.Questions.Any(b => b.QuestionType == GeoQuestionType.ExerciseQuestion)).Id,
                HasGeoLicense = user.HasGeoLicense
            };
            
            if (!user.HasGeoLicense)
                model.ExamStatus = ExamStatus.Disable;
            else
            {
                var lastExam = _userExamRepo.Set()
                    .Where(m => m.UserId == user.Id).OrderByDescending(m => m.Id).FirstOrDefault(m => !m.IsFinished);

                if (lastExam != null  && !lastExam.Timeout())
                {
                    model.ExamStatus = ExamStatus.Continue;
                }
                else if (lastExam != null && lastExam.Timeout())
                {
                    lastExam.IsFinished = true;
                    _userExamRepo.Save(lastExam);
                    model.ExamStatus = ExamStatus.Start;
                }
                else
                {
                    model.ExamStatus = ExamStatus.Start;
                }

                model.PrevExam = _userExamRepo.Set()
                    .Where(m => m.UserId == user.Id).OrderByDescending(m => m.Id).FirstOrDefault(m => m.IsFinished)?.Id;

            }
            return View(model);
        }

        public ActionResult Guest()
        {
            var model = new GeoMainPageViewModel()
            {
                ExamStatus=ExamStatus.Disable,
                PrevExam = null,
                VideoTagId = _grammarTagRepo.Set().First(m => !m.IsPayed).Id,
                VideoAuthorWorkId = _authorRepo.Set().First(m => !m.IsPayed).Id,
                ExerciseAuthorWorkId = 1,
                ExerciseSubTagId = 1
            };

            return View("Index",model);
        }
    }
}