using System.Web;
using System.Web.Optimization;

namespace E_School
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                       "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*", "~/Scripts/jquery.unobtrusive-ajax.min",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/Admin").Include(
                         "~/Content/AdminHtml/js/demo.min.js",
                         "~/ Content/AdminHtml/js/application.min.js",
                         "~/Content/AdminHtml/js/elephant.min.js",
                         "~/Content/AdminHtml/js/vendor.min.js"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(

                        "~/Content/AdminHtml/css/vendor-rtl.min.css",
                        "~/Content/AdminHtml/css/application-rtl.min.css",
                        "~/Content/AdminHtml/css/demo-rtl.min.css",
                        "~/Content/Font/fonts/FontByekan.css",
                        "~/Content/AdminHtml/css/Addon.css"
                        ));
        }
    }
}