using System.Web;
using System.Web.Optimization;

namespace GB.SUTEL.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Scripts

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*",
                "~/Scripts/pnotify.custom.min.js",
                "~/Scripts/notifications.js",
                "~/Scripts/jquery.maskedinput.js",
                "~/Scripts/jsDatepickerES.js",
                "~/Scripts/Select2.min.js",
                "~/Scripts/sweetalert2.min.js",
                "~/Scripts/jquery-ui.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/internalAjax").Include(
                "~/Scripts/reusableComponents.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Espectro").Include(
                "~/Scripts/Espectro.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/internalSelectRowGrid").Include(
                "~/Scripts/selectRowGrid.js",
                "~/Scripts/jquery.validate*"
                ));

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
                "~/Scripts/Mantenimientos/Constructor/ConstructorCrear.js"
                ));
            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"
                ));

            // Scripts: FONATEL

            bundles.Add(new ScriptBundle("~/bundles/FONATEL").Include(
                "~/Scripts/alertifyjs/alertify.min.js",
                "~/Scripts/Fonatel/JsUtilidades.js",
                "~/Scripts/Fonatel/JsMensaje.js",
                "~/Scripts/Fonatel/JsStepper.js",
                "~/Scripts/Moment/moment.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/CategoriaDesagregacion").Include(
                "~/Scripts/Fonatel/JsCategoria.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/RelacionCategoria").Include(
                "~/Scripts/Fonatel/JsRelacionCategoria.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/ReglasValidacion").Include(
                "~/Scripts/Fonatel/JsReglas.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/IndicadorFonatel").Include(
                "~/Scripts/Fonatel/JsIndicador.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/SolicitudFonatel").Include(
                "~/Scripts/Fonatel/JsSolicitud.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/Fuentes").Include(
                "~/Scripts/Fonatel/JsFuentes.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/PublicacionIndicador").Include(
                "~/Scripts/Fonatel/JsPublicar.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/FormularioWeb").Include(
                "~/Scripts/Fonatel/JsFormularios.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/DefinicionIndicador").Include(
                "~/Scripts/Fonatel/JsDefiniciones.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/HistoricoFonatel").Include( 
                "~/Scripts/Fonatel/JsHistorico.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/RegistroIndicadorFonatel").Include(
                "~/Scripts/Fonatel/JsRegistroIndicadores.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/FONATEL/RegistroIndicadorFonatelEdit").Include(
                "~/Scripts/Fonatel/JsRegistroIndicadoresEdit.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/EditarFormulario").Include(
                "~/Scripts/Fonatel/JsEditarFormularioWeb.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/FormulaCalculo").Include(
                "~/Scripts/Fonatel/JsFormulasCalculo.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/FONATEL/BitacoraFonatel").Include(
                "~/Scripts/Fonatel/JsBitacoraFonatel.js"
                ));

            // Stylesheets

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/pnotify.custom.min.css",
                "~/Content/Site.css",
                "~/Content/structure.scss",
                "~/Content/jsTree/themes/default/style.min.css",
                "~/Content/DataTables/datatables.min.css",
                "~/Content/Select2.min.css",
                "~/Content/calendar.css",
                "~/Content/font-awesome.min.css"
              ));

            // Stylesheets: FONATEL

            bundles.Add(new StyleBundle("~/Content/FONATEL/css").Include(
                "~/Content/Fonatel/CustomBootstrap.css",
                "~/Content/Fonatel/StepComponent.css",
                "~/Content/Fonatel/Spinner.css",
                "~/Content/alertifyjs/alertify.min.css",
                "~/Content/alertifyjs/bootstrap.min.css",
                "~/Content/alertifyjs/CustomAlertify.css",
                "~/Content/DataTables/buttons.bootstrap5.min.css",
                "~/Content/DataTables/scroller.bootstrap5.min.css"
                ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
