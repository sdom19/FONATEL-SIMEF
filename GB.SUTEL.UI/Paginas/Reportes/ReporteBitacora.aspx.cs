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
using GB.SUTEL.BL.Seguridad;
using GB.SUTEL.UI.Helpers;

namespace GB.SUTEL.UI.Paginas.Reportes
{
    public partial class ReporteBitacora : System.Web.UI.Page
    {
        #region atributos
        public Shared.ApplicationContext AppContext { get; set; }
        AccesoPermisosBL refAccesoPermisosBL;
        UsersBL refUsersBL;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                WindowsPrincipal currentPrincipal = new WindowsPrincipal(currentIdentity);
                string usuarioDominio = currentPrincipal.Identity.Name;

                AppContext = new ApplicationContext(usuarioDominio, "SUTEL - Captura Indicadores", ExceptionHandler.ExceptionType.Presentation);

                refAccesoPermisosBL = new AccesoPermisosBL(AppContext);
                refUsersBL = new UsersBL(AppContext);
                if (!IsPostBack)
                {
                    divMensajeError.Visible = false;
                    Session["listaTiposIndicador"] = null;
                    lCargarPantalla();
                    lCargarAccion();
                    lCargarUsuario();

                }
            }
            catch (Exception ex)
            {
                lblTextoError.Text = "Ocurrió un error. Por favor, consulte con el administrador";
                divMensajeError.Visible = true;
            }
        }
        #region metodos

        /// <summary>
        /// Carga las pantallas
        /// </summary>
        private void lCargarPantalla()
        {
            Respuesta<List<Pantalla>> resultado = new Respuesta<List<Pantalla>>();
            try
            {
                resultado = refAccesoPermisosBL.ConsultarPantallasReporte();
                ddlPantallas.DataSource = resultado.objObjeto.OrderBy(x => x.Nombre).ToList();
                ddlPantallas.DataBind();
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        /// <summary>
        /// Carga las acciones
        /// </summary>
        private void lCargarAccion()
        {
            Respuesta<List<Accion>> resultado = new Respuesta<List<Accion>>();
            try
            {
                resultado = refAccesoPermisosBL.ConsultarAcciones();
                ddlAccion.DataSource = resultado.objObjeto.OrderBy(x => x.Nombre).ToList();
                ddlAccion.DataBind();
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }
        /// <summary>
        /// Carga de usuarios
        /// </summary>
        private void lCargarUsuario()
        {
            Respuesta<List<Usuario>> resultado = new Respuesta<List<Usuario>>();
            try
            {
                resultado = refUsersBL.ConsultarTodos();
                ddlUsuario.DataSource = resultado.objObjeto.OrderBy(x => x.NombreUsuario).ToList();
                ddlUsuario.DataBind();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;

                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }

        /// <summary>
        /// hace el llamado al reporte
        /// </summary>
        /// <param name="idPantalla"></param>
        /// <param name="idAccion"></param>
        /// <param name="idUsuario"></param>
        private void lCrearReporte(string idPantalla, string idAccion, string idUsuarios, string fechaInicial, string fechaFinal, string idUsuario)
        {

            try
            {


                var paramList = new List<ReportParameter>();

                var pathTemp = Server.MapPath("~/") + "archivosReporte";
                var separador = ConfigurationManager.AppSettings["ReportPath"].ToString();
                rptReporte.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportPath"].ToString() + ConfigurationManager.AppSettings["CarpetaReportesServer"].ToString() + separador + ConfigurationManager.AppSettings["ReporteBitacora"].ToString();
                rptReporte.ProcessingMode = ProcessingMode.Remote;

                paramList.Add(new ReportParameter("ParamUsuario", idUsuarios));
                paramList.Add(new ReportParameter("ParamPantalla", idPantalla));
                paramList.Add(new ReportParameter("ParamAccion", idAccion));
                paramList.Add(new ReportParameter("ParamFechaInicial", fechaInicial));
                paramList.Add(new ReportParameter("ParamFechaFinal", fechaFinal));
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
        protected void ddlPantallas_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Todos", "Sel");
            if (!ddlPantallas.Items.Contains(item))
            {
                ddlPantallas.Items.Insert(0, item);
            }
        }

        protected void ddlAccion_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Todos", "Sel");
            if (!ddlAccion.Items.Contains(item))
            {
                ddlAccion.Items.Insert(0, item);
            }
        }

        protected void ddlUsuario_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Todos", "Sel");
            if (!ddlUsuario.Items.Contains(item))
            {
                ddlUsuario.Items.Insert(0, item);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            String strIdPantalla = "";
            String strIdAccion = "";
            String strIdUsuario = "";
            String strIdFechaInicial = "";
            String strIdFechaFinal = "";
            lblTextoError.Text = "";
            divMensajeError.Visible = false;
            try
            {
                if (lValidarFechas(txtFechaInicial.Text, txtFechaFinal.Text))
                {
                    if (!ddlPantallas.SelectedValue.Equals("Sel"))
                    {
                        strIdPantalla = ddlPantallas.SelectedValue;
                    }
                    if (!ddlAccion.SelectedValue.Equals("Sel"))
                    {
                        strIdAccion = ddlAccion.SelectedValue;
                    }
                    if (!ddlUsuario.SelectedValue.Equals("Sel"))
                    {
                        strIdUsuario = ddlUsuario.SelectedValue;
                    }
                    if (txtFechaInicial.Text.Length > 0) {
                    strIdFechaInicial = String.Format("{0:MM/dd/yyyy}", DateTime.Parse(txtFechaInicial.Text)); }
                    if (txtFechaFinal.Text.Length > 0) {
                    strIdFechaFinal = String.Format("{0:MM/dd/yyyy}",DateTime.Parse( txtFechaFinal.Text)); }
                    lCrearReporte(strIdPantalla, strIdAccion, strIdUsuario, strIdFechaInicial, strIdFechaFinal, (string)Session["usuarioDominio"]);
                }
            }
            catch (Exception ex)
            {
                lblTextoError.Text = "Ocurrió un error. Por favor, consulte con el administrador";
                divMensajeError.Visible = true;
            }
        }

        /// <summary>
        /// Valida las fechas
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        private bool lValidarFechas(string fechaInicial, string fechaFinal)
        {
            divMensajeError.Visible = false;
            if (fechaFinal.Length == 0 && fechaInicial.Length == 0)
            {
                return true;
            }
            if (fechaInicial.Length > 0 && CCatalogo.esFecha(fechaInicial) == false)
            {

                lblTextoError.Text = "La fecha inicial no cumple con el formato correcto de dd/mm/aaaa";
                divMensajeError.Visible = true;
                return false;
            }
            if (fechaFinal.Length > 0 && CCatalogo.esFecha(fechaFinal) == false)
            {

                lblTextoError.Text = "La fecha final no cumple con el formato correcto de dd/mm/aaaa";
                divMensajeError.Visible = true;
                return false;
            }
            return true;
        }
        #endregion

    }
}