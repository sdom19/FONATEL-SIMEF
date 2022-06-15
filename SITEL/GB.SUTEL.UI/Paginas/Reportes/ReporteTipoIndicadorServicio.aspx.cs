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
using System.Security.Claims;

namespace GB.SUTEL.UI.Paginas.Reportes
{
    public partial class ReporteTipoIndicadorServicio : System.Web.UI.Page
    {
        #region atributos

        ServicioBL refServicioBL;

        public Shared.ApplicationContext AppContext { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                WindowsPrincipal currentPrincipal = new WindowsPrincipal(currentIdentity);
                string usuarioDominio = currentPrincipal.Identity.Name;

                AppContext = new ApplicationContext(usuarioDominio, "SUTEL - Captura Indicadores", ExceptionHandler.ExceptionType.Presentation);

                refServicioBL = new ServicioBL(AppContext);
                if (!IsPostBack)
                {
                    Session["listaTiposIndicador"] = null;
                    lCargarServicios();
                    lCargarTipoIndicador();
                    divMensajeError.Visible = false;
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
        /// Carga los servicios
        /// </summary>
        private void lCargarServicios()
        {
            try
            {
                Respuesta<List<Servicio>> resultado = new Respuesta<List<Servicio>>();
                resultado = refServicioBL.ConsultarTodos();
                ddlServicios.DataSource = resultado.objObjeto.OrderBy(x => x.DesServicio).ToList();
                ddlServicios.DataBind();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;

                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }

        /// <summary>
        /// Carga los tipos de indicador
        /// </summary>
        private void lCargarTipoIndicador()
        {
            try
            {
                Respuesta<List<TipoIndicador>> resultado = new Respuesta<List<TipoIndicador>>();
                int idServicio = 0;
                if (!ddlServicios.SelectedValue.Equals("Sel"))
                {
                    idServicio = int.Parse(ddlServicios.SelectedValue);
                }
                resultado = refServicioBL.gObenerTipoIndicadoresPorServicioReporte(idServicio);
                ddlTipoIndicador.DataSource = resultado.objObjeto.OrderBy(x => x.DesTipoInd).ToList();
                ddlTipoIndicador.DataBind();
                Session["listaTiposIndicador"] = resultado;
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;

                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }
        /// <summary>
        /// Obtiene los tipos de indicador cuando el servicio es diferente de todos
        /// </summary>
        /// <returns></returns>
        private String lObtenerIdTiposIndicador()
        {
            String idTiposIndicador = "";
            Respuesta<List<TipoIndicador>> resultado = new Respuesta<List<TipoIndicador>>();
            try
            {
                if (Session["listaTiposIndicador"] != null)
                {
                    resultado = (Respuesta<List<TipoIndicador>>)Session["listaTiposIndicador"];
                }
                foreach (TipoIndicador item in resultado.objObjeto)
                {
                    idTiposIndicador += item.IdTipoInd.ToString() + ",";
                }
                if (idTiposIndicador.Length > 0)
                {
                    idTiposIndicador = idTiposIndicador.Remove(idTiposIndicador.Length - 1);
                }
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;

                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return idTiposIndicador;
        }
        /// <summary>
        /// hace el llamado al reporte
        /// </summary>
        /// <param name="idServicios"></param>
        /// <param name="idTipoIndicador"></param>
        /// <param name="idUsuario"></param>
        private void lCrearReporte(string idServicios, string idTipoIndicador, string idUsuario)
        {

            try
            {


                var paramList = new List<ReportParameter>();

                var pathTemp = Server.MapPath("~/") + "archivosReporte";
                var separador = ConfigurationManager.AppSettings["ReportPath"].ToString();
                rptReporte.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportPath"].ToString() + ConfigurationManager.AppSettings["CarpetaReportesServer"].ToString() + separador + ConfigurationManager.AppSettings["ReporteTipoIndicadorPorServicio"].ToString();
                rptReporte.ProcessingMode = ProcessingMode.Remote;

                paramList.Add(new ReportParameter("ParamServicios", idServicios));
                paramList.Add(new ReportParameter("ParamTipoIdicador", idTipoIndicador));
                paramList.Add(new ReportParameter("ParamUsuario", idUsuario));


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

        /// <summary>
        /// Inserta la primera opción en el combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlServicios_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Todos", "Sel");
            if (!ddlServicios.Items.Contains(item))
            {
                ddlServicios.Items.Insert(0, item);
            }
        }
        /// <summary>
        /// Inserta la primera opción en el combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoIndicador_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Todos", "Sel");
            if (!ddlTipoIndicador.Items.Contains(item))
            {
                ddlTipoIndicador.Items.Insert(0, item);
            }
        }
        /// <summary>
        /// Evento cuando se cambia el servicio seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lCargarTipoIndicador();
            }
            catch (Exception ex)
            {
                lblTextoError.Text = "Ocurrió un error. Por favor, consulte con el administrador";
                divMensajeError.Visible = true;
            }
        }
        /// <summary>
        /// Busca por los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            String strIdServicio = "";
            String strIdTipoIndicador = "";
            lblTextoError.Text = "";
            divMensajeError.Visible = false;
            try
            {
                if (!ddlServicios.SelectedValue.Equals("Sel"))
                {
                    strIdServicio = ddlServicios.SelectedValue;
                    if (ddlTipoIndicador.SelectedValue.Equals("Sel"))
                    {
                        strIdTipoIndicador = lObtenerIdTiposIndicador();
                    }
                    else
                    {
                        strIdTipoIndicador = ddlTipoIndicador.SelectedValue;
                    }
                }

                lCrearReporte(strIdServicio, strIdTipoIndicador, (string)Session["usuarioDominio"]);
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