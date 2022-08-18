using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using GB.SIMEF.BL;
using Microsoft.AspNet.Identity;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class IndicadorFonatelController : Controller
    {
        private readonly IndicadorFonatelBL indicadorBL;
        private readonly TipoIndicadorBL tipoIndicadorBL;
        private readonly GrupoIndicadorBL grupoIndicadorBL;

        public IndicadorFonatelController()
        {
            indicadorBL = new IndicadorFonatelBL(
                EtiquetasViewIndicadorFonatel.TituloIndex, System.Web.HttpContext.Current.User.Identity.GetUserId());

            tipoIndicadorBL = new TipoIndicadorBL(
                EtiquetasViewIndicadorFonatel.TituloIndex, System.Web.HttpContext.Current.User.Identity.GetUserId());

            grupoIndicadorBL = new GrupoIndicadorBL(
                EtiquetasViewIndicadorFonatel.TituloIndex, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }

        #region Eventos de la página

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult DetalleVariables(int id)
        {
            return View();
        }


        public ActionResult DetalleCategoria(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Métodos de async

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores registrados en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaIndicadores()
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();
            await Task.Run(() =>
            {
                resultado = indicadorBL.ObtenerDatos(new Indicador());
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Función que permite realizar un eliminado lógico de un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EliminarIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = indicadorBL.CambioEstado(new Indicador()
                {
                    id = pIdIndicador,
                    nuevoEstado = (int)EstadosRegistro.Eliminado // nuevo estado
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 17/08/2022
        /// José Navarro Acuña
        /// Función que permite desactivar un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DesactivarIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = indicadorBL.CambioEstado(new Indicador()
                {
                    id = pIdIndicador,
                    nuevoEstado = (int)EstadosRegistro.Desactivado // nuevo estado
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Función que permite activar un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ActivarIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = indicadorBL.CambioEstado(new Indicador()
                {
                    id = pIdIndicador,
                    nuevoEstado = (int)EstadosRegistro.Activo // nuevo estado
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Función que verifica si el indicador se encuentra en algún formulario web
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> VerificarIndicadorEnFormularioWeb(string pIdIndicador)
        {
            RespuestaConsulta<List<FormularioWeb>> resultado = new RespuestaConsulta<List<FormularioWeb>>();
            
            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = indicadorBL.ObtenerFormulariosWebSegunIndicador(new Indicador()
                {
                    id = pIdIndicador
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que obtiene un listado de los tipos de indicadores registrados en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaTipoIndicador()
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();
            await Task.Run(() =>
            {
                resultado = tipoIndicadorBL.ObtenerDatos();
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que obtiene un listado de los tipos de indicadores registrados en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaGrupoIndicador()
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();
            await Task.Run(() =>
            {
                resultado = grupoIndicadorBL.ObtenerDatos();
            });

            return JsonConvert.SerializeObject(resultado);
        }
        #endregion
    }
}
