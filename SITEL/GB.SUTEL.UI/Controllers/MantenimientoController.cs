
using GB.SUTEL.DAL;
using GB.SUTEL.Entities;
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mantenimiento
{
    public class MantenimientoController : Controller
    {
        Funcion func = new Funcion();
        //Conexión a la base de datos
        private readonly SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();


        #region DefinicionIndicadores
        /// <summary>
        /// Vista de Definción de indicadores
        /// </summary>
        /// <returns>Vista</returns>
        public ActionResult DefinicionIndicadores()
        {
            ViewBag.Direcciones = getDirecciones();
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Definicion de Indicadores", "Definicion de Indicadores SIGITEL");
            }
           
            catch (Exception e) {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return View();
        }

        /// <summary>
        /// Método para obtener los indicadores filtrado por dirección y servicio
        /// </summary>
        /// <param name="direccion">Id de la Dirección</param>
        /// <param name="servicio">Id del Servicio</param>
        /// <returns>Lista de indicadores</returns>
        public JsonResult GetIndicadores(int direccion, int servicio)
        {
            //Consultar a la BD los indicadores filtrados
            var indicadores = db.Indicador.Where(x => x.Borrado == 0 && x.Direccion.Any(y => y.IdDireccion == direccion) && x.ServicioIndicador.Any(z => z.IdServicio == servicio)).Select(x => new
            {
                x.IdIndicador,
                x.NombreIndicador,
                x.TipoIndicador.DesTipoInd
            }).ToList();
            //Retornar los indicadores en formato JSON
            return Json(indicadores, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Método para obtener un indicador obtenido por id
        /// </summary>
        /// <param name="id">Id del indicador</param>
        /// <returns>Indicador</returns>
        public JsonResult GetIndicador(string id)
        {
            //Consultar a la BD un indicador por id
            var indicador = db.Indicador.Where(i => i.IdIndicador == id).Select(x => new
            {
                x.IdIndicador,
                x.NombreIndicador,
                Definicion = x.DefinicionIndicador,
                Fuente = x.FuenteIndicador,
                Nota = x.NotaAlPie
            }).FirstOrDefault();
            //Retorna el indicador en formato JSON
            return Json(indicador, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Método para guardar los cambios de un indicador
        /// </summary>
        /// <param name="id">Id del indicador</param>
        /// <param name="definicion">Definición del indicador</param>
        /// <param name="fuente">Fuente del indicador</param>
        /// <param name="nota">Nota al pie del indicador</param>
        /// <returns>Resultado de guardado</returns>
        /// 

        public string obt (string id){
            Indicador indicador = db.Indicador.Find(id);

            string def;
            string fu;
            string no;
            def = indicador.DefinicionIndicador;
                
            fu=indicador.FuenteIndicador;
            no=indicador.NotaAlPie;
            String[] s2 = new String[3];
            s2[0] = "Definicion Indicador: " + def + ",  ";
            s2[1] = "Fuente Indicador: " + fu + ",  ";
            s2[2] = "Nota: " + no;

            string res = s2[0] + s2[1] + s2[2];
            return res;
        }

        public JsonResult SaveIndicador(string id, string definicion, string fuente, string nota)
        {
            try
            {
                //Obtener usuario actual
                var user = User.Identity.Name;
                var usuarioId = db.Usuario.FirstOrDefault(x => x.NombreUsuario == user);
                if (usuarioId != null)
                {
                    //Obtener indicador para modificarlo
                    Indicador indicador = db.Indicador.Find(id);
                    string a;

                    a= obt(id);
                    



                    if (indicador != null)
                    {


                        indicador.DefinicionIndicador = definicion;
                        indicador.FuenteIndicador = fuente;
                        indicador.NotaAlPie = nota;
                        indicador.FechaUltimaModificacion = DateTime.Now;
                        indicador.HoraUltimaModificacion = DateTime.Now.TimeOfDay;
                        indicador.UsuarioUltimaModificacion = usuarioId.IdUsuario;


                        string nom;
                        string dat = " ";
                        string dat2 = " ";
                        string nuevo = " ";
                        nom = indicador.NombreIndicador;
                        dat = DateTime.Now.ToString();
                        dat2= DateTime.Now.TimeOfDay.ToString();
                        


                        String[] s1 = new String [20];
                        s1[0] = "Indicador con el ID: "+id+",  ";
                        s1[1] = "Nombre Indicador" + nom + ",  ";
                        s1[2] = "Definición: " + definicion + ",  ";
                        s1[3] = "Fuente: "+fuente + ",  ";
                        s1[4] = "Nota: "+nota + ",  ";
                        s1[5] = "Fecha de Modificación: "+dat+",  ";
                        s1[6] = "Hora ultima Modificación: " + dat2;
                        s1[7] = "Usuario: " + user;
                        nuevo = s1[0] + s1[1] + s1[2] + s1[3] + s1[4] + s1[5] + s1[6] + s1[7];

                        try
                        {
                            db.Bitacora.Add(new Bitacora()
                            {
                                Usuario = User.Identity.GetUserId(),
                                FechaBitacora = DateTime.Now,
                                Pantalla = "Definicion de Indicadores",
                                Descripcion = "Proceso de Edición Indicadores SIGITEL",
                                Accion = 3,
                                RegistroAnterior = a,
                                RegistroNuevo = nuevo
                            });
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Exception caught.", e);
                        }
                        //Guardar cambios
                        db.Entry(indicador).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

              
                        return Json(true, JsonRequestBehavior.AllowGet);
                  
                    }
                    else
                        return Json($"No encuentra el indicador: {id}", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json($"El usuarioId no existe: {user}", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ParametrosIndicadores
        /// <summary>
        /// Vista de Parametros indicadores
        /// </summary>
        /// <returns>Vista</returns>
        public ActionResult ParametrosIndicadores()
        {
            ViewBag.Direcciones = getDirecciones();

            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Parametros de Indicadores", "Parametros de Indicadores SIGITEL");
            }
           
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return View();
        }
        /// <summary>
        /// Método para obtener los parámetros filtrado por dirección y servicio
        /// </summary>
        /// <param name="direccion">Id de la Dirección</param>
        /// <param name="servicio">Id del Servicio</param>
        /// <returns></returns>
        public JsonResult GetParametrosIndicadores(int direccion, int servicio)
        {
            //Consultar a la BD los indicadores filtrados
            var indicadores = db.Indicador.Where(i => i.Borrado == 0 && i.Direccion.Any(d => d.IdDireccion == direccion) && i.ServicioIndicador.Any(s => s.IdServicio == servicio)).Select(i => new
            {
                i.IdIndicador,
                i.NombreIndicador,
                i.ParametroIndicador
            }).ToList();
            var parametroIndicadors = new List<ParametroIndicadorViewModel>();
            //Obtener los parametros si existen y sino los crea
            foreach (var indicador in indicadores)
            {
                //Consultar a la BD el parametro del indicador
                ParametroIndicadorViewModel parametroIndicadorViewModel = indicador.ParametroIndicador.Select(p => new ParametroIndicadorViewModel
                {
                    IdIndicador = indicador.IdIndicador,
                    DescripcionIndicador = indicador.NombreIndicador,
                    IdParametroIndicador = p.IdParametroIndicador,
                    Visualiza = p.Visualiza,
                    MesDesde = p.MesDesde,
                    AnnoDesde = p.AnnoDesde,
                    MesPorOperador = p.MesPorOperador,
                    MesPorTotal = p.MesPorTotal,
                    AnnoPorOperador = p.AnnoPorOperador,
                    AnnoPorTotal = p.AnnoPorTotal
                }).FirstOrDefault();
                if (parametroIndicadorViewModel == null)
                    parametroIndicadorViewModel = new ParametroIndicadorViewModel
                    {
                        IdIndicador = indicador.IdIndicador,
                        DescripcionIndicador = indicador.NombreIndicador
                    };
                //Agregar parametro a la lista
                parametroIndicadors.Add(parametroIndicadorViewModel);
            }
            //Retornar los parametros en formato JSON
            return Json(parametroIndicadors, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Método para guardar el parametro si no existe, si existe lo actualiza
        /// </summary>
        /// <param name="parametro">Parametro Indicador</param>
        /// <returns>Resultado del guardado</returns>
        public JsonResult SaveParametroIndicador(ParametroIndicador parametro)
        {
            try
            {
                //Validación mes 0 por operador
                if (parametro.MesPorOperador == 0)
                {
                    //Fecha mínima para SIGITEL
                    parametro.MesPorOperador = 1;
                    parametro.AnnoPorOperador = 2009;
                }
                //Validación mes 0 por total
                if (parametro.MesPorTotal == 0)
                {
                    parametro.MesPorTotal = parametro.MesPorOperador;
                    parametro.AnnoPorTotal = parametro.AnnoPorOperador;
                }
                //Validación mes 0 Desde
                if (parametro.MesDesde == 0)
                {
                    parametro.MesDesde = parametro.MesPorOperador;
                    parametro.AnnoDesde = parametro.AnnoPorOperador;
                }

                //Actualizar campos
                parametro.UsuarioUltimoPublicador = User.Identity.Name;
                parametro.FechaUltimaPublicacion = DateTime.Now.Date;
                parametro.HoraUltimaPublicacion = DateTime.Now.TimeOfDay;


                if (parametro.IdParametroIndicador == 0)
                {
                    string s,a;
                    s = parametro.IdIndicador;
                   

                    db.ParametroIndicador.Add(parametro);
                    
                    try
                    {
                        db.Bitacora.Add(new Bitacora()
                        {
                            Usuario = User.Identity.GetUserId(),
                            FechaBitacora = DateTime.Now,
                            Pantalla = "Parametros Indicadores",
                            Descripcion = "Proceo de creación,Parametro: "+s,
                            Accion = 2,
                            RegistroAnterior = "  ",
                            RegistroNuevo = " "
                        });
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught.", e);
                    }
                }
                else
                   
                db.Entry(parametro).State = System.Data.Entity.EntityState.Modified;

                string q;
                q = parametro.IdIndicador;
                try
                {
                    db.Bitacora.Add(new Bitacora()
                    {
                        Usuario = User.Identity.GetUserId(),
                        FechaBitacora = DateTime.Now,
                        Pantalla = "Parametros Indicadores",
                        Descripcion = "Proceso de edición en el Parametro: " + q,
                        Accion = 3,
                        RegistroAnterior = "  ",
                        RegistroNuevo = " "
                    });
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }

                //Guardar cambios en base de datos
                db.SaveChanges();
                //Retornar true en formato JSON
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                //Retornar false en formato JSON
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Reporte
        /// <summary>
        /// Vista de Reporte
        /// </summary>
        /// <returns>Vista</returns>
        public ActionResult Reporte()
        {
            ViewBag.Direcciones = getDirecciones();

            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Reporte", "Reporte SIGITEL");
            }
           
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View();
        }

        /// <summary>
        /// Método para obtener las bitácoras filtrado por dirección, servicio y fechas
        /// </summary>
        /// <param name="direccion">Id de Dirección</param>
        /// <param name="servicio">Id de Servicio</param>
        /// <param name="desde">Fecha inicial</param>
        /// <param name="hasta">Fecha final</param>
        /// <returns>Lista de bitacoras</returns>
        public JsonResult GetBitacoraParametrosIndicadores(int direccion, int servicio, string desde, string hasta)
        {
            //Convertir fechas
            DateTime desdeDate = DateTime.Parse(desde);
            DateTime hastaDate = DateTime.Parse(hasta);
            //Consultar a la BD las bitácoras 
            var bitacoras = db.BitacoraParametrizacionIndicador.Where(b => b.Indicador.ServicioIndicador.Any(s => s.IdServicio == servicio) && b.Indicador.Direccion.Any(d => d.IdDireccion == direccion) && DateTime.Compare(desdeDate, b.FechaPublicacion) <= 0 && DateTime.Compare(b.FechaPublicacion, hastaDate) <= 0).Select(b => new
            {
                b.UsuarioPublicador,
                b.FechaPublicacion,
                b.HoraPublicacion,
                b.IdIndicador,
                b.Indicador.NombreIndicador,
                b.MesPorOperador,
                b.MesPorTotal,
                b.AnnoPorOperador,
                b.AnnoPorTotal
            }).ToList();
            //Retornar bitacoras en formato JSON
            return Json(bitacoras, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Globales
        /// <summary>
        /// Método para obtener las direcciones
        /// </summary>
        /// <returns>Lista de direcciones</returns>
        public List<DireccionViewModel> getDirecciones()
        {
            //Consultar a la BD las direcciones 
            var direcciones = db.Direccion.Select(d => new DireccionViewModel
            {
                IdDireccion = d.IdDireccion,
                Nombre = d.Nombre,
                Cantidad = d.Indicador.Where(i => i.Borrado == 0).Count()
            }).ToList();
            //Retornar direcciones en formato JSON
            return direcciones;
        }
        /// <summary>
        /// Método para obtener los servicios
        /// </summary>
        /// <param name="IdDireccion">Id de la Dirección</param>
        /// <returns>Lista de servicios</returns>
        public JsonResult GetServicios(int IdDireccion)
        {
            //Consultar a la BD los servicios
            var servicios = db.Servicio.Select(s => new
            {
                s.IdServicio,
                s.DesServicio,
                Cantidad = s.ServicioIndicador.Where(si => si.Indicador.Borrado == 0 && si.Indicador.Direccion.Any(d => d.IdDireccion == IdDireccion)).Count()
            }).ToList();
            //Retornar servicios en formato JSON
            return Json(servicios, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
