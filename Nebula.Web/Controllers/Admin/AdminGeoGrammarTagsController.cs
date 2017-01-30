using System.Data.Entity;
using System.Linq;
using System.Net; 
using System.Web.Mvc;
using Nebula.Domain.Concrete;
using Nebula.Domain.Entities;

namespace Nebula.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminGeoGrammarTagsController : Controller
    {
        private readonly NebulaDbContext _db = new NebulaDbContext();

        // GET: AGeoGrammarTags
        public ActionResult Index()
        {
            return View(_db.GeoGrammarTags.ToList());
        }

        // GET: AGeoGrammarTags/Details/5
        public ActionResult Details(int? id) 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var geoGrammarTag = _db.GeoGrammarTags.Find(id);
            if (geoGrammarTag == null)
            {
                return HttpNotFound();
            }
            return View(geoGrammarTag);
        }

        // GET: AGeoGrammarTags/Create
        public ActionResult Create()
        {
            return View();
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TagName,IsPayed")] GeoGrammarTag geoGrammarTag)
        {
            if (!ModelState.IsValid)
                return View(geoGrammarTag);

            _db.GeoGrammarTags.Add(geoGrammarTag);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AGeoGrammarTags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var geoGrammarTag = _db.GeoGrammarTags.Find(id);
            if (geoGrammarTag == null)
            {
                return HttpNotFound();
            }
            return View(geoGrammarTag);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TagName,IsPayed")] GeoGrammarTag geoGrammarTag)
        {
            if (!ModelState.IsValid)
                return View(geoGrammarTag);

            _db.Entry(geoGrammarTag).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
         
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var geoGrammarTag = _db.GeoGrammarTags.Find(id);
            if (geoGrammarTag == null)
            {
                return HttpNotFound();
            }
            return View(geoGrammarTag);
        }
         
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GeoGrammarTag geoGrammarTag = _db.GeoGrammarTags.Find(id);
            _db.GeoGrammarTags.Remove(geoGrammarTag);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
