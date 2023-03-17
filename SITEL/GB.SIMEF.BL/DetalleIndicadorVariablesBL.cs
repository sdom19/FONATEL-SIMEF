using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class DetalleIndicadorVariablesBL : IMetodos<DetalleIndicadorVariable>
    {
        private readonly string modulo = "";
        private readonly string user = "";
        private readonly DetalleIndicadorVariablesDAL detalleIndicadorVariablesDAL;
        private readonly IndicadorFonatelDAL indicadorFonatelDAL;

        public DetalleIndicadorVariablesBL(string pView, string pUser)
        {
            modulo = pView;
            user = pUser;
            detalleIndicadorVariablesDAL = new DetalleIndicadorVariablesDAL();
            indicadorFonatelDAL = new IndicadorFonatelDAL();
        }

        /// <summary>
        /// 14/09/2022
        /// José Navarro Acuña
        /// Función que permite actualizar un detalle de variable dato de un indicador
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariable>> ActualizarElemento(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>();
            bool errorControlado = false;

            try
            {
                RespuestaConsulta<List<DetalleIndicadorVariable>> detalleRegistrado = ObtenerDatos(pDetalleIndicadorVariables);

                if (detalleRegistrado.HayError != (int) Error.NoError)
                {
                    return detalleRegistrado;
                }

                if (detalleRegistrado.objetoRespuesta.Count == 0) // el detalle existe?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                resultado = ValidarDatos(pDetalleIndicadorVariables);

                if (resultado.HayError != (int)Error.NoError)
                {
                    return resultado;
                }
                pDetalleIndicadorVariables.Estado = true;
                var objetoAnterior = detalleIndicadorVariablesDAL.ObtenerDatos(pDetalleIndicadorVariables).Single();
                resultado.objetoRespuesta = detalleIndicadorVariablesDAL.ActualizarDatos(pDetalleIndicadorVariables);

                resultado.Usuario = user;
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Editar;

                var objeto = detalleIndicadorVariablesDAL.ObtenerDatos(pDetalleIndicadorVariables).Single();
                string JsonActual = objeto.ToString();
                string JsonAnterior = objetoAnterior.ToString();

                detalleIndicadorVariablesDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario,
                            resultado.Clase, objeto.IdDetalleIndicadorVariable.ToString()
                            , JsonActual, JsonAnterior, "");
                //indicadorFonatelDAL.RegistrarBitacora(resultado.Accion,
                //        resultado.Usuario, resultado.Clase, pDetalleIndicadorVariables.NombreVariable);
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
        /// 15/09/2022
        /// José Navarro Acuña
        /// Función que permite cambiar el estado de un detalle de variable dato de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariable>> CambioEstado(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>();
            bool errorControlado = false;

            try
            {
                RespuestaConsulta<List<DetalleIndicadorVariable>> detalleRegistrado = ObtenerDatos(pDetalleIndicadorVariables);

                if (detalleRegistrado.HayError != (int)Error.NoError)
                {
                    return detalleRegistrado;
                }

                if (detalleRegistrado.objetoRespuesta.Count == 0) // el detalle existe?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // actualizar el estado
                DetalleIndicadorVariable detalle = detalleRegistrado.objetoRespuesta[0];
                detalle.Estado = pDetalleIndicadorVariables.Estado;
                detalle.IdDetalleIndicadorVariable = pDetalleIndicadorVariables.IdDetalleIndicadorVariable;
                detalle.IdIndicador = pDetalleIndicadorVariables.IdIndicador;
                resultado.objetoRespuesta = detalleIndicadorVariablesDAL.ActualizarDatos(detalle);

                resultado.Usuario = user;
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Eliminar;

                indicadorFonatelDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario, resultado.Clase, detalle.NombreVariable);
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

        public RespuestaConsulta<List<DetalleIndicadorVariable>> ClonarDatos(DetalleIndicadorVariable objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariable>> EliminarElemento(DetalleIndicadorVariable objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 01/09/2022
        /// José Navarro Acuña
        /// Función que permite registrar un nuevo detalle de variable dato a un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariable>> InsertarDatos(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>();
            bool errorControlado = false;

            try
            {
                PrepararObjetoDetalle(pDetalleIndicadorVariables);
                resultado = ValidarDatos(pDetalleIndicadorVariables);

                if (resultado.HayError != 0)
                {
                    return resultado;
                }

                pDetalleIndicadorVariables.Estado = true;
                var objetoDetalle = detalleIndicadorVariablesDAL.ActualizarDatos(pDetalleIndicadorVariables);
                resultado.objetoRespuesta = objetoDetalle;

                resultado.Usuario = user;
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;

                var objetoDetalleBitacora = objetoDetalle.Where(x => x.NombreVariable == pDetalleIndicadorVariables.NombreVariable && x.Descripcion == pDetalleIndicadorVariables.Descripcion).Single();
                string JsonInicial = objetoDetalleBitacora.ToString();

                detalleIndicadorVariablesDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario, resultado.Clase, pDetalleIndicadorVariables.IdDetalleIndicadorVariable.ToString(),"","",JsonInicial);
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
        /// 01/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles variables dato. Según el objeto que se envia se pueden realizar filtros por IDs
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariable>> ObtenerDatos(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>> {
                HayError = (int)Error.NoError,
            };

            try
            {
                PrepararObjetoDetalle(pDetalleIndicadorVariables);
                resultado.objetoRespuesta = detalleIndicadorVariablesDAL.ObtenerDatos(pDetalleIndicadorVariables).ToList();
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;
                resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 14/02/2023
        /// José Navarro Acuña
        /// Función que permite obtener los detalles variables dato que no estan siendo utilizados por fórmulas de cálculo
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <param name="pIdFormula"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariable>> ObtenerVariablesSinUsoEnFormula(DetalleIndicadorVariable pDetalleIndicadorVariables, string pIdFormula)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>
            {
                HayError = (int)Error.NoError,
            };

            try
            {
                PrepararObjetoDetalle(pDetalleIndicadorVariables);

                if (pDetalleIndicadorVariables.IdIndicador == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                int idFormula = 0;
                if (!string.IsNullOrEmpty(pIdFormula))
                {
                    int.TryParse(Utilidades.Desencriptar(pIdFormula), out int number);
                    idFormula = number;
                }

                resultado.objetoRespuesta = detalleIndicadorVariablesDAL.ObtenerVariablesSinUsoEnFormula(pDetalleIndicadorVariables, idFormula).ToList();
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;
                resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 12/09/2022
        /// José Navarro Acuña
        /// Función que valida el objeto DetalleIndicadorVariables. Valida la existencia del indicador relacionado y la cantidad de detalles restantes del mismo
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariable>> ValidarDatos(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>
            {
                HayError = (int)Error.NoError,
                objetoRespuesta = new List<DetalleIndicadorVariable>()
            };
            bool errorControlado = false;

            try
            {
                // validar si el indicador existe
                Indicador indicadorExistente = indicadorFonatelDAL.VerificarExistenciaIndicadorPorID(pDetalleIndicadorVariables.IdIndicador);

                if (indicadorExistente == null)
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // validar que el indicador tenga sus datos completos
                string msgIndicadorCompleto = IndicadorFonatelBL.VerificarDatosCompletosIndicador(indicadorExistente);

                if (!string.IsNullOrEmpty(msgIndicadorCompleto))
                {
                    errorControlado = true;
                    throw new Exception(msgIndicadorCompleto);
                }

                RespuestaConsulta<List<DetalleIndicadorVariable>> detallesActuales = ObtenerDatos(new DetalleIndicadorVariable() { 
                    IdIndicador = pDetalleIndicadorVariables.IdIndicador 
                });

                if (detallesActuales.HayError != (int)Error.NoError)
                {
                    errorControlado = true;
                    throw new Exception(detallesActuales.MensajeError);
                }

                // validar si existe un detalle con nombre igual
                for (int i = 0; i < detallesActuales.objetoRespuesta.Count; i++)
                {
                    if (detallesActuales.objetoRespuesta[i].NombreVariable.ToUpper().Trim().Equals(pDetalleIndicadorVariables.NombreVariable.ToUpper().Trim()) 
                        && !detallesActuales.objetoRespuesta[i].id.Equals(pDetalleIndicadorVariables.id)) // en caso de ser modo edición, obviar el propio elemento
                    {
                        errorControlado = true;
                        throw new Exception(Errores.NombreRegistradoVariableDato);
                    }
                }

                bool modoEdicion = detallesActuales.objetoRespuesta.Exists(x => x.id == pDetalleIndicadorVariables.id);

                if (!modoEdicion) // modo creación
                {
                    // validar la cantidad de detalles registrados actualmente: ¿se supera la cantidad establecida en el indicador?
                    if (detallesActuales.CantidadRegistros > indicadorExistente.CantidadVariableDato)
                    {
                        errorControlado = true;
                        throw new Exception(Errores.CantidadRegistros);
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

            resultado.objetoRespuesta.Add(pDetalleIndicadorVariables);
            return resultado;
        }

        /// <summary>
        /// 12/09/2022
        /// José Navarro Acuña
        /// Prepara un objeto detalle variable para ser enviado al servicio DAL.
        /// Se preparan los id's de las tablas relacionadas para poder efectuar consultas debido a la encriptación.
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        private void PrepararObjetoDetalle(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.id))
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorVariables.id), out int number);
                pDetalleIndicadorVariables.IdDetalleIndicadorVariable = number;
            }

            if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.idIndicadorString))
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorVariables.idIndicadorString), out int number);
                pDetalleIndicadorVariables.IdIndicador = number;
            }
        }
    }
}
