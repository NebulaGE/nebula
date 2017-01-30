using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nebula.Domain;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using Nebula.Domain.ViewModels.Admin.Geo;
using Nebula.Services.Services.Admin;
using PagedList;

namespace Nebula.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminGeoTextAnalisysController : Controller
    {
        private readonly BaseRepository<GeoFictionText> _fictionTextRepo;
        private readonly AdminGeoService _adminService;
        private readonly BaseRepository<GeoAnswer> _geoAnswerRepo;
        public AdminGeoTextAnalisysController(
            BaseRepository<GeoQuestion> questionRepo,
            BaseRepository<GeoAuthor> geoAuthor,
            BaseRepository<GeoAuthorWork> geoAuthorWork,
            BaseRepository<GeoAuthorWorkPart> geoAuthorWorkPart,
            BaseRepository<GeoGrammarTag> geoGrammarTag, 
            BaseRepository<GeoAnswer> geoAnswerRepo, 
            BaseRepository<GeoFictionText> fictionTextRepo, 
            BaseRepository<GeoAnswer> geoAnswerRepo1)
        {
            _fictionTextRepo = fictionTextRepo;
            _geoAnswerRepo = geoAnswerRepo1;

            _adminService = new AdminGeoService(new GeneralRepository
            {
                GeoFictionText = _fictionTextRepo,
                GeoQuestion = questionRepo,
                GeoAuthor = geoAuthor,
                GeoAnswer = _geoAnswerRepo
            });
        }
        
        // GET: AdminGeoTextAnalisys
        public ActionResult Index(byte category, int? page)
        {
            var cat = category == 1 ? GeoThemeType.Poetry : GeoThemeType.Prose;

             var fictionTexts = _fictionTextRepo.Set()
                .Include(a=>a.Questions)
                .Where(a=>a.ThemeType==cat)
                .OrderByDescending(a => a.Id);

            ViewBag.CatId = category;

            return View(fictionTexts.ToList().ToPagedList(page ?? 1, 50));
        }


        public ActionResult Manage(byte categoryId, int id = 0)
        {
            var model = _adminService.GetGeoFictionTextViewModel(categoryId, id);
            ViewBag.Id = categoryId; 
            return View(model);
        }
         
        public ActionResult ManageQuestion(byte categoryId, int questionId, int fictionId)
        {
            var model = _adminService.GetGeoQuestionViewModel(categoryId, fictionId, questionId);
            model.Controller = "AdminGeoTextAnalisys";
            model.Action = "SaveQuestion";
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveQuestion(GeoQuestionViewModel model)
        {
            _adminService.SaveGeoQuestion(model);
            int categoryId = (int) _fictionTextRepo.Fetch(model.GeoFictionTextId.GetValueOrDefault()).ThemeType;
            return RedirectToAction("Manage", new {categoryId = categoryId, id = model.GeoFictionTextId });
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(GeoFictionTextViewModel model)
        {
            _adminService.SaveGeoFictionText(model);

            return RedirectToAction("Manage", new { categoryId = (int)model.ThemeType, id = model.Id });
        }


  

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string returnUrl)
        {
            _fictionTextRepo.Delete(id);
            return Redirect(returnUrl);
        }
    }
}