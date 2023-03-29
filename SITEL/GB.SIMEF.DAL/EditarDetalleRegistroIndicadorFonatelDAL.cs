using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
                idFormularioWeb = x.idFormularioWeb,
                IdIndicador = x.IdIndicador,
                IdDetalleRegistroIndicadorFonatel = x.IdDetalleRegistroIndicadorFonatel,
                NombreIndicador = x.NombreIndicador,
                NotaEncargado = x.NotaEncargado,
                NotaInformante = x.NotaInformante,
                CantidadFila = x.CantidadFila,
                CodigoIndicador = x.CodigoIndicador,
                TituloHoja = x.TituloHoja,
                IdSolicitud = x.IdSolicitud,
                DetalleRegistroIndicadorVariableFonatel = ObtenerDatoDetalleRegistroIndicadorVariable(x),
                DetalleRegistroIndicadorCategoriaFonatel = ObtenerDatoDetalleRegistroIndicadorCategoria(x),
                DetalleRegistroIndicadorCategoriaValorFonatel = ObtenerDatoDetalleRegistroIndicadorCategoriaValor(x),
                idFormularioWebString = Utilidades.Encriptar(x.idFormularioWeb.ToString()),
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
                idFormularioWeb = x.idFormularioWeb,
                IdIndicador = x.IdIndicador,
                idVariable = x.idVariable,
                NombreVariable = x.NombreVariable,
                Descripcion = x.Descripcion,
                html = string.Format(Constantes.EstructuraHtmlRegistroIndicador.Variable, x.NombreVariable)

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
                idFormularioWeb = x.idFormularioWeb,
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
                idFormularioWeb = x.idFormularioWeb,
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
                 ("execute spObtenerDetalleRegistroIndicador @idSolicitud, @idFormularioWeb, @idIndicador",
                  new SqlParameter("@idSolicitud", pDetalleRegistroIndicador.IdSolicitud),
                   new SqlParameter("@idFormularioWeb", pDetalleRegistroIndicador.idFormularioWeb),
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
             ("execute spObtenerDetalleRegistroIndicadorVariable @idSolicitud, @idFormularioWeb, @idIndicador",
                new SqlParameter("@idSolicitud", pDetalleRegistroIndicador.IdSolicitud),
                new SqlParameter("@idFormularioWeb", pDetalleRegistroIndicador.idFormularioWeb),
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
             ("execute spObtenerDetalleRegistroIndicadorCategoria @idSolicitud, @idFormularioWeb, @idIndicador",
                new SqlParameter("@idSolicitud", pDetalleRegistroIndicador.IdSolicitud),
                new SqlParameter("@idFormularioWeb", pDetalleRegistroIndicador.idFormularioWeb),
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
             ("execute spObtenerDetalleRegistroIndicadorCategoriaValor @idSolicitud, @idFormularioWeb, @idIndicador",
                new SqlParameter("@idSolicitud", pDetalleRegistroIndicador.IdSolicitud),
                new SqlParameter("@idFormularioWeb", pDetalleRegistroIndicador.idFormularioWeb),
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
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputAlfanumerico, DetalleRegistroIndicadorCategoriaFonatel.NombreCategoria, DetalleRegistroIndicadorCategoriaFonatel.idCategoria);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Texto:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputTexto, DetalleRegistroIndicadorCategoriaFonatel.NombreCategoria, DetalleRegistroIndicadorCategoriaFonatel.idCategoria);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Fecha:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputFecha, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.RangoMinimo, DetalleRegistroIndicadorCategoriaFonatel.RangoMaximo);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Numerico:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputNumerico, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.RangoMinimo, DetalleRegistroIndicadorCategoriaFonatel.RangoMaximo);
                    break;
                default:

                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputSelect, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.DetalleCategoriaDesagregacion);
                    break;
            }
            return control;
        }

        public List<DetalleRegistroIndicadorFonatel> ActualizarDetalleRegistroIndicadorFonatel(DetalleRegistroIndicadorFonatel objeto)
        {
            List<DetalleRegistroIndicadorFonatel> ListaRegistroIndicador = new List<DetalleRegistroIndicadorFonatel>();

            using (db = new SIMEFContext())
            {
                ListaRegistroIndicador = db.Database.SqlQuery<DetalleRegistroIndicadorFonatel>
                 ("execute spActualizarDetalleRegistroIndicador @idSolicitud, @idFormularioWeb, @idIndicador, @IdDetalleRegistroIndicador, @TituloHojas, @NotasEncargado, @NotasInformante, @CodigoIndicador, @NombreIndicador, @CantidadFilas",
                   new SqlParameter("@idSolicitud", objeto.IdSolicitud),
                   new SqlParameter("@idFormularioWeb", objeto.idFormularioWeb),
                   new SqlParameter("@idIndicador", objeto.IdIndicador),
                   new SqlParameter("@IdDetalleRegistroIndicador", objeto.IdDetalleRegistroIndicadorFonatel),
                   new SqlParameter("@TituloHojas", objeto.TituloHoja),
                   new SqlParameter("@NotasEncargado", objeto.NotaEncargado),
                   new SqlParameter("@NotasInformante", objeto.NotaInformante),
                   new SqlParameter("@CodigoIndicador", objeto.CodigoIndicador),
                   new SqlParameter("@NombreIndicador", objeto.NombreIndicador),
                   new SqlParameter("@CantidadFilas", objeto.CantidadFila)
                 ).ToList();

                ListaRegistroIndicador = CrearListado(ListaRegistroIndicador);
            }

            return ListaRegistroIndicador;
        }

        public void EliminarDetalleRegistroIndicadorCategoriaValorFonatel(DetalleRegistroIndicadorCategoriaValorFonatel objeto)
        {
            List<DetalleRegistroIndicadorCategoriaValorFonatel> ListaDetalleRegistroIndicadorCategoriaValorFonatel = new List<DetalleRegistroIndicadorCategoriaValorFonatel>();
            using (db = new SIMEFContext())
            {
                ListaDetalleRegistroIndicadorCategoriaValorFonatel = db.Database.SqlQuery<DetalleRegistroIndicadorCategoriaValorFonatel>
                ("execute spEliminarDetalleRegistroIndicadorCategoriaValor @idSolicitud, @idFormularioWeb, @idIndicador, @idCategoria",
                   new SqlParameter("@idSolicitud", objeto.IdSolicitud),
                   new SqlParameter("@idFormularioWeb", objeto.idFormularioWeb),
                   new SqlParameter("@idIndicador", objeto.IdIndicador),
                   new SqlParameter("@idCategoria", objeto.idCategoria)
                ).ToList();
            }
        }

        public List<DetalleRegistroIndicadorCategoriaValorFonatel> InsertarDetalleRegistroIndicadorCategoriaValorFonatel(DataTable objeto)
        {
            List<DetalleRegistroIndicadorCategoriaValorFonatel> ListaDetalleRegistroIndicadorCategoriaValor = new List<DetalleRegistroIndicadorCategoriaValorFonatel>();

            using (db = new SIMEFContext())
            {
                var parametros = new SqlParameter("@lst", SqlDbType.Structured);
                parametros.SqlValue = objeto;
                parametros.TypeName = "TypeDetalleRegistroIndicadorCategoriaValor";

                ListaDetalleRegistroIndicadorCategoriaValor = db.Database.SqlQuery<DetalleRegistroIndicadorCategoriaValorFonatel>
                ("execute spActualizarDetalleRegistroIndicadorCategoriaValor @lst", parametros
                    ).ToList();

                ListaDetalleRegistroIndicadorCategoriaValor = CrearListadoCategoriaValor(ListaDetalleRegistroIndicadorCategoriaValor);
            }

            return ListaDetalleRegistroIndicadorCategoriaValor;

        }

        #endregion
    }
}
