using System.Data.Entity;
using System.Linq;
using System.Net; 
using System.Web.Mvc;
using Nebula.Domain.Concrete;
using Nebula.Domain.Entities;

namespace Nebula.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminGeoAuthorsController : Controller
    {
        private NebulaDbContext db = new NebulaDbContext();

        // GET: GeoAuthors
        public ActionResult Index()
        {
            return View(db.GeoAuthors.ToList());
        } 

        // GET: GeoAuthors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GeoAuthors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AuthorName,IsPayed")] GeoAuthor geoAuthor)
        {
            if (!ModelState.IsValid)
                return View(geoAuthor);

            db.GeoAuthors.Add(geoAuthor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: GeoAuthors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var geoAuthor = db.GeoAuthors.Find(id);
            if (geoAuthor == null)
            {
                return HttpNotFound();
            }
            return View(geoAuthor);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AuthorName,IsPayed")] GeoAuthor geoAuthor)
        {
            if (!ModelState.IsValid)
                return View(geoAuthor);

            db.Entry(geoAuthor).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: GeoAuthors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var geoAuthor = db.GeoAuthors.Find(id);
            if (geoAuthor == null)
            {
                return HttpNotFound();
            }
            return View(geoAuthor);
        }

        // POST: GeoAuthors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) 
        {
            var geoAuthor = db.GeoAuthors.Find(id);
            db.GeoAuthors.Remove(geoAuthor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
