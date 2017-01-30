using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nebula.Web.Filters
{
    public class Authorize : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!GlobalClass.isAuth)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary 
                {
                {"Controller", "NebulaLogin"}, 
                {"Action", "Login"}
                });
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class TeacherAuthorize : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!GlobalClass.IsAuthTeacher)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary 
                {
                {"Controller", "Teacher"}, 
                {"Action", "Login"}
                });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}