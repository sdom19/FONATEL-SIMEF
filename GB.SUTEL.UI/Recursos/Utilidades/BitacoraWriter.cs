using GB.SUTEL.BL;
using GB.SUTEL.BL.Seguridad;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Recursos.Utilidades
{
    public class BitacoraWriter : LocalContextualizer
    {
        BitacoraBL refBitacoraBL;
        public BitacoraWriter(ApplicationContext appContext): base(appContext)
        {

            refBitacoraBL = new BitacoraBL(AppContext);
        }
        
        public void gRegistrarBitacora<T>(HttpContextBase httpcontext, int Accion, T NuevoDato, T AnteriorDato, string msj =".")
        {
            try
            {
                string pantalla = httpcontext.Request.RequestContext.RouteData.GetRequiredString("controller").ToString();
                pantalla = Char.ToUpper(pantalla[0]) + pantalla.Substring(1).ToLower();                
                var user = ((ClaimsIdentity)httpcontext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                var usuario = user == null ? "" : user.Value;
                string descripcion = "";
                
                var type = typeof(T);
                var p = type.GetProperties();
                string DetalleNuevo = "";
                string DetalleViejo = "";

                foreach (PropertyInfo property in p)
                {
                    if ((!property.PropertyType.IsClass||property.PropertyType.Name=="String") && !property.PropertyType.IsGenericType)
                    {
                        var pName = property.Name;
                        string valor = "";
                        string valor2 = "";
                        if (NuevoDato != null)
                        {                            
                            if (NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null) != null)
                            {
                                valor = NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null).ToString();
                            }
                            else
                            {
                                valor = "NULL";
                            }
                            DetalleNuevo = DetalleNuevo + String.Format("{0}:{1}; ", pName, valor);
                        }
                        if (AnteriorDato != null)
                        {
                            if (AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null) != null)
                            {
                                valor2 = AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null).ToString();
                            }
                            else
                            {
                                valor2 = "NULL";
                            }                            
                            DetalleViejo = DetalleViejo + String.Format("{0}:{1}; ", pName, valor2);
                        }
                    }
                }
                if (msj != "")
                {
                    switch (Accion)
                    {
                        case 2:
                            descripcion = "Proceso de creación de " + Char.ToUpper(typeof(T).Name[0]) + typeof(T).Name.Substring(1).ToLower();
                            break;
                        case 3:
                            descripcion = "Proceso de edición de " + Char.ToUpper(typeof(T).Name[0]) + typeof(T).Name.Substring(1).ToLower();
                            break;
                        case 4:
                            descripcion = "Proceso de eliminación de " + Char.ToUpper(typeof(T).Name[0]) + typeof(T).Name.Substring(1).ToLower();
                            break;
                        case 5:
                            descripcion = "Proceso de recuperación de contraseña de " + Char.ToUpper(typeof(T).Name[0]) + typeof(T).Name.Substring(1).ToLower();
                            break;
                        default:
                            descripcion = "Proceso en " + Char.ToUpper(typeof(T).Name[0]) + typeof(T).Name.Substring(1).ToLower();
                            break;
                    }
                }
                else
                {
                    descripcion = msj;
                }
                refBitacoraBL.InsertarBitacora(Accion, pantalla, usuario, descripcion, DetalleNuevo, DetalleViejo);
            }

            catch (Exception ex)
            {
                if (!(ex is CustomException))
                {
                    AppContext.ExceptionBuilder.BuildException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                }
            }

        }



        public void gRegistrar<T>(HttpContextBase httpcontext, int Accion, T NuevoDato, T AnteriorDato, string msj = ".")
        {
            try
            {
                string pantalla = "Registro Indicador Externo";

                var user = ((ClaimsIdentity)httpcontext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                var usuario = user == null ? "" : user.Value;
                string descripcion = "";

                var type = typeof(T);
                var p = type.GetProperties();
                string DetalleNuevo = "";
                string DetalleViejo = "";

                foreach (PropertyInfo property in p)
                {
                    if ((!property.PropertyType.IsClass || property.PropertyType.Name == "String") && !property.PropertyType.IsGenericType)
                    {
                        var pName = property.Name;
                        string valor = "";
                        string valor2 = "";
                        if (NuevoDato != null)
                        {
                            if (NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null) != null)
                            {
                                valor = NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null).ToString();
                            }
                            else
                            {
                                valor = "NULL";
                            }
                            DetalleNuevo = DetalleNuevo + String.Format("{0}:{1}; ", pName, valor);
                        }
                        if (AnteriorDato != null)
                        {
                            if (AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null) != null)
                            {
                                valor2 = AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null).ToString();
                            }
                            else
                            {
                                valor2 = "NULL";
                            }
                            DetalleViejo = DetalleViejo + String.Format("{0}:{1}; ", pName, valor2);
                        }
                    }
                }
                if (msj != "")
                {
                    switch (Accion)
                    {
                        case 2:
                            descripcion = "Proceso de inserción de Registro Indicador Externo";
                            break;
                        case 3:
                            descripcion = "Proceso de modificación de Registro Indicador Externo ";
                            break;
                        case 4:
                            descripcion = "Proceso de eliminación de Registro Indicador Externo";
                            break;
                        case 5:
                            descripcion = "Proceso de recuperación de contraseña de Registro Indicador Externo";
                            break;
                        default:
                            descripcion = "Proceso en Registro Indicador Externo";
                            break;
                    }
                }
                else
                {
                    descripcion = msj;
                }
                refBitacoraBL.InsertarBitacora(Accion, pantalla, usuario, descripcion, DetalleNuevo, DetalleViejo);
            }

            catch (Exception ex)
            {
                if (!(ex is CustomException))
                {
                    AppContext.ExceptionBuilder.BuildException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                }
            }

        }

        public void gRegistrarRegion<T>(HttpContextBase httpcontext, int Accion, T NuevoDato, T AnteriorDato, string msj = ".")
        {
            try
            {
                string pantalla = "Región Indicador Externo";
               
                var user = ((ClaimsIdentity)httpcontext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                var usuario = user == null ? "" : user.Value;
                string descripcion = "";

                var type = typeof(T);
                var p = type.GetProperties();
                string DetalleNuevo = "";
                string DetalleViejo = "";

                foreach (PropertyInfo property in p)
                {
                    if ((!property.PropertyType.IsClass || property.PropertyType.Name == "String") && !property.PropertyType.IsGenericType)
                    {
                        var pName = property.Name;
                        string valor = "";
                        string valor2 = "";
                        if (NuevoDato != null)
                        {
                            if (NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null) != null)
                            {
                                valor = NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null).ToString();
                            }
                            else
                            {
                                valor = "NULL";
                            }
                            DetalleNuevo = DetalleNuevo + String.Format("{0}:{1}; ", pName, valor);
                        }
                        if (AnteriorDato != null)
                        {
                            if (AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null) != null)
                            {
                                valor2 = AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null).ToString();
                            }
                            else
                            {
                                valor2 = "NULL";
                            }
                            DetalleViejo = DetalleViejo + String.Format("{0}:{1}; ", pName, valor2);
                        }
                    }
                }
                if (msj != "")
                {
                    switch (Accion)
                    {
                        case 2:
                            descripcion = "Proceso de inserción de Región Indicador Externo";
                            break;
                        case 3:
                            descripcion = "Proceso de modificación de Región Indicador Externo ";
                            break;
                        case 4:
                            descripcion = "Proceso de eliminación de Región Indicador Externo";
                            break;
                        case 5:
                            descripcion = "Proceso de recuperación de contraseña de Región Indicador Externo";
                            break;
                        default:
                            descripcion = "Proceso en Región Indicador Externo";
                            break;
                    }
                }
                else
                {
                    descripcion = msj;
                }
                refBitacoraBL.InsertarBitacora(Accion, pantalla, usuario, descripcion, DetalleNuevo, DetalleViejo);
            }

            catch (Exception ex)
            {
                if (!(ex is CustomException))
                {
                    AppContext.ExceptionBuilder.BuildException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                }
            }

        }



        public void gRegistrarIndicador<T>(HttpContextBase httpcontext, int Accion, T NuevoDato, T AnteriorDato, string msj = ".")
        {
            try
            {
                string pantalla = "Indicador Externo";
               
                var user = ((ClaimsIdentity)httpcontext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                var usuario = user == null ? "" : user.Value;
                string descripcion = "";

                var type = typeof(T);
                var p = type.GetProperties();
                string DetalleNuevo = "";
                string DetalleViejo = "";

                foreach (PropertyInfo property in p)
                {
                    if ((!property.PropertyType.IsClass || property.PropertyType.Name == "String") && !property.PropertyType.IsGenericType)
                    {
                        var pName = property.Name;
                        string valor = "";
                        string valor2 = "";
                        if (NuevoDato != null)
                        {
                            if (NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null) != null)
                            {
                                valor = NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null).ToString();
                            }
                            else
                            {
                                valor = "NULL";
                            }
                            DetalleNuevo = DetalleNuevo + String.Format("{0}:{1}; ", pName, valor);
                        }
                        if (AnteriorDato != null)
                        {
                            if (AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null) != null)
                            {
                                valor2 = AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null).ToString();
                            }
                            else
                            {
                                valor2 = "NULL";
                            }
                            DetalleViejo = DetalleViejo + String.Format("{0}:{1}; ", pName, valor2);
                        }
                    }
                }
                if (msj != "")
                {
                    switch (Accion)
                    {
                        case 2:
                            descripcion = "Proceso de inserción de Indicador Externo";
                            break;
                        case 3:
                            descripcion = "Proceso de modificación de Indicador Externo";
                            break;
                        case 4:
                            descripcion = "Proceso de eliminación de Indicador Externo";
                            break;
                        case 5:
                            descripcion = "Proceso de recuperación de contraseña de Indicador Externo";
                            break;
                        default:
                            descripcion = "Proceso en " + Char.ToUpper(typeof(T).Name[0]) + typeof(T).Name.Substring(1).ToLower();
                            break;
                    }
                }
                else
                {
                    descripcion = msj;
                }
                refBitacoraBL.InsertarBitacora(Accion, pantalla, usuario, descripcion, DetalleNuevo, DetalleViejo);
            }

            catch (Exception ex)
            {
                if (!(ex is CustomException))
                {
                    AppContext.ExceptionBuilder.BuildException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                }
            }
        }

   



        public void gRegistrarZona<T>(HttpContextBase httpcontext, int Accion, T NuevoDato, T AnteriorDato, string msj = ".")
        {
            try
            {
                string pantalla ="Zona Indicador Externo";
               
                var user = ((ClaimsIdentity)httpcontext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                var usuario = user == null ? "" : user.Value;
                string descripcion = "";

                var type = typeof(T);
                var p = type.GetProperties();
                string DetalleNuevo = "";
                string DetalleViejo = "";

                foreach (PropertyInfo property in p)
                {
                    if ((!property.PropertyType.IsClass || property.PropertyType.Name == "String") && !property.PropertyType.IsGenericType)
                    {
                        var pName = property.Name;
                        string valor = "";
                        string valor2 = "";
                        if (NuevoDato != null)
                        {
                            if (NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null) != null)
                            {
                                valor = NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null).ToString();
                            }
                            else
                            {
                                valor = "NULL";
                            }
                            DetalleNuevo = DetalleNuevo + String.Format("{0}:{1}; ", pName, valor);
                        }
                        if (AnteriorDato != null)
                        {
                            if (AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null) != null)
                            {
                                valor2 = AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null).ToString();
                            }
                            else
                            {
                                valor2 = "NULL";
                            }
                            DetalleViejo = DetalleViejo + String.Format("{0}:{1}; ", pName, valor2);
                        }
                    }
                }
                if (msj != "")
                {
                    switch (Accion)
                    {
                        case 2:
                            descripcion = "Proceso de inserción de Zona Indicador Externo";
                            break;
                        case 3:
                            descripcion = "Proceso de modificación de Zona Indicador Externo";
                            break;
                        case 4:
                            descripcion = "Proceso de eliminación de Zona Indicador Externo";
                            break;
                        case 5:
                            descripcion = "Proceso de recuperación de contraseña de Zona Indicador Externo";
                            break;
                        default:
                            descripcion = "Proceso en Zona Indicador Externo";
                            break;
                    }
                }
                else
                {
                    descripcion = msj;
                }
                refBitacoraBL.InsertarBitacora(Accion, pantalla, usuario, descripcion, DetalleNuevo, DetalleViejo);
            }

            catch (Exception ex)
            {
                if (!(ex is CustomException))
                {
                    AppContext.ExceptionBuilder.BuildException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                }
            }

        }





        public void gfuente<T>(HttpContextBase httpcontext, int Accion, T NuevoDato, T AnteriorDato, string msj = ".")
        {
            try
            {
                string pantalla = "Fuentes  Externas";

                var user = ((ClaimsIdentity)httpcontext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                var usuario = user == null ? "" : user.Value;
                string descripcion = "";

                var type = typeof(T);
                var p = type.GetProperties();
                string DetalleNuevo = "";
                string DetalleViejo = "";

                foreach (PropertyInfo property in p)
                {
                    if ((!property.PropertyType.IsClass || property.PropertyType.Name == "String") && !property.PropertyType.IsGenericType)
                    {
                        var pName = property.Name;
                        string valor = "";
                        string valor2 = "";
                        if (NuevoDato != null)
                        {
                            if (NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null) != null)
                            {
                                valor = NuevoDato.GetType().GetProperty(pName).GetValue(NuevoDato, null).ToString();
                            }
                            else
                            {
                                valor = "NULL";
                            }
                            DetalleNuevo = DetalleNuevo + String.Format("{0}:{1}; ", pName, valor);
                        }
                        if (AnteriorDato != null)
                        {
                            if (AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null) != null)
                            {
                                valor2 = AnteriorDato.GetType().GetProperty(pName).GetValue(AnteriorDato, null).ToString();
                            }
                            else
                            {
                                valor2 = "NULL";
                            }
                            DetalleViejo = DetalleViejo + String.Format("{0}:{1}; ", pName, valor2);
                        }
                    }
                }
                if (msj != "")
                {
                    switch (Accion)
                    {
                        case 2:
                            descripcion = "Proceso de creación de Fuentes Externas";
                            break;
                        case 3:
                            descripcion = "Proceso de edición de Fuentes Externas";
                            break;
                        case 4:
                            descripcion = "Proceso de eliminación de Fuentes Externas";
                            break;
                        case 5:
                            descripcion = "Proceso de recuperación de contraseña de Fuentes Externas";
                            break;
                        default:
                            descripcion = "Proceso en Fuentes Externas";
                            break;
                    }
                }
                else
                {
                    descripcion = msj;
                }
                refBitacoraBL.InsertarBitacora(Accion, pantalla, usuario, descripcion, DetalleNuevo, DetalleViejo);
            }

            catch (Exception ex)
            {
                if (!(ex is CustomException))
                {
                    AppContext.ExceptionBuilder.BuildException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                }
            }

        }





    }
}
