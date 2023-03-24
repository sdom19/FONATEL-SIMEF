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
    public class RegistroIndicadorFonatelDAL: BitacoraDAL
    {
        private SITELContext db;

        private SIMEFContext SIMEFdb;

        private DetalleRegistroIndicadorFonatelDAL DetalleRegistroIndicadorFonatelDAL;

        public RegistroIndicadorFonatelDAL()
        {
            DetalleRegistroIndicadorFonatelDAL = new DetalleRegistroIndicadorFonatelDAL();
           
        }

        /// Autor: Francisco Vindas
        /// Fecha: 03/01/2023
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
                Mes = x.Mes,
                Estado = x.Estado,
                IdEstado = x.IdEstado,
                FechaModificacion = x.FechaModificacion,
                UsuarioModificacion = x.UsuarioModificacion,
                IdAnno = x.IdAnno,
                Anno = x.Anno,
                IdMes = x.IdMes,
                IdFuente = x.IdFuente,
                Fuente = ObtenerFuente(x.IdFuente),
                Solicitudid = Utilidades.Encriptar(x.IdSolicitud.ToString()),
                FormularioId = Utilidades.Encriptar(x.idFormularioWeb.ToString()),
                DetalleRegistroIndcadorFonatel = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(new DetalleRegistroIndicadorFonatel() { IdSolicitud = x.IdSolicitud, idFormularioWeb = x.idFormularioWeb })

            }).ToList();
        }

        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de RegistroIndicadorFonatel según parametros
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        public List<RegistroIndicadorFonatel> ObtenerDatos(RegistroIndicadorFonatel objRegistroIndicadorFonatel)
        {
            List<RegistroIndicadorFonatel> ListaRegistroIndicadorFonatel = new List<RegistroIndicadorFonatel>();
            using (db = new SITELContext())
            {

                ListaRegistroIndicadorFonatel = db.Database.SqlQuery<RegistroIndicadorFonatel>
                    ("execute Fonatel.pa_ObtenerRegistroIndicadorFonatel @IdSolicitud,@idFormularioWeb,@IdFuente, @IdEstado, @RangoFecha",
                     new SqlParameter("@IdSolicitud", objRegistroIndicadorFonatel.IdSolicitud),
                     new SqlParameter("@idFormularioWeb", objRegistroIndicadorFonatel.idFormularioWeb),
                     new SqlParameter("@IdFuente", objRegistroIndicadorFonatel.IdFuente),
                     new SqlParameter("@IdEstado", objRegistroIndicadorFonatel.IdEstado),
                     new SqlParameter("@RangoFecha", objRegistroIndicadorFonatel.RangoFecha)
                    ).ToList();

                ListaRegistroIndicadorFonatel = CrearListado(ListaRegistroIndicadorFonatel);
            }
            
            return ListaRegistroIndicadorFonatel;
        }

        public FuenteRegistro ObtenerFuente(int id)
        {
            using (SIMEFdb = new SIMEFContext()) { 
                FuenteRegistro fuente = SIMEFdb.FuentesRegistro.Where(i => i.IdFuenteRegistro == id).Single();
                fuente.DetalleFuenteRegistro = SIMEFdb.DetalleFuentesRegistro.Where(i => i.idFuenteRegistro == id).ToList();
                return fuente;
            }
        }

        /// <summary>
        /// Metodo que actualiza los estados de registro indicador fonatel
        /// fecha 25-01-2023
        /// Georgi Mesen Cerdas
        /// </summary>
        /// <param name="objRegistroIndicadorFonatel"></param>
        public List<RegistroIndicadorFonatel> ActualizarRegistroIndicadorFonatel(RegistroIndicadorFonatel objRegistroIndicadorFonatel)
        {
            List<RegistroIndicadorFonatel> ListaRegistroIndicadorFonatel = new List<RegistroIndicadorFonatel>();
            using (db = new SITELContext())
            {

                ListaRegistroIndicadorFonatel = db.Database.SqlQuery<RegistroIndicadorFonatel>
                    ("execute Fonatel.pa_ActualizarRegistroIndicadorFonatel @IdSolicitud,@idFormularioWeb,@Estado, @idEstado",
                     new SqlParameter("@IdSolicitud", objRegistroIndicadorFonatel.IdSolicitud),
                     new SqlParameter("@idFormularioWeb", objRegistroIndicadorFonatel.idFormularioWeb),
                     new SqlParameter("@Estado", objRegistroIndicadorFonatel.Estado),
                     new SqlParameter("@IdEstado", objRegistroIndicadorFonatel.IdEstado)
                    ).ToList();

                ListaRegistroIndicadorFonatel = CrearListado(ListaRegistroIndicadorFonatel);
            }

            return ListaRegistroIndicadorFonatel;
        }


        #endregion

    }
}
