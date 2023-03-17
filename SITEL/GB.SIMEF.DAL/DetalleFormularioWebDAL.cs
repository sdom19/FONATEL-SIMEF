using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class DetalleFormularioWebDAL : BitacoraDAL
    {
        private SIMEFContext db;

        public List<DetalleFormularioWeb> ActualizarDatos(DetalleFormularioWeb objDetalleFormulario)
        {
            List<DetalleFormularioWeb> ListaDetalleFormulariosWeb = new List<DetalleFormularioWeb>();
            using (db = new SIMEFContext())
            {
                ListaDetalleFormulariosWeb = db.Database.SqlQuery<DetalleFormularioWeb>
                    ("execute pa_ActualizarDetalleFormularioWeb @idDetalle, @idFormularioWeb, @idIndicador, @TituloHojas, @NotasInformante, " +
                    "@NotasEncargado, @Estado",
                    new SqlParameter("@idDetalle", objDetalleFormulario.IdDetalleFormularioWeb),
                    new SqlParameter("@idFormularioWeb", objDetalleFormulario.idFormularioWeb),
                    new SqlParameter("@idIndicador", objDetalleFormulario.idIndicador),
                    new SqlParameter("@TituloHojas", string.IsNullOrEmpty(objDetalleFormulario.TituloHoja) ? DBNull.Value.ToString() : objDetalleFormulario.TituloHoja),
                    new SqlParameter("@NotasInformante", string.IsNullOrEmpty(objDetalleFormulario.NotaInformante) ? DBNull.Value.ToString() : objDetalleFormulario.NotaInformante),
                    new SqlParameter("@NotasEncargado", string.IsNullOrEmpty(objDetalleFormulario.NotaEncargado) ? DBNull.Value.ToString() : objDetalleFormulario.NotaEncargado),
                    new SqlParameter("@Estado", objDetalleFormulario.Estado)
                    ).ToList();

                ListaDetalleFormulariosWeb = ListaDetalleFormulariosWeb.Select(x => new DetalleFormularioWeb()
                {
                    IdDetalleFormularioWeb = x.IdDetalleFormularioWeb,
                    idFormularioWeb = x.idFormularioWeb,
                    idIndicador = x.idIndicador,
                    TituloHoja = x.TituloHoja,
                    NotaInformante = x.NotaInformante,
                    NotaEncargado = x.NotaEncargado,
                    Estado = x.Estado,
                    Indicador = db.Indicador.Where(i => i.IdIndicador == x.idIndicador).FirstOrDefault(),
                    formularioweb = db.FormularioWeb.Where(i => i.idFormularioWeb == x.idFormularioWeb).FirstOrDefault(),
                }).ToList();
            }
            return ListaDetalleFormulariosWeb;
        }

        public int ObtenerCantidadIndicadores(int idFormularioWeb) {
            int cantidadFaltante = 0;
            using (db = new SIMEFContext())
            {
                cantidadFaltante = db.Database.SqlQuery<int>("execute pa_ObtenerFormularioWebCantidadIndicador @idFormularioWeb",
                    new SqlParameter("@idFormularioWeb", idFormularioWeb)
                    ).Single();
            }
            return cantidadFaltante;
        }

        public List<DetalleFormularioWeb> ObtenerDatos(DetalleFormularioWeb objDetalleFormulario)
        {
            List<DetalleFormularioWeb> ListaDetalleFormulariosWeb = new List<DetalleFormularioWeb>();
            using (db = new SIMEFContext())
            { 
                ListaDetalleFormulariosWeb = db.Database.SqlQuery<DetalleFormularioWeb>
                    ("execute pa_ObtenerDetalleFormularioWeb @idDetalle, @idFormulario, @idIndicador",
                    new SqlParameter("@idDetalle", objDetalleFormulario.IdDetalleFormularioWeb),
                    new SqlParameter("@idFormulario", objDetalleFormulario.idFormularioWeb),
                    new SqlParameter("@idIndicador", objDetalleFormulario.idIndicador)
                    ).ToList();

                ListaDetalleFormulariosWeb = ListaDetalleFormulariosWeb.Select(x => new DetalleFormularioWeb() 
                {
                    IdDetalleFormularioWeb = x.IdDetalleFormularioWeb,
                    idFormularioWeb = x.idFormularioWeb,
                    idIndicador = x.idIndicador,
                    TituloHoja = x.TituloHoja,
                    NotaInformante = x.NotaInformante,
                    NotaEncargado = x.NotaEncargado,
                    Estado = x.Estado,

                    Indicador = db.Indicador.Where(i => i.IdIndicador == x.idIndicador).FirstOrDefault(),
                    formularioweb = db.FormularioWeb.Where(i => i.idFormularioWeb == x.idFormularioWeb).FirstOrDefault(),
                }
                ).ToList();
            }
            return ListaDetalleFormulariosWeb;

        }
    }
}
