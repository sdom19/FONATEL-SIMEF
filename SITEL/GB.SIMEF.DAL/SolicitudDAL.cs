﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;

namespace GB.SIMEF.DAL
{
    public class SolicitudDAL: BitacoraDAL
    {
        private  SIMEFContext db;
        #region Metodos Consulta Base de Datos

        /// <summary>
        /// Autor: Francisco Vindas
        /// Fecha: 25/11/2022
        /// El metodo crea una lista generica de la solicitud que puede ser utilizado en lo metodos que lo necesiten 
        /// </summary>
        /// <param name="ListaSolicitud"></param>
        /// <returns></returns>

        private List<Solicitud> CrearListadoSolicitud(List<Solicitud> ListaSolicitud)
        {
            return ListaSolicitud.Select(x => new Solicitud
            {
                id = Utilidades.Encriptar(x.idSolicitud.ToString()),
                idSolicitud = x.idSolicitud,
                Nombre = x.Nombre,
                CantidadFormulario = x.CantidadFormulario,
                Codigo = x.Codigo,
                FechaInicio = x.FechaInicio,
                FechaFin = x.FechaFin,
                IdEstadoRegistro = x.IdEstadoRegistro,
                Mensaje = x.Mensaje,
                idAnno = x.idAnno,
                idMes = x.idMes,
                idFuenteRegistro = x.idFuenteRegistro,
                IdFrecuenciaEnvio = x.IdFrecuenciaEnvio,
                FechaCreacion = x.FechaCreacion,
                UsuarioCreacion = x.UsuarioCreacion,
                FechaModificacion = x.FechaModificacion,
                UsuarioModificacion = x.UsuarioModificacion,
                Estado = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.IdEstadoRegistro).Single(),
                Fuente = ObtenerFuente(x.idFuenteRegistro),
                EnvioProgramado = db.SolicitudEnvioProgramado.Where(i => i.IdSolicitud == x.idSolicitud && i.Estado == true).SingleOrDefault(),
                SolicitudFormulario = db.DetalleSolicitudFormulario.Where(i => i.IdSolicitud == x.idSolicitud && i.Estado == true).ToList(),
                FormulariosString = ObtenerListaFormularioString(x.idSolicitud),
                FormularioWeb = ObtenerListaFormulario(x.idSolicitud),
                Mes=db.Mes.Where(p=>p.idMes==x.idMes).FirstOrDefault(),
                Anno=db.Anno.Where(p=>p.idAnno==x.idAnno).FirstOrDefault()

            }).ToList();
        }

        /// <summary>
        /// Metodo que carga los registros de Solicitudes según parametros
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        public List<Solicitud> ObtenerDatos(Solicitud objSolicitud)
        {
            List<Solicitud> ListaSolicitud = new List<Solicitud>();
            using (db = new SIMEFContext())
            {

                ListaSolicitud = db.Database.SqlQuery<Solicitud>
                    ("execute pa_ObtenerSolicitud @idSolicitud ,@codigo,@idEstadoRegistro",
                     new SqlParameter("@idSolicitud", objSolicitud.idSolicitud),
                     new SqlParameter("@codigo", string.IsNullOrEmpty(objSolicitud.Codigo) ? DBNull.Value.ToString() : objSolicitud.Codigo),
                    new SqlParameter("@idEstadoRegistro", objSolicitud.IdEstadoRegistro)
                    ).ToList();

                ListaSolicitud = CrearListadoSolicitud(ListaSolicitud);
            }
            return ListaSolicitud;
        }
        private FuenteRegistro ObtenerFuente(int id)
        {
            FuenteRegistro fuente = db.FuentesRegistro.Where(i => i.IdFuenteRegistro == id).Single();
            fuente.DetalleFuenteRegistro = db.DetalleFuentesRegistro.Where(i => i.idFuenteRegistro == id).ToList();
            return fuente;
        }


