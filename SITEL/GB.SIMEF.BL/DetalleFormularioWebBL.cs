using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class DetalleFormularioWebBL : IMetodos<DetalleFormularioWeb>
    {
        private readonly DetalleFormularioWebDAL clsDatos;
        private RespuestaConsulta<List<DetalleFormularioWeb>> ResultadoConsulta;
        string modulo = Etiquetas.Formulario;
        string user = string.Empty;

        private readonly IndicadorFonatelDAL indicadorFonatelDAL;
        private readonly DetalleIndicadorVariablesDAL detalleIndicadorVariables;
        private readonly DetalleIndicadorCategoriaDAL detalleIndicadorCategoria;

        public DetalleFormularioWebBL(string modulo, string user) 
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new DetalleFormularioWebDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleFormularioWeb>>();

            this.indicadorFonatelDAL = new IndicadorFonatelDAL();
            this.detalleIndicadorVariables = new DetalleIndicadorVariablesDAL();
            this.detalleIndicadorCategoria = new DetalleIndicadorCategoriaDAL();
        }

        private bool ValidarDatosRepetidos(DetalleFormularioWeb objDetalleFormulario)
        {
            List<DetalleFormularioWeb> buscarRegistro = clsDatos.ObtenerDatos(new DetalleFormularioWeb());
            if (buscarRegistro.Where(x => x.idFormulario == objDetalleFormulario.idFormulario && x.idIndicador == objDetalleFormulario.idIndicador)
                .ToList().Count() > 0)
            {
                throw new Exception(Errores.IndicadorFormularioRegistrado);
            }
            return true;
        }

        private int ObtenerCantidadIndicadores(int id)
        {
            var cantidad = clsDatos.ObtenerCantidadIndicadores(id);
            if (cantidad <= 0)
                throw new Exception(Errores.CatidadIndicadoresExcedido);
            return cantidad;
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> ActualizarElemento(DetalleFormularioWeb objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = user;
                if (!String.IsNullOrEmpty(objeto.formularioweb.id))
                {
                    objeto.formularioweb.id = Utilidades.Desencriptar(objeto.formularioweb.id);
                    int temp;
                    if (int.TryParse(objeto.formularioweb.id, out temp))
                    {
                        objeto.idFormulario = temp;
                    }
                }
                DetalleFormularioWeb detalleFormularioWeb = clsDatos.ObtenerDatos(objeto).Single();
                objeto.idDetalle = detalleFormularioWeb.idDetalle;
                objeto.Estado = true;
                ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);

                string jsonValorInicial = objeto.ToString();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Indicador.Codigo, objeto.ToString(), detalleFormularioWeb.ToString(), "");
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> CambioEstado(DetalleFormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> ClonarDatos(DetalleFormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> EliminarElemento(DetalleFormularioWeb objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;
                ResultadoConsulta.Usuario = user;
                
                objeto = clsDatos.ObtenerDatos(objeto).Single();
                objeto.Estado = false;
                ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);

                //string jsonValorInicial = objeto.ToString();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Indicador.Codigo, "", "", "");
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> InsertarDatos(DetalleFormularioWeb objeto)
        {
            try
            {
                objeto.idDetalle = 0;
                objeto.Estado = true;
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = user;
                if (!String.IsNullOrEmpty(objeto.formularioweb.id))
                {
                    objeto.formularioweb.id = Utilidades.Desencriptar(objeto.formularioweb.id);
                    int temp;
                    if (int.TryParse(objeto.formularioweb.id, out temp))
                    {
                        objeto.idFormulario = temp;
                    }
                }
                int cantidad = ObtenerCantidadIndicadores(objeto.idFormulario);
                if (ValidarDatosRepetidos(objeto))
                {
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                    // Se le resta 1 porque para este momento ya se agregó uno nuevo 
                    ResultadoConsulta.objetoRespuesta[0].formularioweb.CantidadActual = cantidad - 1;
                }
                objeto = clsDatos.ObtenerDatos(objeto).Single();

                string jsonValorInicial = objeto.ToString();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Indicador.Codigo, "", "", jsonValorInicial);
            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.IndicadorFormularioRegistrado || ex.Message == Errores.CatidadIndicadoresExcedido)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                } 
                else
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> ObtenerDatos(DetalleFormularioWeb objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }


        public RespuestaConsulta<List<DetalleFormularioWeb>> ValidarDatos(DetalleFormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 05/01/2023
        /// Georgi Mesén Cerdas
        /// Función obtener los datos simulando que son los de registro indicador para Visualizar Formulario
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns>Retorna un DetalleRegistroIndicador</returns>
        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ObtenerVisualizar(DetalleRegistroIndicadorFonatel objeto)
        {
            RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ResultadoVisualizar = new RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>>();
            try
            {
                ResultadoVisualizar.Clase = modulo;
                ResultadoVisualizar.Accion = (int)Accion.Consultar;
                Indicador indicador = new Indicador();
                indicador.idIndicador = objeto.IdIndicador;
                //indicador.idEstado = (int)Constantes.EstadosRegistro.Activo;
                indicador = indicadorFonatelDAL.ObtenerDatos(indicador).FirstOrDefault();
                DetalleRegistroIndicadorFonatel detalle = new DetalleRegistroIndicadorFonatel();
                detalle.IdIndicador = indicador.idIndicador;
                detalle.CodigoIndicador = indicador.Codigo;
                detalle.NombreIndicador = indicador.Nombre;
                detalle.CantidadFilas = objeto.CantidadFilas;
                DetalleIndicadorVariables variable = new DetalleIndicadorVariables();
                variable.idIndicador = indicador.idIndicador;
                List<DetalleIndicadorVariables> variables = detalleIndicadorVariables.ObtenerDatos(variable).Where(x => x.Estado == true).ToList();
                List<DetalleRegistroIndicadorVariableFonatel> registroVariable =
                    variables.Select(x => new DetalleRegistroIndicadorVariableFonatel()
                    {
                        IdFormulario = 0,
                        IdSolicitud = 0,
                        IdIndicador = x.idIndicador,
                        idVariable = x.idDetalleIndicador,
                        NombreVariable = x.NombreVariable,
                        Descripcion = x.Descripcion,
                        html = string.Format(Constantes.EstructuraHtmlRegistroIndicador.Variable, x.NombreVariable),
                    }).ToList();
                detalle.DetalleRegistroIndicadorVariableFonatel = registroVariable;
                DetalleIndicadorCategoria categoria = new DetalleIndicadorCategoria();
                categoria.idIndicador = indicador.idIndicador;
                List<DetalleIndicadorCategoria> categorias = detalleIndicadorCategoria.ObtenerVisualizarCategorias(categoria).Where(x => x.Estado == true).ToList();
                List<DetalleRegistroIndicadorCategoriaFonatel> registroCategoria =
                    categorias.Select(x => new DetalleRegistroIndicadorCategoriaFonatel()
                    {
                        IdFormulario = 0,
                        IdSolicitud = 0,
                        IdIndicador = x.idIndicador,
                        idCategoria = x.idCategoria,
                        NombreCategoria = x.NombreCategoria,
                        IdTipoCategoria = x.IdTipoDetalle,
                        html = DefinirControl(x.IdTipoDetalle, x.NombreCategoria, x.idCategoria)
                    }).ToList();
                detalle.DetalleRegistroIndicadorCategoriaFonatel = registroCategoria;
                List<DetalleRegistroIndicadorFonatel> listaResultado = new List<DetalleRegistroIndicadorFonatel>();
                listaResultado.Add(detalle);
                ResultadoVisualizar.objetoRespuesta = listaResultado;
                ResultadoVisualizar.CantidadRegistros = listaResultado.Count();
            }
            catch (Exception ex)
            {
                ResultadoVisualizar.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoVisualizar.MensajeError = ex.Message;
            }
            return ResultadoVisualizar;
        }

        /// <summary>
        /// 05/01/2023
        /// Georgi Mesén Cerdas
        /// Función obtener el html de cada tipo de elemento para cargar en tabla de visualizar formulario
        /// </summary>
        /// <param name="idTipoCategoria"></param>
        /// <param name="nombre"></param>
        /// <param name="idCategoria"></param>
        /// <returns></returns>
        private string DefinirControl(int idTipoCategoria, string nombre, int idCategoria)
        {
            string control = string.Empty;
            switch (idTipoCategoria)
            {
                case (int)Constantes.TipoDetalleCategoriaEnum.Alfanumerico:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputAlfanumerico, nombre, idCategoria);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Texto:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputTexto, nombre, idCategoria);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Fecha:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputFecha, idCategoria, 0, 0);
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Numerico:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputNumerico, idCategoria, 0, 0);
                    break;
                default:
                    control = string.Format(Constantes.EstructuraHtmlRegistroIndicador.InputSelect, idCategoria, "<option value='-1'>Seleccione</option>");
                    break;
            }
            return control;
        }
    }
}
