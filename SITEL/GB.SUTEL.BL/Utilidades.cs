using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.BL
{
    internal static class Utilidades
    {

        #region Reflection
        
        /// <summary>
        /// Limpia la propiedad indicada de espacios al inicio y al final
        /// </summary>
        /// <param name="pvoClass">clase a limpiar</param>
        /// <param name="pvcProperty">parametro a limpiar</param>
        private static void SetLimpiador(ref object pvoClass, string pvcProperty)
        {
          
            PropertyInfo vloItemProperty;
            vloItemProperty = pvoClass.GetType().GetProperty(pvcProperty);

            if (vloItemProperty != null && (vloItemProperty.GetValue(pvoClass, null) != null))
            {
                
                vloItemProperty.SetValue(pvoClass, vloItemProperty.GetValue(pvoClass, null).ToString().Trim());
            }
        }
       

        /// <summary>
        /// Limpiador de espacios a propiedades de tipo string
        /// </summary>
        /// <param name="t">Objeto a limpiar</param>
        public static void LimpiarEspacios(ref object t)
        {
            //conjunto de propiedades
            PropertyInfo[] vloItemProperty;

            //si el objeto no es nulo
            if (t != null)
            {
                //Se extraen las propiedades del objeto
                vloItemProperty = t.GetType().GetProperties();

                //Se recorre la coleccion de propiedades
                foreach (PropertyInfo info in vloItemProperty)
                {
                    //Se filtran los tipos string
                    if (info.GetGetMethod().ReturnType.FullName == typeof(System.String).FullName)
                    {
                        SetLimpiador(ref t, info.Name);
                    }

                }
            }
        }
        #endregion
    }

}
