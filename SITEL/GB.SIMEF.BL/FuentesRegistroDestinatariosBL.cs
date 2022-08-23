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
    public class FuentesRegistroDestinatariosBL : IMetodos<DetalleFuentesRegistro>
    {
        private readonly FuentesRegistroDestinatarioDAL clsDatos;
        private readonly FuentesRegistroDAL clsfuente;

        private RespuestaConsulta<List<DetalleFuentesRegistro>> ResultadoConsulta;
        string modulo = EtiquetasViewFuentesRegistro.FuentesRegistro;

        public FuentesRegistroDestinatariosBL()
        {
            this.clsDatos = new FuentesRegistroDestinatarioDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleFuentesRegistro>>();
            this.clsfuente = new FuentesRegistroDAL();
        }

        public RespuestaConsulta<List<DetalleFuentesRegistro>> ActualizarElemento(DetalleFuentesRegistro objeto)
        {

            try
            {
               
               var resul = clsDatos.ObtenerDatos(new DetalleFuentesRegistro());
               objeto = resul.Where(x => x.idFuente == objeto.idFuente).Single();
                resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                    
               
            }
            catch (Exception ex)
            {

                if (ex.Message == Errores.NoRegistrosActualizar || ex.Message == Errores.FuentesCantidadDestiatarios 
                    || ex.Message == Errores.CantidadRegistrosLimite || ex.Message== Errores.FuenteRegistrada)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                }


                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleFuentesRegistro>> CambioEstado(DetalleFuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleFuentesRegistro>> ClonarDatos(DetalleFuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleFuentesRegistro>> EliminarElemento(DetalleFuentesRegistro objeto)
        {
            try
            {
                objeto.Estado = false;
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = objeto.Usuario;
                if (!String.IsNullOrEmpty(objeto.FuenteId))
                {
                    objeto.FuenteId = Utilidades.Desencriptar(objeto.FuenteId);
                    int temp;
                    if (int.TryParse(objeto.FuenteId, out temp))
                    {
                        objeto.idFuente = temp;
                    }
                }
                var fuente = clsfuente.ObtenerDatos(new FuentesRegistro() { idFuente = objeto.idFuente }).SingleOrDefault();

                var consultardatos = fuente.DetalleFuentesRegistro.Where(x=>x.idDetalleFuente==objeto.idDetalleFuente).ToList();
                if (consultardatos.Count()==0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else
                {
                    var resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();


                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                              ResultadoConsulta.Usuario,
                              ResultadoConsulta.Clase, fuente.Fuente);
                }                
            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.NoRegistrosActualizar)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                }


                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleFuentesRegistro>> InsertarDatos(DetalleFuentesRegistro objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;

                string Nombre = objeto.NombreDestinatario.Trim();
                string Correo = objeto.CorreoElectronico.Trim();
                objeto.Estado = true;
                if (!string.IsNullOrEmpty(objeto.FuenteId))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.FuenteId), out temp);
                    objeto.idFuente = temp;
                }

                var fuente = clsfuente.ObtenerDatos(new FuentesRegistro() {idFuente=objeto.idFuente }).SingleOrDefault();

                var consultardatos = fuente.DetalleFuentesRegistro;

                if (consultardatos.Where(x => x.CorreoElectronico.ToUpper() == Correo.ToUpper()).Count() > 0)
                {
                    throw new Exception(Errores.CorreoRegistrado);
                }
                else if (consultardatos.Count() >= fuente.CantidadDestinatario)
                {
                    throw new Exception(Errores.CantidadRegistrosLimite);
                }
                else
                {
                    objeto.CorreoElectronico = Correo;
                    objeto.NombreDestinatario = Nombre;
                    var resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message== Errores.CorreoRegistrado || ex.Message== Errores.CantidadRegistrosLimite)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                }

                
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleFuentesRegistro>> ObtenerDatos(DetalleFuentesRegistro objDetalleFuentesRegistro)
        {
            throw new NotImplementedException();
        }



        public RespuestaConsulta<List<DetalleFuentesRegistro>> ValidarDatos(DetalleFuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }
    }
}
