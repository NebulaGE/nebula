
using System;
using System.Collections.Generic;
using System.Linq;  
using System.Web.Mvc; 
using System.Data.Entity;
using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;
using PagedList;

namespace Nebula.Web.Controllers.Admin
{
    [Authorize(Roles="Admin")]
    public class AdminCuponsController : Controller
    {
        private readonly BaseRepository<Cupon> _cuponRepo;
        private readonly BaseRepository<UserCupon> _userCuponRepo;

        public AdminCuponsController(BaseRepository<Cupon> cuponRepository, BaseRepository<UserCupon> userCuponRepository)
        {
            _cuponRepo = cuponRepository;
            _userCuponRepo = userCuponRepository; 
        }

        // GET: AdminCupons
        public ActionResult Index(int? page, int? percent,  bool? isActivated, bool? isUsed)
        {
            var cupons = _cuponRepo.Set().Where(c => (percent == null || c.Percent == percent) &&
                                               (isActivated == null || c.IsActivated == isActivated) &&
                                               (isUsed == null || c.IsUsed == isUsed));
            ViewBag.Percent = percent;
            ViewBag.IsUsed = isUsed;
            ViewBag.IsActivated = isActivated;
            return View(cupons.OrderByDescending(m => m.Id).ToList().ToPagedList(page ?? 1, 100));
        }

        
        public JsonResult GroupByPercent()
        {            
            return Json(new { percents = _cuponRepo.Set().GroupBy(m => m.Percent)
                .Select(m => m.Key)}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            var cupon = _cuponRepo.Fetch(id);
            return View(cupon);
        }

        [HttpPost]
        public ActionResult Edit(Cupon cupon)
        {
            if (!ModelState.IsValid)
                return View(cupon);

            _cuponRepo._context.Entry(cupon).State = EntityState.Modified;
            _cuponRepo._context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            var coupon = new Cupon();
            return View(coupon);
        }

        [HttpPost]
        public ActionResult Create(Cupon cupon, int Count)
        {

            string chars = "qwertyuiopasdfgh jklzxcvbnmABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            byte length = 6;
            var cupons = _cuponRepo.Set().ToList();
            var uniqueCupons = new List<Cupon>();
            string cuponCode;
            var random = new Random();
            if (cupon.Percent < 1 || cupon.Percent > 100)
                return Json("პროცენტი მოთავსებული უნდა იყოს 1-სა და 100-ს შორის");

            for (int i = 0; i < Count; i++)
            {
                var cuponFound = true;
                cuponCode = new string(Enumerable.Repeat(chars, length)
                              .Select(s => s[random.Next(s.Length)]).ToArray());

                while (cuponFound)
                {
                    if (cupons.Any(m => m.CuponCode == cuponCode) || uniqueCupons.Any(m => m.CuponCode == cuponCode))
                        cuponCode = new string(Enumerable.Repeat(chars, length)
                                      .Select(s => s[random.Next(s.Length)]).ToArray());
                    else
                    {
                        uniqueCupons.Add(new Cupon
                        {
                            CuponCode = cuponCode,
                            ExpireDate = cupon.ExpireDate,
                            ExpireTime = cupon.ExpireTime,
                            IsActivated = false,
                            Percent = cupon.Percent,
                            Type=cupon.Type,
                            IsUsed = false
                        });
                        cuponFound = false;
                    }                                                             
                }                          
            }

            foreach (var item in uniqueCupons)
            {
                _cuponRepo.Save(item);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var cupon = _cuponRepo.Fetch(id);
            if (cupon.IsActivated || _userCuponRepo.Set().Any(m => m.CuponId == id) || cupon.IsUsed)
                return Json("კუპონს ვერ წაშლით, რადგან გამოყენებაშია");
            _cuponRepo.Delete(cupon);

            return RedirectToAction("Index");
        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //        _db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}