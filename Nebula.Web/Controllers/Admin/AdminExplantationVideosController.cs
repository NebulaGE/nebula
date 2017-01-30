using System; 
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities; 


namespace Nebula.Web.Controllers.Admin
{
    [Authorize(Roles="Admin")]
    public class AdminExplantationVideosController : Controller
    {

        private readonly BaseRepository<ExplantationVideo> _explantationVideoRepo;

        public AdminExplantationVideosController(BaseRepository<ExplantationVideo> explantationVideoRepository)
        {
            _explantationVideoRepo = explantationVideoRepository;  
        } 

        // GET: ExplantationVideos
        public ActionResult Index()
        {
            return View(_explantationVideoRepo.Set().OrderByDescending(m => m.Id).ToList());
        }

        // GET: ExplantationVideos/Create
        public ActionResult Create()
        {
            return View();
        }

    
        [HttpPost]      
        public ActionResult Create(ExplantationVideo explantationVideo)
        {
            if (ModelState.IsValid)
            {
                
                //string fileName = Guid.NewGuid() + explantationVideo.Video.FileName;
                //string path = GlobalClass.ExplantationVideoPath;
                //GlobalClass.AddFile(explantationVideo.Video, fileName, path);
                explantationVideo.Name = explantationVideo.Name;
                explantationVideo.FileName = explantationVideo.FileName;
                _explantationVideoRepo.Save(explantationVideo);

                return RedirectToAction("Index");
            }

            return View(explantationVideo);
        }

        // GET: ExplantationVideos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExplantationVideo explantationVideo = _explantationVideoRepo.Fetch((int) id);
            if (explantationVideo == null)
            {
                return HttpNotFound();
            }
            return View(explantationVideo);
        }

      
        [HttpPost]
    
        public ActionResult Edit(ExplantationVideo explantationVideo)
        {
            if (ModelState.IsValid)
            {
                //if (explantationVideo.Video != null)
                //{
                //    string fileName = Guid.NewGuid() + explantationVideo.Video.FileName;
                //    GlobalClass.AddFile(explantationVideo.Video, fileName, GlobalClass.ExplantationVideoPath);
                //    GlobalClass.DeleteFile(explantationVideo.FileName, GlobalClass.ExplantationVideoPath);
                //    explantationVideo.FileName = fileName;
                //} 
                _explantationVideoRepo._context.Entry(explantationVideo).State = EntityState.Modified;
                _explantationVideoRepo._context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(explantationVideo);
        }

        // GET: ExplantationVideos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExplantationVideo explantationVideo = _explantationVideoRepo.Fetch((int) id);
            if (explantationVideo == null)
            {
                return HttpNotFound();
            }
            return View(explantationVideo);
        }

        // POST: ExplantationVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ExplantationVideo explantationVideo = _explantationVideoRepo.Fetch(id);
            //string fileName = explantationVideo.FileName;
            _explantationVideoRepo.Delete(explantationVideo);

            //GlobalClass.DeleteFile(fileName, GlobalClass.ExplantationVideoPath);
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
