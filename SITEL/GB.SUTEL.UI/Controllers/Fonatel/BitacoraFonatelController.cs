﻿using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using GB.SUTEL.UI.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    [AuthorizeUserAttribute]
    public class BitacoraFonatelController : Controller
    {
        private readonly BitacoraBL BitacoraBL;

        public BitacoraFonatelController()
        {
            BitacoraBL = new BitacoraBL();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var lista = BitacoraBL.ObtenerDatos(new Bitacora()).objetoRespuesta;

            var listaUsuario = lista.Select(x => x.Usuario).Distinct();

            /*Se verifica si hace falta alguna pantalla para mostrar en las opciones del filtro de Pantallas*/
            List<string> listaPantallas = new List<string> {
                "Relaciones entre Categorías",
                "Fuentes de Registro",
                "Reglas de Validación",
                "Definición de Indicadores",
                "Publicación de Indicadores",
                "Fórmulas de Cálculo",
                "Indicadores",
                "Categorías de Desagregación",
                "Envío Programado",
                "Solicitud de Información",
                "Formulario Web",
                "Descarga y Edición de Formulario",
                "Consulta de Datos Históricos"
            };           

            var ListaAcciones = lista.Select(x => x.Accion).Distinct();

            ViewBag.Pantalla = listaPantallas.Select(x => new SelectListItem() { Selected = false, Value = x, Text = x }).ToList();
            ViewBag.Usuario = listaUsuario.Select(x => new SelectListItem() { Selected = false, Value = x, Text = x }).ToList();

            ViewBag.Accion = ListaAcciones.Select(x => new SelectListItem() { Selected = false, Value = x.ToString(), Text = Enum.GetName(typeof(Accion), x) }).ToList();

            var roles = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(',');
            ViewBag.ConsultasFonatel = roles.Contains(Constantes.RolConsultasFonatel).ToString().ToLower();

            return View();
        }




        #region Métodos de ASYNC
        /// <summary>
        /// Fecha 04-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table de Bitacora INDEX
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> ObtenerListaBitacora(Bitacora bitacora)
        {
            RespuestaConsulta<List<Bitacora>> result = null;
            await Task.Run(() =>
            {
                result = BitacoraBL.ObtenerDatos(bitacora);
            });
            return JsonConvert.SerializeObject(result);
        }

        #endregion

    }
}