        /// <summary>
        /// Actualiza los datos e inserta por medio de merge
        /// 17/08/2022
        /// michael Hernández C
        /// </summary>
        /// <param name="objSolicitud"></param>
        /// <returns></returns>
        public List<Solicitud> ActualizarDatos(Solicitud objeto)
        {
            List<Solicitud> ListaSolicitud = new List<Solicitud>();

            using (db = new SIMEFContext())
            {
                ListaSolicitud = db.Database.SqlQuery<Solicitud>

                ("execute pa_ActualizarSolicitud @idSolicitud ,@Codigo, @Nombre ,@FechaInicio ,@FechaFin ,@idMes ,@idAnno ,@CantidadFormulario ,@idFuente ,@Mensaje ,@UsuarioCreacion ,@UsuarioModificacion ,@idEstadoRegistro ",
                     new SqlParameter("@idSolicitud", objeto.idSolicitud),
                     new SqlParameter("@Codigo", string.IsNullOrEmpty(objeto.Codigo) ? DBNull.Value.ToString() : objeto.Codigo),
                     new SqlParameter("@Nombre", string.IsNullOrEmpty(objeto.Nombre) ? DBNull.Value.ToString() : objeto.Nombre),
                     objeto.FechaInicio == null ?
                        new SqlParameter("@FechaInicio", DBNull.Value)
                        :
                        new SqlParameter("@FechaInicio", objeto.FechaInicio),
                     objeto.FechaFin == null ?
                        new SqlParameter("@FechaFin", DBNull.Value)
                        :
                        new SqlParameter("@FechaFin", objeto.FechaFin),
                     new SqlParameter("@idMes", objeto.idMes),
                     new SqlParameter("@idAnno", objeto.idAnno),
                     new SqlParameter("@CantidadFormulario", objeto.CantidadFormulario),
                     new SqlParameter("@idFuente", objeto.idFuenteRegistro),
                     new SqlParameter("@IdFrecuenciaEnvio", objeto.IdFrecuenciaEnvio),
                     new SqlParameter("@Mensaje", string.IsNullOrEmpty(objeto.Mensaje) ? DBNull.Value.ToString() : objeto.Mensaje),
                     new SqlParameter("@UsuarioCreacion", objeto.UsuarioCreacion),
                     new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty(objeto.UsuarioModificacion) ? DBNull.Value.ToString() : objeto.UsuarioModificacion),
                     new SqlParameter("@idEstadoRegistro", objeto.IdEstadoRegistro)
                    ).ToList();

                ListaSolicitud = CrearListadoSolicitud(ListaSolicitud);

            }

            return ListaSolicitud;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objSolicitud"></param>
        /// <returns></returns>
        public List<FormularioWeb> ObtenerListaFormulario(int objSolicitud)
        {
           var Listado= db.Database.SqlQuery<FormularioWeb>
                  ("execute pa_ObtenerFormularioXSolicitudLista @idSolicitud",
                  new SqlParameter("@idSolicitud", objSolicitud)).ToList();


            List<FormularioWeb> resultado = Listado.Select(x => new FormularioWeb()
            {
                idFormularioWeb = x.idFormularioWeb,
                Codigo=x.Codigo,
                Nombre=x.Nombre,
                Descripcion=x.Descripcion,
                CantidadIndicador=x.CantidadIndicador,
                FrecuenciaEnvio=x.FrecuenciaEnvio,
                FechaCreacion=x.FechaCreacion,
                FechaModificacion=x.FechaModificacion,
                UsuarioCreacion=x.UsuarioCreacion,
                UsuarioModificacion=x.UsuarioModificacion,
                idEstadoRegistro=x.idEstadoRegistro,
                EstadoRegistro=db.EstadoRegistro.Where(s=>s.IdEstadoRegistro==x.idEstadoRegistro).FirstOrDefault()
            }).ToList();

            if (resultado==null)
            {
                resultado = new List<FormularioWeb>();
            }
            return resultado;
        }

        /// <summary>
        /// Obtener el listado de formularios en una fila string
        /// </summary>
        /// <param name="objSolicitud"></param>
        /// <returns></returns>
        public string ObtenerListaFormularioString(int objSolicitud)
        {
            string resultado = string.Empty;
            resultado=  db.Database.SqlQuery<string>
                  ("execute pa_ObtenerFormularioXSolicitud @idSolicitud",
                  new SqlParameter("@idSolicitud", objSolicitud)
                    ).SingleOrDefault();
            if (string.IsNullOrEmpty(resultado))
            {
                resultado = "No Definido";
            }
            return resultado;
        }

        /// <summary>
        /// Valida el existencia de la solicitud si tiene envío o programación automatica
        /// </summary>
        /// <param name="objSolicitud"></param>
        /// <returns></returns>
        public List<string> ValidarSolicitud(Solicitud objSolicitud)
        {
            List<string> resultado = new List<string>();
            using (db = new SIMEFContext())
            {
                resultado = db.Database.SqlQuery<string>("execute pa_ValidarSolicitud @idSolicitud"
                , new SqlParameter("@idSolicitud", objSolicitud.idSolicitud)).ToList();
            }
            return resultado;
        }

        /// <summary>
        /// 14/10/2022
        /// Francisco Vindas Ruiz
        /// Función que clona los detalles de las solicitudes
        /// </summary>
        /// <param name="pIdSolicitudAClonar"></param>
        /// <param name="pIdSolicitudDestino"></param>
        /// <returns></returns>
        public bool ClonarDetallesDeSolicitudes(int pIdSolicitudAClonar, int pIdSolicitudDestino)
        {
            using (db = new SIMEFContext())
            {
                db.Database.SqlQuery<object>
                    ("execute pa_ClonarDetalleDeSolicitud @pIdSolicitudAClonar, @pIdSolicitudDestino",
                     new SqlParameter("@pIdSolicitudAClonar", pIdSolicitudAClonar),
                     new SqlParameter("@pIdSolicitudDestino", pIdSolicitudDestino)
                    ).ToList();
            }

            return true;
        }


        #endregion

    }
}
