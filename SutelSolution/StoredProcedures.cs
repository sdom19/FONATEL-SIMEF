using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient; // to use SqlConnection, SqlCommand and SqlDataReader
using System.Data;
using SutelSolution.Models; // to use CommandType

namespace SutelSolution
{
   public class StoredProcedures
   {

       #region TodosDetalleAgrupacionDetallado
       public Respuesta<List<DetalleAgrupacionSP>> TodosDetalleAgrupacionDetallado(string IdOperador, string IdSolicitud, string connString)
       {
           Respuesta<List<DetalleAgrupacionSP>> respuesta = new Respuesta<List<DetalleAgrupacionSP>>();

           List<DetalleAgrupacionSP> listaDetalleAgrupacion = new List<DetalleAgrupacionSP>();
           
           SqlConnection sqlConnection = new SqlConnection(connString);
           SqlCommand cmd = new SqlCommand();
           SqlDataReader reader;
           Guid guid; // Para convertir el IdSolicitud
          /// guid = guid;
           try
           {
               cmd.CommandText = ("dbo.pa_getDetalleAgrupacionesPorOperadorExcel");
               cmd.Parameters.AddWithValue("@IdOperador", IdOperador);
              
               if (Guid.TryParse(IdSolicitud, out guid))
               {
                   cmd.Parameters.AddWithValue("@IdSolicitudIndicador", SqlDbType.UniqueIdentifier).Value = guid;
               }

               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Connection = sqlConnection;

               sqlConnection.Open();
               

               reader = cmd.ExecuteReader();
           
              
               while (reader.Read())
            {

                DetalleAgrupacionSP detalleAgrupacionModel = new DetalleAgrupacionSP();

               detalleAgrupacionModel.Id_Detalle_Agrupacion = reader.GetInt32(0);
               detalleAgrupacionModel.Nombre_Detalle_Agrupacion = reader.GetString(1);
               detalleAgrupacionModel.Nombre_Agrupacion = reader.GetString(2);
               detalleAgrupacionModel.Nombre_Operador = reader.GetString(3);
               detalleAgrupacionModel.Nombre_Criterio = reader.GetString(4);
               detalleAgrupacionModel.ID_Frecuencia = reader.GetInt32(5);
               detalleAgrupacionModel.Nombre_Frecuencia = reader.GetString(6);
               detalleAgrupacionModel.Cantidad_Meses_Frecuencia = reader.GetInt32(7);
               detalleAgrupacionModel.ID_Desglose = reader.GetInt32(8);
               detalleAgrupacionModel.Nombre_Desglose = reader.GetString(9);
               detalleAgrupacionModel.Cantidad_Meses_Desglose = reader.GetInt32(10);
               detalleAgrupacionModel.ID_Indicador = reader.GetString(11);
               detalleAgrupacionModel.Nombre_Indicador = reader.GetString(12);
               detalleAgrupacionModel.Nivel = reader.GetString(13);

               if (!reader.IsDBNull(14))
               detalleAgrupacionModel.Valor_Inferior = reader.GetString(14);
               if (!reader.IsDBNull(15))
               detalleAgrupacionModel.Valor_Superior = reader.GetString(15);
               if (!reader.IsDBNull(16))
               detalleAgrupacionModel.Id_Tipo_Valor = reader.GetInt32(16);
               if (!reader.IsDBNull(17))
               detalleAgrupacionModel.Tipo_Valor = reader.GetString(17);
               if (!reader.IsDBNull(18))
               detalleAgrupacionModel.Valor_Formato = reader.GetString(18);          
               detalleAgrupacionModel.Id_ConstructorCriterio = reader.GetGuid(19);
               if (!reader.IsDBNull(20))
               detalleAgrupacionModel.Id_Padre_ConstructorCriterio = reader.GetGuid(20);
               if (!reader.IsDBNull(21))
               detalleAgrupacionModel.Id_Padre_Detalle_Agrupacion = reader.GetInt32(21);

               if (!reader.IsDBNull(22))
               detalleAgrupacionModel.Nombre_Padre_Detalle_Agrupacion = reader.GetString(22);
               detalleAgrupacionModel.Fecha_Inicio = reader.GetDateTime(23);
               detalleAgrupacionModel.Fecha_Fin = reader.GetDateTime(24);
               detalleAgrupacionModel.FechaBaseParaCrearExcel = reader.GetDateTime(25);
               if (!reader.IsDBNull(26))
               if (reader.GetByte(26) != 0)
               detalleAgrupacionModel.UsaReglaEstadisticaConNivelDetalle = reader.GetByte(26);

               detalleAgrupacionModel.Constructor_Criterio_Ayuda = reader.GetString(27);
               detalleAgrupacionModel.Descripcion_Criterio = reader.GetString(28);
               detalleAgrupacionModel.Id_Solicitud_Indicador = reader.GetGuid(29);
               detalleAgrupacionModel.Id_Solicitud_Constructor = reader.GetGuid(30);
               detalleAgrupacionModel.Nombre_Direccion = reader.GetString(31);
               detalleAgrupacionModel.Nombre_Servicio = reader.GetString(32);
               if (!reader.IsDBNull(33))
               detalleAgrupacionModel.Tipo_Nivel_Detalle = reader.GetString(33);
               if (!reader.IsDBNull(34))
               detalleAgrupacionModel.Id_Tipo_Nivel_Detalle = reader.GetString(34);
               if (!reader.IsDBNull(35))
               detalleAgrupacionModel.Tabla_Tipo_Nivel_Detalle = reader.GetString(35);
               if (!reader.IsDBNull(36))
               detalleAgrupacionModel.Id_Tipo_Nivel_Detalle_Genero = reader.GetInt32(36);

               listaDetalleAgrupacion.Add(detalleAgrupacionModel);
            }

               sqlConnection.Close();

               respuesta.objObjeto = listaDetalleAgrupacion;
           }
           catch (Exception ex)
           {
               respuesta.blnIndicadorTransaccion = false;
               respuesta.strMensaje = "El IdSolicitud: " + IdSolicitud + ", y el IdOperador: " + IdOperador + "\nError: " + ex.Message;            
           }

           return respuesta;
       }
      
