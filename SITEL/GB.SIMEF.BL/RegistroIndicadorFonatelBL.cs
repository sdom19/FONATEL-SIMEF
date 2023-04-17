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
    public class RegistroIndicadorFonatelBL : IMetodos<RegistroIndicadorFonatel>
    {
        private readonly RegistroIndicadorFonatelDAL clsDatos;
        private readonly FuentesRegistroDestinatarioDAL clsFuentesRegistroDestinatarioDAL;
        private readonly SolicitudDAL clsSolicitudesDAL;
        private readonly FuentesRegistroDAL clsFuentesRegistroDAL;
        private readonly UsuarioFonatelDAL clsUsuarioFonatelDAL;
        private readonly EnvioSolicitudDAL clsEnvioSolicitudesDAL;

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
            this.clsEnvioSolicitudesDAL = new EnvioSolicitudDAL();
            this.clsSolicitudesDAL = new SolicitudDAL();
            this.user = user;
            this.modulo = modulo;
        }

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 14-02-2023
        /// Metodo que permite enviar un correo electornica al informante del formulario web de la carga exitosa de la solucitud 
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
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

                if (objeto.Fuente.IdEstadoRegistro == (int)Constantes.EstadosRegistro.Activo)
                {

                    plantilla.Html = string.Format(plantilla.Html, Utilidades.Encriptar(objeto.Fuente.Fuente), objeto.Codigo, objeto.Nombre, objeto.Fuente.Fuente, FechaActual, HoraActual);

                    foreach (var detalleFuente in objeto.Fuente.DetalleFuenteRegistro.Where(x => x.Estado == true))
                    {
                        correoDal = new CorreoDal(detalleFuente.CorreoElectronico, "", plantilla.Html.Replace(Utilidades.Encriptar(objeto.Fuente.Fuente), detalleFuente.NombreDestinatario), EtiquetasViewRegistroIndicadorFonatel.CargaExitosa);
                        var result = correoDal.EnviarCorreo();
                        envioCorreo.objetoRespuesta = result == 0 ? false : true;
                    }
                }
                else
                {
                    envioCorreo.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.SolicitudesFuenteRegistrada);
                }

                clsDatos.RegistrarBitacora((int)Constantes.Accion.EnviarCorreoInformante, EtiquetasViewRegistroIndicadorFonatel.SistemaAutomatico, modulo, objeto.Codigo);

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

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 14-02-2023
        /// Metodo que permite enviar un correo electornica al Administrador del formulario web de la carga exitosa de la solucitud 
        /// </summary>
        /// <param name="objeto"></param>
        public RespuestaConsulta<bool> EnvioCorreoEncargado(RegistroIndicadorFonatel objeto)
        {
            RespuestaConsulta<bool> envioCorreo = new RespuestaConsulta<bool>();

            Solicitud solicitud = new Solicitud();

            try
            {
                envioCorreo.objetoRespuesta = false;

                var EnvioSolicitudes = clsEnvioSolicitudesDAL.ObtenerEnviosCorrectos().ToList();

                var SolicitudDeEnvio = EnvioSolicitudes.Where(x => x.idEnvioSolicitud == objeto.IdSolicitud).FirstOrDefault();

                var Solicitudes = clsSolicitudesDAL.ObtenerDatos(solicitud).ToList();

                var SolicitudSIMEF = Solicitudes.Where(x => x.idSolicitud == SolicitudDeEnvio.IdSolicitud).FirstOrDefault();

                var UsuarioEncargado = clsUsuarioFonatelDAL.ObtenerDatos(1).Where(x => x.AccesoUsuario.ToUpper() == SolicitudSIMEF.UsuarioCreacion.ToUpper()).ToList();

                string NombreEncargado = UsuarioEncargado[0].NombreUsuario;

                string CorreoEncargado = UsuarioEncargado[0].CorreoUsuario;

                PlantillaHtml plantilla = plantillaDal.ObtenerDatos((int)Constantes.PlantillaCorreoEnum.EnvioRegistroIndicadorEncargado);

                objeto = clsDatos.ObtenerDatos(objeto).Single();

                var FechaActual = DateTime.Today.ToShortDateString();
                var HoraActual = DateTime.Now.ToShortTimeString();

                if (objeto.Fuente.IdEstadoRegistro == (int)Constantes.EstadosRegistro.Activo)
                {

                    plantilla.Html = string.Format(plantilla.Html, Utilidades.Encriptar(objeto.Fuente.Fuente), objeto.Codigo, objeto.Nombre, objeto.Fuente.Fuente, FechaActual, HoraActual);

                    correoDal = new CorreoDal(CorreoEncargado, "", plantilla.Html.Replace(Utilidades.Encriptar(objeto.Fuente.Fuente), NombreEncargado), EtiquetasViewRegistroIndicadorFonatel.CargaExitosa);
                    var result = correoDal.EnviarCorreo();
                    envioCorreo.objetoRespuesta = result == 0 ? false : true;

                }
                else
                {
                    envioCorreo.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.SolicitudesFuenteRegistrada);
                }

                clsDatos.RegistrarBitacora((int)Constantes.Accion.EnviarCorreoEncargado, EtiquetasViewRegistroIndicadorFonatel.SistemaAutomatico, modulo, objeto.Codigo);

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
                    objeto.idFormularioWeb = temp;
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

            ResultadoConsulta.Clase = modulo;
            
            ResultadoConsulta.Usuario = user;
            if (!string.IsNullOrEmpty(objeto.FormularioId))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.FormularioId), out int temp);
                objeto.idFormularioWeb = temp;
            }
            if (!string.IsNullOrEmpty(objeto.Solicitudid))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.Solicitudid), out int temp);
                objeto.IdSolicitud = temp;
            }

            switch (objeto.IdEstado)
            {
                case 1:
                    objeto.Estado = EtiquetasViewRegistroIndicadorFonatel.EstadoSolicitudProceso;
                    break;
                case 6:
                    objeto.Estado = EtiquetasViewRegistroIndicadorFonatel.EstadoSolicitudEnviado;
                    break;
                default:
                    break;
            }

            //89482 obtención de datos para bitacora
            string JsonAnterior = 
                ObtenerDatos(new RegistroIndicadorFonatel() {idFormularioWeb=objeto.idFormularioWeb
                ,IdSolicitud=objeto.IdSolicitud }).objetoRespuesta[0].ToString();

            var result = clsDatos.ActualizarRegistroIndicadorFonatel(objeto);

            ResultadoConsulta.objetoRespuesta = result;
            ResultadoConsulta.CantidadRegistros = result.Count();

            ResultadoConsulta.Accion = (int)Accion.Editar;
            //89482 datos actual para bitacora
            string JsonActual = objeto.ToString();
            //89482 registrar bitacora
             //clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
             //       ResultadoConsulta.Usuario,
             //           ResultadoConsulta.Clase, objeto.IdSolicitud.ToString()
             //           , JsonActual, JsonAnterior, "");

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
                List<DetalleFuenteRegistro> detalle = 
                    clsFuentesRegistroDestinatarioDAL.ObtenerDatos(new DetalleFuenteRegistro()).Where(x => x.CorreoElectronico == usuario).ToList();

                FuenteRegistro fuente = clsFuentesRegistroDAL.ObtenerDatos(new FuenteRegistro()).Where(x => detalle.Any(y => y.idFuenteRegistro== x.IdFuenteRegistro)).FirstOrDefault();

                if (fuente != null)
                {
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Consultar;
                    objeto.IdFuente = fuente.IdFuenteRegistro;
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
                objeto.IdEstado = (int)Constantes.EstadosRegistro.Enviado;
                objeto.RangoFecha = false;
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

        /// <summary>
        /// Fecha 04-04-2023
        /// Georgi Mesen Cerdas
        /// Metodo para registrar la bitacora cuando se descarga la plantilla de registro indicador
        /// </summary>
        /// <returns></returns>
        public void BitacoraDescargar(RegistroIndicadorFonatel objeto)
        {
            RespuestaConsulta<List<RegistroIndicadorFonatel>> resultado = new RespuestaConsulta<List<RegistroIndicadorFonatel>>();
            resultado.Clase = modulo;
            resultado.Accion = (int)Accion.Descargar;
            resultado.Usuario = user;

            clsDatos.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario,
                            resultado.Clase, objeto.Solicitudid, "", "", "");
        }
    }
}