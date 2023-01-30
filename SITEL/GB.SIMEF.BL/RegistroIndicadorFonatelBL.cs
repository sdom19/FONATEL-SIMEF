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
    public class RegistroIndicadorFonatelBL : IMetodos<RegistroIndicadorFonatel>
    {
        private readonly RegistroIndicadorFonatelDAL clsDatos;
        private readonly FuentesRegistroDestinatarioDAL clsFuentesRegistroDestinatarioDAL;
        private readonly FuentesRegistroDAL clsFuentesRegistroDAL;
        private readonly UsuarioFonatelDAL clsUsuarioFonatelDAL;

        private CorreoDal correoDal;
        private PlantillaHtmlDAL plantillaDal;


        private RespuestaConsulta<List<RegistroIndicadorFonatel>> ResultadoConsulta;
        string modulo = string.Empty;
        string user = string.Empty;
       
        public RegistroIndicadorFonatelBL(string modulo, string user )
        {
            this.clsDatos = new RegistroIndicadorFonatelDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<RegistroIndicadorFonatel>>();
            this.clsFuentesRegistroDestinatarioDAL = new FuentesRegistroDestinatarioDAL();
            this.clsFuentesRegistroDAL = new FuentesRegistroDAL();
            this.plantillaDal = new PlantillaHtmlDAL();
            this.clsUsuarioFonatelDAL = new UsuarioFonatelDAL();
            this.user = user;
            this.modulo = modulo;
        }

        public RespuestaConsulta<bool> EnvioCorreoInformante(RegistroIndicadorFonatel objeto)
        {
            RespuestaConsulta<bool> envioCorreo = new RespuestaConsulta<bool>();

            try
            {
                
                envioCorreo.objetoRespuesta = false;

                PlantillaHtml plantilla = plantillaDal.ObtenerDatos((int)Constantes.PlantillaCorreoEnum.EnvioRegistroIndicadorInformante);

                objeto = clsDatos.ObtenerDatos(objeto).Single();

                var FechaActual = DateTime.Today.ToShortDateString();
                var HoraActual = DateTime.Now.ToShortTimeString(); 

                if (objeto.Fuente.idEstado == (int)Constantes.EstadosRegistro.Activo)
                {

                    plantilla.Html = string.Format(plantilla.Html, Utilidades.Encriptar(objeto.Fuente.Fuente), objeto.Codigo, objeto.Nombre, objeto.Fuente.Fuente, FechaActual, HoraActual);

                    foreach (var detalleFuente in objeto.Fuente.DetalleFuentesRegistro.Where(x => x.Estado == true))
                    {
                        correoDal = new CorreoDal(detalleFuente.CorreoElectronico, "", plantilla.Html.Replace(Utilidades.Encriptar(objeto.Fuente.Fuente), detalleFuente.NombreDestinatario), "Carga de Solicitud exitosa");
                        var result = correoDal.EnviarCorreo();
                        envioCorreo.objetoRespuesta = result == 0 ? false : true;
                    }

                }
                else
                {
                    envioCorreo.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.SolicitudesFuenteRegistrada);
                }

                //clsDatos.RegistrarBitacora((int)Constantes.Accion.EnviarSolicitud, "Sistema automático", modulo, objeto.Codigo);


            }
            catch (Exception ex)
            {
                if (envioCorreo.HayError != (int)Constantes.Error.ErrorControlado)
                {
                    envioCorreo.HayError = (int)Constantes.Error.ErrorSistema;
                }
                envioCorreo.MensajeError = ex.Message;
            }

            return envioCorreo;

        }

        public RespuestaConsulta<bool> EnvioCorreoEncargado(RegistroIndicadorFonatel objeto)
        {
            RespuestaConsulta<bool> envioCorreo = new RespuestaConsulta<bool>();

            try
            {

                envioCorreo.objetoRespuesta = false;

                //FILTRAR POR MEDIO DEL USER
                var usuarioEncargado = clsUsuarioFonatelDAL.ObtenerDatos().ToList();

                PlantillaHtml plantilla = plantillaDal.ObtenerDatos((int)Constantes.PlantillaCorreoEnum.EnvioRegistroIndicadorEncargado);

                objeto = clsDatos.ObtenerDatos(objeto).Single();

                var FechaActual = DateTime.Today.ToShortDateString();
                var HoraActual = DateTime.Now.ToShortTimeString();

                if (objeto.Fuente.idEstado == (int)Constantes.EstadosRegistro.Activo)
                {

                    plantilla.Html = string.Format(plantilla.Html, Utilidades.Encriptar(objeto.Fuente.Fuente), objeto.Codigo, objeto.Nombre, objeto.Fuente.Fuente, FechaActual, HoraActual);

                    foreach (var detalleFuente in objeto.Fuente.DetalleFuentesRegistro.Where(x => x.Estado == true))
                    {
                        correoDal = new CorreoDal(detalleFuente.CorreoElectronico, "", plantilla.Html.Replace(Utilidades.Encriptar(objeto.Fuente.Fuente), user), "Carga de Solicitud exitosa");
                        var result = correoDal.EnviarCorreo();
                        envioCorreo.objetoRespuesta = result == 0 ? false : true;
                    }

                }
                else
                {
                    envioCorreo.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.SolicitudesFuenteRegistrada);
                }

                //clsDatos.RegistrarBitacora((int)Constantes.Accion.EnviarSolicitud, "Sistema automático", modulo, objeto.Codigo);


            }
            catch (Exception ex)
            {
                if (envioCorreo.HayError != (int)Constantes.Error.ErrorControlado)
                {
                    envioCorreo.HayError = (int)Constantes.Error.ErrorSistema;
                }
                envioCorreo.MensajeError = ex.Message;
            }

            return envioCorreo;

        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> ObtenerDatos(RegistroIndicadorFonatel objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                if (!string.IsNullOrEmpty(objeto.FormularioId))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.FormularioId), out int temp );
                    objeto.IdFormulario = temp;
                }
                if (!string.IsNullOrEmpty(objeto.Solicitudid))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.Solicitudid), out int temp);
                    objeto.IdSolicitud = temp;
                }
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

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> ActualizarElemento(RegistroIndicadorFonatel objeto)
        {
           

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> CambioEstado(RegistroIndicadorFonatel objeto)
        { 
        
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> ClonarDatos(RegistroIndicadorFonatel objeto)
        { 
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> EliminarElemento(RegistroIndicadorFonatel objeto)
        {
            
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> InsertarDatos(RegistroIndicadorFonatel objeto)
        {
   
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> ValidarDatos(RegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> ObtenerRegistroIndicador(RegistroIndicadorFonatel objeto,string usuario)
        {
            try
            {

                List<DetalleFuentesRegistro> detalle = 
                    clsFuentesRegistroDestinatarioDAL.ObtenerDatos(new DetalleFuentesRegistro()).Where(x => x.CorreoElectronico == usuario).ToList();

                FuentesRegistro fuente = clsFuentesRegistroDAL.ObtenerDatos(new FuentesRegistro()).Where(x => detalle.Any(y => y.idFuente== x.idFuente)).FirstOrDefault();

                if (fuente != null)
                {
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Consultar;
                    objeto.IdFuente = fuente.idFuente;
                    var resul = clsDatos.ObtenerDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
                }
                else
                {
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Consultar;
                    ResultadoConsulta.objetoRespuesta = new List<RegistroIndicadorFonatel>();
                    ResultadoConsulta.CantidadRegistros = 0;
                }
                

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 27/01/2023
        /// Metodo: El metodo funciona para obtener los datos de Registro Indicador en el modulo de Descarga y Edicion el cual solo obtendran los registros activos y registros de fechas pasadas
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<RegistroIndicadorFonatel>> ObtenerEditarRegistroIndicador(RegistroIndicadorFonatel objeto, string usuario)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

                if(resul == null)
                {
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Consultar;
                    ResultadoConsulta.objetoRespuesta = new List<RegistroIndicadorFonatel>();
                    ResultadoConsulta.CantidadRegistros = 0;
                }

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

    }
}