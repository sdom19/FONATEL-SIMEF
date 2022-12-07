using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class EditarDetalleRegistroIndicadorFonatelDAL : BitacoraDAL
    {
        private SIMEFContext db;


        #region Listados

        /// <summary>
        /// Autor: Francisco Vindas
        /// Fecha: 01/12/2022
        /// El metodo crea una lista generica de la registro indicador que puede ser utilizado en lo metodos que lo necesiten 
        /// </summary>
        /// <param name="ListaSolicitud"></param>
        /// <returns></returns>

        private List<DetalleRegistroIndicadorFonatel> CrearListado(List<DetalleRegistroIndicadorFonatel> ListaDetalle)
        {
            return ListaDetalle.Select(x => new DetalleRegistroIndicadorFonatel
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
                DetalleRegistroIndicadorVariableFonatel = ObtenerDatoDetalleRegistroIndicadorVariable(x),
                DetalleRegistroIndicadorCategoriaFonatel = ObtenerDatoDetalleRegistroIndicadorCategoria(x),
                DetalleRegistroIndicadorCategoriaValorFonatel = ObtenerDatoDetalleRegistroIndicadorCategoriaValor(x),
                IdFormularioString = Utilidades.Encriptar(x.IdFormulario.ToString()),
                IdIndicadorString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                IdSolicitudString = Utilidades.Encriptar(x.IdSolicitud.ToString()),

            }).ToList();
        }

        /// <summary>
        /// Autor: Francisco Vindas
        /// Fecha: 01/12/2022
        /// El metodo crea una lista generica de la registro indicador que puede ser utilizado en lo metodos que lo necesiten 
        /// </summary>
        /// <param name="ListaSolicitud"></param>
        /// <returns></returns>

        private List<DetalleRegistroIndicadorVariableFonatel> CrearListadoVariables(List<DetalleRegistroIndicadorVariableFonatel> ListaVariables)
        {
            return ListaVariables.Select(x => new DetalleRegistroIndicadorVariableFonatel
            {
                IdSolicitud = x.IdSolicitud,
                IdFormulario = x.IdFormulario,
                IdIndicador = x.IdIndicador,
                idVariable = x.idVariable,
                NombreVariable = x.NombreVariable,
                Descripcion = x.Descripcion,
                html = string.Format(Constantes.EstructuraHtmlRegistroIdicador.Variable, x.NombreVariable)

            }).ToList();
        }

        /// <summary>
        /// Autor: Francisco Vindas
        /// Fecha: 01/12/2022
        /// El metodo crea una lista generica de la registro indicador que puede ser utilizado en lo metodos que lo necesiten 
        /// </summary>
        /// <param name="ListaSolicitud"></param>
        /// <returns></returns>

        private List<DetalleRegistroIndicadorCategoriaFonatel> CrearListadoCategoria(List<DetalleRegistroIndicadorCategoriaFonatel> ListaCategorias)
        {
            return ListaCategorias.Select(x => new DetalleRegistroIndicadorCategoriaFonatel
            {
                NombreCategoria = x.NombreCategoria,
                RangoMaximo = x.RangoMaximo,
                RangoMinimo = x.RangoMinimo,
                CantidadDetalleDesagregacion = x.CantidadDetalleDesagregacion,
                idCategoria = x.idCategoria,
                IdIndicador = x.IdIndicador,
                IdSolicitud = x.IdSolicitud,
                IdFormulario = x.IdFormulario,
                IdTipoCategoria = x.IdTipoCategoria,
                html = DefinirControl(x)

            }).ToList();
        }

        /// <summary>
        /// Autor: Francisco Vindas
        /// Fecha: 01/12/2022
        /// El metodo crea una lista generica de la registro indicador que puede ser utilizado en lo metodos que lo necesiten 
        /// </summary>
        /// <param name="ListaSolicitud"></param>
        /// <returns></returns>

        private List<DetalleRegistroIndicadorCategoriaValorFonatel> CrearListadoCategoriaValor(List<DetalleRegistroIndicadorCategoriaValorFonatel> ListaCategorias)
        {
            return ListaCategorias.Select(x => new DetalleRegistroIndicadorCategoriaValorFonatel
            {
                IdSolicitud = x.IdSolicitud,
                IdFormulario = x.IdFormulario,
                IdIndicador = x.IdIndicador,
                idCategoria = x.idCategoria,
                NumeroFila = x.NumeroFila,
                Valor = x.Valor
                
            }).ToList();
        }

        #endregion

        #region Consultas

        public List<DetalleRegistroIndicadorFonatel> ObtenerDatoDetalleRegistroIndicador(DetalleRegistroIndicadorFonatel pDetalleRegistroIndicador)
        {
            List<DetalleRegistroIndicadorFonatel> ListaRegistroIndicadorFonatel = new List<DetalleRegistroIndicadorFonatel>();
            using (db = new SIMEFContext())
            {
                ListaRegistroIndicadorFonatel = db.Database.SqlQuery<DetalleRegistroIndicadorFonatel>
                 ("execute spObtenerDetalleRegistroIndicador @idSolicitud, @idFormulario, @idIndicador",
                  new SqlParameter("@idSolicitud", pDetalleRegistroIndicador.IdSolicitud),
                   new SqlParameter("@idFormulario", pDetalleRegistroIndicador.IdFormulario),
                   new SqlParameter("@idIndicador", pDetalleRegistroIndicador.IdIndicador)
                 ).ToList();

                ListaRegistroIndicadorFonatel = CrearListado(ListaRegistroIndicadorFonatel);
            }
            return ListaRegistroIndicadorFonatel;
        }

        public List<DetalleRegistroIndicadorVariableFonatel> ObtenerDatoDetalleRegistroIndicadorVariable(DetalleRegistroIndicadorFonatel pDetalleRegistroIndicador)
        {
            List<DetalleRegistroIndicadorVariableFonatel> ListaRegistroIndicadorFonatelVariable = new List<DetalleRegistroIndicadorVariableFonatel>();

            using (db = new SIMEFContext())
            {
                ListaRegistroIndicadorFonatelVariable = db.Database.SqlQuery<DetalleRegistroIndicadorVariableFonatel>
             ("execute spObtenerDetalleRegistroIndicadorVariable @idSolicitud, @idFormulario, @idIndicador",
                new SqlParameter("@idSolicitud", pDetalleRegistroIndicador.IdSolicitud),
                new SqlParameter("@idFormulario", pDetalleRegistroIndicador.IdFormulario),
                new SqlParameter("@idIndicador", pDetalleRegistroIndicador.IdIndicador)
             ).ToList();

                ListaRegistroIndicadorFonatelVariable = CrearListadoVariables(ListaRegistroIndicadorFonatelVariable);
            }


            return ListaRegistroIndicadorFonatelVariable;
        }

        public List<DetalleRegistroIndicadorCategoriaFonatel> ObtenerDatoDetalleRegistroIndicadorCategoria(DetalleRegistroIndicadorFonatel pDetalleRegistroIndicador)
        {
            List<DetalleRegistroIndicadorCategoriaFonatel> ListaRegistroIndicadorFonatelCategoria = new List<DetalleRegistroIndicadorCategoriaFonatel>();

            using (db = new SIMEFContext())
            {
                ListaRegistroIndicadorFonatelCategoria = db.Database.SqlQuery<DetalleRegistroIndicadorCategoriaFonatel>
             ("execute spObtenerDetalleRegistroIndicadorCategoria @idSolicitud, @idFormulario, @idIndicador",
                new SqlParameter("@idSolicitud", pDetalleRegistroIndicador.IdSolicitud),
                new SqlParameter("@idFormulario", pDetalleRegistroIndicador.IdFormulario),
                new SqlParameter("@idIndicador", pDetalleRegistroIndicador.IdIndicador)
             ).ToList();

                ListaRegistroIndicadorFonatelCategoria = CrearListadoCategoria(ListaRegistroIndicadorFonatelCategoria);
            }

            return ListaRegistroIndicadorFonatelCategoria;
        }

        public List<DetalleRegistroIndicadorCategoriaValorFonatel> ObtenerDatoDetalleRegistroIndicadorCategoriaValor(DetalleRegistroIndicadorFonatel pDetalleRegistroIndicador)
        {
            List<DetalleRegistroIndicadorCategoriaValorFonatel> ListaRegistroIndicadorFonatelCategoria = new List<DetalleRegistroIndicadorCategoriaValorFonatel>();

            using (db = new SIMEFContext())
            {
                ListaRegistroIndicadorFonatelCategoria = db.Database.SqlQuery<DetalleRegistroIndicadorCategoriaValorFonatel>
             ("execute spObtenerDetalleRegistroIndicadorCategoriaValor @idSolicitud, @idFormulario, @idIndicador",
                new SqlParameter("@idSolicitud", pDetalleRegistroIndicador.IdSolicitud),
                new SqlParameter("@idFormulario", pDetalleRegistroIndicador.IdFormulario),
                new SqlParameter("@idIndicador", pDetalleRegistroIndicador.IdIndicador)
             ).ToList();

                ListaRegistroIndicadorFonatelCategoria = CrearListadoCategoriaValor(ListaRegistroIndicadorFonatelCategoria);
            }

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
                    control = string.Format(Constantes.EstructuraHtmlRegistroIdicador.InputTexto, DetalleRegistroIndicadorCategoriaFonatel.NombreCategoria, DetalleRegistroIndicadorCategoriaFonatel.idCategoria);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Fecha:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIdicador.InputFecha, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.RangoMinimo, DetalleRegistroIndicadorCategoriaFonatel.RangoMaximo);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Numerico:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIdicador.InputNumerico, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.RangoMinimo, DetalleRegistroIndicadorCategoriaFonatel.RangoMaximo);
                    break;
                default:

                    control = string.Format(Constantes.EstructuraHtmlRegistroIdicador.InputSelect, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.JSON);
                    break;
            }
            return control;
        }

        #endregion
    }
}
