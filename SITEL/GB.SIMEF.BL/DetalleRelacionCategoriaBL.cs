using System;
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
    public class DetalleRelacionCategoriaBL : IMetodos<DetalleRelacionCategoria>
    {
        private readonly DetalleRelacionCategoriaDAL clsDatos;

        private readonly RelacionCategoriaDAL clsDatosRelacionCategoria;

        private readonly CategoriasDesagregacionDAL clsDatosCategorias;

        private RespuestaConsulta<List<DetalleRelacionCategoria>> ResultadoConsulta;

        string modulo = string.Empty;
        string user = string.Empty;

        public DetalleRelacionCategoriaBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            clsDatos = new DetalleRelacionCategoriaDAL();
            clsDatosRelacionCategoria = new RelacionCategoriaDAL();
            ResultadoConsulta = new RespuestaConsulta<List<DetalleRelacionCategoria>>();
            clsDatosCategorias = new CategoriasDesagregacionDAL();
        }

        public void CargarExcel(HttpPostedFileBase file) //NOMBRE DEL ARCHIVO Y UN CODIGO - SI
        {

            List<DetalleRelacionCategoria> lista = new List<DetalleRelacionCategoria>();
            Boolean ind = true;

            using (var package = new ExcelPackage(file.InputStream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1]; //POSICION DEL CODIGO y NOMBRE
                string Codigo = worksheet.Name; //POSICION DEL CODIGO

                int codigoRelacionCategoria = Convert.ToInt32(worksheet.Cells[2, 1].Value.ToString());

                RelacionCategoria relacion = clsDatosRelacionCategoria.ObtenerDatos(new RelacionCategoria() { idRelacionCategoria = codigoRelacionCategoria }).SingleOrDefault();

                //relacion.DetalleRelacionCategoria = new List<DetalleRelacionCategoria>(); //TRAE LA CANTIDAD 

                //for (int i = 2; i < relacion.CantidadCategoria + 2; i++) //Recorre las columnas
                //{

                //    if (worksheet.Cells[2, i].Value != null || worksheet.Cells[1, i].Value != null) //Se revisa si tienen valores o no
                //    {
                //        string NombreCategoria = string.Empty;
                //        string CategoriaTexto = string.Empty;

                //        NombreCategoria = worksheet.Cells[1, i].Value.ToString().Trim();
                //        CategoriaTexto = worksheet.Cells[2, i].Value.ToString().Trim();

                //        DetalleCategoriaTexto categoriaTexto = clsDatosCategorias.ObtenerCategoriasParaExcel(NombreCategoria, CategoriaTexto).SingleOrDefault();

                //        if (categoriaTexto != null)
                //        {
                //            var detallerelacion = new DetalleRelacionCategoria()
                //            {
                //                IdRelacionCategoria = relacion.idRelacionCategoria,
                //                idCategoriaAtributo = categoriaTexto.idCategoria,
                //                idCategoriaDetalle = categoriaTexto.idCategoriaDetalle,
                //                Estado = true
                //            };

                //            lista.Add(detallerelacion);
                //        }
                //        else
                //        {
                //            ind = false;
                //            break;
                //        }
                //    }

                //}

                if (ind)
                {
                    foreach (DetalleRelacionCategoria item in lista)
                    {
                        InsertarDatos(item);
                    }
                }         
            }
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ActualizarElemento(DetalleRelacionCategoria objeto)
        {
            try
            {
                objeto.Estado = true;
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = user;
                objeto.usuario = user;

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.IdRelacionCategoria = temp;
                }
              
                var RegistrosEncontrados = clsDatos.ObtenerDatos(new DetalleRelacionCategoria()
                { IdRelacionCategoria = objeto.IdRelacionCategoria }).ToList();

                RegistrosEncontrados = RegistrosEncontrados.Where(x => x.idCategoriaAtributo == objeto.idCategoriaAtributo).ToList();

                if (RegistrosEncontrados.Where(x=>x.idCategoriaDetalle == objeto.idCategoriaDetalle).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }
                else
                {
                    var relacion = clsDatosRelacionCategoria
                      .ObtenerDatos(new RelacionCategoria() { idRelacionCategoria = objeto.IdRelacionCategoria }).Single();

                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();

                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                     ResultadoConsulta.Usuario,
                     ResultadoConsulta.Clase, string.Format("{0}/{1}",
                     relacion.idCategoriaValor, objeto.idCategoriaAtributo));
                }
            }
            catch (Exception ex)
            {

                if (ResultadoConsulta.HayError!= (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError= (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> CambioEstado(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ClonarDatos(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> EliminarElemento(DetalleRelacionCategoria objeto)
        {
            
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;
                ResultadoConsulta.Usuario = user;
                objeto.usuario = user;
                DetalleRelacionCategoria registroActualizar;

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idDetalleRelacionCategoria = temp;
                }

                if (!string.IsNullOrEmpty(objeto.relacionid))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.relacionid), out temp);
                    objeto.IdRelacionCategoria = temp;
                }

                var resul = clsDatos.ObtenerDatos(objeto);

                objeto.RelacionCategoria =
                    clsDatosRelacionCategoria.ObtenerDatos(new RelacionCategoria() { idRelacionCategoria = objeto.IdRelacionCategoria }).Single();

                int cantidadDisponible = (int)objeto.RelacionCategoria.CantidadCategoria
                            - objeto.RelacionCategoria.DetalleRelacionCategoria.Count();

                if (resul.Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);

                }
                else
                {
                    registroActualizar = resul.SingleOrDefault();
                    registroActualizar.Estado = false;
                    resul = clsDatos.ActualizarDatos(registroActualizar);


                    if (cantidadDisponible == 0)
                    {
                        objeto.RelacionCategoria.idEstado = (int)Constantes.EstadosRegistro.EnProceso;
                        clsDatosRelacionCategoria.CambiarEstado(objeto.RelacionCategoria);
                    }
                }

                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

            }
            catch (Exception ex)
            {

                if (ResultadoConsulta.HayError!= (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }

                ResultadoConsulta.MensajeError = ex.Message;

            }    

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> InsertarDatos(DetalleRelacionCategoria objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = user;
                objeto.usuario = user;

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.IdRelacionCategoria = temp;
                    objeto.Estado = true;
                }


                objeto.RelacionCategoria = 
                    clsDatosRelacionCategoria.ObtenerDatos(new RelacionCategoria() { idRelacionCategoria = objeto.IdRelacionCategoria}).Single();

                int cantidadDisponible = (int)objeto.RelacionCategoria.CantidadCategoria
                            - objeto.RelacionCategoria.DetalleRelacionCategoria.Count();


                if (cantidadDisponible <= 0)
                {
                    throw new Exception(Errores.CantidadRegistros);
                }
                else if (clsDatos.ObtenerDatos(new DetalleRelacionCategoria() { idCategoriaDetalle = objeto.idCategoriaDetalle, IdRelacionCategoria = objeto.IdRelacionCategoria }).Count() > 0)
                {
                    throw new Exception(Errores.DetalleRegistrado);
                }
                else
                {
                   
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                    

                    if (cantidadDisponible == 1)
                    {
                        
                        //objeto.RelacionCategoria.idEstado = (int)Constantes.EstadosRegistro.Activo;

                        objeto.RelacionCategoria.UsuarioModificacion = objeto.usuario;
                        clsDatosRelacionCategoria.ActualizarDatos(objeto.RelacionCategoria);
                    }

                  
                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                     ResultadoConsulta.Usuario,
                     ResultadoConsulta.Clase, string.Format("{0}/{1}",
                     objeto.RelacionCategoria.idCategoriaValor, objeto.idCategoriaAtributo));

                }

            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.DetalleRegistrado)
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

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ObtenerDatos(DetalleRelacionCategoria objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.IdRelacionCategoria = temp;
                }
                if (!string.IsNullOrEmpty(objeto.relacionid))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.relacionid), out temp);
                    objeto.IdRelacionCategoria = temp;
                }

                List<DetalleRelacionCategoria> resul = clsDatos.ObtenerDatos(objeto);
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

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ValidarDatos(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }


    }
  
}
