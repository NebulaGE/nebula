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

namespace Nebula.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminGeoEssaysController : Controller
    {
        private NebulaDbContext db = new NebulaDbContext();

        // GET: AdminGeoEssays
        public ActionResult Index()
        {
            return View(db.GeoEssays.ToList());
        }

        // GET: AdminGeoEssays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoEssay geoEssay = db.GeoEssays.Find(id);
            if (geoEssay == null)
            {
                return HttpNotFound();
            }
            return View(geoEssay);
        }

        // GET: AdminGeoEssays/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Topic")] GeoEssay geoEssay)
        {
            if (ModelState.IsValid)
            {
                db.GeoEssays.Add(geoEssay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(geoEssay);
        }

        // GET: AdminGeoEssays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoEssay geoEssay = db.GeoEssays.Find(id);
            if (geoEssay == null)
            {
                return HttpNotFound();
            }
            return View(geoEssay);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Topic")] GeoEssay geoEssay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(geoEssay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(geoEssay);
        }

        // GET: AdminGeoEssays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoEssay geoEssay = db.GeoEssays.Find(id);
            if (geoEssay == null)
            {
                return HttpNotFound();
            }
            return View(geoEssay);
        }

        // POST: AdminGeoEssays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GeoEssay geoEssay = db.GeoEssays.Find(id);
            db.GeoEssays.Remove(geoEssay);
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
