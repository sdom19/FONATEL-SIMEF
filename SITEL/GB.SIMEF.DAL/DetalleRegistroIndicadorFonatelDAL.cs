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
    public class DetalleRegistroIndicadorFonatelDAL : BitacoraDAL
    {
        private SITELContext db;

        #region Consultas

        /// <summary>
        /// Detalle Registro Indicador Variable Fonatel
        /// </summary>
        /// <returns></returns>
        public List<DetalleRegistroIndicadorFonatel> ObtenerDatoDetalleRegistroIndicador(DetalleRegistroIndicadorFonatel pDetalleRegistroIndicador)
        {
            List<DetalleRegistroIndicadorFonatel> ListaRegistroIndicadorFonatel = new List<DetalleRegistroIndicadorFonatel>();
            using (db=new SITELContext())
            {
                ListaRegistroIndicadorFonatel = db.Database.SqlQuery<DetalleRegistroIndicadorFonatel>
                 ("execute Fonatel.pa_obtenerDetalleRegistroIndicadorFonatel   @idSolicitud, @idFormulario, @idIndicador",
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
             ("execute FONATEL.pa_obtenerDetalleRegistroIndicadorVariableFonatel   @idSolicitud, @idFormulario, @idIndicador",
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
                html=string.Format(Constantes.EstructuraHtmlRegistroIndicador.Variable,x.NombreVariable)
            }).ToList();
            return ListaRegistroIndicadorFonatelVariable;
        }


        public List<DetalleRegistroIndicadorCategoriaFonatel> ObtenerDatoDetalleRegistroIndicadorCategoria(DetalleRegistroIndicadorFonatel pDetalleRegistroIndicador)
        {
            List<DetalleRegistroIndicadorCategoriaFonatel> ListaRegistroIndicadorFonatelCategoria = new List<DetalleRegistroIndicadorCategoriaFonatel>();
            ListaRegistroIndicadorFonatelCategoria = db.Database.SqlQuery<DetalleRegistroIndicadorCategoriaFonatel>
             ("execute FONATEL.pa_obtenerDetalleRegistroIndicadorCategoriaFonatel   @idSolicitud, @idFormulario, @idIndicador",
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
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputAlfanumerico, DetalleRegistroIndicadorCategoriaFonatel.NombreCategoria, DetalleRegistroIndicadorCategoriaFonatel.idCategoria);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Texto:
                    control = string.Format( Constantes.EstructuraHtmlRegistroIndicador.InputTexto, DetalleRegistroIndicadorCategoriaFonatel.NombreCategoria, DetalleRegistroIndicadorCategoriaFonatel.idCategoria);
                    break;
                 case (int)Constantes.TipoDetalleCategoriaEnum.Fecha:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputFecha, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.RangoMinimo,DetalleRegistroIndicadorCategoriaFonatel.RangoMaximo);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Numerico:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputNumerico, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.RangoMinimo, DetalleRegistroIndicadorCategoriaFonatel.RangoMaximo);
                    break;
                default:

                

                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputSelect, DetalleRegistroIndicadorCategoriaFonatel.idCategoria, DetalleRegistroIndicadorCategoriaFonatel.JSON);
                    break;
            }
            return control;
        }


        #endregion

        #region DetalleRegistroIndicadorCategoriaValorFonatel

        /// <summary>
        /// Autor: Georgi Mesen Cerdas
        /// Fecha: 17/11/2022
        /// Ejecutar procedimiento almacenado para actualizar o insertar datos de DetalleRegistroIndicadorCategoriaValorFonatel
        /// </summary>
        /// <param name="objCategoria"></param>
        /// <returns></returns>
        public List<DetalleRegistroIndicadorCategoriaValorFonatel> InsertarDetalleRegistroIndicadorCategoriaValorFonatel(DataTable objeto)
        {
            List<DetalleRegistroIndicadorCategoriaValorFonatel> ListaDetalleRegistroIndicadorCategoriaValorFonatel = new List<DetalleRegistroIndicadorCategoriaValorFonatel>();
            using (db = new SIMEFContext())
            {
                var parametros = new SqlParameter("@lst", SqlDbType.Structured);
                parametros.SqlValue = objeto;
                parametros.TypeName = "dbo.TypeDetalleRegistroIndicadorCategoriaValorFonatel";

                ListaDetalleRegistroIndicadorCategoriaValorFonatel = db.Database.SqlQuery<DetalleRegistroIndicadorCategoriaValorFonatel>
                ("execute spInsertarDetalleRegistroIndicadorCategoriaValorFonatel @lst", parametros
                    ).ToList();

                ListaDetalleRegistroIndicadorCategoriaValorFonatel = ListaDetalleRegistroIndicadorCategoriaValorFonatel.Select(x => new DetalleRegistroIndicadorCategoriaValorFonatel()
                {
                    IdSolicitud = x.IdSolicitud,
                    IdFormulario = x.IdFormulario,
                    IdIndicador = x.IdIndicador,
                    idCategoria = x.idCategoria,
                    NumeroFila = x.NumeroFila,
                    Valor = x.Valor,
                }).ToList();
            }
            return ListaDetalleRegistroIndicadorCategoriaValorFonatel;
        }

        /// <summary>
        /// Autor: Georgi Mesen Cerdas
        /// Fecha: 26/11/2022
        /// Ejecutar procedimiento almacenado para actualizar o insertar datos de DetalleRegistroIndicadorFonatel
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public List<DetalleRegistroIndicadorFonatel> ActualizarDetalleRegistroIndicadorFonatel(DetalleRegistroIndicadorFonatel objeto)
        {
            List<DetalleRegistroIndicadorFonatel> ListaRegistroIndicadorFonatel = new List<DetalleRegistroIndicadorFonatel>();
            using (db = new SIMEFContext())
            {
                ListaRegistroIndicadorFonatel = db.Database.SqlQuery<DetalleRegistroIndicadorFonatel>
                 ("execute sitel.spActualizarDetalleRegistroIndicadorFonatel   @idSolicitud, @idFormulario, @idIndicador, @IdDetalleRegistroIndicador, @TituloHojas, @NotasEncargado, @NotasInformante, @CodigoIndicador, @NombreIndicador, @CantidadFilas",
                   new SqlParameter("@idSolicitud", objeto.IdSolicitud),
                   new SqlParameter("@idFormulario", objeto.IdFormulario),
                   new SqlParameter("@idIndicador", objeto.IdIndicador),
                   new SqlParameter("@IdDetalleRegistroIndicador", objeto.IdDetalleRegistroIndicador),
                   new SqlParameter("@TituloHojas", objeto.TituloHojas),
                   new SqlParameter("@NotasEncargado", objeto.NotasEncargado),
                   new SqlParameter("@NotasInformante", objeto.NotasInformante), 
                   new SqlParameter("@CodigoIndicador", objeto.CodigoIndicador),
                   new SqlParameter("@NombreIndicador", objeto.NombreIndicador),
                   new SqlParameter("@CantidadFilas", objeto.CantidadFilas)
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
                    DetalleRegistroIndicadorVariableFonatel = ObtenerDatoDetalleRegistroIndicadorVariable(x),
                    DetalleRegistroIndicadorCategoriaFonatel = ObtenerDatoDetalleRegistroIndicadorCategoria(x),
                    IdFormularioString = Utilidades.Encriptar(x.IdFormulario.ToString()),
                    IdIndicadorString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    IdSolicitudString = Utilidades.Encriptar(x.IdSolicitud.ToString())
                }).ToList();
            }
            return ListaRegistroIndicadorFonatel;
        }

        /// <summary>
        /// Autor: Georgi Mesen Cerdas
        /// Fecha: 26/11/2022
        /// Ejecutar procedimiento almacenado para obtener DetalleRegistroIndicadorCategoriaValorFonatel
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public List<DetalleRegistroIndicadorCategoriaValorFonatel> ObtenerDetalleRegistroIndicadorCategoriaValorFonatel(DetalleRegistroIndicadorCategoriaValorFonatel objeto)
        {
            List<DetalleRegistroIndicadorCategoriaValorFonatel> ListaDetalleRegistroIndicadorCategoriaValorFonatel = new List<DetalleRegistroIndicadorCategoriaValorFonatel>();
            using (db = new SIMEFContext())
            {
                 ListaDetalleRegistroIndicadorCategoriaValorFonatel = db.Database.SqlQuery<DetalleRegistroIndicadorCategoriaValorFonatel>
                 ("execute SITEL.spObtenerDetalleRegistroIndicadorCategoriaValorFonatel  @idSolicitud, @idFormulario, @idIndicador, @idCategoria",
                    new SqlParameter("@idSolicitud", objeto.IdSolicitud),
                    new SqlParameter("@idFormulario", objeto.IdFormulario),
                    new SqlParameter("@idIndicador", objeto.IdIndicador),
                    new SqlParameter("@idCategoria", objeto.idCategoria)
                 ).ToList();

                    ListaDetalleRegistroIndicadorCategoriaValorFonatel = ListaDetalleRegistroIndicadorCategoriaValorFonatel.Select(x => new DetalleRegistroIndicadorCategoriaValorFonatel()
                    {
                        IdSolicitud = x.IdSolicitud,
                        IdFormulario = x.IdFormulario,
                        IdIndicador = x.IdIndicador,
                        idCategoria = x.idCategoria,
                        NumeroFila = x.NumeroFila,
                        Valor = x.Valor,
                    }).ToList();
            }
          
            return ListaDetalleRegistroIndicadorCategoriaValorFonatel;
        }

        #endregion
    }
}
