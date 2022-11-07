using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class DetalleRegistroIndicadorFonatelDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// Detalle Registro Indicador Variable Fonatel
        /// </summary>
        /// <returns></returns>
        public List<DetalleRegistroIndicadorFonatel> ObtenerDatoDetalleRegistroIndicador(DetalleRegistroIndicadorFonatel pDetalleRegistroIndicadorCategoria)
        {
            List<DetalleRegistroIndicadorFonatel> ListaRegistroIndicadorFonatel = new List<DetalleRegistroIndicadorFonatel>();
            using (db=new SIMEFContext())
            {
                ListaRegistroIndicadorFonatel = db.Database.SqlQuery<DetalleRegistroIndicadorFonatel>
                 ("execute sitel.spObtenerDetalleRegistroIndicadorFonatel   @idSolicitud, @idFormulario, @idIndicador",
                  new SqlParameter("@idSolicitud", pDetalleRegistroIndicadorCategoria.IdSolicitud),
                   new SqlParameter("@idFormulario", pDetalleRegistroIndicadorCategoria.IdFormulario),
                   new SqlParameter("@idIndicador", pDetalleRegistroIndicadorCategoria.IdIndicador)
                 ).ToList();
                ListaRegistroIndicadorFonatel = ListaRegistroIndicadorFonatel.Select(x => new DetalleRegistroIndicadorFonatel()
                {
                    IdFormulario = x.IdFormulario,
                    IdIndicador = x.IdIndicador,
                    IdDetalleRegistroIndicador = x.IdDetalleRegistroIndicador,
                    NombreIndicador = x.NombreIndicador,
                    NotasEncargado = x.NotasEncargado,
                    NotasInformante = x.NotasInformante,
                    CantidadFilas = x.CantidadFilas,
                    CodigoIndicador = x.CodigoIndicador,
                    TituloHojas = x.TituloHojas,
                    IdSolicitud = x.IdSolicitud,
                    IdFormularioString = Utilidades.Encriptar(x.IdFormulario.ToString()),
                    IdIndicadorString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    IdSolicitudString = Utilidades.Encriptar(x.IdSolicitud.ToString())
                }).ToList();
            }
            return ListaRegistroIndicadorFonatel;
        }
    }
}
