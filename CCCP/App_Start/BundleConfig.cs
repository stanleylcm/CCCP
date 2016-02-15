using System.Web;
using System.Web.Optimization;

namespace CCCP
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/dataTables.tableTools.js",
                        "~/Scripts/dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/jquery.slimscroll.min.js",
                      "~/Scripts/fastclick.min.js",
                      "~/Scripts/app.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/site.css",
                      "~/Content/skins/_all-skins.min.css",
                      "~/Content/bootstrap-theme.min.css",
                      "~/Content/bootstrap-switch.min.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.structure.css",
                      "~/Content/jquery-ui.theme.css",
                      "~/Content/dataTables.bootstrap.css",
                      "~/Content/dataTables.tableTools.css"));
        }
    }
}
