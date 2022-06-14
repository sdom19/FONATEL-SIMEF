using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;

using GB.SUTEL.Shared;
using GB.SUTEL.UI.Helpers;

namespace GB.SUTEL.UI.Controllers
{
    /// <summary>
    /// Make the controllers safe, so they won't horribly fall down
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    public class BaseController : AsyncController
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                        null;
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }
        public ApplicationContext AppContext { get; private set; }
        // GET: Safe
        public BaseController()
        {
            WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();            
            WindowsPrincipal currentPrincipal = new WindowsPrincipal(currentIdentity); 
            string usuarioDominio = currentPrincipal.Identity.Name;

            AppContext = new ApplicationContext(usuarioDominio, "SUTEL - Captura Indicadores", ExceptionHandler.ExceptionType.Presentation);
        }
        /// <summary>
        ///  this will let us handle exceptions not managed in controllers
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            var foo = 0;

        }
    }
}