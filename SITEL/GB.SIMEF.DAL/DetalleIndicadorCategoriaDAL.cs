using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class DetalleIndicadorCategoriaDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 01/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles indicador de una categoría
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        public List<DetalleIndicadorCategoria> ObtenerDatos(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            List<DetalleIndicadorCategoria> listaDetalles = new List<DetalleIndicadorCategoria>();
            List<Model_spObtenerDetallesIndicadorCategoria> listado = new List<Model_spObtenerDetallesIndicadorCategoria>();

            using (db = new SIMEFContext())
            {
                listado = db.Database.SqlQuery<Model_spObtenerDetallesIndicadorCategoria>
                    ("execute spObtenerDetallesIndicadorCategoria @pIdIndicador, @pIdCategoria, @pDetallesAgrupados",
                     new SqlParameter("@pIdIndicador", pDetalleIndicadorCategoria.idIndicador),
                     new SqlParameter("@pIdCategoria", pDetalleIndicadorCategoria.idCategoria),
                     new SqlParameter("@pDetallesAgrupados", pDetalleIndicadorCategoria.DetallesAgrupados)
                    ).ToList();
            }

            listaDetalles = listado.Select(x => new DetalleIndicadorCategoria()
            {
                idIndicadorString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                idCategoriaString = Utilidades.Encriptar(x.IdCategoria.ToString()),
                idCategoriaDetalleString = Utilidades.Encriptar(x.IdCategoriaDetalle.ToString()),
                Etiquetas = x.Etiquetas,
                Codigo = x.Codigo,
                NombreCategoria = x.NombreCategoria,
            }).ToList();

            return listaDetalles;
        }

        /// <summary>
        /// 06/09/2022
        /// José Navarro Acuña
        /// Modelo privado para obtener el resultado del procedimiento almacenado: spObtenerDetallesIndicadorCategoria
        /// </summary>
        private class Model_spObtenerDetallesIndicadorCategoria
        {
            public int IdIndicador { get; set; }
            public int IdCategoria { get; set; }
            public int IdCategoriaDetalle { get; set; }
            public string Etiquetas { get; set; }
            public string Codigo { get; set; }
            public string NombreCategoria { get; set; }
        }
    }
}
