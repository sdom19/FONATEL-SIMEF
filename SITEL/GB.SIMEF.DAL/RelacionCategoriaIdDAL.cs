using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class RelacionCategoriaIdDAL : BitacoraDAL
    {

        private SIMEFContext db;


        public RelacionCategoriaIdDAL()
        {

        }

        #region Metodos de Consulta a Base Datos
        /// <summary>
        /// Autor: Georgi Mesen Cerdas 
        /// Fecha: 15/11/2022
        /// Ejecutar procedimiento almacenado para obtener datos de relacion categoria id
        /// </summary>
        /// <param name="objRelacionCategoriaId"></param>
        /// <returns></returns>
        public List<RelacionCategoriaId> ObtenerDatos(RelacionCategoriaId objRelacionCategoriaId)
        {
            List<RelacionCategoriaId> ListaRelacionCategoriaId = new List<RelacionCategoriaId>();

            using (db = new SIMEFContext())
            {
                ListaRelacionCategoriaId = db.Database.SqlQuery<RelacionCategoriaId>
                ("execute spObtenerRelacionCategoriaId @idRelacion,@idCategoriaId",
                new SqlParameter("@idRelacion", objRelacionCategoriaId.idRelacion),
                new SqlParameter("@idCategoriaId", objRelacionCategoriaId.idCategoriaId)
                ).ToList();


                ListaRelacionCategoriaId = ListaRelacionCategoriaId.Select(X => new RelacionCategoriaId
                {
                    idRelacion = X.idRelacion,
                    idCategoriaId = X.idCategoriaId,

                }).ToList();

                return ListaRelacionCategoriaId;
            }

        }

        /// <summary>
        /// Autor: Georgi Mesen Cerdas
        /// Fecha: 15/11/2022
        /// Ejecutar procedimiento almacenado para actualizar datos de relacion categoria id
        /// </summary>
        /// <param name="objCategoria"></param>
        /// <returns></returns>
        public List<RelacionCategoriaId> ActualizarDatos(RelacionCategoriaId objeto)
        {
            List<RelacionCategoriaId> ListaRelacionCategoriaId = new List<RelacionCategoriaId>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoriaId = db.Database.SqlQuery<RelacionCategoriaId>
                ("execute spActualizarRelacionCategoria @idRelacion ,@idCategoriaId ",
                     new SqlParameter("@idRelacion", objeto.idRelacion),
                     new SqlParameter("@idCategoriaId", objeto.idCategoriaId)
                    ).ToList();

                ListaRelacionCategoriaId = ListaRelacionCategoriaId.Select(x => new RelacionCategoriaId()
                {
                    idRelacion = x.idRelacion,
                    idCategoriaId = x.idCategoriaId,
                }).ToList();
            }
            return ListaRelacionCategoriaId;
        }

        #endregion|

    }
}
