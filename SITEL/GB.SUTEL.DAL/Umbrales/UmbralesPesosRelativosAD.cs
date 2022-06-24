using GB.SUTEL.Entities;
using GB.SUTEL.Entities.UmbralesPesosRelativos;
using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.DAL.Umbrales
{
    public class UmbralesPesosRelativosAD : LocalContextualizer
    {
        #region atributos
        private SUTEL_IndicadoresEntities context;
        #endregion

        #region constructor
        public UmbralesPesosRelativosAD(ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();

        }


        #endregion

        #region metodos

        /// <summary>
        /// Obtiene las los nombres de la direcciones almacendas en tabla Direcciones
        /// </summary>
        /// <returns></returns>
        public List<Direccion> GetLisDirecciones()
        {
            try
            {
                using (var context = new SUTEL_IndicadoresEntities())
                {
                    var _ListDireccion = new List<Direccion>();

                    var ListResult = context.Direccion.ToList();

                    if (ListResult.Count() > 1)
                    {
                        foreach (var item in ListResult)
                        {
                            _ListDireccion.Add(new Direccion { IdDireccion = item.IdDireccion, Nombre = item.Nombre });
                        }
                    }
                    else { return null; }
                    return _ListDireccion;
                }
            }
            catch (Exception ex)
            {
                string msj = "Erro al consultar el listado de Direcciones : GetLisDirecciones";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return null;
            }
        }

        /// <summary>
        /// Retorna un listado de servicios activos en la tabla Servicios
        /// </summary>
        /// <returns></returns>
        public List<Servicio> GetLisServicios()
        {
            try
            {
                using (var context = new SUTEL_IndicadoresEntities())
                {
                    var _ListServicio = new List<Servicio>();

                    var ListResult = context.Servicio.Where(t => t.Borrado == 0).ToList();

                    if (ListResult.Count() > 1)
                    {
                        foreach (var item in ListResult)
                        {
                            _ListServicio.Add(new Servicio { IdServicio = item.IdServicio, DesServicio = item.DesServicio });
                        }
                    }
                    else { return null; }
                    return _ListServicio;
                }
            }
            catch (Exception ex)
            {
                string msj = "Erro al consultar el listado de servicios : GetLisServicios";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return null;
            }
        }

        /// <summary>
        ///  Obtiene Listado de Indicadores pertenecientes a un servicio filtrados por recha
        /// </summary>
        /// <param name="IdServicio"></param>
        /// <param name="fechaInic"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<ServiIndicadorEnti> GetLisIndicadorXservicio(int IdServicio, DateTime fechaInic, DateTime fechaFin, int IdDireccion)
        {
            try
            {


                var LisRespuesta = new List<ServiIndicadorEnti>();
                DataSet retVal = new DataSet();
                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand("pa_GetIndicadorXservicio", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                using (cmdReport)
                {
                    cmdReport.CommandType = CommandType.StoredProcedure;

                    SqlParameter Opcion = new SqlParameter("Opcion", 2);
                    cmdReport.Parameters.Add(Opcion);

                    SqlParameter ParIdServ = new SqlParameter("Servicio", IdServicio);
                    cmdReport.Parameters.Add(ParIdServ);

                    SqlParameter _FechaInic = new SqlParameter("FechaInic", fechaInic);
                    cmdReport.Parameters.Add(_FechaInic);

                    SqlParameter _FechaFin = new SqlParameter("FechaFin", fechaFin);
                    cmdReport.Parameters.Add(_FechaFin);

                    SqlParameter _Usuario = new SqlParameter("Usuario", "");
                    cmdReport.Parameters.Add(_Usuario);

                    SqlParameter _Direccion = new SqlParameter("Direccion", IdDireccion);
                    cmdReport.Parameters.Add(_Direccion);

                    daReport.Fill(retVal);


                    if (retVal.Tables[0] != null && retVal.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = retVal.Tables[0];
                        foreach (DataRow item in dt.Rows)
                        {
                            ServiIndicadorEnti enti = new ServiIndicadorEnti();
                            enti.IdIndicador = item["IdIndicador"].ToString();
                            enti.NombreIndicador = item["NombreIndicador"].ToString();
                            enti.Umbral = Convert.ToDecimal(item["Umbral"]);
                            enti.Peso = Convert.ToDecimal(item["PesoRelativo"]);
                            enti.Usuario = item["UsuarioUltimaModificacion"].ToString();
                            enti.FechaUltimaModificacion = item["FechaUltimaModificacion"].ToString();
                            LisRespuesta.Add(enti);

                        }
                    }
                }


                return LisRespuesta;
            }
            catch (Exception ex)
            {

                string msj = "Erro al consultar los indicadores para el servicios : " + IdServicio + " GetLisIndicadorXservicio";
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }

        public List<ServiIndicadorEnti> GetLisIndicadorXservicio(int IdServicio, string Usuario, int IdDireccion)
        {
            try
            {

                var LisRespuesta = new List<ServiIndicadorEnti>();
                DataSet retVal = new DataSet();
                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand("pa_GetIndicadorXservicio", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                using (cmdReport)
                {
                    cmdReport.CommandType = CommandType.StoredProcedure;

                    SqlParameter Opcion = new SqlParameter("Opcion", 3);
                    cmdReport.Parameters.Add(Opcion);

                    SqlParameter ParIdServ = new SqlParameter("Servicio", IdServicio);
                    cmdReport.Parameters.Add(ParIdServ);

                    SqlParameter _FechaInic = new SqlParameter("FechaInic", DateTime.Now);
                    cmdReport.Parameters.Add(_FechaInic);

                    SqlParameter _FechaFin = new SqlParameter("FechaFin", DateTime.Now);
                    cmdReport.Parameters.Add(_FechaFin);

                    SqlParameter _Usuario = new SqlParameter("Usuario", Usuario);
                    cmdReport.Parameters.Add(_Usuario);

                    SqlParameter _Direccion = new SqlParameter("Direccion", IdDireccion);
                    cmdReport.Parameters.Add(_Direccion);

                    daReport.Fill(retVal);


                    if (retVal.Tables[0] != null && retVal.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = retVal.Tables[0];
                        foreach (DataRow item in dt.Rows)
                        {
                            ServiIndicadorEnti enti = new ServiIndicadorEnti();
                            enti.IdIndicador = item["IdIndicador"].ToString();
                            enti.NombreIndicador = item["NombreIndicador"].ToString();
                            enti.Umbral = Convert.ToDecimal(item["Umbral"]);
                            enti.Peso = Convert.ToDecimal(item["PesoRelativo"]);
                            enti.Usuario = item["UsuarioUltimaModificacion"].ToString();
                            enti.FechaUltimaModificacion = item["FechaUltimaModificacion"].ToString();
                            LisRespuesta.Add(enti);

                        }
                    }
                }


                return LisRespuesta;
            }
            catch (Exception ex)
            {

                string msj = "Erro al consultar los indicadores para el servicios : " + IdServicio + " GetLisIndicadorXservicio";
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }


        /// <summary>
        /// Obtiene Listado de Indicadores pertenecientes a un servicio
        /// </summary>
        /// <returns>Listado de indicadores asociados a servicios</returns>
        public List<ServiIndicadorEnti> GetLisIndicadorXservicio(int IdServicio, int IdDireccion)
        {
            try
            {

                var LisRespuesta = new List<ServiIndicadorEnti>();
                DataSet retVal = new DataSet();
                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand("pa_GetIndicadorXservicio", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                using (cmdReport)
                {
                    cmdReport.CommandType = CommandType.StoredProcedure;


                    SqlParameter Opcion = new SqlParameter("Opcion", 1);
                    cmdReport.Parameters.Add(Opcion);

                    SqlParameter ParIdServ = new SqlParameter("Servicio", IdServicio);
                    cmdReport.Parameters.Add(ParIdServ);


                    SqlParameter _FechaInic = new SqlParameter("FechaInic", DateTime.Now);
                    cmdReport.Parameters.Add(_FechaInic);

                    SqlParameter _FechaFin = new SqlParameter("FechaFin", DateTime.Now);
                    cmdReport.Parameters.Add(_FechaFin);

                    SqlParameter _Usuario = new SqlParameter("Usuario", "");
                    cmdReport.Parameters.Add(_Usuario);

                    SqlParameter _Direccion = new SqlParameter("Direccion", IdDireccion);
                    cmdReport.Parameters.Add(_Direccion);

                    daReport.Fill(retVal);


                    if (retVal.Tables[0] != null && retVal.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = retVal.Tables[0];
                        foreach (DataRow item in dt.Rows)
                        {
                            ServiIndicadorEnti enti = new ServiIndicadorEnti();
                            enti.IdIndicador = item["IdIndicador"].ToString();
                            enti.NombreIndicador = item["NombreIndicador"].ToString();
                            enti.Umbral = Convert.ToDecimal(item["Umbral"]);
                            enti.Peso = Convert.ToDecimal(item["PesoRelativo"]);
                            enti.Usuario = item["UsuarioUltimaModificacion"].ToString();
                            enti.FechaUltimaModificacion = item["FechaUltimaModificacion"].ToString();
                            LisRespuesta.Add(enti);

                        }
                    }
                }


                return LisRespuesta;
            }
            catch (Exception ex)
            {

                string msj = "Error al consultar los indicadores para el servicios : " + IdServicio + " GetLisIndicadorXservicio";
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }

        /// <summary>
        /// Metodo para Crear Umbrales y pesos relativos a umbrales
        /// </summary>
        /// <param name="Entidad">ServiIndicadorEnti</param>
        /// <returns></returns>
        public int CrearIndicadorUmbral(ServiIndicadorEnti Entidad)
        {
            try
            {
                var _Resulvalidacion = ExisteIndicador(Entidad.IdIndicador);
                if (_Resulvalidacion == 1)
                {
                    UpdateIndicadorUmrbal(Entidad);
                    return 1;
                }
                else if (_Resulvalidacion == 2)
                {
                    InsertIndicadorUmrbal(Entidad);
                    return 2;
                }

                return 3;
            }
            catch (Exception ex)
            {
                string msj = "Erro al intentar crear el umbral para el indicador";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return 4;
            }
        }

        public List<Usuario> GetUsuarios()
        {
            try
            {

                var oUseres = new List<Usuario>();
                using (var context = new SUTEL_IndicadoresEntities())
                {
                    oUseres = context.Usuario.OrderBy(x => x.NombreUsuario).Where(x => x.Borrado == 0).OrderBy(p => p.NombreUsuario).ToList();
                }
                return oUseres;
            }
            catch (Exception ex)
            {
                string msj = "Erro al obetener usuarios GetUsuarios";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return new List<Usuario>();
            }

        }
        #endregion


        #region MetodosPrivados

        private int ExisteIndicador(string IdIndicador)
        {
            try
            {
                DataSet retVal = new DataSet();
                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand("pa_MetodosIndicarXServicio", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                using (cmdReport)
                {

                    cmdReport.CommandType = CommandType.StoredProcedure;

                    SqlParameter Opcion = new SqlParameter("Opcion", 3);
                    cmdReport.Parameters.Add(Opcion);
                    SqlParameter _IdIndicador = new SqlParameter("IdIndicador", IdIndicador);
                    cmdReport.Parameters.Add(_IdIndicador);
                    SqlParameter Umbral = new SqlParameter("Umbral", 0.0);
                    cmdReport.Parameters.Add(Umbral);
                    SqlParameter PesoRelativo = new SqlParameter("PesoRelativo", 0.0);
                    cmdReport.Parameters.Add(PesoRelativo);
                    SqlParameter Usuario = new SqlParameter("Usuario", "SutelUser");
                    cmdReport.Parameters.Add(Usuario);
                    SqlParameter _fecha = new SqlParameter("fecha", "");
                    cmdReport.Parameters.Add(_fecha);

                    daReport.Fill(retVal);


                    if (retVal.Tables[0] != null && retVal.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = retVal.Tables[0];
                        foreach (DataRow item in dt.Rows)
                        {
                            if (item["IdIndicador"].ToString() == IdIndicador)
                                return 1;

                        }
                    }
                    else
                    {
                        return 2;
                    }
                }
                return 5;
            }
            catch (Exception ex)
            {
                string msj = "Erro al consulta indicador:ExisteIndicador";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return 3;
            }
        }

        private void InsertIndicadorUmrbal(ServiIndicadorEnti IndicadorUmbral)
        {
            DataSet retVal = new DataSet();
            SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
            SqlCommand cmdReport = new SqlCommand("pa_MetodosIndicarXServicio", sqlConn);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

            try
            {


                using (cmdReport)
                {

                    DateTime fecha = DateTime.Now;
                    string formarinic = fecha.ToString("yyyy-dd-MM");

                    cmdReport.CommandType = CommandType.StoredProcedure;
                    SqlParameter Opcion = new SqlParameter("Opcion", 1);
                    cmdReport.Parameters.Add(Opcion);
                    SqlParameter _IdIndicador = new SqlParameter("IdIndicador", IndicadorUmbral.IdIndicador);
                    cmdReport.Parameters.Add(_IdIndicador);
                    SqlParameter Umbral = new SqlParameter("Umbral", IndicadorUmbral.Umbral);
                    cmdReport.Parameters.Add(Umbral);
                    SqlParameter PesoRelativo = new SqlParameter("PesoRelativo", IndicadorUmbral.Peso);
                    cmdReport.Parameters.Add(PesoRelativo);

                    SqlParameter _fecha = new SqlParameter("fecha", DateTime.Now);
                    cmdReport.Parameters.Add(_fecha);

                    SqlParameter Usuario = new SqlParameter("Usuario", IndicadorUmbral.Usuario);
                    cmdReport.Parameters.Add(Usuario);
                    cmdReport.Connection.Open();
                    int result = cmdReport.ExecuteNonQuery();
                    cmdReport.Connection.Close(); 

                }

            }
            catch (Exception ex)
            {
                string msj = "Erro al crear umbrales para indicador:InsertIndicadorUmrbal";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                cmdReport.Connection.Close();
            }
        }

        private void UpdateIndicadorUmrbal(ServiIndicadorEnti IndicadorUmbral)
        {
            DataSet retVal = new DataSet();
            SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
            SqlCommand cmdReport = new SqlCommand("pa_MetodosIndicarXServicio", sqlConn);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

            try
            {

                using (cmdReport)
                {

                    DateTime fecha = DateTime.Now;
                    string formarinic = fecha.ToString("yyyy-dd-MM");

                    cmdReport.CommandType = CommandType.StoredProcedure;
                    SqlParameter Opcion = new SqlParameter("Opcion", 2);
                    cmdReport.Parameters.Add(Opcion);
                    SqlParameter _IdIndicador = new SqlParameter("IdIndicador", IndicadorUmbral.IdIndicador);
                    cmdReport.Parameters.Add(_IdIndicador);
                    SqlParameter Umbral = new SqlParameter("Umbral", IndicadorUmbral.Umbral);
                    cmdReport.Parameters.Add(Umbral);
                    SqlParameter PesoRelativo = new SqlParameter("PesoRelativo", IndicadorUmbral.Peso);
                    cmdReport.Parameters.Add(PesoRelativo);

                    SqlParameter _fecha = new SqlParameter("fecha", DateTime.Now);
                    cmdReport.Parameters.Add(_fecha);

                    SqlParameter Usuario = new SqlParameter("Usuario", IndicadorUmbral.Usuario);
                    cmdReport.Parameters.Add(Usuario);
                    cmdReport.Connection.Open();
                    int result = cmdReport.ExecuteNonQuery();
                    cmdReport.Connection.Close();

                }

            }
            catch (Exception ex)
            {
                string msj = "Erro al crear modificar umbrales :UpdateIndicadorUmrbal";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                cmdReport.Connection.Close();
            }
        }

        #endregion
    }
}
