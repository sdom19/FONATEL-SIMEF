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

        public RespuestaConsulta<List<DefinicionIndicador>> ActualizarElemento(DefinicionIndicador objeto)
        {
            try
            {
                ValidarObjeto(objeto);
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Usuario = user;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.objetoRespuesta = DefinicionIndicadorDAL.ActualizarDatos(objeto);
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                DefinicionIndicadorDAL.RegistrarBitacora(ResultadoConsulta.Accion,
                ResultadoConsulta.Usuario,
                ResultadoConsulta.Clase, objeto.Indicador.Codigo);

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<DefinicionIndicador>> ObtenerDatos(DefinicionIndicador pDefinicionIndicador)
        {
            try
            { 
                int temp = 0;
                if (!string.IsNullOrEmpty(pDefinicionIndicador.id))
                {
                    int.TryParse(Utilidades.Desencriptar(pDefinicionIndicador.id), out temp);
                    pDefinicionIndicador.idDefinicion = temp;
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