       #endregion

       #region ExtraerSolicitudesPendientes
       public Respuesta<List<SolicitudPendiente>> ExtraerSolicitudesPendientes(string connString)
       {
           
           List<SolicitudPendiente> listaSolicitudesPendientes = new List<SolicitudPendiente>();

           Respuesta<List<SolicitudPendiente>> respuesta = new Respuesta<List<SolicitudPendiente>>();
          

           SqlConnection sqlConnection = new SqlConnection(connString);
           SqlCommand cmd = new SqlCommand();
           SqlDataReader reader;

           try
           {
               cmd.CommandText = ("dbo.pa_ExtraerSolicitudesPendientes");
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Connection = sqlConnection;

               sqlConnection.Open();
               reader = cmd.ExecuteReader();

               while (reader.Read())
               {
                   SolicitudPendiente solicitudPendiente = new SolicitudPendiente();

                   solicitudPendiente.Id_ArchivoExcel = reader.GetInt32(0);
                   solicitudPendiente.Id_Solicitud_Indicador = reader.GetGuid(1);
                   solicitudPendiente.Id_Operador = reader.GetString(2);

                   listaSolicitudesPendientes.Add(solicitudPendiente);
               }

               respuesta.objObjeto = listaSolicitudesPendientes;
               sqlConnection.Close();

           }
           catch (Exception ex)
           {
               respuesta.blnIndicadorTransaccion = false;
               respuesta.strMensaje = "Error: " + ex.Message;
           }

           return respuesta;
       }
       /// <summary>
       /// /Cambio para generar los cantones 
       /// </summary>
       /// <param name="archivoExcelBytes"></param>
       /// <param name="IdArchivoExcel"></param>
       /// <param name="connString"></param>
       /// <returns></returns>
       /// 
       #endregion
    
