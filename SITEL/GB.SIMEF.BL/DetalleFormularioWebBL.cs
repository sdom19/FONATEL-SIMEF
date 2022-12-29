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


        public DetalleFormularioWebBL(string modulo, string user) 
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new DetalleFormularioWebDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleFormularioWeb>>();
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
                                ResultadoConsulta.Clase, "", "", "", jsonValorInicial);
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
    }
}
