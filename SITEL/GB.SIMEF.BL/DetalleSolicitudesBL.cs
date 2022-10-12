﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using OfficeOpenXml;
using static GB.SIMEF.Resources.Constantes;
namespace GB.SIMEF.BL
{
    public class DetalleSolicitudesBL : IMetodos<DetalleSolicitudFormulario>
    {
        private readonly DetalleSolicitudesDAL clsDatos;

        private readonly SolicitudDAL clsDatosSolicitud;

        string modulo = EtiquetasViewSolicitudes.Solicitudes;

        private RespuestaConsulta<List<DetalleSolicitudFormulario>> ResultadoConsulta;

        public DetalleSolicitudesBL()
        {
            clsDatos = new DetalleSolicitudesDAL();
            clsDatosSolicitud = new SolicitudDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleSolicitudFormulario>>();
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> ActualizarElemento(DetalleSolicitudFormulario objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> CambioEstado(DetalleSolicitudFormulario objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;

                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.IdSolicitud = temp;
                    }
                }

                if (!String.IsNullOrEmpty(objeto.Formularioid))
                {
                    objeto.Formularioid = Utilidades.Desencriptar(objeto.Formularioid);
                    int temp;
                    if (int.TryParse(objeto.Formularioid, out temp))
                    {
                        objeto.IdFormulario = temp;
                    }
                }


                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;

            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.NombreRegistrado
                    || ex.Message == Errores.ValorMinimo || ex.Message == Errores.ValorFecha)
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

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> ClonarDatos(DetalleSolicitudFormulario objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> EliminarElemento(DetalleSolicitudFormulario objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> InsertarDatos(DetalleSolicitudFormulario objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;

                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.IdSolicitud = temp;
                    }
                }

                if (!String.IsNullOrEmpty(objeto.Formularioid))
                {
                    objeto.Formularioid = Utilidades.Desencriptar(objeto.Formularioid);
                    int temp;
                    if (int.TryParse(objeto.Formularioid, out temp))
                    {
                        objeto.IdFormulario = temp;
                    }
                }

               
                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                
            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.NombreRegistrado
                    || ex.Message == Errores.ValorMinimo || ex.Message == Errores.ValorFecha)
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

        public RespuestaConsulta<List<FormularioWeb>> ObtenerListaFormularios(DetalleSolicitudFormulario objeto)
        {

            RespuestaConsulta<List<FormularioWeb>> ResulFormularios = new RespuestaConsulta<List<FormularioWeb>>();

            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.IdSolicitud = temp;
                    }
                }

                if (!String.IsNullOrEmpty(objeto.Formularioid))
                {
                    objeto.Formularioid = Utilidades.Desencriptar(objeto.Formularioid);
                    int temp;
                    if (int.TryParse(objeto.Formularioid, out temp))
                    {
                        objeto.IdFormulario = temp;
                    }
                }

                var resul = clsDatos.ObtenerListaFormularios(objeto);

                ResulFormularios.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

            }
            catch (Exception ex)
            {
                ResulFormularios.HayError = (int)Constantes.Error.ErrorSistema;
                ResulFormularios.MensajeError = ex.Message;
            }

            return ResulFormularios;
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> ValidarDatos(DetalleSolicitudFormulario objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> ObtenerDatos(DetalleSolicitudFormulario objeto)
        {
            throw new NotImplementedException();
        }

    }
}
