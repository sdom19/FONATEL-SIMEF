using GB.SIMEF.Entities;
using GB.SIMEF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class GestionTextoSIGITELBL
    {
        private GestionTextoSIGITELDAL clsDatos;
        string modulo = string.Empty;
        string user = string.Empty;
        public GestionTextoSIGITELBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new GestionTextoSIGITELDAL();
        }

        public RespuestaConsulta<List<CatalogoPantallaSIGITEL>> ObtenerPantallasSIGITEL()
        {
            var ResultadoConsulta = new RespuestaConsulta<List<CatalogoPantallaSIGITEL>>();
            try 
            {
                ResultadoConsulta.objetoRespuesta = clsDatos.ObtenerPantallasSIGITEL();
                for (var i = 0; i < ResultadoConsulta.objetoRespuesta.Count(); i++)
                {
                    ResultadoConsulta.objetoRespuesta[i].id = Utilidades.Encriptar(ResultadoConsulta.objetoRespuesta[i].IdCatalogoPantallaSIGITEL.ToString());
                }
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
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

        public RespuestaConsulta<List<TipoContenidoTextoSIGITEL>> ObtenerTipoContenido()
        {
            var ResultadoConsulta = new RespuestaConsulta<List<TipoContenidoTextoSIGITEL>>();
            try
            {
                ResultadoConsulta.objetoRespuesta = clsDatos.ObtenerTipoContenido();
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
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

        public RespuestaConsulta<List<ContenidoPantallaSIGITEL>> ObtenerDatos(ContenidoPantallaSIGITEL obj)
        {
            var ResultadoConsulta = new RespuestaConsulta<List<ContenidoPantallaSIGITEL>>();
            try
            {
                obj.IdCatalogoPantallaSIGITEL = int.Parse(Utilidades.Desencriptar(obj.IdCatalogoPantallaSIGITELString));
                ResultadoConsulta.objetoRespuesta = clsDatos.ObtenerDatos(obj);
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
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

        public RespuestaConsulta<List<ContenidoPantallaSIGITEL>> ActualizarDatos(ContenidoPantallaSIGITEL obj)
        {
            var ResultadoConsulta = new RespuestaConsulta<List<ContenidoPantallaSIGITEL>>();
            try
            {
                string jsonAnterior = string.Empty;
                string jsonInicial = string.Empty;
                string jsonActual = string.Empty;

                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Crear;
                ResultadoConsulta.Usuario = user;

                obj.IdCatalogoPantallaSIGITEL = int.Parse(Utilidades.Desencriptar(obj.IdCatalogoPantallaSIGITELString));

                var listaPantallas = clsDatos.ObtenerPantallasSIGITEL();
                var listaTipoContenido = clsDatos.ObtenerTipoContenido();
                
                obj.Pantalla = listaPantallas.Where(p => p.IdCatalogoPantallaSIGITEL == obj.IdCatalogoPantallaSIGITEL).FirstOrDefault();
                obj.TipoContenidoTextoSIGITEL = listaTipoContenido.Where(t => t.IdTipoContenidoTextoSIGITEL == obj.IdTipoContenidoTextoSIGITEL).FirstOrDefault();
                jsonActual = obj.ToString();

                if (obj.IdContenidoPantallaSIGITEL != 0)
                {
                    var objAnterior = clsDatos.ObtenerDatos(obj).Where(c => c.IdContenidoPantallaSIGITEL == obj.IdContenidoPantallaSIGITEL).FirstOrDefault();
                    ResultadoConsulta.Accion = (int)Accion.Editar;
                    jsonAnterior = objAnterior.ToString();
                }
                else 
                {
                    jsonInicial = obj.ToString();
                    jsonActual = string.Empty;
                }

                if (!obj.Estado)
                {
                    ResultadoConsulta.Accion = (int)Accion.Eliminar;
                }

                ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(obj);
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                        ResultadoConsulta.Clase, obj.Pantalla.NombrePantalla, jsonActual, jsonAnterior, jsonInicial);
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
    }
}
