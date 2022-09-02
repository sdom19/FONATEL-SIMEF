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

        public RespuestaConsulta<List<DefinicionIndicador>> ActualizarElemento(DefinicionIndicador objeto)
        {
            throw new NotImplementedException();
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
                int temp = 0;
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idDefinicion = temp;
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Usuario = user;
                var resul = DefinicionIndicadorDAL.ObtenerDatos(objeto);
                objeto = resul.Single();
                objeto.idEstado = (int)EstadosRegistro.Eliminado;
                ResultadoConsulta.Accion = (int)Accion.Eliminar ;
                resul = DefinicionIndicadorDAL.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                DefinicionIndicadorDAL.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                       ResultadoConsulta.Clase, objeto.Indicador.Codigo);

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
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
