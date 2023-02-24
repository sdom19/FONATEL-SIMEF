using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using GB.SIMEF.Resources;

namespace GB.SUTEL.UI.Filters
{
    public class ConsultasFonatelFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            var roles = ((ClaimsIdentity)ctx.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(',');
            if(roles.Contains(Constantes.RolConsultasFonatel))
            {
                var descriptor = filterContext.ActionDescriptor;
                var controllerName = descriptor.ControllerDescriptor.ControllerName;

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = controllerName, action = Constantes.RedirectActionConsultasFonatel }));
            }
        }
    }
}