using System.Web.Mvc;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using System.Data.Entity;
using System.Linq;
using PagedList;
using Nebula.Services.Services.Admin;
using Nebula.Domain;
using Nebula.Domain.ViewModels.Admin.Geo;

namespace Nebula.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminGeoQuestionController : Controller
    {
        private readonly BaseRepository<GeoQuestion> _questionRepo;
        private readonly BaseRepository<GeoAuthorWork> _geoAuthorWorkRepo;
        private readonly BaseRepository<GeoAuthorWorkPart> _geoAuthorWorkPartRepo;
        private readonly BaseRepository<GeoGrammarSubTag> _geoGrammarSubTagRepo;
        private readonly AdminGeoService _adminService; 

        public AdminGeoQuestionController(
            BaseRepository<GeoQuestion> questionRepo, 
            BaseRepository<GeoAuthor> geoAuthor, 
            BaseRepository<GeoAuthorWork> geoAuthorWork, 
            BaseRepository<GeoAuthorWorkPart> geoAuthorWorkPart, 
            BaseRepository<GeoGrammarTag> geoGrammarTag, 
            BaseRepository<GeoGrammarSubTag> geoGrammarSubTag, 
            BaseRepository<GeoAnswer> geoAnswerRepo)
        {
            _questionRepo = questionRepo;
            _geoAuthorWorkRepo = geoAuthorWork;
            _geoAuthorWorkPartRepo = geoAuthorWorkPart;
            _geoGrammarSubTagRepo = geoGrammarSubTag;

            _adminService = new AdminGeoService(new GeneralRepository
            {
                GeoGrammarTag=geoGrammarTag,
                GeoQuestion = _questionRepo,
                GeoAuthor = geoAuthor,
                GeoAuthorWork=_geoAuthorWorkRepo,
                GeoAuthorWorkPart = _geoAuthorWorkPartRepo,
                GeoAnswer=geoAnswerRepo
            });
        } 

        public ActionResult Index(byte id, int? page)
        {
            var cat = id == 1 ? GeoQuestionCategory.Literature : GeoQuestionCategory.Grammar;

            var questions =
                _questionRepo.Set()
                .Include(a => a.GeoAuthorWorkPart.AuthorWork.Author)
                .Include(a=>a.GeoGrammarSubTag.GeoGrammarTag)
                .Include(m => m.Answers)
                .Where(a=>a.Category==cat && a.GeoFictionTextId == null)
                .OrderByDescending(a=>a.CreateTime); 

            ViewBag.Id = id;
            ViewBag.Count = questions.Count();

            return View(questions.ToList().ToPagedList(page ?? 1, 50));
        }

        [HttpPost]
        public JsonResult GetAuthorWorkPartsById(int id)
        {
            var parts = _geoAuthorWorkPartRepo.Set().Where(a => a.AuthorWorkId == id);

            return Json(parts);
        }

        public JsonResult GetChoosenAuthorWorksById(int id, int partId)
        {
            int selectedWork = 0;
            var works = _geoAuthorWorkRepo.Set().Where(a => a.AuthorId == id).Include(a=>a.GeoAuthorWorkParts).Select( o => new
            {
                o.Id,
                o.Title,
                o.GeoAuthorWorkParts, 
            }); 

            foreach (var work in works)
            {  
                if (work.GeoAuthorWorkParts.Any(a => a.Id == partId))
                    selectedWork = work.Id;
            }

            return Json(new { works = works.ToList(), selectedWorkId = selectedWork});
        }

        public JsonResult GetSubTagsByTagId(int id) 
        {
            var tags = _geoGrammarSubTagRepo.Set().Where(a => a.GeoGrammarTagId == id);

            return Json(tags);
        }

        public ActionResult Manage(byte questionCategoryId, int id = 0)
        {
            var model = _adminService.GetGeoQuestionViewModel(questionCategoryId,0, id);
            model.Action = "Save";
            model.Controller = "AdminGeoQuestion";
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(GeoQuestionViewModel model) 
        {  
            _adminService.SaveGeoQuestion(model); 

            return RedirectToAction("Index", new { id = model.CategoryId });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string returnUrl)
        {
            _questionRepo.Delete(id);
            return Redirect(returnUrl);
        }
    }
}