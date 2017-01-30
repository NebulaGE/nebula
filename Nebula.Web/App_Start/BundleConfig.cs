using System.Web.Optimization;

namespace Nebula.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/mainCss").Include(
                      "~/Content/reset.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/jquery-confirm.min.css",
                      "~/Content/main.css",
                      "~/Content/nika-style.css",
                      "~/Content/loader.css"
                      ));


            bundles.Add(new ScriptBundle("~/bundles/mainJs").Include(
                       "~/Scripts/geoLang.js",
                       "~/Scripts/jquery-confirm.min.js",
                       "~/Scripts/bootstrap.min.js",
                       "~/Scripts/knockout.js",
                       "~/Scripts/knockout-postbox.js",
                       "~/Js/global.js",
                       "~/Js/temo.js",
                       "~/Js/forms.js"));

            BundleTable.EnableOptimizations = true;

        }
    }
}
