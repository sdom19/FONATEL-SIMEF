using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GB.SUTEL.UI.Models;
using GB.SUTEL.Entities;
using System.Security.Claims;
using System.Security.Principal;
using MMLib.Extensions;

using GB.SUTEL.BL.Seguridad;
using GB.SUTEL.Shared;


namespace GB.SUTEL.UI.Helpers
{

    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        //public string AccessLevel { get; set; }

        List<string> whiteList = new List<string>() { "HOME.INDEX","CAMBIARCLAVE" };        

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                string controllerName = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
                string actionName = httpContext.Request.RequestContext.RouteData.GetRequiredString("action").ToUpper();
                if (whiteList.Contains(controllerName.ToUpper() + "." + actionName.ToUpper())) return true;

                if (!httpContext.User.Identity.IsAuthenticated) return false;
                var user = ((ClaimsIdentity)httpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                var name = ((ClaimsIdentity)httpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").FirstOrDefault().Value;
                var email = ((ClaimsIdentity)httpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault().Value;
                var roles = ((ClaimsIdentity)httpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault().Value.Split(',').ToList();
                //var allrolees = ((ClaimsIdentity)httpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality").FirstOrDefault().Value;                
                if (whiteList.Contains(controllerName.ToUpper())) 
                    return true;
                RolBL rolBL = new RolBL(new ApplicationContext(user, "SUTEL - Captura Indicadores"));                
                return rolBL.VerifyRoles(roles, controllerName);
            }
            catch(Exception ex){
                return false;
            }
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {      
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (AuthorizeCore(filterContext.HttpContext))
            {  
                var auth = filterContext.HttpContext.User.Identity.IsAuthenticated;
                if (auth)
                {
                    var User = ((ClaimsIdentity)filterContext.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                    string controllerName = filterContext.HttpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
                    UsersBL UsuariosBL = new UsersBL(new ApplicationContext(User, "SUTEL - Captura Indicadores"));
                    if (UsuariosBL.PrimerLogueo(User) && !whiteList.Contains(controllerName.ToUpper()))
                        filterContext.Result = new RedirectResult("/CambiarClave/Index?cp=true");
                }
            
            }
            /// This code added to support custom Unauthorized pages.
            else if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                
            }
            /// End of additional code
            else
            {
                // Redirect to Login page.
                HandleUnauthorizedRequest(filterContext);
            }
        }
    }

    public class rolValidation
    {
        public static bool ValidateScreenFunctions(HttpContextBase httpContext, string pantalla, string accion)
        {
            if (!httpContext.User.Identity.IsAuthenticated) return false;
            var rolesDeUsuarioClaim = ((ClaimsIdentity)httpContext.User.Identity).Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault();
            string rolesDeUsuario = rolesDeUsuarioClaim == null ? "" : rolesDeUsuarioClaim.Value;
            List<string> currentRoles = rolesDeUsuario.Split(',').ToList();
            RolBL rolBL = new RolBL(new ApplicationContext(httpContext.User.Identity.Name, "SUTEL - Captura Indicadores"));

            return rolBL.ValidateScreenFunctions(currentRoles,pantalla,accion);            
        }

        public static bool ValidateMenuFunctions(List<string> rolPantallas, string pantalla)
        {
            foreach (var item in rolPantallas)
            {
                if (item.RemoveDiacritics().ToUpper().Replace(" ", "").Replace("E", "").Replace("S", "")
                .Equals(pantalla.RemoveDiacritics().ToUpper().Replace(" ", "").Replace("E", "").Replace("S", "")))
                return true;
            }            
            return false;            
        }

        public static List<string> acciones(IPrincipal User, string pantalla)
        {
            var rolesClaim = ((ClaimsIdentity)User.Identity).Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault();
            List<string> roles = rolesClaim == null ? new List<string>() : rolesClaim.Value.Split(',').ToList();
            RolBL rolBL = new RolBL(new ApplicationContext(User.Identity.Name, "SUTEL - Captura Indicadores"));
            return rolBL.GetActionsRolScreen(roles, pantalla); 
        }
        public static List<string> pantallas(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated) return new List<string>();
            var rolesDeUsuarioClaim = ((ClaimsIdentity)httpContext.User.Identity).Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault();
            string rolesDeUsuario = rolesDeUsuarioClaim == null ? "" : rolesDeUsuarioClaim.Value;

            List<string> currentRoles = rolesDeUsuario.Split(',').ToList();
            RolBL rolBL = new RolBL(new ApplicationContext(httpContext.User.Identity.Name, "SUTEL - Captura Indicadores"));
            return rolBL.GetRolScreen(currentRoles);
        }
    }

    public class usuarioHelper {
        public static bool isInterno(IPrincipal User)
        {
            var user = ((ClaimsIdentity)User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").FirstOrDefault();
            return user==null? false: (user.Value=="0"?false:true);
        }
    }

}