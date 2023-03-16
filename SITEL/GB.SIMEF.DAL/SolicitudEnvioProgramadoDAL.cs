using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;

namespace GB.SIMEF.DAL
{
    public class SolicitudEnvioProgramadoDAL : BitacoraDAL
    {
        private SIMEFContext db;


        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 19/10/2022
        /// Consultar los envios programados 
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        public List<SolicitudEnvioProgramado> ObtenerDatos(SolicitudEnvioProgramado objeto)
        {
            List<SolicitudEnvioProgramado> ListaEnvios = new List<SolicitudEnvioProgramado>();

            using (db = new SIMEFContext())
            {
                ListaEnvios = db.Database.SqlQuery<SolicitudEnvioProgramado>
                    ("execute pa_ObtenerEnvioProgramado @pidSolicitud",
                      new SqlParameter("@pidSolicitud", objeto.IdSolicitud)
                    ).ToList();

                ListaEnvios = ListaEnvios.Select(x => new SolicitudEnvioProgramado()
                {
                    IdSolicitudEnvioProgramado = x.IdSolicitudEnvioProgramado,
                    IdSolicitud = x.IdSolicitud,
                    IdFrecuenciaEnvio = x.IdFrecuenciaEnvio,
                    CantidadRepeticion = x.CantidadRepeticion,
                    FechaCiclo = x.FechaCiclo,
                    Estado = x.Estado
                }).ToList();
            }

            return ListaEnvios;
        }

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 18/10/2022
        /// Ejecutar procedimiento almacenado para insertar envios programados
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public List<SolicitudEnvioProgramado> ActualizarDatos(SolicitudEnvioProgramado objeto)
        {
            List<SolicitudEnvioProgramado> ListaDetalle = new List<SolicitudEnvioProgramado>();

            using (db = new SIMEFContext())
            {
                ListaDetalle = db.Database.SqlQuery<SolicitudEnvioProgramado>

                    ("execute pa_ActualizarEnvioProgramado @pIdEnvioProgramado ,@pIdSolicitud ,@pIdFrecuenciaEnvio ,@pCantidadRepeticion ,@pFechaCiclo, @pEstado",
                      new SqlParameter("@pIdEnvioProgramado", objeto.IdSolicitudEnvioProgramado),
                      new SqlParameter("@pIdSolicitud", objeto.IdSolicitud),
                      new SqlParameter("@pIdFrecuenciaEnvio", objeto.IdFrecuenciaEnvio),
                      new SqlParameter("@pCantidadRepeticion", objeto.CantidadRepeticion),
                      new SqlParameter("@pFechaCiclo", objeto.FechaCiclo),
                      new SqlParameter("@pEstado", objeto.Estado)
                    ).ToList();
            }

            return ListaDetalle;
        }
    }
}
