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


            EnvioSolicitudBL envioSolicitudBL = new EnvioSolicitudBL("Proceso aútomatico de envío de correos", "Solicitudes");
            SolicitudBL solicitudBL = new SolicitudBL("Proceso aútomatico de envío de correos", "Solicitudes");


            List<EnvioSolicitud> envioSolicitud = envioSolicitudBL.ObtenerDatos(new EnvioSolicitud()).objetoRespuesta;

            foreach (var item in envioSolicitud)
            {
                bool respuesta = solicitudBL.EnvioCorreo(new Solicitud() { idSolicitud = item.IdSolicitud }).objetoRespuesta;
                item.Enviado = respuesta;
                item.MensajeError = respuesta == true ? "Correos envíados" : "Correos fallidos";
                envioSolicitudBL.ActualizarElemento(item);
            }






        }
    }
}
