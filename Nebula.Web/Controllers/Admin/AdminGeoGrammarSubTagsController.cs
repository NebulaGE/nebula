using System.Data.Entity;
using System.Linq;
using System.Net; 
using System.Web.Mvc;
using Nebula.Domain.Concrete;
using Nebula.Domain.Entities;

namespace Nebula.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminGeoGrammarSubTagsController : Controller
    {
        private readonly NebulaDbContext _db = new NebulaDbContext();
         
        public ActionResult Index()
        {
            var geoGrammarSubTags = _db.GeoGrammarSubTags.Include(g => g.GeoGrammarTag);
            return View(geoGrammarSubTags.ToList());
        }
         
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var geoGrammarSubTag = _db.GeoGrammarSubTags.Find(id);
            if (geoGrammarSubTag == null)
            {
                return HttpNotFound();
            }
            return View(geoGrammarSubTag);
        }
 
        public ActionResult Create()
        {
            ViewBag.GeoGrammarTagId = new SelectList(_db.GeoGrammarTags, "Id", "TagName");
            return View();
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TagName,GeoGrammarTagId")] GeoGrammarSubTag geoGrammarSubTag)
        {
            if (ModelState.IsValid)
            {
                _db.GeoGrammarSubTags.Add(geoGrammarSubTag);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GeoGrammarTagId = new SelectList(_db.GeoGrammarTags, "Id", "TagName", geoGrammarSubTag.GeoGrammarTagId);
            return View(geoGrammarSubTag);
        }
         
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var geoGrammarSubTag = _db.GeoGrammarSubTags.Find(id);
            if (geoGrammarSubTag == null)
            {
                return HttpNotFound();
            }
            ViewBag.GeoGrammarTagId = new SelectList(_db.GeoGrammarTags, "Id", "TagName", geoGrammarSubTag.GeoGrammarTagId);
            return View(geoGrammarSubTag);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TagName,GeoGrammarTagId")] GeoGrammarSubTag geoGrammarSubTag)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(geoGrammarSubTag).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GeoGrammarTagId = new SelectList(_db.GeoGrammarTags, "Id", "TagName", geoGrammarSubTag.GeoGrammarTagId);
            return View(geoGrammarSubTag);
        }
 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var geoGrammarSubTag = _db.GeoGrammarSubTags.Find(id);
            if (geoGrammarSubTag == null)
            {
                return HttpNotFound();
            }
            return View(geoGrammarSubTag);
        }
         
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var geoGrammarSubTag = _db.GeoGrammarSubTags.Find(id);
            _db.GeoGrammarSubTags.Remove(geoGrammarSubTag);
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
