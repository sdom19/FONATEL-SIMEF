using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class EditarRegistroIndicadorFonatelBL : IMetodos<RegistroIndicadorFonatel>
    {
        private readonly EditarRegistroIndicadorFonatelDAL clsDatos;


        private RespuestaConsulta<List<RegistroIndicadorFonatel>> ResultadoConsulta;
        string modulo = string.Empty;
        string user = string.Empty;
       
        public EditarRegistroIndicadorFonatelBL(string modulo, string user )
        {
            this.clsDatos = new EditarRegistroIndicadorFonatelDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<RegistroIndicadorFonatel>>();
            this.user = user;
            this.modulo = modulo;
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> ObtenerDatos(RegistroIndicadorFonatel objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                if (!string.IsNullOrEmpty(objeto.FormularioId))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.FormularioId), out int temp );
                    objeto.IdFormulario = temp;
                }
                if (!string.IsNullOrEmpty(objeto.Solicitudid))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.Solicitudid), out int temp);
                    objeto.IdSolicitud = temp;
                }

                var resul = clsDatos.ObtenerDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

    

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> ActualizarElemento(RegistroIndicadorFonatel objeto)
        {
           

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> CambioEstado(RegistroIndicadorFonatel objeto)
        { 
        
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> ClonarDatos(RegistroIndicadorFonatel objeto)
        { 
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> EliminarElemento(RegistroIndicadorFonatel objeto)
        {
            
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> InsertarDatos(RegistroIndicadorFonatel objeto)
        {
   
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<RegistroIndicadorFonatel>> ValidarDatos(RegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }

     

    }
}