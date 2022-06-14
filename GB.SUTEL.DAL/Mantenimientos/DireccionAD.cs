using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Omu.ValueInjecter;
using System.Data.Entity;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class DireccionAD
    {
        SUTEL_IndicadoresEntities context;

        #region Metodos

        public DireccionAD()
        {
            context = new SUTEL_IndicadoresEntities();
        }

        /// <summary>
        /// Lista un conjuto de Direcciones
        /// </summary>
        /// <returns>Listado de Direcciones</returns>
        public Respuesta<List<Direccion>> Listar()
        {
            List<Direccion> listadoDirecciones = new List<Direccion>();
            Respuesta<List<Direccion>> respuesta = new Respuesta<List<Direccion>>();

            try
            {
                
                    listadoDirecciones = (from direcciones in context.Direccion
                                          orderby direcciones.Nombre
                                          select direcciones).Include("Indicador").ToList();
               

                respuesta.objObjeto = listadoDirecciones;

            }
            catch (Exception ex)
            {
                respuesta.toError(ex.Message, listadoDirecciones);
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
            List<Direccion> listadoDirecciones = new List<Direccion>();
            Respuesta<List<Direccion>> respuesta = new Respuesta<List<Direccion>>();

            try
            {
                
                    listadoDirecciones =  context.Direccion.Where(x=>psNombreDireccion.Equals("")   || x.Nombre.ToUpper().Contains(psNombreDireccion.ToUpper())) .ToList();
               

                respuesta.objObjeto = listadoDirecciones;

            }
            catch (Exception ex)
            {
                respuesta.toError(ex.Message, listadoDirecciones);
            }

            return respuesta;
        }


        public Respuesta<Direccion> ConsultarPorId(int poIdDireccion)
        {
            Respuesta<Direccion> resultado = new Respuesta<Direccion>();
            Direccion direccion = null;
            try
            {


               
                    // Realizamos la consulta
                    direccion = context.Direccion.Where(c => c.IdDireccion == poIdDireccion).First();

               
                resultado.objObjeto = direccion;
                return resultado;


            }
            catch (Exception ex)
            {
                resultado.toError(ex.Message, direccion);
            }

            return resultado;
        }
        #endregion



    }
}
