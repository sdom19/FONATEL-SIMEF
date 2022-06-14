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


namespace GB.SUTEL.UI.Paginas
{
    public partial class ReporteIndicadorPorOperador : System.Web.UI.Page
    {

        #region atributos
        ServicioBL refServicioBL;
        OperadorBL refOperadorBL;
        DireccionBL refDireccionBL;
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
                refOperadorBL = new OperadorBL(AppContext);
                refDireccionBL = new DireccionBL();
                if (!IsPostBack)
                {
                    Session["listaIndicador"] = null;
                    lCargarOperadores();
                    lCargarServicios();
                    lCargarDirecciones();
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
        /// Carga la lista de operadores
        /// </summary>
        private void lCargarOperadores()
        {
            try
            {
                Respuesta<List<Operador>> resultado = new Respuesta<List<Operador>>();
                resultado = refOperadorBL.ConsultarTodos();
                ddlOperador.DataSource = resultado.objObjeto.OrderBy(x => x.NombreOperador).ToList();
                ddlOperador.DataBind();
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
        private void lCargarServicios()
        {
            try
            {
                Respuesta<List<Servicio>> resultado = new Respuesta<List<Servicio>>();
                string idOperador = "";
                if (!ddlOperador.SelectedValue.Equals("Sel"))
                {
                    idOperador = ddlOperador.SelectedValue;
                }
                resultado = refServicioBL.gObtenerServiciosPorOperadorReporte(idOperador);
                ddlServicio.DataSource = resultado.objObjeto.OrderBy(x => x.DesServicio).ToList();
                ddlServicio.DataBind();
                Session["listaServicios"] = resultado;
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }
        /// <summary>
        /// Carga de direcciones
        /// </summary>
        private void lCargarDirecciones()
        {
            try
            {
                Respuesta<List<Direccion>> resultado = new Respuesta<List<Direccion>>();
                resultado = refDireccionBL.gListar();
                ddlDireccion.DataSource = resultado.objObjeto.OrderBy(x => x.Nombre).ToList();
                ddlDireccion.DataBind();
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
        private String lObtenerIdServicio()
        {
            String idsServicio = "";
            Respuesta<List<Servicio>> resultado = new Respuesta<List<Servicio>>();
            if (Session["listaServicios"] != null)
            {
                resultado = (Respuesta<List<Servicio>>)Session["listaServicios"];
            }
            foreach (Servicio item in resultado.objObjeto)
            {
                idsServicio += item.IdServicio.ToString() + ",";
            }
            if (idsServicio.Length > 0)
            {
                idsServicio = idsServicio.Remove(idsServicio.Length - 1);
            }
            return idsServicio;
        }
        /// <summary>
        /// hace el llamado al reporte
        /// </summary>
        /// <param name="idServicios"></param>
        /// <param name="idOperador"></param>
        /// <param name="idUsuario"></param>
        private void lCrearReporte(string idOperador, string idServicios, string idDireccion, string idUsuario)
        {

            try
            {


                var paramList = new List<ReportParameter>();

                var pathTemp = Server.MapPath("~/") + "archivosReporte";
                var separador = ConfigurationManager.AppSettings["ReportPath"].ToString();
                rptReporte.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportPath"].ToString() + ConfigurationManager.AppSettings["CarpetaReportesServer"].ToString() + separador + ConfigurationManager.AppSettings["ReporteIndicadorPorOperador"].ToString();
                rptReporte.ProcessingMode = ProcessingMode.Remote;

                paramList.Add(new ReportParameter("ParamServicios", idServicios));
                paramList.Add(new ReportParameter("ParamOperador", idOperador));
                paramList.Add(new ReportParameter("ParamDireccion", idDireccion));
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
        protected void ddlOperador_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                divMensajeError.Visible = false;
                lCargarServicios();
            }
            catch (Exception ex)
            {
                lblTextoError.Text = "Ocurrió un error. Por favor, consulte con el administrador";
                divMensajeError.Visible = true;
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

        protected void ddlServicio_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Todos", "Sel");
            if (!ddlServicio.Items.Contains(item))
            {
                ddlServicio.Items.Insert(0, item);
            }
        }

        protected void ddlDireccion_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Todos", "Sel");
            if (!ddlDireccion.Items.Contains(item))
            {
                ddlDireccion.Items.Insert(0, item);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            String strIdOperador = "";
            String strIdIndicador = "";
            String strIdDireccion = "";
            lblTextoError.Text = "";
            divMensajeError.Visible = false;
            try
            {
                if (!ddlOperador.SelectedValue.Equals("Sel"))
                {
                    strIdOperador = ddlOperador.SelectedValue;
                    if (ddlServicio.SelectedValue.Equals("Sel"))
                    {
                        strIdIndicador = lObtenerIdServicio();
                    }
                    else
                    {
                        strIdIndicador = ddlServicio.SelectedValue;
                    }
                }
                if (!ddlDireccion.SelectedValue.Equals("Sel"))
                {
                    strIdDireccion = ddlDireccion.SelectedValue;
                }

                lCrearReporte(strIdOperador, strIdIndicador, strIdDireccion, (string)Session["usuarioDominio"]);
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