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
                RowId=x.RowId,
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