       #region InsertarArchivoExcel
       public Respuesta<Byte[]> InsertarArchivoExcel(Byte[] archivoExcelBytes, string IdArchivoExcel, string connString)
       {
           
           Respuesta<Byte[]> respuesta = new Respuesta<Byte[]>();


           SqlConnection sqlConnection = new SqlConnection(connString);
           SqlCommand cmd = new SqlCommand();
           

           try
           {
               cmd.CommandText = ("dbo.pa_InsertarArchivoExcel");
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Connection = sqlConnection;

               cmd.Parameters.AddWithValue("@ArchivoExcelBytes", archivoExcelBytes);
               cmd.Parameters.AddWithValue("@IdArchivoExcel", IdArchivoExcel);

               sqlConnection.Open();
               cmd.ExecuteReader();

              

               sqlConnection.Close();

           }
           catch (Exception ex)
           {
               respuesta.blnIndicadorTransaccion = false;
               respuesta.strMensaje = "Error: " + ex.Message;
           }

           return respuesta;
       }

       #endregion

       #region SendEmail
       public Respuesta<Byte[]> SendEmail(string to, string asunto,string html, string profile_name, string connString)
       {

           Respuesta<Byte[]> respuesta = new Respuesta<Byte[]>();

           SqlConnection sqlConnection = new SqlConnection(connString);
           SqlCommand cmd = new SqlCommand();


           try
           {
               cmd.CommandText = ("dbo.pa_SendEmail");
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Connection = sqlConnection;

               cmd.Parameters.AddWithValue("@to", to);
               cmd.Parameters.AddWithValue("@asunto", asunto);
               cmd.Parameters.AddWithValue("@html", html);
               cmd.Parameters.AddWithValue("@profile_name", profile_name);
               cmd.Parameters.AddWithValue("@direccion",100);
                sqlConnection.Open();
               cmd.ExecuteReader();

               sqlConnection.Close();

           }
           catch (Exception ex)
           {
               respuesta.blnIndicadorTransaccion = false;
               respuesta.strMensaje = "Error: " + ex.Message;
           }

           return respuesta;
       }

       #endregion




        #region pa_getValoresReglaEstadisticaConNivelDetalle
       public Respuesta<List<ValoresReglaSP>> pa_getValoresReglaEstadisticaConNivelDetalle(string IdConstructorCriterio, int IdNivelDetalle, string connString, string IdOperador, string IdSolicitud)
       {
           Respuesta<List<ValoresReglaSP>> respuesta = new Respuesta<List<ValoresReglaSP>>();

           List<ValoresReglaSP> listaDetalleAgrupacion = new List<ValoresReglaSP>();

           SqlConnection sqlConnection = new SqlConnection(connString);
           SqlCommand cmd = new SqlCommand();
           SqlDataReader reader;
           Guid guid; // Para convertir el IdSolicitud

           try
           {
               cmd.CommandText = ("dbo.pa_getValoresReglaEstadisticaConNivelDetalle");
               cmd.Parameters.AddWithValue("@IdNivelDetalle", IdNivelDetalle);

               if (Guid.TryParse(IdConstructorCriterio, out guid))
               {
                   cmd.Parameters.AddWithValue("@IdConstructorCriterio", SqlDbType.UniqueIdentifier).Value = guid;
               }

               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Connection = sqlConnection;

               sqlConnection.Open();
               
               reader = cmd.ExecuteReader();

               while (reader.Read())
               {
                   ValoresReglaSP valoresRegla = new ValoresReglaSP();

                   valoresRegla.ValorLimiteInferior = reader.GetString(0);
                   valoresRegla.ValorLimiteSuperior = reader.GetString(1);

                   listaDetalleAgrupacion.Add(valoresRegla);
               }

               sqlConnection.Close();

               respuesta.objObjeto = listaDetalleAgrupacion;

               if (respuesta.objObjeto.Count > 0)
               respuesta.blnIndicadorTransaccion = true;
               else
               respuesta.blnIndicadorTransaccion = false;
           }
           catch (Exception ex)
           {
               respuesta.blnIndicadorTransaccion = false;
               respuesta.strMensaje = "El IdSolicitud: " + IdSolicitud + ", y el IdOperador: " + IdOperador + "\nError: " + ex.Message;
           }

           return respuesta;
       }

