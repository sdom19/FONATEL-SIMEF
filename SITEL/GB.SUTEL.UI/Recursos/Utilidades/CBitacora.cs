using GB.SUTEL.BL.Seguridad;
using GB.SUTEL.Shared;
using GB.SUTEL.UI.Controllers;
using GB.SUTEL.UI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Recursos.Utilidades
{
    public class CBitacora
    {


        public enum BitacoraOpciones
        {

            [Description("Agrupación")]
            agrupacion,
            [Description("Constructor")]
            constructor,
            [Description("Criterios")]
            criterio,
            [Description("Detalle de Agrupación")]
            detalleAgrupacion,
            [Description("Frecuencia")]
            frecuencia,
            [Description("Indicador")]
            indicador,
            [Description("Indicador UIT")]
            indicadorUIT,
            [Description("Login")]
            login,
            [Description("Nivel")]
            nivel,
            [Description("Operador")]
            operador,
            [Description("Pantalla")]
            pantalla,
            [Description("Roles")]
            rol,
            [Description("Seguridad")]
            seguridad,
            [Description("Servicios")]
            servicio,
            [Description("Servicio Operador")]
            servioOperador,
            [Description("Tipo Indicador")]
            tipoIndicador,
            [Description("Usuarios")]
            usuario,
            [Description("Solicitud")]
            solicitud,
            [Description("Registro Indicador Interno")]
            registroIndicadorInterno

        };

        public static void gRegistrarBitacora(ApplicationContext context, int piAccion, String psPantalla, String psDescripcion, String psNuevoDato, String psAnteriorDato)
        {
            try
            {

              
              
                //String usuario = HttpContext.Current.User.Identity.Name;
                String usuario = HttpContext.Current.User.Identity.GetUserId();
                BitacoraBL refBitacoraBL = new BitacoraBL(context);
                refBitacoraBL.InsertarBitacora(piAccion, psPantalla, usuario,
                                                   psDescripcion, psNuevoDato, psAnteriorDato);
            }

            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                context.ExceptionBuilder.BuildException(msj, ex);
            }

        }

        public static string GetValueAsString(BitacoraOpciones environment)
        {
            // get the field 
            var field = environment.GetType().GetField(environment.ToString());
            var customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (customAttributes.Length > 0)
            {
                return (customAttributes[0] as DescriptionAttribute).Description;
            }
            else
            {
                return environment.ToString();
            }
        }


    }
}
