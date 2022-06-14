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
    public partial class ReporteIndicadorPorServicio : System.Web.UI.Page
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
                    Session["listaIndicador"] = null;
                    lCargarServicios();
                    lCargarIndicador();

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
            try {
            Respuesta<List<Servicio>> resultado = new Respuesta<List<Servicio>>();
            resultado = refServicioBL.ConsultarTodos();
            ddlServicios.DataSource = resultado.objObjeto.ToList();
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
        private void lCargarIndicador()
        {
            try {
            Respuesta<List<Indicador>> resultado = new Respuesta<List<Indicador>>();
            int idServicio = 0;
            if (!ddlServicios.SelectedValue.Equals("Sel"))
            {
                idServicio = int.Parse(ddlServicios.SelectedValue);
            }
            resultado = refServicioBL.gObtenerIndicadoresPorServicioReporte(idServicio);
            ddlIndicador.DataSource = resultado.objObjeto.ToList();
            ddlIndicador.DataBind();
            Session["listaIndicador"] = resultado;
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
        private String lObtenerIdIndicador()
        {
            String idIndicador = "";
            Respuesta<List<Indicador>> resultado = new Respuesta<List<Indicador>>();
            if (Session["listaIndicador"] != null)
            {
                resultado = (Respuesta<List<Indicador>>)Session["listaIndicador"];
            }
            foreach (Indicador item in resultado.objObjeto)
            {
                idIndicador += item.IdIndicador + ",";
            }
            if (idIndicador.Length > 0)
            {
                idIndicador = idIndicador.Remove(idIndicador.Length - 1);
            }
            return idIndicador;
        }

        /// <summary>
        /// hace el llamado al reporte
        /// </summary>
        /// <param name="idServicios"></param>
        /// <param name="idIndicador"></param>
        /// <param name="idUsuario"></param>
        private void lCrearReporte(string idServicios, string idIndicador, string idUsuario)
        {

            try
            {


                var paramList = new List<ReportParameter>();

                var pathTemp = Server.MapPath("~/") + "archivosReporte";
                var separador = ConfigurationManager.AppSettings["ReportPath"].ToString();
                rptReporte.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportPath"].ToString() + ConfigurationManager.AppSettings["CarpetaReportesServer"].ToString() + separador + ConfigurationManager.AppSettings["ReporteIndicadorPorServicio"].ToString();
                rptReporte.ProcessingMode = ProcessingMode.Remote;

                paramList.Add(new ReportParameter("ParamServicio", idServicios));
                paramList.Add(new ReportParameter("ParamIndicador", idIndicador));
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
        /// <summary>
        /// Opcion por default del combo
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
        /// Obtiene los indicadores decuaerdo al servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
               
                lCargarIndicador();
                divMensajeError.Visible = false;
            }
            catch (Exception ex)
            {
                lblTextoError.Text = "Ocurrió un error. Por favor, consulte con el administrador";
                divMensajeError.Visible = true;
            }
        }
        /// <summary>
        /// Opcion por default del combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlIndicador_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Todos", "Sel");
            if (!ddlIndicador.Items.Contains(item))
            {
                ddlIndicador.Items.Insert(0, item);
            }
        }
        /// <summary>
        /// Busca y muestra el reporte decuaerdo a los paramettros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            String strIdServicio = "";
            String strIdIndicador = "";
            lblTextoError.Text = "";
            divMensajeError.Visible = false;
            try
            {
                if (!ddlServicios.SelectedValue.Equals("Sel"))
                {
                    strIdServicio = ddlServicios.SelectedValue;
                    if (ddlIndicador.SelectedValue.Equals("Sel"))
                    {
                        strIdIndicador = lObtenerIdIndicador();
                    }
                    else
                    {
                        strIdIndicador = ddlIndicador.SelectedValue;
                    }
                }

                lCrearReporte(strIdServicio, strIdIndicador, (string)Session["usuarioDominio"]);
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