       #endregion







       internal Respuesta<List<Canton>> ExtraerCantonesXProvincia(string ConnString, int? idProvincia)
       {
           List<Canton> ListaCantones = new List<Canton>();

           Respuesta<List<Canton>> respuesta = new Respuesta<List<Canton>>();


           SqlConnection sqlConnection = new SqlConnection(ConnString);
           SqlCommand cmd = new SqlCommand();
           SqlDataReader reader;

           try
           {
               cmd.CommandText = ("dbo.pa_ExtraerCantoXprovincia");
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Connection = sqlConnection;

               cmd.Parameters.AddWithValue("@IdProvincia", idProvincia);
               sqlConnection.Open();
               reader = cmd.ExecuteReader();

               while (reader.Read())
               {
                   Canton Cantones = new Canton();

                   Cantones.IdCanton = reader.GetInt32(0);
                   Cantones.Nombre = reader.GetString(2);
                   Cantones.IdProvincia = reader.GetInt32(0);

                   ListaCantones.Add(Cantones);
               }

               respuesta.objObjeto = ListaCantones;
               sqlConnection.Close();

           }
           catch (Exception ex)
           {
               respuesta.blnIndicadorTransaccion = false;
               respuesta.strMensaje = "Error: " + ex.Message;
           }

           return respuesta;
       }
       /// sp de Distrito
       /// 
       /// 
       internal Respuesta<List<Distrito>> ExtraerDistritoXCanton(string ConnString, int? IdCanton)
       {
           List<Distrito> ListaCantones = new List<Distrito>();

           Respuesta<List<Distrito>> respuesta = new Respuesta<List<Distrito>>();


           SqlConnection sqlConnection = new SqlConnection(ConnString);
           SqlCommand cmd = new SqlCommand();
           SqlDataReader reader;

           try
           {
               cmd.CommandText = ("dbo.pa_ExtraerDistritoXCanton");
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Connection = sqlConnection;

               cmd.Parameters.AddWithValue("@IdCanton", IdCanton);
               sqlConnection.Open();
               reader = cmd.ExecuteReader();

               while (reader.Read())
               {
                   Distrito Distrito = new Distrito();

                   Distrito.IdDistrito = reader.GetInt32(0);
                   Distrito.Nombre = reader.GetString(2);
                   Distrito.IdCanton = reader.GetInt32(0);

                   ListaCantones.Add(Distrito);
               }

               respuesta.objObjeto = ListaCantones;
               sqlConnection.Close();

           }
           catch (Exception ex)
           {
               respuesta.blnIndicadorTransaccion = false;
               respuesta.strMensaje = "Error: " + ex.Message;
           }

           return respuesta;
       }

       ///Fin del distrito
       ///
       internal Respuesta<List<Genero>> ExtraerGenero(string ConnString)
       {
           List<Genero> ListaGenero = new List<Genero>();

           Respuesta<List<Genero>> respuesta = new Respuesta<List<Genero>>();


           SqlConnection sqlConnection = new SqlConnection(ConnString);
           SqlCommand cmd = new SqlCommand();
           SqlDataReader reader;

           try
           {
               cmd.CommandText = ("dbo.pa_ExtraerGenero");
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Connection = sqlConnection;

               //cmd.Parameters.AddWithValue("@IdCanton", IdCanton);
               sqlConnection.Open();
               reader = cmd.ExecuteReader();

               while (reader.Read())
               {
                   Genero Genero = new Genero();

                   Genero.IdGenero = reader.GetInt32(0);
                   Genero.Nombre = reader.GetString(1);
                  // Distrito.IdCanton = reader.GetInt32(0);

                   ListaGenero.Add(Genero);
               }

               respuesta.objObjeto = ListaGenero;
               sqlConnection.Close();

           }
           catch (Exception ex)
           {
               respuesta.blnIndicadorTransaccion = false;
               respuesta.strMensaje = "Error: " + ex.Message;
           }

           return respuesta;
       }
   }
}
