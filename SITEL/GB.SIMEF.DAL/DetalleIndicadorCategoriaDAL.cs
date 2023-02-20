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
                    ("execute spObtenerDetallesIndicadorCategoria  @pIdIndicador, @pIdCategoria, @pDetallesAgrupados",
                     new SqlParameter("@pIdIndicador", pDetalleIndicadorCategoria.idIndicador),
                     new SqlParameter("@pIdCategoria", pDetalleIndicadorCategoria.idCategoria),
                     new SqlParameter("@pDetallesAgrupados", pDetalleIndicadorCategoria.DetallesAgrupados)
                    ).ToList();

                listaDetalles = listado.Select(x => new DetalleIndicadorCategoria()
                {
                    id = Utilidades.Encriptar(x.IdDetalleIndicador.ToString()),
                    idIndicadorString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    idCategoriaString = Utilidades.Encriptar(x.IdCategoria.ToString()),
                    idCategoriaDetalleString = x.IdCategoriaDetalle != null ? Utilidades.Encriptar(x.IdCategoriaDetalle.ToString()) : null,
                    Estado = x.Estado,
                    Etiquetas = string.IsNullOrEmpty(x.Etiquetas) ? Constantes.defaultInputTextValue : x.Etiquetas,
                    Codigo = x.Codigo,
                    NombreCategoria = x.NombreCategoria,
                }).ToList();
            }

            return listaDetalles;
        }

        /// <summary>
        /// 20/02/2023
        /// José Navarro Acuña
        /// Función que permite obtener los detalles indicador de una categoría e indicador
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        public List<DetalleIndicadorCategoria> ObtenerDetallesDeCategoria(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            List<DetalleIndicadorCategoria> listaDetalles = new List<DetalleIndicadorCategoria>();
            List<Model_spObtenerDetallesIndicadorCategoria> listado = new List<Model_spObtenerDetallesIndicadorCategoria>();

            using (db = new SIMEFContext())
            {
                listado = db.Database.SqlQuery<Model_spObtenerDetallesIndicadorCategoria>
                    ("execute spObtenerDetallesDeCategoria  @pIdIndicador, @pIdCategoria",
                     new SqlParameter("@pIdIndicador", pDetalleIndicadorCategoria.idIndicador),
                     new SqlParameter("@pIdCategoria", pDetalleIndicadorCategoria.idCategoria)
                    ).ToList();

                listaDetalles = listado.Select(x => new DetalleIndicadorCategoria()
                {
                    id = Utilidades.Encriptar(x.IdDetalleIndicador.ToString()),
                    idIndicadorString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    idCategoriaString = Utilidades.Encriptar(x.IdCategoria.ToString()),
                    idCategoriaDetalleString = x.IdCategoriaDetalle != null ? Utilidades.Encriptar(x.IdCategoriaDetalle.ToString()) : null,
                    Estado = x.Estado,
                    Etiquetas = x.Etiquetas,
                    Codigo = x.Codigo,
                    NombreCategoria = x.NombreCategoria,
                }).ToList();
            }

            return listaDetalles;
        }

        /// <summary>
        /// 08/11/2022
        /// José Navarro Acuña
        /// Función que permite insertar o actualizar un detalle categoria de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        public List<DetalleIndicadorCategoria> ActualizarDatos(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            List<DetalleIndicadorCategoria> listaDetalles = new List<DetalleIndicadorCategoria>();

            using (db = new SIMEFContext())
            {
                listaDetalles = db.Database.SqlQuery<DetalleIndicadorCategoria>
                    ("execute spActualizarIndicadorCategoria @pIdDetalleIndicador, @pIdIndicador,@pidCategoria ,@pidCategoriaDetalle, @pEstado ",
                     new SqlParameter("@pIdDetalleIndicador", pDetalleIndicadorCategoria.idDetalleIndicador),
                     new SqlParameter("@pIdIndicador", pDetalleIndicadorCategoria.idIndicador),
                     new SqlParameter("@pidCategoria", pDetalleIndicadorCategoria.idCategoria),
                     new SqlParameter("@pidCategoriaDetalle", pDetalleIndicadorCategoria.idCategoriaDetalle),
                     new SqlParameter("@pEstado", pDetalleIndicadorCategoria.Estado)
                    ).ToList();
            }

            listaDetalles = listaDetalles.Select(x => new DetalleIndicadorCategoria()
            {
                id = Utilidades.Encriptar(x.idDetalleIndicador.ToString()),
                idCategoriaString = Utilidades.Encriptar(x.idCategoria.ToString()),
                idIndicadorString = Utilidades.Encriptar(x.idIndicador.ToString()),
                Estado = x.Estado
            }).ToList();

            return listaDetalles;
        }

        /// <summary>
        /// 06/09/2022
        /// José Navarro Acuña
        /// Modelo privado para obtener el resultado del procedimiento almacenado: spObtenerDetallesIndicadorCategoria
        /// </summary>
        /// 
        private class Model_spObtenerDetallesIndicadorCategoria
        {
            public int IdDetalleIndicador { get; set; }
            public int IdIndicador { get; set; }
            public int IdCategoria { get; set; }
            public int? IdCategoriaDetalle { get; set; }
            public string Etiquetas { get; set; }
            public string Codigo { get; set; }
            public string NombreCategoria { get; set; }
            public bool Estado { get; set; }
            public int IdTipoDetalle { get; set; }
        }

        /// <summary>
        /// 05/01/2023
        /// Georgi Mesén Cerdas
        /// Función obtener los datos los DetalleIndicadorCategoria
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns>Retorna un DetalleIndicadorCategoria</returns>
        public List<DetalleIndicadorCategoria> ObtenerVisualizarCategorias(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            List<DetalleIndicadorCategoria> listaDetalles = new List<DetalleIndicadorCategoria>();
            List<Model_spObtenerDetallesIndicadorCategoria> listado = new List<Model_spObtenerDetallesIndicadorCategoria>();
            using (db = new SIMEFContext())
            {
                listado = db.Database.SqlQuery<Model_spObtenerDetallesIndicadorCategoria>
                    ("execute spObtenerVisualizarCategoria  @pIdIndicador, @pIdCategoria, @pDetallesAgrupados",
                     new SqlParameter("@pIdIndicador", pDetalleIndicadorCategoria.idIndicador),
                     new SqlParameter("@pIdCategoria", pDetalleIndicadorCategoria.idCategoria),
                     new SqlParameter("@pDetallesAgrupados", pDetalleIndicadorCategoria.DetallesAgrupados)
                    ).ToList();
                listaDetalles = listado.Select(x => new DetalleIndicadorCategoria()
                {
                    id = Utilidades.Encriptar(x.IdDetalleIndicador.ToString()),
                    idIndicadorString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    idCategoriaString = Utilidades.Encriptar(x.IdCategoria.ToString()),
                    idCategoriaDetalleString = x.IdCategoriaDetalle != null ? Utilidades.Encriptar(x.IdCategoriaDetalle.ToString()) : null,
                    Estado = x.Estado,
                    Etiquetas = string.IsNullOrEmpty(x.Etiquetas) ? Constantes.defaultInputTextValue : x.Etiquetas,
                    Codigo = x.Codigo,
                    NombreCategoria = x.NombreCategoria,
                    IdTipoDetalle = x.IdTipoDetalle,
                    idCategoria = x.IdCategoria
                }).ToList();
            }
            return listaDetalles;
        }
    }
}
