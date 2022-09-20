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
    public class FuentesRegistroDestinatariosBL : IMetodos<DetalleFuentesRegistro>
    {
        private readonly FuentesRegistroDestinatarioDAL clsDatos;
        private readonly FuentesRegistroDAL clsfuente;

        private RespuestaConsulta<List<DetalleFuentesRegistro>> ResultadoConsulta;
        string modulo = string.Empty;
        string user = string.Empty;
        public FuentesRegistroDestinatariosBL(string modulo, string user )
        {
            this.clsDatos = new FuentesRegistroDestinatarioDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleFuentesRegistro>>();
            this.clsfuente = new FuentesRegistroDAL();
        }
        private string SerializarObjetoBitacora(DetalleFuentesRegistro objDestinatario)
        {
            return JsonConvert.SerializeObject(objDestinatario, new JsonSerializerSettings
            { ContractResolver = new JsonIgnoreResolver(objDestinatario.NoSerialize) });
        }

        public RespuestaConsulta<List<DetalleFuentesRegistro>> ActualizarElemento(DetalleFuentesRegistro objeto)
        {

            try
            {

                if (!string.IsNullOrEmpty(objeto.FuenteId))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.FuenteId), out temp);
                    objeto.idFuente = temp;
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Usuario = objeto.Usuario;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                string nombre = objeto.NombreDestinatario.Trim();
                string Correo = objeto.CorreoElectronico.Trim();
                var resul = clsfuente.ObtenerDatos(new FuentesRegistro() {idFuente=objeto.idFuente }).SingleOrDefault();
                var listado = resul.DetalleFuentesRegistro.Where(x => x.idDetalleFuente == objeto.idDetalleFuente).ToList();
                if (listado.Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if(listado.Where(x=>x.idFuente!=objeto.idFuente && objeto.CorreoElectronico.ToUpper()==Correo.ToUpper()).Count()>0)
                {
                    throw new Exception(Errores.CorreoRegistrado);
                }
                else if (listado.Where(x => x.idFuente != objeto.idFuente && objeto.NombreDestinatario.ToUpper() == nombre.ToUpper()).Count() > 0)
                {
                    throw new Exception(Errores.NombreRegistrado);
                }
                else if (!Utilidades.ValidarEmail(Correo))
                {
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "correo electrónico"));
                }
                else
                {
                    objeto = listado.Single();
                    objeto.NombreDestinatario = nombre;
                    objeto.CorreoElectronico = Correo;
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                             ResultadoConsulta.Usuario,
                             ResultadoConsulta.Clase, resul.Fuente);
                }
                    
               
            }
            catch (Exception ex)
            {

                if (ex.Message == Errores.CorreoRegistrado || ex.Message== Errores.NoRegistrosActualizar 
                    || ex.Message==Errores.NombreRegistrado || ex.Message== string.Format(Errores.CampoConFormatoInvalido, "correo electrónico"))
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
                ResultadoConsulta.Usuario = user;
                objeto.Usuario=user;
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
                objeto.Usuario = user;
                ResultadoConsulta.Usuario = user;

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
                else if (consultardatos.Where(x => x.idFuente != objeto.idFuente && objeto.NombreDestinatario.ToUpper() == Nombre.ToUpper()).Count() > 0)
                {
                    throw new Exception(Errores.NombreRegistrado);
                }

                else if (!Utilidades.ValidarEmail(Correo))
                {
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "correo electrónico"));
                }
                else
                {
                    objeto.CorreoElectronico = Correo;
                    objeto.NombreDestinatario = Nombre;
                    var resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();

                    string jsonincial = SerializarObjetoBitacora(objeto);

                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                             ResultadoConsulta.Usuario,
                             ResultadoConsulta.Clase, fuente.Fuente,"","",jsonincial);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message== Errores.CorreoRegistrado 
                    || ex.Message== Errores.CantidadRegistrosLimite 
                    || ex.Message == string.Format(Errores.CampoConFormatoInvalido, "correo electrónico")
                    || ex.Message == Errores.NombreRegistrado)
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
            try
            {
              
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objDetalleFuentesRegistro);
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



        public RespuestaConsulta<List<DetalleFuentesRegistro>> ValidarDatos(DetalleFuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }
    }
}
