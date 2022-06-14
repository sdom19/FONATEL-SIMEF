using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace GB.SUTEL.UI.Helpers
{
    /// <summary>
    /// Actions identifier 
    /// </summary>
    public enum ActionsBinnacle
    {
        Index = 1, Crear = 2, Editar = 3, Borrar = 4, Consultar = 5, Imprimir = 6
    }
 

    /* Catalogo en la base de datos
     * 1= Index
     * 2= Crear
     * 3= Editar
     * 4=Eliminar
     * 5= Consultar
     * 
     * 
     * Catalogo en Programacion
     * 1= Insertar
     * 2= Modificar
     * 3= Borrar
     * 4= Consultar
     * 5= Procesar
     * 
     * 
    */
    public class CCatalogo
    {
        public enum EstadosSolicitud
        {
            PENDIENTE = 1,
            ENVIADO = 2,
            EN_PROCESO = 3,
            FINALIZADO = 4
        };

   public enum TipoValor
        {
            TEXTO = 1,
            FECHA = 2,
            PORCENTAJE = 3,
            MONTO = 4,
            CANTIDAD=5
        };

   public enum TipoNivelValor
   {
       Provincia = 1,
       Canton = 2,
       Genero = 3,
      
   };
   public static bool esFecha(String s)
   {
       try
       {
           Convert.ToDateTime(s);
           return true;
       }
       catch (Exception ex)
       {
           return false;
       }
       
   }
    }
}