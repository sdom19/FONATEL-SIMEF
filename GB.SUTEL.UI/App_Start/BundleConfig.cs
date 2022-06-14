using System.Web;
using System.Web.Optimization;

namespace GB.SUTEL.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*",
                "~/Scripts/pnotify.custom.min.js",
                "~/Scripts/notifications.js",
                "~/Scripts/jquery.maskedinput.js",
                 "~/Scripts/jsDatepickerES.js",
                 "~/Scripts/Select2.min.js",
                 "~/Scripts/sweetalert2.min.js",
                "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/internalAjax").Include(
                "~/Scripts/reusableComponents.js"));

            bundles.Add(new ScriptBundle("~/bundles/Espectro").Include(
               "~/Scripts/Espectro.js"));

            bundles.Add(new ScriptBundle("~/bundles/internalSelectRowGrid").Include(
                "~/Scripts/selectRowGrid.js",
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/internal").Include(
                "~/Scripts/reusableComponents.js",
                "~/Scripts/selectRowGrid.js",
                "~/Content/DataTables-1.10.5/media/js/jquery.dataTables.min.js",
                 "~/Content/jsTree/jstree.min.js",
                "~/Scripts/main.js"));
            bundles.Add(new ScriptBundle("~/bundles/funcionalidadesExtra").Include(
               "~/Scripts/Mantenimientos/Constructor/ConstructorCrear.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/pnotify.custom.min.css",
                      "~/Content/Site.css",
                      "~/Content/structure.scss",
                      "~/Content/jsTree/themes/default/style.min.css",
                      "~/Content/DataTables-1.10.5/media/css/jquery.dataTables.min.css",
                      "~/Content/Select2.min.css",
                      "~/Content/calendar.css"));
            BundleTable.EnableOptimizations = true;
        }
    }
}
