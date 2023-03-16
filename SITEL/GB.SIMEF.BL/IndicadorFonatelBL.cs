using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class IndicadorFonatelBL : IMetodos<Indicador>
    {
        private readonly string modulo = string.Empty;
        private readonly string user = string.Empty;
        private readonly IndicadorFonatelDAL indicadorFonatelDAL;
        private readonly TipoIndicadorDAL tipoIndicadorDAL;
        private readonly FrecuenciaEnvioDAL frecuenciaEnvioDAL;
        private readonly ClasificacionIndicadorDAL clasificacionIndicadorDAL;
        private readonly TipoMedidaDAL tipoMedidaDAL;
        private readonly GrupoIndicadorDAL grupoIndicadorDAL;
        private readonly UnidadEstudioDAL unidadEstudioDAL;
        private readonly DetalleIndicadorVariablesDAL detalleIndicadorVariablesDAL;
        private readonly DetalleIndicadorCategoriaDAL detalleIndicadorCategoriaDAL;
        private readonly FormularioWebDAL formularioWebDAL;
        private readonly FormulasCalculoDAL formulasCalculoDAL;

        public IndicadorFonatelBL(string pView, string pUser)
        {
            modulo = pView;
            user = pUser;
            indicadorFonatelDAL = new IndicadorFonatelDAL();
            tipoIndicadorDAL = new TipoIndicadorDAL();
            frecuenciaEnvioDAL = new FrecuenciaEnvioDAL();
            clasificacionIndicadorDAL = new ClasificacionIndicadorDAL();
            tipoMedidaDAL = new TipoMedidaDAL();
            grupoIndicadorDAL = new GrupoIndicadorDAL();
            unidadEstudioDAL = new UnidadEstudioDAL();
            detalleIndicadorVariablesDAL = new DetalleIndicadorVariablesDAL();
            detalleIndicadorCategoriaDAL = new DetalleIndicadorCategoriaDAL();
            formularioWebDAL = new FormularioWebDAL();
            formulasCalculoDAL = new FormulasCalculoDAL();
        }

        public RespuestaConsulta<List<Indicador>> ActualizarElemento(Indicador pIndicador)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Función que permite realizar un cambio de estado de un indicador, puede ser eliminado o desactivo
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> CambioEstado(Indicador pIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();
            bool errorControlado = false;
            int nuevoEstado = pIndicador.nuevoEstado;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.id), out int number);
                pIndicador.idIndicador = number;

                if (pIndicador.idIndicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                Indicador indicadorViejo = indicadorFonatelDAL.ObtenerDatos(pIndicador).FirstOrDefault();
                string JsonAnterior = indicadorViejo?.ToString();

                pIndicador = indicadorFonatelDAL.VerificarExistenciaIndicadorPorID(pIndicador.idIndicador);

                if (pIndicador == null) // ¿el indicador existe?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // validación para cuando se desactiva un indicador. 
                if (nuevoEstado == (int)EstadosRegistro.Desactivado)
                {
                    if (pIndicador.idEstado == (int)EstadosRegistro.EnProceso) // Para desactivar tiene que estar en estado "Activo"
                    { 
                        errorControlado = true;
                        throw new Exception(Errores.NoRegistrosActualizar);
                    }

                    CambioEstadoDependenciasIndicador(pIndicador, EstadosRegistro.EnProceso); // cambiar de estado las dependencias del indicador
                }

                // actualizar el estado del indicador
                pIndicador.UsuarioModificacion = user;
                pIndicador.idEstado = nuevoEstado;

                List<Indicador> indicadorActualizado = indicadorFonatelDAL.ActualizarDatos(pIndicador);

                // construir respuesta
                int accion = 0;
                switch (nuevoEstado)
                {
                    case (int)EstadosRegistro.Eliminado:
                        accion = (int)Accion.Eliminar; break;
                    case (int)EstadosRegistro.Activo:
                        accion = (int)Accion.Activar; break;
                    case (int)EstadosRegistro.Desactivado:
                        accion = (int)Accion.Inactiva; break;
                    case (int)EstadosRegistro.EnProceso:
                        accion = (int)Accion.Editar; break;
                }

                resultado.Accion = accion;
                resultado.Clase = modulo;
                resultado.Usuario = user;
                resultado.CantidadRegistros = indicadorActualizado.Count();

                var objeto = indicadorActualizado[0];

                string JsonActual = objeto.ToString();

                indicadorFonatelDAL.RegistrarBitacora(resultado.Accion,
                      resultado.Usuario,
                      resultado.Clase, objeto.Codigo, JsonActual, JsonAnterior, "");
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        public RespuestaConsulta<List<Indicador>> ClonarDatos(Indicador pIndicador)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Indicador>> EliminarElemento(Indicador pIndicador)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Función que verifica si el indicador se encuentra en algún formulario web o una formula de calculo.
        /// Retorna un listado indicando las dependencias según corresponda
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<string>> VerificarUsoIndicador(Indicador pIndicador)
        {
            RespuestaConsulta<List<string>> resultado = new RespuestaConsulta<List<string>>();
            bool errorControlado = false;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.id), out int number);
                pIndicador.idIndicador = number;

                if (pIndicador.idIndicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                pIndicador = indicadorFonatelDAL.ObtenerDatos(pIndicador).FirstOrDefault();

                PrepararObjetoIndicador(pIndicador);
                List<string> result = indicadorFonatelDAL.VerificarDependenciasIndicador(pIndicador);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 29/08/2022
        /// José Navarro Acuña
        /// Función que crea un nuevo registro en la entidad Indicador
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> InsertarDatos(Indicador pIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();
            bool errorControlado = false;

            try
            {
                PrepararObjetoIndicador(pIndicador);
                resultado = ValidarDatos(pIndicador);

                if (resultado.HayError != 0)
                {
                    return resultado;
                }

                resultado.objetoRespuesta = indicadorFonatelDAL.ActualizarDatos(pIndicador);
                
                resultado.Usuario = user;
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;

                //var nuevoIndicador = new Indicador { idIndicador = pIndicador.idIndicador };

                pIndicador = indicadorFonatelDAL.ObtenerDatos(pIndicador).Single();

                string jsonValorInicial = pIndicador.ToString();
                

                indicadorFonatelDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario, resultado.Clase, pIndicador.Codigo, "", "", jsonValorInicial);
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 04/10/2022
        /// José Navarro Acuña
        /// Función que permite realizar un guardado definitivo de un indicador
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> GuardadoDefinitivoIndicador(Indicador pIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();
            bool errorControlado = false;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.id), out int number);
                pIndicador.idIndicador = number;

                if (pIndicador.idIndicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // validar si el indicador existe
                Indicador indicadorRegistrado = indicadorFonatelDAL.VerificarExistenciaIndicadorPorID(pIndicador.idIndicador);

                if (indicadorRegistrado == null) // el indicador existe?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // validar que el indicador tenga sus datos completos
                string msgIndicadorCompleto = VerificarDatosCompletosIndicador(indicadorRegistrado);

                if (!string.IsNullOrEmpty(msgIndicadorCompleto))
                {
                    errorControlado = true;
                    throw new Exception(msgIndicadorCompleto);
                }

                List<DetalleIndicadorVariables> objDetallesVariables = detalleIndicadorVariablesDAL.ObtenerDatos(
                    new DetalleIndicadorVariables() { idIndicador = indicadorRegistrado.idIndicador });

                List<DetalleIndicadorCategoria> objDetallesCategoria = detalleIndicadorCategoriaDAL.ObtenerDatos(
                    new DetalleIndicadorCategoria() { idIndicador = indicadorRegistrado.idIndicador, DetallesAgrupados = true });

                // verificar que los detalles esten completos en cuanto a cantidad
                if (indicadorRegistrado.CantidadVariableDato != objDetallesVariables.Count() ||
                    indicadorRegistrado.CantidadCategoriasDesagregacion != objDetallesCategoria.Count())
                {
                    errorControlado = true;
                    throw new Exception(Errores.CamposIncompletos);
                }

                pIndicador.nuevoEstado = (int)EstadosRegistro.Activo;

                return CambioEstado(pIndicador); // reutilizar la función de cambio de estado
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 29/08/2022
        /// José Navarro Acuña
        /// Función que valida los datos de un indicador antes de insertar o actualizar
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> ValidarDatos(Indicador pIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            resultado.HayError = (int)Error.NoError;
            bool errorControlado = false;

            try
            {
                // validar la existencia del indicador por medio del nombre y/o código
                Indicador indicadorExistente = indicadorFonatelDAL.VerificarExistenciaIndicadorPorCodigoNombre(pIndicador);
                if (indicadorExistente != null) {

                    if (indicadorExistente.Codigo.Trim().ToUpper().Equals(pIndicador.Codigo.Trim().ToUpper()))
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CodigoRegistrado, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelCodigo));
                    }
                    else
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.NombreRegistrado, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelNombre));
                    }
                }

                // validar si el valor selecionado en los comboboxes existe y se encuentra habilitado
                if ((pIndicador.esGuardadoParcial && pIndicador.TipoIndicadores.IdTipoIndicador != defaultDropDownValue) || !pIndicador.esGuardadoParcial)
                {
                    if (tipoIndicadorDAL.ObtenerDatos(pIndicador.TipoIndicadores).Count <= 0) 
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelTipo));
                    }
                }

                if ((pIndicador.esGuardadoParcial && pIndicador.FrecuenciaEnvio.idFrecuenciaEnvio != defaultDropDownValue) || !pIndicador.esGuardadoParcial)
                {
                    if (frecuenciaEnvioDAL.ObtenerDatos(pIndicador.FrecuenciaEnvio).Count <= 0)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelFrecuencias));
                    }
                }

                if ((pIndicador.esGuardadoParcial && pIndicador.ClasificacionIndicadores.idClasificacion != defaultDropDownValue) || !pIndicador.esGuardadoParcial)
                {
                    if (clasificacionIndicadorDAL.ObtenerDatos(pIndicador.ClasificacionIndicadores).Count <= 0)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelClasificacion));
                    }
                }

                if ((pIndicador.esGuardadoParcial && pIndicador.TipoMedida.idMedida != defaultDropDownValue) || !pIndicador.esGuardadoParcial)
                {
                    if (tipoMedidaDAL.ObtenerDatos(pIndicador.TipoMedida).Count <= 0)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelTipoMedida));
                    }
                }

                if ((pIndicador.esGuardadoParcial && pIndicador.GrupoIndicadores.idGrupo != defaultDropDownValue) || !pIndicador.esGuardadoParcial)
                {
                    if (grupoIndicadorDAL.ObtenerDatos(pIndicador.GrupoIndicadores).Count <= 0)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelGrupo));
                    }
                }

                if ((pIndicador.esGuardadoParcial && pIndicador.UnidadEstudio.idUnidad != defaultDropDownValue) || !pIndicador.esGuardadoParcial)
                {
                    if (unidadEstudioDAL.ObtenerDatos(pIndicador.UnidadEstudio).Count <= 0)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelUnidadEstudio));
                    }
                }

                // validar la cantidad de variables dato y categorias establecidas en el indicador según lo registrado respectivamente en cada detalle, sucede en caso de actualizar
                if (pIndicador.idIndicador != 0)
                {

                    Indicador indicadorRegistradoActualmente = indicadorFonatelDAL.ObtenerDatos(new Indicador() { idIndicador = pIndicador.idIndicador }).FirstOrDefault();
                    List<DetalleIndicadorVariables> ListaVariables = detalleIndicadorVariablesDAL.ObtenerDatos(new DetalleIndicadorVariables() { idIndicador = pIndicador.idIndicador});
                    List<DetalleIndicadorCategoria> ListaCategorias = detalleIndicadorCategoriaDAL.ObtenerDatos(new DetalleIndicadorCategoria() { idIndicador = pIndicador.idIndicador, DetallesAgrupados = true });

                    if (indicadorRegistradoActualmente != null)
                    {
                        if (pIndicador.CantidadVariableDato != null && indicadorRegistradoActualmente.CantidadVariableDato != null)
                        {
                            if (pIndicador.CantidadVariableDato < ListaVariables.Count) // la nueva cantidad registrada debe ser mayor o igual a la actual
                            {
                                errorControlado = true;
                                throw new Exception(string.Format(Errores.CampoConValorMenorAlActual, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelCantidadVariableDatosIndicador));
                            }
                        }

                        if (pIndicador.CantidadCategoriasDesagregacion != null && indicadorRegistradoActualmente.CantidadCategoriasDesagregacion != null)
                        {
                            if (pIndicador.CantidadCategoriasDesagregacion < ListaCategorias.Count)
                            {
                                errorControlado = true;
                                throw new Exception(string.Format(Errores.CampoConValorMenorAlActual, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelCantidadCategoriaIndicador));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }

            return resultado;
        }

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores registrados en el sistema.
        /// Se puede realizar un filtrado de acuerdo al objeto que se envia.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> ObtenerDatos(Indicador pIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            try
            {
                PrepararObjetoIndicador(pIndicador);
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = indicadorFonatelDAL.ObtenerDatos(pIndicador);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        /// <summary>
        /// 19/12/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores de mercado registrados en el sistema
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <param name="pServicioSitel"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> ObtenerDatosMercado(Indicador pIndicador, ServicioSitel pServicioSitel)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            try
            {
                pIndicador.GrupoIndicadores.Nombre = pIndicador.GrupoIndicadores.id;
                pIndicador.GrupoIndicadores.id = string.Empty; // en la vista de mercados no tenemos id de agrupación

                PrepararObjetoIndicador(pIndicador);

                if (!string.IsNullOrEmpty(pServicioSitel?.id))
                {
                    int.TryParse(Utilidades.Desencriptar(pServicioSitel.id), out int number);
                    pServicioSitel.IdServicio = number;
                }

                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = indicadorFonatelDAL.ObtenerDatosMercado(pIndicador, pServicioSitel);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        /// <summary>
        /// 20/12/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores de calidad
        /// Función
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <param name="pServicioSitel"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> ObtenerDatosCalidad(Indicador pIndicador, ServicioSitel pServicioSitel)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            try
            {
                pIndicador.GrupoIndicadores.Nombre = pIndicador.GrupoIndicadores.id;
                pIndicador.GrupoIndicadores.id = string.Empty; // en la vista de mercados no tenemos id de agrupación

                PrepararObjetoIndicador(pIndicador);

                if (!string.IsNullOrEmpty(pServicioSitel?.id))
                {
                    int.TryParse(Utilidades.Desencriptar(pServicioSitel.id), out int number);
                    pServicioSitel.IdServicio = number;
                }

                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = indicadorFonatelDAL.ObtenerDatosCalidad(pIndicador, pServicioSitel);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        /// <summary>
        /// 20/12/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores de UIT
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <param name="pServicioSitel"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> ObtenerDatosUIT(Indicador pIndicador, ServicioSitel pServicioSitel)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            try
            {
                pIndicador.GrupoIndicadores.Nombre = pIndicador.GrupoIndicadores.id;
                pIndicador.GrupoIndicadores.id = string.Empty; // en la vista de UIT no tenemos id de agrupación

                PrepararObjetoIndicador(pIndicador);

                if (!string.IsNullOrEmpty(pServicioSitel?.id))
                {
                    int.TryParse(Utilidades.Desencriptar(pServicioSitel.id), out int number);
                    pServicioSitel.IdServicio = number;
                }

                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = indicadorFonatelDAL.ObtenerDatosUIT(pIndicador, pServicioSitel);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }

            return resultado;
        }

        /// <summary>
        /// 20/12/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores cruzados
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <param name="pServicioSitel"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> ObtenerDatosCruzado(Indicador pIndicador, ServicioSitel pServicioSitel)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            try
            {
                pIndicador.GrupoIndicadores.Nombre = pIndicador.GrupoIndicadores.id;
                pIndicador.GrupoIndicadores.id = string.Empty; // en la vista de UIT no tenemos id de agrupación

                PrepararObjetoIndicador(pIndicador);

                if (!string.IsNullOrEmpty(pServicioSitel?.id))
                {
                    int.TryParse(Utilidades.Desencriptar(pServicioSitel.id), out int number);
                    pServicioSitel.IdServicio = number;
                }

                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = indicadorFonatelDAL.ObtenerDatosCruzados(pIndicador, pServicioSitel);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }

            return resultado;
        }

        /// <summary>
        /// 19/12/2022
        /// José Navarro Acuña
        /// Función que permite obtener los indicadores provientes de fuente externa
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> ObtenerDatosFuenteExterna()
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = indicadorFonatelDAL.ObtenerDatosFuenteExterna();
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }

            return resultado;
        }

        /// <summary>
        /// 23/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los usos de indicador disponibles.
        /// Al no ser entidades catálogo en BD, se manejan a nivel de BL
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<EstadoLogico>> ObtenerListaUsosIndicador()
        {
            RespuestaConsulta<List<EstadoLogico>> resultado = new RespuestaConsulta<List<EstadoLogico>>();
            List<EstadoLogico> listado = new List<EstadoLogico> { 
                new EstadoLogico() { Nombre = UsosIndicador.interno, Valor = true },
                new EstadoLogico() { Nombre = UsosIndicador.externo, Valor = false } 
            };

            resultado.Clase = modulo;
            resultado.Accion = (int)Accion.Consultar;
            resultado.objetoRespuesta = listado;
            resultado.CantidadRegistros = listado.Count;
            return resultado;
        }

        /// <summary>
        /// 23/08/2022
        /// José Navarro Acuña
        /// Función que retorna las opciones disponibles si mostar el indicador o no
        /// Al no ser entidades catálogo en BD, se manejan a nivel de BL
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<EstadoLogico>> ObtenerListaMostrarIndicadorEnSolicitud()
        {
            RespuestaConsulta<List<EstadoLogico>> resultado = new RespuestaConsulta<List<EstadoLogico>>();
            List<EstadoLogico> listado = new List<EstadoLogico> {
                new EstadoLogico() { Nombre = MostrarIndicadorEnSolicitud.si, Valor = true },
                new EstadoLogico() { Nombre = MostrarIndicadorEnSolicitud.no, Valor = false }
            };

            resultado.Clase = modulo;
            resultado.Accion = (int)Accion.Consultar;
            resultado.objetoRespuesta = listado;
            resultado.CantidadRegistros = listado.Count;
            return resultado;
        }

        /// <summary>
        /// 03/10/2022
        /// José Navarro Acuña
        /// Función que permite clonar los detalles variables dato y detalles categoria de un indicador hacia otro indicador
        /// </summary>
        /// <param name="pIdIndicadorAClonar"></param>
        /// <param name="pIdIndicadorDestino"></param>
        /// <returns></returns>
        public RespuestaConsulta<Indicador> ClonarDetallesDeIndicador(string pIdIndicadorAClonar, string pIdIndicadorDestino)
        {
            RespuestaConsulta<Indicador> resultado = new RespuestaConsulta<Indicador>();
            int idIndicadorAClonar, idIndicadorDestino;
            bool errorControlado = false;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pIdIndicadorAClonar), out int number);
                idIndicadorAClonar = number;

                if (idIndicadorAClonar == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                int.TryParse(Utilidades.Desencriptar(pIdIndicadorDestino), out number);
                idIndicadorDestino = number;

                if (idIndicadorDestino == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                indicadorFonatelDAL.ClonarDetallesDeIndicador(idIndicadorAClonar, idIndicadorDestino);

                var objeto = new Indicador() { id = pIdIndicadorDestino };
                resultado.objetoRespuesta = objeto;

                resultado.Usuario = user;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Clonar;

                var objetoNuevo = indicadorFonatelDAL.ObtenerDatos(objeto).Where(x => x.idIndicador == idIndicadorDestino).Single();

                objeto = indicadorFonatelDAL.ObtenerDatos(objeto).Where(x => x.idIndicador == idIndicadorAClonar).Single();

                string jsonValorInicial = objeto.ToString();
                string jsonClonado = objetoNuevo.ToString();

                indicadorFonatelDAL.RegistrarBitacora(resultado.Accion,
                            resultado.Usuario,
                                resultado.Clase, objeto.Codigo, jsonClonado, "",jsonValorInicial );
                //indicadorFonatelDAL.RegistrarBitacora(resultado.Accion,
                //        resultado.Usuario, resultado.Clase, idIndicadorDestino.ToString());
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// Michael Hernandez
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> PublicacionSigitel(Indicador pIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();
            bool errorControlado = false;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.id), out int number);
                pIndicador.idIndicador = number;
                // actualizar el estado del indicador
                PrepararObjetoIndicador(pIndicador);
                pIndicador.UsuarioModificacion = user;
                var indicadorActualizado = indicadorFonatelDAL.PublicacionSigitel(pIndicador);

                if (indicadorActualizado.Count() <= 0) // ¿actualizó correctamente?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }


                resultado.Accion = pIndicador.VisualizaSigitel == true ? (int)Accion.Publicado : (int)Accion.NoPublicado;
                resultado.Clase = modulo;
                resultado.Usuario = user;
                resultado.CantidadRegistros = indicadorActualizado.Count();

                indicadorFonatelDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario, resultado.Clase, pIndicador.Codigo);
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 05/10/2022
        /// José Navarro Acuña
        /// Función que verifica si todos los campos de un indicador estan completos.
        /// Nota: es un método estático.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public static string VerificarDatosCompletosIndicador(Indicador pIndicador)
        {
            if (
                pIndicador.Codigo == null || string.IsNullOrEmpty(pIndicador.Codigo.Trim()) ||
                pIndicador.Nombre == null || string.IsNullOrEmpty(pIndicador.Nombre.Trim()) ||
                pIndicador.IdTipoIndicador == 0 || pIndicador.IdTipoIndicador == defaultDropDownValue ||
                pIndicador.IdClasificacion == 0 || pIndicador.IdClasificacion == defaultDropDownValue ||
                pIndicador.idGrupo == 0 || pIndicador.idGrupo == defaultDropDownValue ||
                pIndicador.Descripcion == null || string.IsNullOrEmpty(pIndicador.Descripcion.Trim()) ||
                pIndicador.CantidadVariableDato == null ||
                pIndicador.CantidadCategoriasDesagregacion == null ||
                pIndicador.IdUnidadEstudio == null || pIndicador.IdUnidadEstudio == defaultDropDownValue ||
                pIndicador.idTipoMedida == 0 || pIndicador.idTipoMedida == defaultDropDownValue ||
                pIndicador.IdFrecuencia == 0 || pIndicador.IdFrecuencia == defaultDropDownValue ||
                pIndicador.Interno == null ||
                pIndicador.Solicitud == null ||
                pIndicador.Fuente == null || string.IsNullOrEmpty(pIndicador.Fuente.Trim()) ||
                pIndicador.Notas == null || string.IsNullOrEmpty(pIndicador.Notas.Trim())
                )
            {
                return Errores.CamposIncompletos;
            }
            return string.Empty;
        }

        /// <summary>
        /// 23/08/2022
        /// José Navarro Acuña
        /// Prepara un objeto indicador para ser enviado al servicio DAL.
        /// Se preparan los id's de las tablas relacionadas para poder efectuar consultas debido a la encriptación.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        private void PrepararObjetoIndicador(Indicador pIndicador)
        {
            if (!string.IsNullOrEmpty(pIndicador.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.id), out int number);
                pIndicador.idIndicador = number;
            }

            if (!string.IsNullOrEmpty(pIndicador.TipoIndicadores?.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.TipoIndicadores.id), out int number);
                pIndicador.IdTipoIndicador = number;
                pIndicador.TipoIndicadores.IdTipoIndicador = pIndicador.TipoIndicadores != null ? number : 0;
            }
            
            if (!string.IsNullOrEmpty(pIndicador.ClasificacionIndicadores?.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.ClasificacionIndicadores.id), out int number);
                pIndicador.IdClasificacion = number;
                pIndicador.ClasificacionIndicadores.idClasificacion = pIndicador.ClasificacionIndicadores != null ? number : 0;
            }
            
            if (!string.IsNullOrEmpty(pIndicador.GrupoIndicadores?.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.GrupoIndicadores.id), out int number);
                pIndicador.idGrupo = number;
                pIndicador.GrupoIndicadores.idGrupo = pIndicador.GrupoIndicadores != null ? number: 0;
            }
            
            if (!string.IsNullOrEmpty(pIndicador.UnidadEstudio?.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.UnidadEstudio.id), out int number);
                pIndicador.IdUnidadEstudio = number;
                pIndicador.UnidadEstudio.idUnidad = pIndicador.UnidadEstudio != null ? number : 0;
            }

            if (!string.IsNullOrEmpty(pIndicador.TipoMedida?.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.TipoMedida.id), out int number);
                pIndicador.idTipoMedida = number;
                pIndicador.TipoMedida.idMedida = pIndicador.TipoMedida != null ? number : 0;
            }

            if (!string.IsNullOrEmpty(pIndicador.FrecuenciaEnvio?.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.FrecuenciaEnvio.id), out int number);
                pIndicador.IdFrecuencia = number;
                pIndicador.FrecuenciaEnvio.idFrecuenciaEnvio = pIndicador.FrecuenciaEnvio != null ? number : 0;
            }
        }

        /// <summary>
        /// 11/10/2022
        /// José Navarro Acuña
        /// Permite cambiar de estado todas las dependencias que se encuentren asociadas al indicador proporcionado
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        private bool CambioEstadoDependenciasIndicador(Indicador pIndicador, EstadosRegistro pNuevoEstado)
        {
            List<FormularioWeb> listaFormularioWeb = formularioWebDAL.ObtenerDependenciasIndicadorConFormulariosWeb(pIndicador.idIndicador);

            foreach (FormularioWeb formulario in listaFormularioWeb)
            {
                formulario.idEstadoRegistro = (int)pNuevoEstado;
                formulario.UsuarioModificacion = user;
                formularioWebDAL.ActualizarDatos(formulario);
            }
            
            List<FormulasCalculo> listaFormulas = formulasCalculoDAL.ObtenerDependenciasIndicadorConFormulasCalculo(pIndicador.idIndicador);

            foreach (FormulasCalculo formula in listaFormulas)
            {
                formula.IdEstado = (int)pNuevoEstado;
                formula.UsuarioModificacion = user;
                formulasCalculoDAL.ActualizarDatos(formula);
            }

            return true;
        }
    }
}
