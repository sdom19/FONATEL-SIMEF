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
    public class DetalleIndicadorVariablesBL : IMetodos<DetalleIndicadorVariables>
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
        public RespuestaConsulta<List<DetalleIndicadorVariables>> ActualizarElemento(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();
            bool errorControlado = false;

            try
            {
                RespuestaConsulta<List<DetalleIndicadorVariables>> detalleRegistrado = ObtenerDatos(pDetalleIndicadorVariables);

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
                resultado.objetoRespuesta = detalleIndicadorVariablesDAL.ActualizarDatos(pDetalleIndicadorVariables);

                resultado.Usuario = user;
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Editar;

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
        public RespuestaConsulta<List<DetalleIndicadorVariables>> CambioEstado(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();
            bool errorControlado = false;

            try
            {
                RespuestaConsulta<List<DetalleIndicadorVariables>> detalleRegistrado = ObtenerDatos(pDetalleIndicadorVariables);

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
                DetalleIndicadorVariables detalle = detalleRegistrado.objetoRespuesta[0];
                detalle.Estado = pDetalleIndicadorVariables.Estado;
                detalle.idDetalleIndicador = pDetalleIndicadorVariables.idDetalleIndicador;
                detalle.idIndicador = pDetalleIndicadorVariables.idIndicador;
                resultado.objetoRespuesta = detalleIndicadorVariablesDAL.ActualizarDatos(detalle);

                // actualizar el indicador
                Indicador indicadorDelDetalle = indicadorFonatelDAL.ObtenerDatos(new Indicador() { idIndicador = pDetalleIndicadorVariables.idIndicador }).FirstOrDefault();
                indicadorDelDetalle.CantidadVariableDato--;
                indicadorFonatelDAL.ActualizarCantidadVariablesDato(indicadorDelDetalle);

                resultado.Usuario = user;
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Eliminar;

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

        public RespuestaConsulta<List<DetalleIndicadorVariables>> ClonarDatos(DetalleIndicadorVariables objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariables>> EliminarElemento(DetalleIndicadorVariables objeto)
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
        public RespuestaConsulta<List<DetalleIndicadorVariables>> InsertarDatos(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();
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
                resultado.objetoRespuesta = detalleIndicadorVariablesDAL.ActualizarDatos(pDetalleIndicadorVariables);

                resultado.Usuario = user;
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;

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
        /// 01/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles variables dato. Según el objeto que se envia se pueden realizar filtros por IDs
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariables>> ObtenerDatos(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>> {
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
        /// 12/09/2022
        /// José Navarro Acuña
        /// Función que valida el objeto DetalleIndicadorVariables. Valida la existencia del indicador relacionado y la cantidad de detalles restantes del mismo
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariables>> ValidarDatos(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>
            {
                HayError = (int)Error.NoError,
                objetoRespuesta = new List<DetalleIndicadorVariables>()
            };
            bool errorControlado = false;
            Indicador indicadorExistente = null;

            try
            {
                // validar si el indicador existe
                indicadorExistente = indicadorFonatelDAL.VerificarExistenciaIndicadorPorID(pDetalleIndicadorVariables.idIndicador);

                if (indicadorExistente == null)
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // validar la cantidad de detalles registrados actualmente
                RespuestaConsulta<List<DetalleIndicadorVariables>> detallesActuales = ObtenerDatos(pDetalleIndicadorVariables);

                if (detallesActuales.HayError != (int)Error.NoError)
                {
                    errorControlado = true;
                    throw new Exception(detallesActuales.MensajeError);
                }

                bool modoEdicion = detallesActuales.objetoRespuesta.Exists(x => x.id == pDetalleIndicadorVariables.id);

                if (!modoEdicion) // solo en modo creación se realiza la validación de la cantidad
                {
                    if (detallesActuales.CantidadRegistros + 1 > indicadorExistente.CantidadVariableDato)
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
        private void PrepararObjetoDetalle(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.id))
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorVariables.id), out int number);
                pDetalleIndicadorVariables.idDetalleIndicador = number;
            }

            if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.idIndicadorString))
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorVariables.idIndicadorString), out int number);
                pDetalleIndicadorVariables.idIndicador = number;
            }
        }
    }
}
