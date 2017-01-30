using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Nebula.Domain.Concrete;
using Nebula.Domain.Entities;

namespace Nebula.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminGeoAuthorWorksController : Controller
    {
        private NebulaDbContext db = new NebulaDbContext();

        // GET: AdminGeoAuthorWorks
        public ActionResult Index()
        {
            var geoAuthorWorks = db.GeoAuthorWorks.Include(g => g.Author).OrderBy(a => a.Author.AuthorName); 
            
            return View(geoAuthorWorks.ToList());
        } 

        // GET: AdminGeoAuthorWorks/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.GeoAuthors, "Id", "AuthorName");
            return View();
        }

        // POST: AdminGeoAuthorWorks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,AuthorId")] GeoAuthorWork geoAuthorWork)
        {
            if (ModelState.IsValid)
            {
                db.GeoAuthorWorks.Add(geoAuthorWork);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.GeoAuthors, "Id", "AuthorName", geoAuthorWork.AuthorId);
            return View(geoAuthorWork);
        }

        // GET: AdminGeoAuthorWorks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoAuthorWork geoAuthorWork = db.GeoAuthorWorks.Find(id);
            if (geoAuthorWork == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.GeoAuthors, "Id", "AuthorName", geoAuthorWork.AuthorId);
            return View(geoAuthorWork);
        }

        // POST: AdminGeoAuthorWorks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,AuthorId")] GeoAuthorWork geoAuthorWork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(geoAuthorWork).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.GeoAuthors, "Id", "AuthorName", geoAuthorWork.AuthorId);
            return View(geoAuthorWork);
        }

        // GET: AdminGeoAuthorWorks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoAuthorWork geoAuthorWork = db.GeoAuthorWorks.Find(id);
            if (geoAuthorWork == null)
            {
                return HttpNotFound();
            }
            return View(geoAuthorWork);
        }

        // POST: AdminGeoAuthorWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GeoAuthorWork geoAuthorWork = db.GeoAuthorWorks.Find(id);
            db.GeoAuthorWorks.Remove(geoAuthorWork);
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
