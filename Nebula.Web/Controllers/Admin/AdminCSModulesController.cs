
using System.Data.Entity;
using System.Linq;
using System.Net; 
using System.Web.Mvc;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities; 


namespace Nebula.Web.Controllers.Admin
{
   [Authorize(Roles="Admin")]
    public class AdminCSModulesController : Controller
    {
        private readonly BaseRepository<Module> _moduleRepo;
        private readonly BaseRepository<CSQuestionCategory> _cSQuestionCategoryRepo;

        public AdminCSModulesController(BaseRepository<Module> moduleRepository, BaseRepository<CSQuestionCategory> cSQuestionCategoryRepository)
        {
            _moduleRepo = moduleRepository;
            _cSQuestionCategoryRepo=cSQuestionCategoryRepository;
        }
        
        // GET: AdminCSModules
        public ActionResult Index()
        {
            var modules = _moduleRepo.Set().Include(e => e.CSQuestionCategory);
            return View(modules.ToList());
        }
        
        // GET: AdminCSModules/Create
        public ActionResult Create()
        {
            ViewBag.CSQuestionCategoryId = new SelectList(_cSQuestionCategoryRepo.Set(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Module module)
        {
            if (ModelState.IsValid)
            {
                _moduleRepo.Save(module); 
                return RedirectToAction("Index");
            }

            ViewBag.CSQuestionCategoryId = new SelectList(_cSQuestionCategoryRepo.Set(), "Id", "Name", module.CSQuestionCategoryId);
            return View(module);
        }

   
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var module = _moduleRepo.Fetch((int) id);
            if (module == null)
            {
                return HttpNotFound();
            }
            ViewBag.CSQuestionCategoryId = new SelectList(_cSQuestionCategoryRepo.Set(), "Id", "Name", module.CSQuestionCategoryId);
            return View(module);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Module module)
        {
            if (ModelState.IsValid)
            {
                _moduleRepo._context.Entry(module).State = EntityState.Modified;
                _moduleRepo._context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CSQuestionCategoryId = new SelectList(_cSQuestionCategoryRepo.Set(), "Id", "Name", module.CSQuestionCategoryId);
            return View(module);
        }

        // GET: AdminCSModules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var module = _moduleRepo.Fetch((int) id);
            if (module== null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

 
    }
}
