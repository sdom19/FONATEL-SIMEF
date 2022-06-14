using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.Utilidades
{
    public class Respuesta<T>
    {
        public Respuesta()
        {
            this.blnIndicadorTransaccion = true;
            this.strMensaje = string.Empty;
            this.blnIndicadorState = 200;        
        }

        public void toError(string msg, T objObjeto)
        {
            this.objObjeto = objObjeto;
            this.blnIndicadorTransaccion = false;
            this.strMensaje = msg;
          
        }

        public T objObjeto { get; set; }
        //Manejo de errores para try catch
        public bool blnIndicadorTransaccion { get; set; }

        //Manejo de errores para validaciones de requerimientos
        public int blnIndicadorState { get; set; }
        public string strMensaje { get; set; }
        public string strData{ get; set; }
    }
}