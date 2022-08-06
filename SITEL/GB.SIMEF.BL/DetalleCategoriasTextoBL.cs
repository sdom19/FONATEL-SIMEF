using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class DetalleCategoriasTextoBL:IMetodos<DetalleCategoriaTexto>
    {
        private readonly DetalleCategoriaTextoDAL clsDatos;

        private RespuestaConsulta<List<DetalleCategoriaTexto>> ResultadoConsulta;
        string modulo = Etiquetas.DetalleCategorias;

        public DetalleCategoriasTextoBL()
        {
            clsDatos = new DetalleCategoriaTextoDAL();
            ResultadoConsulta = new RespuestaConsulta<List<DetalleCategoriaTexto>>();
        }

        public RespuestaConsulta<DetalleCategoriaTexto> ActualizarElemento(DetalleCategoriaTexto objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<DetalleCategoriaTexto> CambioEstado(DetalleCategoriaTexto objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<DetalleCategoriaTexto> ClonarDatos(DetalleCategoriaTexto objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> EliminarElemento(DetalleCategoriaTexto objeto)
        {
            DetalleCategoriaTexto objCategoria = (DetalleCategoriaTexto)objeto;
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;
                ResultadoConsulta.Usuario = objeto.usuario;
                DetalleCategoriaTexto registroActializar;
                var resul = clsDatos.ObtenerDatos(objCategoria);
                if (resul.Count()==0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                    
                }
                else
                {
                    registroActializar = resul.SingleOrDefault();
                    registroActializar.Estado = false;
                    resul = clsDatos.ActualizarDatos(registroActializar);
                }
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion, ResultadoConsulta.Usuario, 
                    ResultadoConsulta.Clase, 
                   string.Format("{0}/{1}", 
                   registroActializar.CategoriasDesagregacion.Codigo,
                   registroActializar.Codigo)
                 );
            }
            catch (Exception ex)
            {
                ResultadoConsulta.MensajeError = ex.Message;
                if (ex.Message== Errores.NoRegistrosActualizar)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;             
   
                }       
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<DetalleCategoriaTexto> InsertarDatos(DetalleCategoriaTexto objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> ObtenerDatos(DetalleCategoriaTexto objeto)
        {
            DetalleCategoriaTexto objCategoria = (DetalleCategoriaTexto)objeto;
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = 1;
                var resul = clsDatos.ObtenerDatos(objCategoria);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;

            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<DetalleCategoriaTexto> ValidarDatos(DetalleCategoriaTexto objeto)
        {
            throw new NotImplementedException();
        }
    }
}
