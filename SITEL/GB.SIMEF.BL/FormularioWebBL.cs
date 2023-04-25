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
    public class FormularioWebBL : IMetodos<FormularioWeb>
    {
        private readonly FormularioWebDAL clsDatos;
        private readonly DetalleFormularioWebDAL detalleFormularioWebDAL;

        private RespuestaConsulta<List<FormularioWeb>> ResultadoConsulta;
        string modulo = string.Empty;
        string user = string.Empty;

        public FormularioWebBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new FormularioWebDAL();
            this.detalleFormularioWebDAL = new DetalleFormularioWebDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<FormularioWeb>>();
        }

        private int DesencriptarId(string id)
        {
            int idFormularioWeb = 0;
            if (!String.IsNullOrEmpty(id))
            {
                id = Utilidades.Desencriptar(id);
                int temp;
                if (int.TryParse(id, out temp))
                {
                    idFormularioWeb = temp;
                }
            }
            return idFormularioWeb;
        }

        // Valida si existen formularios con el mismo nombre o código
        private bool ValidarDatosRepetidos(FormularioWeb objFormularioWeb)
        {
            List<FormularioWeb> buscarRegistro = clsDatos.ObtenerDatos(new FormularioWeb());
            if (buscarRegistro.Where(x => x.Codigo.ToUpper().TrimStart().TrimEnd() == objFormularioWeb.Codigo.ToUpper().TrimStart().TrimEnd()).ToList().Count() > 0)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(Errores.CodigoRegistrado);
            }
            else if (buscarRegistro.Where(x => x.Nombre.ToUpper().TrimStart().TrimEnd() == objFormularioWeb.Nombre.ToUpper().TrimStart().TrimEnd()).ToList().Count() > 0)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(Errores.NombreRegistrado);
            }
            return true;
        }

        // La cantidad de Indicadores no puede ser inferior
        private bool ValidarCantidadIndicadores(FormularioWeb formularioWebNuevo)
        {
            bool resultadoValidacion = false;

            formularioWebNuevo.idEstadoRegistro = 0;
            string[] arrayIndicadores = new string[] { };
            int cantidadIndicadores = 0;
            FormularioWeb formularioWebViejo = clsDatos.ObtenerDatos(formularioWebNuevo).Single();
            if (formularioWebViejo.ListaIndicadores != null)
            {
                arrayIndicadores = formularioWebViejo.ListaIndicadores.Split(',');
                cantidadIndicadores = arrayIndicadores.Count();
            }
            if (cantidadIndicadores > 0)
            {
                if (formularioWebNuevo.CantidadIndicador < cantidadIndicadores)
                {
                    resultadoValidacion = true;
                }
            }
            else if(formularioWebViejo.ListaIndicadores == null)
            {
                resultadoValidacion = false;
            }
            else
            {
                resultadoValidacion = true;
            }

            return resultadoValidacion;
        }

        private int ValidarEstado(FormularioWeb obj)
        {
            FormularioWeb formularioWebViejo = clsDatos.ObtenerDatos(obj).Single();

            if (formularioWebViejo.idEstadoRegistro == (int)Constantes.EstadosRegistro.Desactivado)
            {
                return (int)Constantes.EstadosRegistro.Desactivado;
            }

            if (obj.Descripcion == null || obj.Descripcion == "" || obj.CantidadIndicador == 0 || obj.idFrecuenciaEnvio == 0 
                || obj.CantidadIndicador != formularioWebViejo.CantidadIndicador || formularioWebViejo.idEstadoRegistro == (int)Constantes.EstadosRegistro.EnProceso)
            { 
                if (formularioWebViejo.EstadoRegistro.IdEstadoRegistro == (int)Constantes.EstadosRegistro.Desactivado)
                { 
                    return (int)Constantes.EstadosRegistro.Desactivado;
                }
                else
                { 
                    return (int)Constantes.EstadosRegistro.EnProceso;
                }
            }
            else
            { 
                return (int)Constantes.EstadosRegistro.Activo;
            }
        }

        public RespuestaConsulta<List<FormularioWeb>> ActualizarElemento(FormularioWeb objeto)
        {
            try
            {
                var objetoAnterior = new FormularioWeb { idFormularioWeb = objeto.idFormularioWeb, idEstadoRegistro = 0, Codigo = objeto.Codigo };
                objetoAnterior.idFormularioWeb = DesencriptarId(objetoAnterior.id);
                List<FormularioWeb> buscarRegistro = clsDatos.ObtenerDatos(new FormularioWeb());
                
                string jsonAnterior = clsDatos.ObtenerDatos(objetoAnterior).FirstOrDefault().ToString();
                objeto.idFormularioWeb = DesencriptarId(objeto.id); 
                
                if (buscarRegistro.Where(x => x.Nombre.ToUpper().TrimStart().TrimEnd() == objeto.Nombre.ToUpper().TrimStart().TrimEnd() && x.Codigo.ToUpper().TrimStart().TrimEnd() != objeto.Codigo.ToUpper().TrimStart().TrimEnd()).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }

                if (buscarRegistro.Where(X => X.Codigo.ToUpper() == objeto.Codigo.ToUpper() && !X.id.Equals(objeto.id)).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }

                if (ValidarCantidadIndicadores(objeto))
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CantidadIndicadoresMenor);
                }
                
                objeto.idEstadoRegistro = ValidarEstado(objeto);
                ResultadoConsulta.Clase = modulo;
                objeto.UsuarioModificacion = user;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                var resul = clsDatos.ActualizarDatos(objeto);
                string jsonActual = resul.FirstOrDefault().ToString();
                ResultadoConsulta.Usuario = user;
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo, jsonActual, jsonAnterior);

            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError != (int)Constantes.Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<FormularioWeb>> CambioEstado(FormularioWeb objeto)
        {
            try
            {
                var objetoAnterior = new FormularioWeb { idFormularioWeb = objeto.idFormularioWeb, idEstadoRegistro = 0, Codigo = objeto.Codigo};
                objetoAnterior.idFormularioWeb = DesencriptarId(objetoAnterior.id);
                string jsonAnterior = clsDatos.ObtenerDatos(objetoAnterior).FirstOrDefault().ToString();

                ResultadoConsulta.Clase = modulo;
                objeto.UsuarioModificacion = user;
                ResultadoConsulta.Accion = objeto.idEstadoRegistro == (int)EstadosRegistro.Activo ? (int)Accion.Activar : (int)Accion.Desactivar;
                var resul = clsDatos.ActualizarDatos(objeto);
                string jsonActual = resul.FirstOrDefault().ToString();
                ResultadoConsulta.Usuario = user;
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo);

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<FormularioWeb>> ClonarDatos(FormularioWeb objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Clonar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioCreacion = user;
                objeto.idFormularioWeb = DesencriptarId(objeto.id);

                var objInicial = clsDatos.ObtenerDatos(new FormularioWeb { idFormularioWeb = objeto.idFormularioWeb});
                string jsonInicial = objInicial.FirstOrDefault().ToString();

                bool validacionCantidad =  ValidarCantidadIndicadores(new FormularioWeb() { idFormularioWeb = objeto.idFormularioWeb, Codigo = "", CantidadIndicador = objeto.CantidadIndicador });
                
                if (validacionCantidad)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CantidadIndicadoresMenor);
                }

                //var ListaDetalleFormulariosWeb = ObtenerDetalleFormularioWeb(objeto.idFormularioWeb);

                // resetear el id original
                objeto.idFormularioWeb = 0;
                if (ValidarDatosRepetidos(objeto))
                {
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                }

                objeto = clsDatos.ObtenerDatos(objeto).Single();
                //ClonarDetalleFormularioWeb(ListaDetalleFormulariosWeb, objeto.idFormularioWeb);

                string jsonActual = objeto.ToString();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo, jsonActual, "", jsonInicial);

            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<FormularioWeb>> EliminarElemento(FormularioWeb objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                objeto.UsuarioModificacion = user;
                ResultadoConsulta.Accion = (int)EstadosRegistro.Eliminado;
                ResultadoConsulta.Usuario = user;
                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo);

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<FormularioWeb>> InsertarDatos(FormularioWeb objeto)
        {
            try
            {
                objeto.idFormularioWeb = 0;
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Crear;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioCreacion = user;
                if (ValidarDatosRepetidos(objeto))
                {
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                }

                objeto = ResultadoConsulta.objetoRespuesta.Single();

                string jsonValorInicial = objeto.ToString();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo, "", "", jsonValorInicial);

            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                { 
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<FormularioWeb>> ObtenerDatos(FormularioWeb objeto)
        {
            try
            {
                objeto.idFormularioWeb = DesencriptarId(objeto.id); 
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

        public RespuestaConsulta<List<string>> ValidarDatos(FormularioWeb objeto)
        {
             RespuestaConsulta<List<string>> Resultado=new RespuestaConsulta<List<string>>();
            try
            {
                Resultado.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ValidarFuente(objeto);
                Resultado.objetoRespuesta = resul;
                Resultado.CantidadRegistros = resul.Count();

            }
            catch (Exception ex)
            {
                Resultado.HayError = (int)Constantes.Error.ErrorSistema;
                Resultado.MensajeError = ex.Message;
            }
            return Resultado;
        }

        public RespuestaConsulta<List<Indicador>> ObtenerIndicadoresFormulario(FormularioWeb objeto)
        {
            RespuestaConsulta<List<Indicador>> ResultadoConsultaIndicadores = new RespuestaConsulta<List<Indicador>>();
            try
            {
                objeto.idFormularioWeb = DesencriptarId(objeto.id);
                ResultadoConsultaIndicadores.Clase = modulo;
                ResultadoConsultaIndicadores.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerIndicadoresFormulario(objeto.idFormularioWeb);

                ResultadoConsultaIndicadores.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

            }
            catch (Exception ex)
            {
                ResultadoConsultaIndicadores.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsultaIndicadores.MensajeError = ex.Message;
            }
            return ResultadoConsultaIndicadores;
        }

        private List<DetalleFormularioWeb> ObtenerDetalleFormularioWeb(int idFormularioWeb)
        {
            DetalleFormularioWeb objDetalleFormulario = new DetalleFormularioWeb();
            objDetalleFormulario.IdDetalleFormularioWeb = 0;
            objDetalleFormulario.idFormularioWeb = idFormularioWeb;
            objDetalleFormulario.idIndicador = 0;
            return detalleFormularioWebDAL.ObtenerDatos(objDetalleFormulario);
        }

        public List<DetalleFormularioWeb> ObtenerTodosDetalleFormularioWeb(FormularioWeb objeto)
        {
            var idFormularioWeb = objeto.idFormularioWeb;
            List<DetalleFormularioWeb> lista = new List<DetalleFormularioWeb>();
            foreach (Indicador i in objeto.ListaIndicadoresObj) 
            {
                var df = detalleFormularioWebDAL.ObtenerDatos(new DetalleFormularioWeb() { idFormularioWeb = idFormularioWeb, idIndicador = i.IdIndicador });
                if(df.Count > 0)
                {
                    lista.Add(df.Single());
                }
            }
            return lista;
        }

        private List<DetalleFormularioWeb> ClonarDetalleFormularioWeb(List<DetalleFormularioWeb> ListaDetalleFormulariosWeb, int nuevoidFormularioWeb)
        {
            foreach (DetalleFormularioWeb df in ListaDetalleFormulariosWeb)
            {
                df.idFormularioWeb = nuevoidFormularioWeb;
                df.IdDetalleFormularioWeb = 0;
                detalleFormularioWebDAL.ActualizarDatos(df);
            }
            return ListaDetalleFormulariosWeb;
        }
        RespuestaConsulta<List<FormularioWeb>> IMetodos<FormularioWeb>.ValidarDatos(FormularioWeb objeto)
        {
            throw new NotImplementedException();
        }
    }
}
