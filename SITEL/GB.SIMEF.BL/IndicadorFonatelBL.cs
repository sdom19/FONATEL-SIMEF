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
        private readonly string modulo = "";
        private readonly string user = "";
        private readonly IndicadorFonatelDAL indicadorFonatelDAL;
        private readonly TipoIndicadorDAL tipoIndicadorDAL;
        private readonly FrecuenciaEnvioDAL frecuenciaEnvioDAL;
        private readonly ClasificacionIndicadorDAL clasificacionIndicadorDAL;
        private readonly TipoMedidaDAL tipoMedidaDAL;
        private readonly GrupoIndicadorDAL grupoIndicadorDAL;
        private readonly UnidadEstudioDAL unidadEstudioDAL;

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
        }

        public RespuestaConsulta<List<Indicador>> ActualizarElemento(Indicador pIndicador)
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


                resultado.Accion = pIndicador.VisualizaSigitel == true? (int)Accion.Publicado:(int)Accion.NoPublicado;
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

                pIndicador = indicadorFonatelDAL.ObtenerDatos(pIndicador).Single();

                // actualizar el estado del indicador
                PrepararObjetoIndicador(pIndicador);
                pIndicador.UsuarioModificacion = user;
                pIndicador.idEstado = nuevoEstado;
                var indicadorActualizado = indicadorFonatelDAL.ActualizarDatos(pIndicador);

                if (indicadorActualizado.Count() <= 0) // ¿actualizó correctamente?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

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
                }

                resultado.Accion = accion;
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

                pIndicador = indicadorFonatelDAL.ObtenerDatos(pIndicador).Single();

                PrepararObjetoIndicador(pIndicador);
                var result = indicadorFonatelDAL.VerificarUsoIndicador(pIndicador);
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
                if (pIndicador.Modo == 5)//clonar
                {
                    pIndicador.idIndicador = 0;
                }
                if (pIndicador.esGuardadoParcial == false)
                {
                    resultado = ValidarDatos(pIndicador);
                    if (resultado.HayError != 0)
                    {
                        return resultado;
                    }
                }
                pIndicador.UsuarioCreacion = user;
                resultado.objetoRespuesta = indicadorFonatelDAL.ActualizarDatos(pIndicador);
                
                resultado.Usuario = user;
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;

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
                Indicador indicadorExistente = indicadorFonatelDAL.VerificarExistenciaIndicador(pIndicador);
                if (indicadorExistente != null) {
                    errorControlado = true;

                    if (indicadorExistente.Codigo.Trim().ToUpper().Equals(pIndicador.Codigo.Trim().ToUpper()))
                    {
                        throw new Exception(string.Format(Errores.CodigoRegistrado, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelCodigo));
                    }
                    else
                    {
                        throw new Exception(string.Format(Errores.NombreRegistrado, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelNombre));
                    }
                }

                if ((pIndicador.esGuardadoParcial && pIndicador.TipoIndicadores.IdTipoIdicador != defaultDropDownValue) || !pIndicador.esGuardadoParcial)
                {
                    if (tipoIndicadorDAL.ObtenerDatos(pIndicador.TipoIndicadores).Count <= 0)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelTipo));
                    }
                }

                if ((pIndicador.esGuardadoParcial && pIndicador.FrecuenciaEnvio.idFrecuencia != defaultDropDownValue) || !pIndicador.esGuardadoParcial)
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
        /// Se puede realizar un filtrado de acuerdo al objecto que se envia.
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
                resultado.HayError = (int)Constantes.Error.ErrorSistema;
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
                pIndicador.TipoIndicadores.IdTipoIdicador = pIndicador.TipoIndicadores != null ? number : 0;
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
                pIndicador.FrecuenciaEnvio.idFrecuencia = pIndicador.FrecuenciaEnvio != null ? number : 0;
            }
        }
    }
}
