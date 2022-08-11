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

   

        public RespuestaConsulta<List<CategoriasDesagregacion>> ObtenerDatos(CategoriasDesagregacion objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idCategoria = temp;
                    }
                }
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

        public RespuestaConsulta<List<CategoriasDesagregacion>> ValidarDatos(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<CategoriasDesagregacion>> ActualizarElemento(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<CategoriasDesagregacion>> CambioEstado(CategoriasDesagregacion objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idCategoria = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                int nuevoEstado = objeto.idEstado;
                objeto.idEstado = 0;
                ResultadoConsulta.Usuario = objeto.UsuarioModificacion;
                var resul = clsDatos.ObtenerDatos(objeto);
                objeto = resul.Single();
                objeto.idEstado = nuevoEstado;
                objeto.UsuarioModificacion = ResultadoConsulta.Usuario;
                ResultadoConsulta.Accion= (int)EstadosRegistro.Activo == objeto.idEstado ? (int)Accion.Activar : (int)Accion.Inactiva;
                resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                       ResultadoConsulta.Clase,objeto.Codigo);

            }
            catch (Exception ex)
            { 
                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<CategoriasDesagregacion>> ClonarDatos(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<CategoriasDesagregacion>> EliminarElemento(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<CategoriasDesagregacion>> InsertarDatos(CategoriasDesagregacion objeto)
        {
            try
            {
                objeto.idCategoria = 0;
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;
                List<CategoriasDesagregacion> buscarRegistro = clsDatos.ObtenerDatos(new CategoriasDesagregacion());

                if (buscarRegistro.Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).ToList().Count() > 0)
                {
                    throw new Exception(Errores.CodigoRegistrado);
                }
                else if (buscarRegistro.Where(x => x.NombreCategoria.ToUpper() == objeto.NombreCategoria.ToUpper()).ToList().Count() > 0)
                {
                    throw new Exception(Errores.NombreRegistrado);
                }
                else
                {
                    objeto.idEstado = (int)EstadosRegistro.Desactivado;
                    var result =clsDatos.ActualizarDatos(objeto).Where(x=>x.Codigo==objeto.Codigo).FirstOrDefault();
                    if (objeto.idTipoDetalle== (int)TipoDetalleCategoriaEnum.Fecha)
                    {
                        objeto.DetalleCategoriaFecha.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaFecha.Estado = true;
                        clsDatos.InsertarDetalleFecha(objeto.DetalleCategoriaFecha);
                    }
                    else if(objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Numerico)
                    {
                        objeto.DetalleCategoriaNumerico.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaNumerico.Estado = true;
                        clsDatos.InsertarDetalleNumerico(objeto.DetalleCategoriaNumerico);
                    }

  
                }

                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo);
            }
            catch (Exception ex)
            {

                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }
    }
}
