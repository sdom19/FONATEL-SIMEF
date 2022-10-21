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
    public class DefinicionIndicadorBL : IMetodos<DefinicionIndicador>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly DefinicionIndicadorDAL DefinicionIndicadorDAL;

        private RespuestaConsulta<List<DefinicionIndicador>> ResultadoConsulta;

        public DefinicionIndicadorBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            DefinicionIndicadorDAL = new DefinicionIndicadorDAL();
            ResultadoConsulta = new RespuestaConsulta<List<DefinicionIndicador>>();
        }


       public string SerializarObjetoBitacora(DefinicionIndicador objdefinicion)
        {
            return JsonConvert.SerializeObject(objdefinicion, new JsonSerializerSettings
            { ContractResolver = new JsonIgnoreResolver(objdefinicion.NoSerialize) });
        }

        private void ValidarObjeto(DefinicionIndicador objeto)
        {
            if (objeto == null)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(Errores.NoRegistrosActualizar);
            }
            else if (!Utilidades.rx_alfanumerico.Match(objeto.Notas.Trim()).Success)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "Notas"));
            }
            else if (!Utilidades.rx_alfanumerico.Match(objeto.Definicion.Trim()).Success)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "Definición"));
            }
            else if (!Utilidades.rx_alfanumerico.Match(objeto.Fuente.Trim()).Success)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "Fuente"));
            }
        }


       public RespuestaConsulta<DefinicionIndicador> VisualizarElemento(DefinicionIndicador objeto)
        {
             RespuestaConsulta<DefinicionIndicador> ResultadoConsultaValorUnico=new RespuestaConsulta<DefinicionIndicador>();
            try
            {

                int temp = 0;
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idIndicador = temp;
                }
                ResultadoConsultaValorUnico.Clase = modulo;
                ResultadoConsultaValorUnico.Accion = (int)Accion.Visualizar;
                ResultadoConsultaValorUnico.Usuario = user;
                var result = DefinicionIndicadorDAL.ObtenerDatos(objeto);
                ResultadoConsultaValorUnico.CantidadRegistros = result.Count();
                if (ResultadoConsultaValorUnico.CantidadRegistros > 1)
                {
                    ResultadoConsultaValorUnico.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.CatidadIndicadoresExcedido);
                }


                ResultadoConsultaValorUnico.objetoRespuesta = result.Single();

                DefinicionIndicadorDAL.RegistrarBitacora(ResultadoConsultaValorUnico.Accion,
                ResultadoConsultaValorUnico.Usuario,
                ResultadoConsultaValorUnico.Clase, ResultadoConsultaValorUnico.objetoRespuesta.Indicador.Codigo);

            }
            catch (Exception ex)
            {
                ResultadoConsultaValorUnico.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsultaValorUnico.MensajeError = ex.Message;
            }
            return ResultadoConsultaValorUnico;
        }

       public RespuestaConsulta<List<DefinicionIndicador>> ActualizarElemento(DefinicionIndicador objeto)
        {
            try
            {
                ValidarObjeto(objeto);
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Usuario = user;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                string jsonInicial = objeto.json;
                ResultadoConsulta.objetoRespuesta = DefinicionIndicadorDAL.ActualizarDatos(objeto);
                objeto = ResultadoConsulta.objetoRespuesta.Single();
                string JsonFinal = SerializarObjetoBitacora(objeto);
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                DefinicionIndicadorDAL.RegistrarBitacora(ResultadoConsulta.Accion,
                ResultadoConsulta.Usuario,
                ResultadoConsulta.Clase, objeto.Indicador.Codigo, JsonFinal, jsonInicial,"");

            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError != (int) Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DefinicionIndicador>> CambioEstado(DefinicionIndicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DefinicionIndicador>> ClonarDatos(DefinicionIndicador objeto)
        {
            try
            {
                int temp = 0;
                if (!string.IsNullOrEmpty(objeto.idClonado))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.idClonado), out temp);
                    objeto.idIndicador = temp;
                }

               
                objeto.idEstado = (int)Constantes.EstadosRegistro.Activo;
                ValidarObjeto(objeto);
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Usuario = user;
                ResultadoConsulta.Accion = (int)Accion.Clonar;
                string jsonInicial = objeto.json;
                ResultadoConsulta.objetoRespuesta = DefinicionIndicadorDAL.ActualizarDatos(objeto);
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                objeto = ResultadoConsulta.objetoRespuesta.Single();
                string jsonFinal = SerializarObjetoBitacora(objeto);    
                DefinicionIndicadorDAL.RegistrarBitacora(ResultadoConsulta.Accion,
                ResultadoConsulta.Usuario,
                ResultadoConsulta.Clase, objeto.Indicador.Codigo, jsonFinal,"",jsonInicial);

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

        public RespuestaConsulta<List<DefinicionIndicador>> EliminarElemento(DefinicionIndicador objeto)
        {
            try
            {
                if (objeto==null)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Usuario = user;
                objeto.idEstado = (int)EstadosRegistro.Eliminado;
                ResultadoConsulta.Accion = (int)Accion.Eliminar ;
                ResultadoConsulta.objetoRespuesta = DefinicionIndicadorDAL.ActualizarDatos(objeto);
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                DefinicionIndicadorDAL.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                       ResultadoConsulta.Clase, objeto.Indicador.Codigo);

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

        public RespuestaConsulta<List<DefinicionIndicador>> InsertarDatos(DefinicionIndicador objeto)
        {
            try
            {
                int temp = 0;
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idIndicador = temp;
                }
                objeto.idEstado = (int)Constantes.EstadosRegistro.Activo;
                ValidarObjeto(objeto);
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Usuario = user;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.objetoRespuesta = DefinicionIndicadorDAL.ActualizarDatos(objeto);
                objeto = ResultadoConsulta.objetoRespuesta.Single();
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                string jsonValorInicial = SerializarObjetoBitacora(objeto);

                DefinicionIndicadorDAL.RegistrarBitacora(ResultadoConsulta.Accion,
                ResultadoConsulta.Usuario,
                ResultadoConsulta.Clase, objeto.Indicador.Codigo,"","",jsonValorInicial);

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


        public RespuestaConsulta<List<DefinicionIndicador>> ObtenerDatos(DefinicionIndicador pDefinicionIndicador)
        {
            try
            { 
                int temp = 0;
                if (!string.IsNullOrEmpty(pDefinicionIndicador.id))
                {
                    int.TryParse(Utilidades.Desencriptar(pDefinicionIndicador.id), out temp);
                    pDefinicionIndicador.idIndicador = temp;
                }      
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var result = DefinicionIndicadorDAL.ObtenerDatos(pDefinicionIndicador);
                
                ResultadoConsulta.objetoRespuesta = result;
                ResultadoConsulta.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DefinicionIndicador>> ValidarDatos(DefinicionIndicador objeto)
        {
            throw new NotImplementedException();
        }
    }
}
