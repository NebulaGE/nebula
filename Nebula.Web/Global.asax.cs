using System; 
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing; 
using Nebula.Web.Infrastructure;


namespace Nebula.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           // GlobalConfiguration.Configure(WebApiConfig.Register);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //protected void Application_BeginRequest()
        //{
        //    if (!Context.Request.IsSecureConnection)
        //        Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));
        //}
        //protected void Application_PostAuthorizeRequest()
        //{
        //    System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        //}

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    if (!Request.Url.Host.StartsWith("www") && !Request.Url.IsLoopback)
        //    {
        //        var builder = new UriBuilder(Request.Url) {Host = "www." + Request.Url.Host};
        //        Response.StatusCode = 301;
        //        Response.AddHeader("Location", builder.ToString());
        //        Response.End();
        //    }
        //}
    }
}
