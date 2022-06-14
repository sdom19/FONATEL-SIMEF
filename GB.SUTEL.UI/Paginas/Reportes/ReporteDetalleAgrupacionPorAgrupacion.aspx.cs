using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.UI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using GB.SUTEL.Shared;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using GB.SUTEL.UI.Recursos.Utilidades;


namespace GB.SUTEL.UI.Paginas.Reportes
{
    public partial class ReporteDetalleAgrupacionPorAgrupacion : System.Web.UI.Page
    {

        #region atributos
        AgrupacionBL refAgrupacionBL;
        OperadorBL refOperadorBL;
        public Shared.ApplicationContext AppContext { get; set; }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal currentPrincipal = new WindowsPrincipal(currentIdentity);
            string usuarioDominio = currentPrincipal.Identity.Name;

            AppContext = new ApplicationContext(usuarioDominio, "SUTEL - Captura Indicadores", ExceptionHandler.ExceptionType.Presentation);

            refAgrupacionBL = new AgrupacionBL(AppContext);
            refOperadorBL = new OperadorBL(AppContext);
            if (!IsPostBack)
            {
               
                lCargarAgrupacion();
                lCargarOperador();

            }

        }
        #region metodos

        /// <summary>
        /// Carga las agrupaciones
        /// </summary>
        private void lCargarAgrupacion()
        {
            Respuesta<List<Agrupacion>> resultado = new Respuesta<List<Agrupacion>>();
            resultado = refAgrupacionBL.gObtenerAgrupaciones();
            ddlAgrupacion.DataSource = resultado.objObjeto.OrderBy(x=> x.DescAgrupacion).ToList();
            ddlAgrupacion.DataBind();
        }
        /// <summary>
        /// Carga los operadores
        /// </summary>
        private void lCargarOperador()
        {
            Respuesta<List<Operador>> resultado = new Respuesta<List<Operador>>();
            resultado = refOperadorBL.ConsultarTodos();
            ddlOperador.DataSource = resultado.objObjeto.OrderBy(x=> x.NombreOperador).ToList();
            ddlOperador.DataBind();
        }

        /// <summary>
        /// hace el llamado al reporte
        /// </summary>
        /// <param name="idAgrupacion"></param>
        /// <param name="idOperador"></param>
        /// <param name="idUsuario"></param>
        private void lCrearReporte(string idAgrupacion, string idOperador, string idUsuario)
        {

            try
            {


                var paramList = new List<ReportParameter>();

                var pathTemp = Server.MapPath("~/") + "archivosReporte";
                var separador = ConfigurationManager.AppSettings["ReportPath"].ToString();
                rptReporte.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportPath"].ToString() + ConfigurationManager.AppSettings["CarpetaReportesServer"].ToString() + separador + ConfigurationManager.AppSettings["ReporteDetalleAgrupacionPorAgrupacion"].ToString();
                rptReporte.ProcessingMode = ProcessingMode.Remote;

                paramList.Add(new ReportParameter("ParamAgrupacion", idAgrupacion));
                paramList.Add(new ReportParameter("ParamOperadores", idOperador));
                paramList.Add(new ReportParameter("ParamUser", idUsuario));


                var irsc = new UtilidadReporteCredenciales(ConfigurationManager.AppSettings["usuarioSSRS"], ConfigurationManager.AppSettings["passwordSSRS"], ConfigurationManager.AppSettings["dominioSSRS"]);
                rptReporte.ServerReport.ReportServerCredentials = irsc;
                rptReporte.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["URLReport"]);
                rptReporte.ServerReport.SetParameters(paramList);
                rptReporte.Visible = true;
                rptReporte.DataBind();
                rptReporte.ServerReport.Refresh();

                //
                pnlResultado.Visible = true;
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;

                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }
        #endregion

        #region eventos
        protected void ddlAgrupacion_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Todos", "Sel");
            if (!ddlAgrupacion.Items.Contains(item))
            {
                ddlAgrupacion.Items.Insert(0, item);
            }
        }
        protected void ddlOperador_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Todos", "Sel");
            if (!ddlOperador.Items.Contains(item))
            {
                ddlOperador.Items.Insert(0, item);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            String strIdAgrupacion = "";
            String strIdOperador = "";
            lblTextoError.Text = "";
            divMensajeError.Visible = false;
            try
            {
                if (!ddlAgrupacion.SelectedValue.Equals("Sel")) {
                    strIdAgrupacion = ddlAgrupacion.SelectedValue;
                 }

                if (!ddlOperador.SelectedValue.Equals("Sel"))
                {
                    strIdOperador = ddlOperador.SelectedValue;
                }
                
                lCrearReporte(strIdAgrupacion, strIdOperador, (string)Session["usuarioDominio"]);
            }
            catch (Exception ex)
            {
                lblTextoError.Text = "Ocurrió un error. Por favor, consulte con el administrador";
                divMensajeError.Visible = true;
            }
        }

       
        #endregion
    }
}