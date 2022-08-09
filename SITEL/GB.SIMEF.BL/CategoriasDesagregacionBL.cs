using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class CategoriasDesagregacionBL:IMetodos<CategoriasDesagregacion>
    {
        private readonly CategoriasDesagregacionDAL clsDatos;



        private RespuestaConsulta<List<CategoriasDesagregacion>> ResultadoConsulta;
        string modulo = Etiquetas.Categorias;

        public CategoriasDesagregacionBL()
        {
            this.clsDatos = new CategoriasDesagregacionDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<CategoriasDesagregacion>>();
        }

        public RespuestaConsulta<CategoriasDesagregacion> ActualizarElemento(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<CategoriasDesagregacion> CambioEstado(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<CategoriasDesagregacion> ClonarDatos(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<CategoriasDesagregacion>> EliminarElemento(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<CategoriasDesagregacion> InsertarDatos(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<CategoriasDesagregacion>> ObtenerDatos(CategoriasDesagregacion objCategoria)
        {
            try
            {
                if (!String.IsNullOrEmpty(objCategoria.id))
                {
                    objCategoria.id = Utilidades.Desencriptar(objCategoria.id);
                    int temp;
                    if (int.TryParse(objCategoria.id, out temp))
                    {
                        objCategoria.idCategoria = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objCategoria);
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

        public RespuestaConsulta<CategoriasDesagregacion> ValidarDatos(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }
    }
}
