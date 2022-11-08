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

        #region Consultas

        /// <summary>
        /// Detalle Registro Indicador Variable Fonatel
        /// </summary>
        /// <returns></returns>
        public List<DetalleRegistroIndicadorFonatel> ObtenerDatoDetalleRegistroIndicador(DetalleRegistroIndicadorFonatel pDetalleRegistroIndicador)
        {
            List<DetalleRegistroIndicadorFonatel> ListaRegistroIndicadorFonatel = new List<DetalleRegistroIndicadorFonatel>();
            using (db=new SIMEFContext())
            {
                ListaRegistroIndicadorFonatel = db.Database.SqlQuery<DetalleRegistroIndicadorFonatel>
                 ("execute sitel.spObtenerDetalleRegistroIndicadorFonatel   @idSolicitud, @idFormulario, @idIndicador",
                  new SqlParameter("@idSolicitud", pDetalleRegistroIndicador.IdSolicitud),
                   new SqlParameter("@idFormulario", pDetalleRegistroIndicador.IdFormulario),
                   new SqlParameter("@idIndicador", pDetalleRegistroIndicador.IdIndicador)
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
                    DetalleRegistroIndicadorVariableFonatel=ObtenerDatoDetalleRegistroIndicadorVariable(x),
                    DetalleRegistroIndicadorCategoriaFonatel=ObtenerDatoDetalleRegistroIndicadorCategoria(x),
                    IdFormularioString = Utilidades.Encriptar(x.IdFormulario.ToString()),
                    IdIndicadorString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    IdSolicitudString = Utilidades.Encriptar(x.IdSolicitud.ToString())
                }).ToList();
            }
            return ListaRegistroIndicadorFonatel;
        }


        /// <summary>
        /// Carga las variable de registro indicador
        /// Michael Hernández Cordero
        /// 11/08/2022
        /// </summary>
        /// <param name="pDetalleRegistroIndicador"></param>
        /// <returns></returns>

        public List<DetalleRegistroIndicadorVariableFonatel> ObtenerDatoDetalleRegistroIndicadorVariable(DetalleRegistroIndicadorFonatel pDetalleRegistroIndicador)
        {
            List<DetalleRegistroIndicadorVariableFonatel> ListaRegistroIndicadorFonatelVariable = new List<DetalleRegistroIndicadorVariableFonatel>();
            ListaRegistroIndicadorFonatelVariable = db.Database.SqlQuery<DetalleRegistroIndicadorVariableFonatel>
             ("execute SITEL.spObtenerDetalleRegistroIndicadorVariableFonatel   @idSolicitud, @idFormulario, @idIndicador",
                new SqlParameter("@idSolicitud", pDetalleRegistroIndicador.IdSolicitud),
                new SqlParameter("@idFormulario", pDetalleRegistroIndicador.IdFormulario),
                new SqlParameter("@idIndicador", pDetalleRegistroIndicador.IdIndicador)
             ).ToList();
            ListaRegistroIndicadorFonatelVariable = ListaRegistroIndicadorFonatelVariable.Select(x => new DetalleRegistroIndicadorVariableFonatel()
            {
                IdSolicitud=x.IdSolicitud,
                IdFormulario=x.IdFormulario,
                IdIndicador=x.IdIndicador,
                idVariable=x.idVariable,
                NombreVariable=x.NombreVariable,
                Descripcion=x.Descripcion,
                html=string.Format(Constantes.EstructuraHtmlRegistroIdicador.Variable,x.NombreVariable)
            }).ToList();
            return ListaRegistroIndicadorFonatelVariable;
        }


        public List<DetalleRegistroIndicadorCategoriaFonatel> ObtenerDatoDetalleRegistroIndicadorCategoria(DetalleRegistroIndicadorFonatel pDetalleRegistroIndicador)
        {
            List<DetalleRegistroIndicadorCategoriaFonatel> ListaRegistroIndicadorFonatelCategoria = new List<DetalleRegistroIndicadorCategoriaFonatel>();
            ListaRegistroIndicadorFonatelCategoria = db.Database.SqlQuery<DetalleRegistroIndicadorCategoriaFonatel>
             ("execute SITEL.spObtenerDetalleRegistroIndicadorCategoriaFonatel   @idSolicitud, @idFormulario, @idIndicador",
                new SqlParameter("@idSolicitud", pDetalleRegistroIndicador.IdSolicitud),
                new SqlParameter("@idFormulario", pDetalleRegistroIndicador.IdFormulario),
                new SqlParameter("@idIndicador", pDetalleRegistroIndicador.IdIndicador)
             ).ToList();
            ListaRegistroIndicadorFonatelCategoria = ListaRegistroIndicadorFonatelCategoria.Select(x => new DetalleRegistroIndicadorCategoriaFonatel()
            {
                NombreCategoria=x.NombreCategoria,
                RangoMaximo=x.RangoMaximo,
                RangoMinimo=x.RangoMinimo,
                CantidadDetalleDesagregacion=x.CantidadDetalleDesagregacion,
                idCategoria=x.idCategoria,
                IdIndicador=x.IdIndicador,
                IdSolicitud=x.IdSolicitud,
                IdFormulario=x.IdFormulario,
                IdTipoCategoria=x.IdTipoCategoria,
                html=DefinirControl(x)
            }).ToList();
            


            return ListaRegistroIndicadorFonatelCategoria;
        }



        private string DefinirControl(DetalleRegistroIndicadorCategoriaFonatel DetalleRegistroIndicadorCategoriaFonatel)
        {
            string control = string.Empty;
            switch (DetalleRegistroIndicadorCategoriaFonatel.IdTipoCategoria)
            {
                case (int)Constantes.TipoDetalleCategoriaEnum.Alfanumerico:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIdicador.InputAlfanumerico, DetalleRegistroIndicadorCategoriaFonatel.NombreCategoria, DetalleRegistroIndicadorCategoriaFonatel.idCategoria);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Texto:
                    control = string.Format( Constantes.EstructuraHtmlRegistroIdicador.InputTexto, DetalleRegistroIndicadorCategoriaFonatel.NombreCategoria, DetalleRegistroIndicadorCategoriaFonatel.idCategoria);
                    break;
                 case (int)Constantes.TipoDetalleCategoriaEnum.Fecha:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIdicador.InputFecha, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.RangoMinimo,DetalleRegistroIndicadorCategoriaFonatel.RangoMaximo);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Numerico:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIdicador.InputNumerico, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.RangoMinimo, DetalleRegistroIndicadorCategoriaFonatel.RangoMaximo);
                    break;
                default:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIdicador.InputNumerico, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.RangoMinimo, "");
                    break;
            }
            return control;
        }

        #endregion
    }
}
