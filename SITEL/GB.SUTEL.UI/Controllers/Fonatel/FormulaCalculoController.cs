using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class FormulaCalculoController : Controller
    {
        #region Variables Públicas del controller

        private readonly FormulasCalculoBL formulaBL;
        private readonly FrecuenciaEnvioBL frecuenciaEnvioBL;
        private readonly IndicadorFonatelBL indicadorFonatelBL;
        private readonly DetalleIndicadorVariablesBL detalleIndicadorVariablesBL;
        private readonly CategoriasDesagregacionBL categoriasDesagregacionBL;
        private readonly string usuario = string.Empty;
        private readonly string nombreVista = string.Empty;

        #endregion

        public FormulaCalculoController()
        {
            usuario = System.Web.HttpContext.Current.User.Identity.GetUserId();
            nombreVista = EtiquetasViewFormulasCalculo.TituloIndex;
            frecuenciaEnvioBL = new FrecuenciaEnvioBL(nombreVista, usuario);
            formulaBL = new FormulasCalculoBL(nombreVista, usuario);
            indicadorFonatelBL = new IndicadorFonatelBL(nombreVista, usuario);
            detalleIndicadorVariablesBL = new DetalleIndicadorVariablesBL(nombreVista, usuario);
            categoriasDesagregacionBL = new CategoriasDesagregacionBL(nombreVista, usuario);
        }

        #region Eventos de la página

        // GET: Solicitud
        public ActionResult Index()
        {
            return View();
        }

        // GET: Solicitud/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            CargarDatosEnVistas();
            ViewBag.ModoFormulario = ((int)Accion.Insertar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloCrear;

            return View(new FormulasCalculo());
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("Index");

            FormulasCalculo objFormulaCalculo = null;

            try
            {
                objFormulaCalculo = formulaBL.ObtenerDatos(new FormulasCalculo() { id = id }).objetoRespuesta.Single();
            }
            catch (Exception) { }

            if (objFormulaCalculo == null)
                return View("Index");

            CargarDatosEnVistas();

            ViewBag.ModoFormulario = ((int)Accion.Editar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloEditar;

            return View("Create", objFormulaCalculo);
        }

        //// POST: Solicitud/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Solicitud/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Solicitud/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// POST: Solicitud/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Solicitud/Clone/5

        [HttpGet]
        public ActionResult Clone(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("Index");

            FormulasCalculo objFormulaCalculo = null;

            try
            {
                objFormulaCalculo = formulaBL.ObtenerDatos(new FormulasCalculo() { id = id }).objetoRespuesta.Single();
            }
            catch (Exception) { }

            if (objFormulaCalculo == null)
                return View("Index");

            objFormulaCalculo.Nombre = string.Empty;
            objFormulaCalculo.Codigo = string.Empty;

            CargarDatosEnVistas();

            ViewBag.ModoFormulario = ((int)Accion.Clonar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloClonar;

            return View("Create", objFormulaCalculo);
        }

        [HttpGet]
        public ActionResult Visualize(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("Index");

            FormulasCalculo objFormulaCalculo = null;

            try
            {
                objFormulaCalculo = formulaBL.ObtenerDatos(new FormulasCalculo() { id = id }).objetoRespuesta.Single();
            }
            catch (Exception) { }

            if (objFormulaCalculo == null)
                return View("Index");

            CargarDatosEnVistas();

            ViewBag.ModoFormulario = ((int)Accion.Consultar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloCrear;

            return View("Create", objFormulaCalculo);
        }

        #endregion

        #region Funciones async - Index

        /// <summary>
        /// Obtiene el listado de fórmulas registras
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaFormulas()
        {
            RespuestaConsulta<List<FormulasCalculo>> result = null;
            await Task.Run(() =>
            {
                result = formulaBL.ObtenerDatos(new FormulasCalculo());
            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Permite eliminar una fórmula
        /// </summary>
        /// <param name="formulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EliminarFormula(FormulasCalculo formulaCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> result = null;

            await Task.Run(() =>
            {
                return formulaBL.ObtenerDatos(formulaCalculo);

            }).ContinueWith(data =>
            {
                var objetoValidar = data.Result.objetoRespuesta.Single();
                objetoValidar.IdEstado = (int)Constantes.EstadosRegistro.Eliminado;
                result = formulaBL.EliminarElemento(objetoValidar);
            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Permite activar una fórmula
        /// </summary>
        /// <param name="formulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ActivarFormula(FormulasCalculo formulaCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> result = null;

            await Task.Run(() =>
            {
                return formulaBL.ObtenerDatos(formulaCalculo);

            }).ContinueWith(data =>
            {
                var objetoValidar = data.Result.objetoRespuesta.Single();
                objetoValidar.IdEstado = (int)Constantes.EstadosRegistro.Activo;
                result = formulaBL.CambioEstado(objetoValidar);
            }
            );
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Permite desactivar una fórmula
        /// </summary>
        /// <param name="formulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DesactivarFormula(FormulasCalculo formulaCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> result = null;

            await Task.Run(() =>
            {
                return formulaBL.ObtenerDatos(formulaCalculo);

            }).ContinueWith(data =>
            {
                var objetoValidar = data.Result.objetoRespuesta.Single();
                objetoValidar.IdEstado = (int)Constantes.EstadosRegistro.Desactivado;
                result = formulaBL.CambioEstado(objetoValidar);
            }
            );
            return JsonConvert.SerializeObject(result);
        }

        #endregion

        #region Funciones async - Create

        /// <summary>
        /// 18/10/20122
        /// José Navarro Acuña
        /// Obtiene un listado de las variables dato de un indicador
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerVariablesDatoDeIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = detalleIndicadorVariablesBL.ObtenerDatos(new DetalleIndicadorVariables()
                {
                    idIndicadorString = pIdIndicador
                });
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 18/10/2022
        /// José Navarro Acuña
        /// Obtiene un listado de las categorias de desagregación relacionadas a un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerCategoriasDesagregacionDeIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> resultado = new RespuestaConsulta<List<CategoriasDesagregacion>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = categoriasDesagregacionBL.ObtenerCategoriasDesagregacionDeIndicador(pIdIndicador);
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Función que permite crear una fórmula de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CrearFormulaCalculo(FormulasCalculo pFormulaCalculo)
        {
            string mensajesValidacion = ValidarObjectoCrearFormulaCalculo(pFormulaCalculo);

            if (!string.IsNullOrEmpty(mensajesValidacion))
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<FormulasCalculo>() { HayError = (int)Error.ErrorControlado, MensajeError = mensajesValidacion });
            }

            pFormulaCalculo.IdEstado = (int)EstadosRegistro.EnProceso;
            pFormulaCalculo.UsuarioCreacion = usuario;
            pFormulaCalculo.IdFormula = 0;
            pFormulaCalculo.IdFrecuencia = 0;
            pFormulaCalculo.IdIndicador = 0;
            pFormulaCalculo.IdIndicadorVariable = 0;

            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();

            await Task.Run(() =>
            {
                resultado = formulaBL.InsertarDatos(pFormulaCalculo);
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 15/11/2022
        /// José Navarro Acuña
        /// Función que permite editar una fórmula de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EditarFormulaCalculo(FormulasCalculo pFormulaCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();

            await Task.Run(() =>
            {
                //resultado = formulaBL.InsertarDatos(pFormulasCalculo);
            });
            return JsonConvert.SerializeObject(resultado);
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// 13/10/2022
        /// José Navarro Acuña
        /// Método que permite cargar los datos necesarios del formulario de Fórmulas de Cálculo
        /// </summary>
        private void CargarDatosEnVistas()
        {
            ViewBag.FrecuenciaEnvio = frecuenciaEnvioBL.ObtenerDatos(new FrecuenciaEnvio() { }).objetoRespuesta;
            ViewBag.IndicadorSalida = indicadorFonatelBL.ObtenerDatos(new Indicador() { IdClasificacion = (int)ClasificacionIndicadorEnum.Salida })
                .objetoRespuesta.Select(x => new Indicador()
                {
                    id = x.id,
                    Nombre = Utilidades.ConcatenadoCombos(x.Codigo, x.Nombre)
                }).ToList();
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Permite verificar los datos requeridos y el formato de los campos de una formula de cálcula al momento de crear
        /// </summary>
        /// <param name="pFormulaCalculo"></param>
        /// <param name="pEsGuardadoParcial"></param>
        /// <returns></returns>
        public string ValidarObjectoCrearFormulaCalculo(FormulasCalculo pFormulaCalculo)
        {
            if (!pFormulaCalculo.EsGuardadoParcial)
            {
                if (
                    string.IsNullOrEmpty(pFormulaCalculo.IdFrecuenciaString) ||
                    pFormulaCalculo.FechaCalculo == null ||
                    pFormulaCalculo.Descripcion == null || string.IsNullOrEmpty(pFormulaCalculo.Descripcion.Trim()) ||
                    string.IsNullOrEmpty(pFormulaCalculo.IdIndicadorSalidaString) ||
                    string.IsNullOrEmpty(pFormulaCalculo.IdVariableDatoString) ||
                    (!pFormulaCalculo.NivelCalculoTotal && pFormulaCalculo.ListaCategoriasNivelesCalculo.Count < 1)
                )
                {
                    return Errores.CamposIncompletos;
                }
            }

            if (pFormulaCalculo.Codigo == null || string.IsNullOrEmpty(pFormulaCalculo.Codigo.Trim())) // campo requerido
            {
                return string.Format(Errores.CampoRequeridoV2, EtiquetasViewFormulasCalculo.CrearFormula_LabelCodigo);
            }
            else if (!Utilidades.rx_alfanumerico.Match(pFormulaCalculo.Codigo.Trim()).Success // validar formato
                || pFormulaCalculo.Codigo.Trim().Length > 30)
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelCodigo);
            }

            if (pFormulaCalculo.Nombre == null || string.IsNullOrEmpty(pFormulaCalculo.Nombre.Trim())) // campo requerido
            {
                return string.Format(Errores.CampoRequeridoV2, EtiquetasViewFormulasCalculo.CrearFormula_LabelNombre);
            }
            else if (!Utilidades.rx_soloTexto.Match(pFormulaCalculo.Nombre.Trim()).Success // validar formato
                || pFormulaCalculo.Nombre.Trim().Length > 300)
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelNombre);
            }

            if (pFormulaCalculo.FechaCalculo == DateTime.MinValue)
            {
                return string.Format(Errores.CampoRequeridoV2, EtiquetasViewFormulasCalculo.CrearFormula_LabelFechaCalculo);
            }

            if (!string.IsNullOrEmpty(pFormulaCalculo.Descripcion?.Trim())) // ¿se ingresó el dato?
            {
                if (!Utilidades.rx_soloTexto.Match(pFormulaCalculo.Descripcion).Success               // la descripción solo debe contener texto como valor
                    || pFormulaCalculo.Descripcion.Trim().Length > 1500)                               // validar la cantidad de caracteres
                {
                    return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelDescripcion);
                }
            }

            return null;
        }

        #endregion
    }
}
