using System;  
using System.Data.Entity;
using System.Linq;
using System.Net; 
using System.Web.Mvc;
using Nebula.Domain.Concrete;
using Nebula.Domain.Entities;

namespace Nebula.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminGeoAuthorWorkPartsController : Controller
    {
        private NebulaDbContext db = new NebulaDbContext();

        // GET: AdminGeoAuthorWorkParts
        public ActionResult Index()
        {
            var geoAuthorWorkParts = db.GeoAuthorWorkParts.Include(g => g.AuthorWork).OrderBy(a=>a.AuthorWork.Title).ThenBy(a=>a.Id);
            return View(geoAuthorWorkParts.ToList());
        }

        public JsonResult GetAuthors()
        {
            return Json(db.GeoAuthors.ToList(),JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAuthorWorksById(int id)
        {  
            var works = db.GeoAuthorWorks.Where(a => a.AuthorId == id);

            return Json(works);
        }

        // GET: AdminGeoAuthorWorkParts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var geoAuthorWorkPart = db.GeoAuthorWorkParts.Find(id);
            if (geoAuthorWorkPart == null)
            {
                return HttpNotFound();
            }
            return View(geoAuthorWorkPart);
        }

        // GET: AdminGeoAuthorWorkParts/Create
        public ActionResult Create()
        {
            ViewBag.AuthorWorkId = new SelectList(db.GeoAuthorWorks, "Id", "Title");
            return View();
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PartName,AuthorWorkId")] GeoAuthorWorkPart geoAuthorWorkPart)
        {
            if (ModelState.IsValid)
            {
                db.GeoAuthorWorkParts.Add(geoAuthorWorkPart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorWorkId = new SelectList(db.GeoAuthorWorks, "Id", "Title", geoAuthorWorkPart.AuthorWorkId);
            return View(geoAuthorWorkPart);
        }

        // GET: AdminGeoAuthorWorkParts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var geoAuthorWorkPart = db.GeoAuthorWorkParts.Find(id);

            if (geoAuthorWorkPart == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorWorkId = new SelectList(db.GeoAuthorWorks, "Id", "Title", geoAuthorWorkPart.AuthorWorkId);
            return View(geoAuthorWorkPart);
        }

        // POST: AdminGeoAuthorWorkParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PartName,AuthorWorkId")] GeoAuthorWorkPart geoAuthorWorkPart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(geoAuthorWorkPart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorWorkId = new SelectList(db.GeoAuthorWorks, "Id", "Title", geoAuthorWorkPart.AuthorWorkId);
            return View(geoAuthorWorkPart);
        }

        // GET: AdminGeoAuthorWorkParts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var geoAuthorWorkPart = db.GeoAuthorWorkParts.Find(id);
            if (geoAuthorWorkPart == null)
            {
                return HttpNotFound();
            }
            return View(geoAuthorWorkPart);
        }

        // POST: AdminGeoAuthorWorkParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var geoAuthorWorkPart = db.GeoAuthorWorkParts.Find(id);
            db.GeoAuthorWorkParts.Remove(geoAuthorWorkPart);
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
