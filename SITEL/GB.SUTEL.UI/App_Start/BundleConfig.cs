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
                "~/Scripts/jquery-ui.js",
                "~/Scripts/alertifyjs/alertify.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/internalAjax").Include(
                "~/Scripts/reusableComponents.js"));

            bundles.Add(new ScriptBundle("~/bundles/Espectro").Include(
               "~/Scripts/Espectro.js"));

            bundles.Add(new ScriptBundle("~/bundles/Fonatel").Include(
             "~/Scripts/Fonatel/JsMensaje.js",
             "~/Scripts/Fonatel/JsCategoria.js",
             "~/Scripts/Fonatel/JsRelacionCategoria.js",
              "~/Scripts/Fonatel/JsUtilidades.js",
              "~/Scripts/Fonatel/JsReglas.js",
              "~/Scripts/Fonatel/JsIndicador.js",
              "~/Scripts/Fonatel/JsSolicitud.js",
              "~/Scripts/Fonatel/JsFuentes.js",
              "~/Scripts/Fonatel/JsPublicar.js",
              "~/Scripts/Fonatel/JsFormularios.js",
              "~/Scripts/Fonatel/JsDefiniciones.js",
              "~/Scripts/Fonatel/JsStepper.js",
               "~/Scripts/Fonatel/JsHistorico.js",
               "~/Scripts/Fonatel/JsRegistroIndicadores.js",
               "~/Scripts/Fonatel/JsDescargaFormularioWeb.js",
               "~/Scripts/Fonatel/JsFormulasCalculo.js"));

            bundles.Add(new ScriptBundle("~/bundles/internalSelectRowGrid").Include(
                "~/Scripts/selectRowGrid.js",
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/internal").Include(
                "~/Scripts/reusableComponents.js",
                "~/Scripts/selectRowGrid.js",
                "~/Scripts/DataTables/datatables.min.js",
                "~/Content/jsTree/jstree.min.js",
                "~/Scripts/main.js",
                "~/Scripts/DataTables/pdfmake.min.js",
                "~/Scripts/DataTables/scroller.bootstrap.min.js",
                  "~/Scripts/DataTables/dataTables.scroller.min.js",
                  "~/Scripts/DataTables/dataTables.jszip.min.js",
                  "~/Scripts/DataTables/vfs_fonts.js"

                ));
            bundles.Add(new ScriptBundle("~/bundles/funcionalidadesExtra").Include(
               "~/Scripts/Mantenimientos/Constructor/ConstructorCrear.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/pnotify.custom.min.css",
                      "~/Content/Site.css",
                      "~/Content/structure.scss",
                      "~/Content/jsTree/themes/default/style.min.css",
                      "~/Content/DataTables/datatables.min.css",
                      "~/Content/Select2.min.css",
                      "~/Content/calendar.css",
                      "~/Content/Fonatel/CustomBootstrap.css",
                        "~/Content/Fonatel/StepComponent.css",
                         "~/Content/Fonatel/Spinner.css",
                      "~/Content/alertifyjs/alertify.min.css",
                      "~/Content/alertifyjs/bootstrap.min.css",
                       "~/Content/alertifyjs/CustomAlertify.css",
                        "~/Content/DataTables/buttons.bootstrap5.min.css",
                        "~/Content/DataTables/scroller.bootstrap5.min.css",
                        "~/Content/font-awesome.min.css"));
            BundleTable.EnableOptimizations = true;
        }
    }
}
