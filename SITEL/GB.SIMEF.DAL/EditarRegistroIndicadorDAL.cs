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
    public class EditarRegistroIndicadorFonatelDAL: BitacoraDAL
    {
        private SIMEFContext db;
        private EditarDetalleRegistroIndicadorFonatelDAL EditarDetalleRegistroIndicador;

        public EditarRegistroIndicadorFonatelDAL()
        {
            EditarDetalleRegistroIndicador = new EditarDetalleRegistroIndicadorFonatelDAL();
           
        }

        /// <summary>
        /// Autor: Francisco Vindas
        /// Fecha: 25/11/2022
        /// El metodo crea una lista generica de la solicitud que puede ser utilizado en lo metodos que lo necesiten 
        /// </summary>
        /// <param name="ListaSolicitud"></param>
        /// <returns></returns>

        private List<RegistroIndicadorFonatel> CrearListado(List<RegistroIndicadorFonatel> ListaSolicitud)
        {
            return ListaSolicitud.Select(x => new RegistroIndicadorFonatel
            {
                Codigo = x.Codigo,
                IdSolicitud = x.IdSolicitud,
                Mensaje = x.Mensaje,
                Nombre = x.Nombre,
                FechaInicio = x.FechaInicio,
                FechaFin = x.FechaFin,
                Formulario = x.Formulario,
                idFormularioWeb = x.idFormularioWeb,
                IdEstado = x.IdEstado,
                FechaModificacion = x.FechaModificacion,
                UsuarioModificacion = x.UsuarioModificacion,
                IdMes = x.IdMes,
                Mes = x.Mes,
                IdAnno = x.IdAnno,
                Anno = x.Anno,
                IdFuente = x.IdFuente,
                Fuente = ObtenerFuente(x.IdFuente),
                EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.IdEstado).Single(),
                Solicitud = ObtenerSolicitud(x.IdSolicitud),
                Solicitudid = Utilidades.Encriptar(x.IdSolicitud.ToString()),
                FormularioId = Utilidades.Encriptar(x.idFormularioWeb.ToString()),
                DetalleRegistroIndcadorFonatel = EditarDetalleRegistroIndicador.ObtenerDatoDetalleRegistroIndicador(new DetalleRegistroIndicadorFonatel() { IdSolicitud = x.IdSolicitud, idFormularioWeb = x.idFormularioWeb })
            }).ToList();
        }



        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de RegistroIndicadorFonatel según parametros
        /// Fecha 29-11-2022
        /// Francisco Vindas
        /// </summary>
        /// <returns>Lista</returns>
        public List<RegistroIndicadorFonatel> ObtenerDatos(RegistroIndicadorFonatel objeto)
        {
            List<RegistroIndicadorFonatel> ListaRegistroIndicadorFonatel = new List<RegistroIndicadorFonatel>();
            using (db = new SIMEFContext())
            {
                //SE NECESITARAN LOS PARAMETROS @RangoFecha
                ListaRegistroIndicadorFonatel = db.Database.SqlQuery<RegistroIndicadorFonatel>
                    ("execute spObtenerRegistroIndicador @IdSolicitud, @idFormularioWeb, @Codigo, @IdEstado",
                     new SqlParameter("@IdSolicitud", objeto.IdSolicitud),
                     new SqlParameter("@idFormularioWeb", objeto.idFormularioWeb),
                     new SqlParameter("@Codigo", string.IsNullOrEmpty(objeto.Codigo) ? DBNull.Value.ToString() : objeto.Codigo),
                     new SqlParameter("@IdEstado", objeto.IdEstado)
                     //new SqlParameter("@RangoFecha", objeto.RangoFecha)
                    ).ToList();
            
                ListaRegistroIndicadorFonatel = CrearListado(ListaRegistroIndicadorFonatel);
            }

            return ListaRegistroIndicadorFonatel;
        }

        public FuenteRegistro ObtenerFuente(int id)
        {
            FuenteRegistro fuente = db.FuentesRegistro.Where(i => i.IdFuenteRegistro == id).Single();
            fuente.DetalleFuenteRegistro = db.DetalleFuentesRegistro.Where(i => i.idFuenteRegistro == id).ToList();
            return fuente;
        }

        public Solicitud ObtenerSolicitud(int id)
        {
            Solicitud solicitud = db.Solicitud.Where(i => i.idSolicitud == id).Single();
            solicitud.SolicitudFormulario = db.DetalleSolicitudFormulario.Where(i => i.IdSolicitud == id).ToList();
            return solicitud;
        }

        #endregion

    }
}
