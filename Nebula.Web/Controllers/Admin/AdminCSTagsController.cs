using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities; 
using Ninject.Infrastructure.Language;


namespace Nebula.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminCSTagsController : Controller
    {
        private readonly BaseRepository<CSTag> _tagRepo;
        private readonly BaseRepository<Video> _videoRepo;
        private readonly BaseRepository<CSQuestionCategory> _cSQuestionCategoryRepo; 

        public AdminCSTagsController(
            BaseRepository<CSTag> tagRepository, 
            BaseRepository<Video> videoRepository, 
            BaseRepository<CSQuestionCategory> cSQuestionCategoryRepository)
        {
            _tagRepo = tagRepository;
            _videoRepo = videoRepository;
            _cSQuestionCategoryRepo = cSQuestionCategoryRepository;
        } 

        public ActionResult Index()
        {
            var tags = _tagRepo.Set().Include(t => t.QuestionType);
            return View(tags.ToList());
        }
         
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var csTag = _tagRepo.Fetch((int) id);
            if (csTag == null)
            {
                return HttpNotFound();
            }
            return View(csTag);
        }

        // GET: AdminCSTags/Create
        public ActionResult Create()
        {
            ViewBag.QuestionTypeId = new SelectList(_cSQuestionCategoryRepo.Set(), "Id", "Name");
            return View();
        }
        // ras svhebi nikush ? : 
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( CSTag csTag)
        {
            if (ModelState.IsValid)
            {
                _tagRepo.Save(csTag); 
                return RedirectToAction("Index");
            }

            ViewBag.QuestionTypeId = new SelectList(_cSQuestionCategoryRepo.Set(), "Id", "Name", csTag.QuestionTypeId);
            return View(csTag);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var csTag = _tagRepo.Set().FirstOrDefault(m => m.Id == id);
            if (csTag == null)
            {
                return HttpNotFound();
            }
          
            ViewBag.QuestionTypeId = new SelectList(_cSQuestionCategoryRepo.Set(), "Id", "Name", csTag.QuestionTypeId);
            return View(csTag);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CSTag cSTag)
        {
            if (ModelState.IsValid)
            { 
                _tagRepo._context.Entry(cSTag).State = EntityState.Modified;
                _tagRepo._context.SaveChanges();
              
                if (cSTag.IsPayed)
                {
                    foreach (var video in _videoRepo.Set().ToEnumerable().Where(m => m.TagId == cSTag.Id).ToList())
                    {
                        var freeVideoPath = GlobalClass.VideoPath + "\\" + video.FileName;
                        var payedVideoPath = GlobalClass.PayedVideoPath + "\\" + video.FileName;
                        if (System.IO.File.Exists(freeVideoPath))
                        {
                            if (!System.IO.File.Exists(payedVideoPath))
                            {
                                System.IO.File.Move(freeVideoPath, payedVideoPath);
                            }
                            else
                            {
                                System.IO.File.Delete(freeVideoPath);
                            }
                        }
                    }

                }
                else
                {
                    foreach (var video in _videoRepo.Set().ToEnumerable().Where(m => m.TagId == cSTag.Id).ToList())
                    {
                        var freeVideoPath = GlobalClass.VideoPath + "\\" + video.FileName;
                        var payedVideoPath = GlobalClass.PayedVideoPath + "\\" + video.FileName;
                        if (System.IO.File.Exists(payedVideoPath))
                        {
                            if (!System.IO.File.Exists(freeVideoPath))
                            {
                                System.IO.File.Move(payedVideoPath, freeVideoPath);
                            }
                            else
                            {
                                System.IO.File.Delete(payedVideoPath);
                            }
                        }

                    } 
                }
                return RedirectToAction("Index");
            }
            ViewBag.QuestionTypeId = new SelectList(_cSQuestionCategoryRepo.Set(), "Id", "Name", cSTag.QuestionTypeId);
            return View(cSTag);
        }

        // GET: AdminCSTags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTag cSTag = _tagRepo.Fetch((int) id);
            if (cSTag == null)
            {
                return HttpNotFound();
            }
            return View(cSTag);
        }

        // POST: AdminCSTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        { 
            _tagRepo.Delete(id); 
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
