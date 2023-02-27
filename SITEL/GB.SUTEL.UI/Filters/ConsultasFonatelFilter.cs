using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using GB.SIMEF.Resources;
using System.Net;

namespace GB.SUTEL.UI.Filters
{
    public class ConsultasFonatelFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Fecha 27-03-2023
        /// Adolfo Cunquero
        /// ActionFilter para interceptar las solicitudes y redireccionar al index para los usuarios de solo lectura
        /// </summary>
        /// <returns></returns>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            var roles = ((ClaimsIdentity)ctx.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(',');
            if(roles.Contains(Constantes.RolConsultasFonatel))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    var descriptor = filterContext.ActionDescriptor;
                    var controllerName = descriptor.ControllerDescriptor.ControllerName;

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = controllerName, action = Constantes.RedirectActionConsultasFonatel }));
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}