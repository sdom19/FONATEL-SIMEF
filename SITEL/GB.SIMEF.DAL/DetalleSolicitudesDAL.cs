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
    public class DetalleSolicitudesDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 24/08/2022
        /// Ejecutar procedimiento almacenado para insertar y editar datos detalle relacion categoria
        /// </summary>
        /// <param name="detalleSolicitud"></param>
        /// <returns></returns>
        public List<DetalleSolicitudFormulario> ObtenerDatos(DetalleSolicitudFormulario detalleSolicitud)
        {
            List<DetalleSolicitudFormulario> ListaDetalle = new List<DetalleSolicitudFormulario>();

            using (db = new SIMEFContext())
            {
                ListaDetalle = db.Database.SqlQuery<DetalleSolicitudFormulario>
                    ("execute spObtenerDetalleSolicitud @pidSolicitud",
                      new SqlParameter("@pidSolicitud", detalleSolicitud.IdSolicitud)
                    ).ToList();

                ListaDetalle = ListaDetalle.Select(x => new DetalleSolicitudFormulario()
                {
                    IdSolicitud = x.IdSolicitud,
                    IdFormulario = x.IdFormulario,
                    Estado = x.Estado
                }).ToList();
            }

            return ListaDetalle;
        }

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 24/08/2022
        /// Ejecutar procedimiento almacenado para insertar y editar datos detalle relacion categoria
        /// </summary>
        /// <param name="detalleSolicitud"></param>
        /// <returns></returns>
        public List<DetalleSolicitudFormulario> ActualizarDatos(DetalleSolicitudFormulario detalleSolicitud)
        {
            List<DetalleSolicitudFormulario> ListaDetalle = new List<DetalleSolicitudFormulario>();

            using (db = new SIMEFContext())
            {
                ListaDetalle = db.Database.SqlQuery<DetalleSolicitudFormulario>
                    ("execute spActualizarDetalleSolicitud @idSolicitud, @idFormulario, @Estado",
                      new SqlParameter("@idSolicitud", detalleSolicitud.IdSolicitud),
                      new SqlParameter("@idFormulario", detalleSolicitud.IdFormulario),
                      new SqlParameter("@Estado", detalleSolicitud.Estado)
                    ).ToList();

                //ListaDetalle = ListaDetalle.Select(x => new DetalleRelacionCategoria()
                //{
                //    Completo = db.RelacionCategoria.Where(i => i.idRelacionCategoria == x.IdRelacionCategoria).Single().CantidadCategoria == ListaDetalle.Count() ? true : false

                //}).ToList();
            }

            return ListaDetalle;
        }

        public List<FormularioWeb> ObtenerListaFormularios(DetalleSolicitudFormulario detalleSolicitud)
        {

            List<FormularioWeb> listaFormularios  = new List<FormularioWeb>();

            using (db = new SIMEFContext())
            {
                listaFormularios = db.Database.SqlQuery<FormularioWeb>(
                    "execute spObtenerFormularioXSolicitudLista @idSolicitud",
                    new SqlParameter("@idSolicitud", detalleSolicitud.IdSolicitud)
                    ).ToList();

                listaFormularios = listaFormularios.Select(x => new FormularioWeb()
                {
                    id = Utilidades.Encriptar(x.idFormulario.ToString()),
                    idFormulario = x.idFormulario,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == x.idEstado).FirstOrDefault(),
                }).ToList();
            }
            return listaFormularios;
        }
    }
}
