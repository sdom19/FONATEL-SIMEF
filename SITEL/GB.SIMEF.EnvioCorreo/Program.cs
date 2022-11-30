using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.EnvioCorreo
{
    class Program
    {
        static void Main(string[] args)
        {


            EnvioSolicitudesBL envioSolicitudBL = new EnvioSolicitudesBL();
            SolicitudBL solicitudBL = new SolicitudBL("Proceso aútomatico de envío de correos", "Solicitudes");


            List<EnvioSolicitudes> envioSolicitud = envioSolicitudBL.ObtenerDatos(new EnvioSolicitudes()).objetoRespuesta;

            foreach (var item in envioSolicitud)
            {
                bool respuesta=solicitudBL.EnvioCorreo(new Solicitud() { idSolicitud = item.IdSolicitud }).objetoRespuesta;
                item.Enviado = respuesta;
                item.MensajError = respuesta==true? "Correos envíados":"Correos fallidos";
                envioSolicitudBL.ActualizarElemento(item);
            }
            
            
            
            
            

        }
    }
}
