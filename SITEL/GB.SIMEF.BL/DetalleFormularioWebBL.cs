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
        private string SerializarObjetoBitacora(DetalleFormularioWeb objDetalleFormulario)
        {
            return JsonConvert.SerializeObject(objDetalleFormulario, new JsonSerializerSettings
            { ContractResolver = new JsonIgnoreResolver(objDetalleFormulario.NoSerialize) });
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

        private int ValidarCantidadIndicadores(int id)
        {
            var cantidad = clsDatos.ObtenerCantidadIndicadores(id);
            if (cantidad <= 0)
                throw new Exception(Errores.CatidadIndicadoresExcedido);
            return cantidad;
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> ActualizarElemento(DetalleFormularioWeb objeto)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
                ValidarCantidadIndicadores(objeto.idFormulario);
                if (ValidarDatosRepetidos(objeto))
                {
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                }

                objeto = clsDatos.ObtenerDatos(objeto).Single();

                //string jsonValorInicial = SerializarObjetoBitacora(objeto);

                //clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                //            ResultadoConsulta.Usuario,
                //                ResultadoConsulta.Clase, objeto.Indicador.Codigo, "", "", jsonValorInicial);
            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.IndicadorFormularioRegistrado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                }
                if (ex.Message == Errores.CatidadIndicadoresExcedido)
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
