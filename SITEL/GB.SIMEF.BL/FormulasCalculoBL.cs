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

        private RespuestaConsulta<List<FormulasCalculo>> ResultadoConsulta;

        public FormulasCalculoBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            formulasCalculoDAL = new FormulasCalculoDAL();
            ResultadoConsulta = new RespuestaConsulta<List<FormulasCalculo>>();
        }

        public RespuestaConsulta<List<FormulasCalculo>> ActualizarElemento(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculo>> CambioEstado(FormulasCalculo objeto)
        {
            try
            {

                ResultadoConsulta.Clase = modulo;
                objeto.UsuarioModificacion = user;
                ResultadoConsulta.Accion =  objeto.IdEstado;
                ResultadoConsulta.Usuario = user;
                var resul = formulasCalculoDAL.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

                formulasCalculoDAL.RegistrarBitacora(ResultadoConsulta.Accion,
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

        public RespuestaConsulta<List<FormulasCalculo>> ClonarDatos(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculo>> EliminarElemento(FormulasCalculo objeto)
        {
            try
            {

                ResultadoConsulta.Clase = modulo;
                objeto.UsuarioModificacion = user;
                ResultadoConsulta.Accion = (int)EstadosRegistro.Eliminado;
                ResultadoConsulta.Usuario = user;
                var resul = formulasCalculoDAL.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

                formulasCalculoDAL.RegistrarBitacora(ResultadoConsulta.Accion,
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

        public RespuestaConsulta<List<FormulasCalculo>> InsertarDatos(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<FormulasCalculo>> ObtenerDatos(FormulasCalculo pFormulasCalculo)
        {
           

            try
            {

                if (!String.IsNullOrEmpty(pFormulasCalculo.id))
                {
                    pFormulasCalculo.id = Utilidades.Desencriptar(pFormulasCalculo.id);
                    if (int.TryParse(pFormulasCalculo.id, out int temp))
                    {
                        pFormulasCalculo.idFormula = temp;
                    }
                }


                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var result = formulasCalculoDAL.ObtenerDatos(pFormulasCalculo);
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

        public RespuestaConsulta<List<FormulasCalculo>> ValidarDatos(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }
    }
}
