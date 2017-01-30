using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Nebula.Web.Controllers
{
    public class BaseController : Controller
    {
        public string _currentUserId;

        public JsonResult SuccessResult()
        {
            return Json(new
            {
                success = true
            });
        }
        public JsonResult NeedLicenseJson()
        {
            return Json("Need Lisence");
        }

        public JsonResult ErrorResult(string text)
        {
            return Json(new
            {
                error = text
            });
        }
        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            _currentUserId = User.Identity.GetUserId();
        }
    }

}