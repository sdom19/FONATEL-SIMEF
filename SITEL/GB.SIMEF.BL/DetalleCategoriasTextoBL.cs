using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using OfficeOpenXml;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class DetalleCategoriasTextoBL : IMetodos<DetalleCategoriaTexto>
    {
        private readonly DetalleCategoriaTextoDAL clsDatos;

        private readonly CategoriasDesagregacionDAL clsDatosCategoria;

        private readonly RelacionCategoriaDAL clsRelacionCategoriaDetalles;

        private RespuestaConsulta<List<DetalleCategoriaTexto>> ResultadoConsulta;
        string modulo = Etiquetas.DetalleCategorias;

        string user = string.Empty;

        public DetalleCategoriasTextoBL(string modulo, string user)
        {
            this.user = user;
            this.modulo = modulo;
            clsDatos = new DetalleCategoriaTextoDAL();
            clsDatosCategoria = new CategoriasDesagregacionDAL();
            clsRelacionCategoriaDetalles = new RelacionCategoriaDAL();
            ResultadoConsulta = new RespuestaConsulta<List<DetalleCategoriaTexto>>();
        }
        private string SerializarObjetoBitacora(DetalleCategoriaTexto objCategoria)
        {
            return JsonConvert.SerializeObject(objCategoria, new JsonSerializerSettings
            { ContractResolver = new JsonIgnoreResolver(objCategoria.NoSerialize) });
        }
        public RespuestaConsulta<List<DetalleCategoriaTexto>> ActualizarElemento(DetalleCategoriaTexto objeto)
        {
            try
            {
                objeto.Estado = true;
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = user;
                objeto.usuario=user;
                objeto = ValidarObjeto(objeto, false);
                string jsonAnterior = objeto.Json;

                ResultadoConsulta.objetoRespuesta= clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                objeto = ResultadoConsulta.objetoRespuesta.Single();
                string jsonActual = SerializarObjetoBitacora(objeto);
                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                     ResultadoConsulta.Usuario,
                     ResultadoConsulta.Clase, string.Format("{0}/{1}",
                     objeto.CodigoCategoria, objeto.Codigo),jsonActual,jsonAnterior);
                
            }
            catch (Exception ex)
            {

                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> CambioEstado(DetalleCategoriaTexto objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> ClonarDatos(DetalleCategoriaTexto objeto)
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
                ResultadoConsulta.Usuario = user;
                objeto.usuario = user;
                DetalleCategoriaTexto registroActializar;
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objCategoria.id = Utilidades.Desencriptar(objCategoria.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objCategoria.idDetalleCategoriaTexto = temp;
                    }
                }
                var resul = clsDatos.ObtenerDatos(objCategoria);
                if (resul.Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);

                }
                else
                {
                    var detallesRelacion = clsRelacionCategoriaDetalles.ObtenerRelacionCategoriaAtributoXIdCategoriaDetalle(objCategoria.idDetalleCategoriaTexto);
                    foreach (RelacionCategoriaAtributo item in detallesRelacion)
                    {
                        item.idDetalleCategoriaTextoAtributo = 0;
                        clsRelacionCategoriaDetalles.ActualizarRelacionAtributo(item);
                        RelacionCategoriaId rel = new RelacionCategoriaId() { idRelacionCategoriaId = item.idRelacionCategoriaId, idCategoriaDesagregacion = item.idCategoriaDesagregacion, idEstadoRegistro = (int)Constantes.EstadosRegistro.EnProceso, OpcionEliminar = false };
                        clsRelacionCategoriaDetalles.ActualizarRelacionCategoriaidSinReturn(rel);
                        //break;
                        //clsRelacionCategoriaDetalles.EliminarDetalleRelacionCategoria(item);
                    }
                    registroActializar = resul.SingleOrDefault();
                    registroActializar.Estado = false;
                    resul = clsDatos.ActualizarDatos(registroActializar);
                }
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion, ResultadoConsulta.Usuario,
                    ResultadoConsulta.Clase,
                   string.Format("{0}/{1}",
                   registroActializar.CodigoCategoria,
                   registroActializar.Codigo)
                 );
            }
            catch (Exception ex)
            {
                ResultadoConsulta.MensajeError = ex.Message;
                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
            }
            return ResultadoConsulta;
        }

        public DetalleCategoriaTexto ValidarObjeto(DetalleCategoriaTexto objeto, bool Agregar=true)
        {

            if (!string.IsNullOrEmpty(objeto.categoriaid))
            {
                int temp = 0;
                int.TryParse(Utilidades.Desencriptar(objeto.categoriaid), out temp);
                objeto.idCategoriaDesagregacion = temp;
            }
            if (!string.IsNullOrEmpty(objeto.id))
            {
                int temp = 0;
                int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                objeto.idDetalleCategoriaTexto = temp;
            }

            objeto.usuario = user;
            var categoria =
                       clsDatosCategoria.ObtenerDatos(new CategoriaDesagregacion() { idCategoriaDesagregacion = objeto.idCategoriaDesagregacion 
                       }).Where(x => x.idEstadoRegistro != (int)Constantes.EstadosRegistro.Eliminado).Single();
            int cantidadDisponible = (int)categoria.CantidadDetalleDesagregacion
                                        - categoria.DetalleCategoriaTexto.Count();

            List<DetalleCategoriaTexto> detalleCategoria = categoria.DetalleCategoriaTexto.
                                 Where(x=>x.idCategoriaDesagregacion == objeto.idCategoriaDesagregacion ).ToList();

            objeto.Json = Agregar == true ? string.Empty : SerializarObjetoBitacora(detalleCategoria.Where(x => x.Codigo == objeto.Codigo).Single());

            if (cantidadDisponible <= 0 && Agregar)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(Errores.CantidadRegistros);
            }
            else if (detalleCategoria.Where(x => x.Codigo == objeto.Codigo && Agregar).Count() > 0)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(Errores.CodigoRegistrado);
            }
            else if (detalleCategoria.Where(x => x.Etiqueta == objeto.Etiqueta.ToUpper()).Count() > 0 && Agregar)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(Errores.EtiquetaRegistrada);
            }
            else if (detalleCategoria.Where(x => x.Codigo == objeto.Codigo && x.idDetalleCategoriaTexto!=objeto.idDetalleCategoriaTexto).Count() > 0 && !Agregar)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(Errores.CodigoRegistrado);
            }
            else if (detalleCategoria.Where(x => x.Etiqueta == objeto.Etiqueta.ToUpper() && x.idDetalleCategoriaTexto != objeto.idDetalleCategoriaTexto).Count() > 0 && !Agregar)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(Errores.EtiquetaRegistrada);
            }

            else if (!Utilidades.rx_soloTexto.Match(objeto.Etiqueta.Trim()).Success && objeto.CategoriasDesagregacion.IdTipoDetalleCategoria == (int)Constantes.TipoDetalleCategoriaEnum.Texto)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "Etiqueta"));
            }
            else if (!Utilidades.rx_alfanumerico.Match(objeto.Etiqueta.Trim()).Success && objeto.CategoriasDesagregacion.IdTipoDetalleCategoria == (int)Constantes.TipoDetalleCategoriaEnum.Alfanumerico)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "Etiqueta"));
            }
            return objeto;
        }


        public RespuestaConsulta<List<DetalleCategoriaTexto>> InsertarDatos(DetalleCategoriaTexto objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Crear;
                ResultadoConsulta.Usuario = user;
                objeto = ValidarObjeto(objeto);
                objeto.Estado = true;
                ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                string jsonInicial = SerializarObjetoBitacora(objeto);
                      clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                       ResultadoConsulta.Clase, string.Format("{0}/{1}",
                       objeto.CategoriasDesagregacion.Codigo, objeto.Codigo),"","",jsonInicial);             
            }
            catch (Exception ex)
            {

                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                { 
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> ObtenerDatos(DetalleCategoriaTexto objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idDetalleCategoriaTexto = temp;
                }
                if (!string.IsNullOrEmpty(objeto.categoriaid))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.categoriaid), out temp);
                    objeto.idCategoriaDesagregacion = temp;
                }


                var resul = clsDatos.ObtenerDatos(objeto);
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

        public RespuestaConsulta<List<DetalleCategoriaTexto>> ValidarDatos(DetalleCategoriaTexto objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<DetalleCategoriaTexto>> CargarExcel(HttpPostedFileBase file)
        {
          
            using (var package = new ExcelPackage(file.InputStream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                string Codigo = worksheet.Name;
                int cantFinal = 0;

                CategoriaDesagregacion categoria =
                                     clsDatosCategoria.ObtenerDatos(new CategoriaDesagregacion() { Codigo = Codigo })
                                     .Where(x => x.idEstadoRegistro != (int)Constantes.EstadosRegistro.Eliminado)
                                    .SingleOrDefault();
                ResultadoConsulta.objetoRespuesta = new List<DetalleCategoriaTexto>();
                for (int i = 0; i < categoria.CantidadDetalleDesagregacion; i++)
                {
                    int fila = i + 10;
                    if (worksheet.Cells[fila, 1].Value != null || worksheet.Cells[fila, 2].Value != null)
                    {
                        cantFinal = i;
                        int codigo = 0;
                        string Etiqueta = string.Empty;
                        int.TryParse(worksheet.Cells[fila, 1].Value.ToString().Trim(), out codigo);
                        Etiqueta = worksheet.Cells[fila, 2].Value.ToString().Trim();

                        var detallecategoria = new DetalleCategoriaTexto()
                        {
                            idCategoriaDesagregacion = categoria.idCategoriaDesagregacion,
                            Codigo = codigo,
                            Etiqueta = Etiqueta,
                            Estado = true
                        };

                        DetalleCategoriaTexto consultarCategoria = categoria.DetalleCategoriaTexto.Where
                                (x=>x.Codigo == detallecategoria.Codigo && x.idCategoriaDesagregacion == detallecategoria.idCategoriaDesagregacion  ).SingleOrDefault();
                        
                        if (consultarCategoria==null)
                        {
                            detallecategoria.CategoriasDesagregacion = categoria;
                            if (InsertarDatos(detallecategoria).HayError != 0)
                            {
                                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                                break;
                            }
                        }
                        else
                        {
                            consultarCategoria.Etiqueta = detallecategoria.Etiqueta;
                            consultarCategoria.CategoriasDesagregacion = categoria;
                            if (ActualizarElemento(consultarCategoria).HayError != 0)
                            {
                                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                                break;
                            }
                        }
                        ResultadoConsulta.objetoRespuesta.Add(detallecategoria);
                    }
                }
                if (cantFinal+1 == categoria.CantidadDetalleDesagregacion)
                {
                    categoria.idEstadoRegistro = (int)Constantes.EstadosRegistro.Activo;
                    clsDatosCategoria.ActualizarDatos(categoria);
                }
                else if (cantFinal == 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    ResultadoConsulta.MensajeError = Errores.ErrorCargarDetalles;
                }
            }

            return ResultadoConsulta;
        }
    }
}
