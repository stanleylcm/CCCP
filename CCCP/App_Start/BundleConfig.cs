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
                        "~/Scripts/dataTables.bootstrap.js",
                        "~/Scripts/jquery.signalR-2.2.0.js",
                        "~/Scripts/bootstrap-checkbox.js",
                        "~/Scripts/input-mask/jquery.inputmask.js",
                        "~/Scripts/input-mask/jquery.inputmask.date.extensions.js",
                        "~/Scripts/input-mask/jquery.inputmask.extensions.js",
                        "~/Scripts/timepicker/bootstrap-timepicker.min.js",
                        "~/Scripts/bootstrap-switch.js",
                        "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate-vsdoc.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.method.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-2.6.2.js"));

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
                      "~/Content/AdminLTE.css",
                      "~/Content/Site.css",
                      "~/Content/skins/_all-skins.min.css",
                      "~/Content/bootstrap-theme.min.css",
                      "~/Content/bootstrap-switch.min.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.structure.css",
                      "~/Content/jquery-ui.theme.css",
                      "~/Content/dataTables.bootstrap.css",
                      "~/Content/dataTables.tableTools.css",
                      "~/Content/bootstrap-checkbox.css",
                      "~/Scripts/timepicker/bootstrap-timepicker.min.css",
                      "~/Content/bootstrap-switch.css"));

            bundles.UseCdn = true;

            // bundles code

            var cdnPath = "https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic";
            bundles.Add(new StyleBundle("~/fonts", cdnPath));
        }
    }
}
