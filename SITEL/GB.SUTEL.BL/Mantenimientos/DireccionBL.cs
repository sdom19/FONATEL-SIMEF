using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.Resources;
using GB.SUTEL.DAL;


namespace GB.SUTEL.BL.Mantenimientos
{
    public class DireccionBL
    {
        #region atributos
        DireccionAD direccionAD = new DireccionAD();
        #endregion

        #region metodos

       

        /// <summary>
        /// Listado de Direcciones
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Direccion>> gListar()
        {
            Respuesta<List<Direccion>> respuesta = new Respuesta<List<Direccion>>();

            try
            {
                respuesta = direccionAD.Listar();
            }
            catch (Exception ex)
            {
                respuesta.toError(ex.Message, null);
            }

            return respuesta;
        }

         /// <summary>
        /// Filtro de direcciones
        /// </summary>
        /// <param name="psNombreDireccion"></param>
        /// <returns></returns>
        public Respuesta<List<Direccion>> gFiltrarDirecciones(String psNombreDireccion)
        {
            Respuesta<List<Direccion>> respuesta = new Respuesta<List<Direccion>>();

            try
            {
                respuesta = direccionAD.gFiltrarDirecciones(psNombreDireccion);
            }
            catch (Exception ex)
            {
                respuesta.toError(ex.Message, null);
            }

            return respuesta;
        }

        public Respuesta<Direccion> gConsultarPorId(int idDireccion)
        {
            Respuesta<Direccion> respuesta = new Respuesta<Direccion>();

            try
            {
                respuesta = direccionAD.ConsultarPorId(idDireccion);
            }
            catch (Exception ex)
            {
                respuesta.toError(ex.Message, null);
            }

            return respuesta;
        }


        #endregion
    }
}
