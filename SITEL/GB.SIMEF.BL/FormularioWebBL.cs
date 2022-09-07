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
    public class FormularioWebBL : IMetodos<FormularioWeb>
    {
        private readonly FormularioWebDAL clsDatos;
        private readonly DetalleFormularioWebDAL clsDatos2;

        private RespuestaConsulta<List<FormularioWeb>> ResultadoConsulta;
        string modulo = string.Empty;
        string user = string.Empty;

        public FormularioWebBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new FormularioWebDAL();
            this.clsDatos2 = new DetalleFormularioWebDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<FormularioWeb>>();
        }

        public RespuestaConsulta<List<FormularioWeb>> ActualizarElemento(FormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormularioWeb>> CambioEstado(FormularioWeb objeto)
        {
            try
            {
                
                ResultadoConsulta.Clase = modulo;
                objeto.UsuarioModificacion = user;
                ResultadoConsulta.Accion =  objeto.idEstado==(int)EstadosRegistro.Activo? (int)Accion.Activar:(int)Accion.Inactiva;
                var resul = clsDatos.ActualizarDatos(objeto);
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormularioWeb>> ObtenerDatos(FormularioWeb objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idFormulario = temp;
                    }
                }
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
                 
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idFormulario = temp;
                    }
                }
                ResultadoConsultaIndicadores.Clase = modulo;
                ResultadoConsultaIndicadores.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerIndicadoresFormulario(objeto.idFormulario);

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



        RespuestaConsulta<List<FormularioWeb>> IMetodos<FormularioWeb>.ValidarDatos(FormularioWeb objeto)
        {
            throw new NotImplementedException();
        }
    }
}
