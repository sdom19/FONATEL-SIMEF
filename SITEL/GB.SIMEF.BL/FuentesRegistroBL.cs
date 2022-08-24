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
    public class FuentesRegistroBL : IMetodos<FuentesRegistro>
    {
        private readonly FuentesRegistroDAL clsDatos;
        private readonly CategoriasDesagregacionDAL clsDatosTexto;

        private RespuestaConsulta<List<FuentesRegistro>> ResultadoConsulta;
        string modulo = EtiquetasViewFuentesRegistro.FuentesRegistro;

        public FuentesRegistroBL()
        {
            this.clsDatos = new FuentesRegistroDAL();
            this.clsDatosTexto = new CategoriasDesagregacionDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<FuentesRegistro>>();
        }
        /// <summary>
        /// Evalua si la fuente genero cambios para actualizar
        /// Michael Hernández Cordero
        /// 24/08/2022
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>

        public RespuestaConsulta<List<FuentesRegistro>> ActualizarElemento(FuentesRegistro objeto)
        {


            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idFuente = temp;
                    }
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion =  (int)Accion.Editar;
                    ResultadoConsulta.Usuario = objeto.UsuarioModificacion;

                    string fuente = objeto.Fuente.Trim();
                    int Cantidad = objeto.CantidadDestinatario;

                    var resul = clsDatos.ObtenerDatos(new FuentesRegistro());
                    objeto = resul.Where(x => x.idFuente == objeto.idFuente).Single();


                    if (objeto.CantidadDestinatario == Cantidad && objeto.Fuente == fuente.ToUpper())
                    {
                        ResultadoConsulta.objetoRespuesta= resul.Where(x => x.idFuente == objeto.idFuente).ToList();
                        ResultadoConsulta.CantidadRegistros = 1;
                        return ResultadoConsulta;
                    }
                    else if (resul.Where(x => x.idFuente == objeto.idFuente).Count() == 0)
                    {
                        throw new Exception(Errores.NoRegistrosActualizar);
                    }
                   else if (resul.Where(x => x.idFuente != objeto.idFuente && x.Fuente.ToUpper()==fuente.ToUpper()).Count() > 0)
                    {
                        throw new Exception(Errores.FuenteRegistrada);
                    }
                    else if (Cantidad <= 0)
                    {
                        throw new Exception(Errores.FuentesCantidadDestiatarios);
                    }
                    else if (Cantidad < objeto.DetalleFuentesRegistro.Count())
                    {
                        throw new Exception(Errores.CantidadRegistrosLimite);
                    }
                   
                    else
                    {

                        objeto.Fuente = fuente;
                        objeto.CantidadDestinatario = Cantidad;

                        resul = clsDatos.ActualizarDatos(objeto);
                        ResultadoConsulta.objetoRespuesta = resul;
                        ResultadoConsulta.CantidadRegistros = resul.Count();
                        clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                               ResultadoConsulta.Usuario,
                               ResultadoConsulta.Clase, objeto.Fuente);
                    }
                }
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
        /// <summary>
        /// Activa la fuente en el proceso de finalizar
        /// Michael Hernández Cordero
        /// 24-08-2022
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FuentesRegistro>> CambioEstado(FuentesRegistro objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idFuente = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                int nuevoEstado = (int)Constantes.EstadosRegistro.Activo;
                ResultadoConsulta.Usuario = objeto.UsuarioModificacion;
                var resul = clsDatos.ObtenerDatos(objeto).ToList();
                var fuente = resul.Single();
                if (resul.Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (fuente.CantidadDestinatario != fuente.DetalleFuentesRegistro.Count())
                {
                    throw new Exception(Errores.CantidadDestinatariosIncorrecta);
                }
                else if (fuente.CantidadDestinatario ==0)
                {
                    throw new Exception(Errores.FuentesCantidadDestiatarios);
                }
                else
                {



                    objeto = fuente;
                    objeto.idEstado = nuevoEstado;
                    objeto.UsuarioModificacion = ResultadoConsulta.Usuario;
                    ResultadoConsulta.Accion = (int)Accion.Activar;
                    resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                           ResultadoConsulta.Usuario,
                           ResultadoConsulta.Clase, objeto.Fuente);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.NoRegistrosActualizar || ex.Message== Errores.CantidadDestinatariosIncorrecta || ex.Message== Errores.FuentesCantidadDestiatarios)
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

        public RespuestaConsulta<List<FuentesRegistro>> ClonarDatos(FuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Elimina la fuente desde de manera lógica, estado 4
        /// Michael Hernández Cordero
        /// 24/08/2022
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>

        public RespuestaConsulta<List<FuentesRegistro>> EliminarElemento(FuentesRegistro objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idFuente = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                int nuevoEstado = (int)Constantes.EstadosRegistro.Eliminado;
                ResultadoConsulta.Usuario = objeto.UsuarioModificacion;
                var resul = clsDatos.ObtenerDatos(objeto).ToList();

                if (resul.Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else
                {

                    objeto = resul.Single();
                    objeto.idEstado = nuevoEstado;
                    objeto.UsuarioModificacion = ResultadoConsulta.Usuario;
                    ResultadoConsulta.Accion = (int)Accion.Eliminar;
                    resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                           ResultadoConsulta.Usuario,
                           ResultadoConsulta.Clase, objeto.Fuente);
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
        /// <summary>
        /// Crea la fuente en estado en proceso 
        /// Michael Hernández Cordero
        /// 24-08-2022
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FuentesRegistro>> InsertarDatos(FuentesRegistro objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;
                objeto.Fuente = objeto.Fuente.Trim();
                objeto.idEstado = (int)EstadosRegistro.EnProceso;
                var consultardatos = clsDatos.ObtenerDatos(new FuentesRegistro());

                if (consultardatos.Where(x=>x.Fuente.ToUpper()==objeto.Fuente.ToUpper()).Count()>0)
                {
                    throw new Exception(Errores.FuenteRegistrada);
                }


                if (objeto.CantidadDestinatario<=0)
                {
                    throw new Exception(Errores.FuentesCantidadDestiatarios);
                }



                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                     ResultadoConsulta.Usuario,
                     ResultadoConsulta.Clase, objeto.Fuente);
            }
            catch (Exception ex)
            {
                if (ex.Message== Errores.FuenteRegistrada || ex.Message == Errores.FuentesCantidadDestiatarios)
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


        /// <summary>
        /// Fecha 10/08/2022
        /// Michael Hernández
        /// Metodo para obtener la lista de fuentes
        /// </summary>
        public RespuestaConsulta<List<FuentesRegistro>> ObtenerDatos(FuentesRegistro objFuentesRegistro)
        {
            try
            {
                if (!string.IsNullOrEmpty(objFuentesRegistro.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objFuentesRegistro.id), out temp);
                    objFuentesRegistro.idFuente = temp;
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objFuentesRegistro);
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

            public RespuestaConsulta<List<FuentesRegistro>> ValidarDatos(FuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }
    }
}
