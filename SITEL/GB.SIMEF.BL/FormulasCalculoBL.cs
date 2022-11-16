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
    public class FormulasCalculoBL : IMetodos<FormulasCalculo>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly FormulasCalculoDAL formulasCalculoDAL;
        private readonly IndicadorFonatelDAL indicadorFonatelDAL;
        private readonly FrecuenciaEnvioDAL frecuenciaEnvioDAL;
        private readonly DetalleIndicadorVariablesDAL detalleIndicadorVariablesDAL;
        private readonly FormulaNivelCalculoCategoriaDAL formulaNivelCalculoCategoriaDAL;

        public FormulasCalculoBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            formulasCalculoDAL = new FormulasCalculoDAL();
            indicadorFonatelDAL = new IndicadorFonatelDAL();
            frecuenciaEnvioDAL = new FrecuenciaEnvioDAL();
            detalleIndicadorVariablesDAL = new DetalleIndicadorVariablesDAL();
            formulaNivelCalculoCategoriaDAL = new FormulaNivelCalculoCategoriaDAL();
        }

        public RespuestaConsulta<List<FormulasCalculo>> ActualizarElemento(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Michael Hernández C
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulasCalculo>> CambioEstado(FormulasCalculo objeto)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultadoConsulta = new RespuestaConsulta<List<FormulasCalculo>>();

            try
            {
                resultadoConsulta.Clase = modulo;
                objeto.UsuarioModificacion = user;
                resultadoConsulta.Accion = objeto.IdEstado;
                resultadoConsulta.Usuario = user;
                List<FormulasCalculo> resul = formulasCalculoDAL.ActualizarDatos(objeto);
                resultadoConsulta.objetoRespuesta = resul;
                resultadoConsulta.CantidadRegistros = resul.Count();

                formulasCalculoDAL.RegistrarBitacora(resultadoConsulta.Accion,
                        resultadoConsulta.Usuario,
                            resultadoConsulta.Clase, objeto.Codigo);

            }
            catch (Exception ex)
            {
                resultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                resultadoConsulta.MensajeError = ex.Message;
            }
            return resultadoConsulta;
        }

        public RespuestaConsulta<List<FormulasCalculo>> ClonarDatos(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Michael Hernández C
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulasCalculo>> EliminarElemento(FormulasCalculo objeto)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultadoConsulta = new RespuestaConsulta<List<FormulasCalculo>>();

            try
            {
                resultadoConsulta.Clase = modulo;
                objeto.UsuarioModificacion = user;
                resultadoConsulta.Accion = (int)EstadosRegistro.Eliminado;
                resultadoConsulta.Usuario = user;
                List<FormulasCalculo> resul = formulasCalculoDAL.ActualizarDatos(objeto);
                resultadoConsulta.objetoRespuesta = resul;
                resultadoConsulta.CantidadRegistros = resul.Count();

                formulasCalculoDAL.RegistrarBitacora(resultadoConsulta.Accion,
                        resultadoConsulta.Usuario,
                            resultadoConsulta.Clase, objeto.Codigo);

            }
            catch (Exception ex)
            {
                resultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                resultadoConsulta.MensajeError = ex.Message;
            }
            return resultadoConsulta;
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Función que crea un nuevo registro en la entidad Fórmula de Cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulasCalculo>> InsertarDatos(FormulasCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();
            bool errorControlado = false;

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculo);
                resultado = ValidarDatos(pFormulasCalculo);

                if (resultado.HayError != 0)
                {
                    return resultado;
                }

                List<FormulasCalculo> formulaCalculo = formulasCalculoDAL.ActualizarDatos(pFormulasCalculo);

                // en este punto tenemos la fórmula creada/actualizada
                // eliminar las categorias del nivel de cálculo
                formulaNivelCalculoCategoriaDAL.EliminarFormulaNivelCalculoCategoriaPorIDFormula(formulaCalculo[0].IdFormula);

                // el indicador fue proporcionado, marcada la opcion de categorias y se incluye el listado de categorias?
                if (pFormulasCalculo.IdIndicador != 0 && pFormulasCalculo.ListaCategoriasNivelesCalculo.Count > 0 && !pFormulasCalculo.NivelCalculoTotal)
                {
                    formulaNivelCalculoCategoriaDAL.InsertarFormulaNivelCalculoCategoria(formulaCalculo[0].IdFormula, pFormulasCalculo.ListaCategoriasNivelesCalculo);
                }

                formulaCalculo[0].IdFormula = 0;
                resultado.objetoRespuesta = formulaCalculo;
                resultado.Usuario = user;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;

                formulasCalculoDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario, resultado.Clase, pFormulasCalculo.Codigo);
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
        /// Michael Hernández C
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulasCalculo>> ObtenerDatos(FormulasCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultadoConsulta = new RespuestaConsulta<List<FormulasCalculo>>();

            try
            {
                if (!string.IsNullOrEmpty(pFormulasCalculo.id))
                {
                    pFormulasCalculo.id = Utilidades.Desencriptar(pFormulasCalculo.id);
                    if (int.TryParse(pFormulasCalculo.id, out int temp))
                    {
                        pFormulasCalculo.IdFormula = temp;
                    }
                }

                resultadoConsulta.Clase = modulo;
                resultadoConsulta.Accion = (int)Accion.Consultar;
                List<FormulasCalculo> result = formulasCalculoDAL.ObtenerDatos(pFormulasCalculo);
                resultadoConsulta.objetoRespuesta = result;
                resultadoConsulta.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                resultadoConsulta.MensajeError = ex.Message;
            }
            return resultadoConsulta;
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Función que valida los datos de una fórmula de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulasCalculo>> ValidarDatos(FormulasCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();
            resultado.HayError = (int)Error.NoError;
            bool errorControlado = false;

            try
            {
                // validar la existencia de la fórmula por medio del nombre y/o código
                FormulasCalculo formulasExistente = formulasCalculoDAL.VerificarExistenciaFormulaPorCodigoNombre(pFormulasCalculo);
                if (formulasExistente != null)
                {
                    if (formulasExistente.Codigo.Trim().ToUpper().Equals(pFormulasCalculo.Codigo.Trim().ToUpper()))
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CodigoRegistrado, EtiquetasViewFormulasCalculo.CrearFormula_LabelCodigo));
                    }
                    else
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.NombreRegistrado, EtiquetasViewFormulasCalculo.CrearFormula_LabelNombre));
                    }
                }

                // validar si el valor selecionado en los comboboxes existe y se encuentra habilitado
                if (pFormulasCalculo.IdIndicador != 0)
                {
                    if (indicadorFonatelDAL.VerificarExistenciaIndicadorPorID((int)pFormulasCalculo.IdIndicador) == null)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelIndicadorSalida));
                    }
                }

                if (pFormulasCalculo.IdIndicadorVariable != 0) // variable dato
                {
                    if (detalleIndicadorVariablesDAL.ObtenerDatos(new DetalleIndicadorVariables() { idDetalleIndicador = (int)pFormulasCalculo.IdIndicadorVariable }).Count <= 0)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelVariableDato));
                    }
                }

                if (pFormulasCalculo.IdFrecuencia != 0)
                {
                    if (frecuenciaEnvioDAL.ObtenerDatos(new FrecuenciaEnvio() { idFrecuencia = (int)pFormulasCalculo.IdFrecuencia }).Count <= 0)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelFrecuencia));
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
        /// 24/10/2022
        /// José Navarro Acuña
        /// Prepara un objeto de fórmula para ser enviado al servicio DAL.
        /// Se preparan los id's de las tablas relacionadas para poder efectuar consultas debido a la encriptación.
        /// </summary>
        /// <param name="pIndicador"></param>
        private void PrepararObjetoFormulaCalculo(FormulasCalculo pFormulasCalculo)
        {
            if (!string.IsNullOrEmpty(pFormulasCalculo.id))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.id), out int number);
                pFormulasCalculo.IdFormula = number;
            }

            if (!string.IsNullOrEmpty(pFormulasCalculo.IdFrecuenciaString))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.IdFrecuenciaString), out int number);
                pFormulasCalculo.IdFrecuencia = number;
            }

            if (!string.IsNullOrEmpty(pFormulasCalculo.IdIndicadorSalidaString))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.IdIndicadorSalidaString), out int number);
                pFormulasCalculo.IdIndicador = number;
            }

            if (!string.IsNullOrEmpty(pFormulasCalculo.IdVariableDatoString))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.IdVariableDatoString), out int number);
                pFormulasCalculo.IdIndicadorVariable = number;
            }

            if (!pFormulasCalculo.NivelCalculoTotal && pFormulasCalculo.ListaCategoriasNivelesCalculo.Count > 0)
            {
                for (int i = 0; i < pFormulasCalculo.ListaCategoriasNivelesCalculo.Count; i++)
                {
                    int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.ListaCategoriasNivelesCalculo[i].IdCategoriaString), out int number);
                    pFormulasCalculo.ListaCategoriasNivelesCalculo[i].IdCategoria = number;
                }
            }
        }
    }
}
