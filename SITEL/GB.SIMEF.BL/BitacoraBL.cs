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
    public class BitacoraBL:IMetodos<Bitacora>
    {
        private readonly CategoriasDesagregacionDAL clsDatos;



        private RespuestaConsulta<List<Bitacora>> ResultadoConsulta;
        string modulo = Etiquetas.Categorias;

        public BitacoraBL()
        {
            this.clsDatos = new CategoriasDesagregacionDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<Bitacora>>();
        }
        #region Metedos sin Usar
        public RespuestaConsulta<Bitacora> ActualizarElemento(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<Bitacora> CambioEstado(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<Bitacora> ClonarDatos(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Bitacora>> EliminarElemento(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<Bitacora> InsertarDatos(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<Bitacora> ValidarDatos(Bitacora objeto)
        {
            throw new NotImplementedException();
        }
        #endregion


        /// <summary>
        /// 05/08/2022
        /// Obtiene la lista con base a un parametro de fecha desde y fecha hasta
        /// Michael Hernández Cordero
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Bitacora>> ObtenerDatos(Bitacora objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;      
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

      
    }
}
