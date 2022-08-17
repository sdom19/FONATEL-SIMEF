using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;

namespace GB.SIMEF.DAL
{
    public class RelacionCategoriaDAL:BitacoraDAL
    {
        private SIMEFContext db;

        #region Metodos de Consulta a Base Datos

        public List<RelacionCategoria> ObtenerDatos(RelacionCategoria objRelacionCategoria)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();

            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute spObtenerRelacionCategorias @idRelacionCategoria,@idCategoria,@idEstado",
                new SqlParameter("@idRelacionCategoria", objRelacionCategoria.idRelacionCategoria),
                new SqlParameter("@idCategoria", objRelacionCategoria.idCategoria),
                new SqlParameter("@idEstado", objRelacionCategoria.idEstado)
                ).ToList();


                ListaRelacionCategoria = ListaRelacionCategoria.Select(X => new RelacionCategoria
                {
                    idRelacionCategoria = X.idRelacionCategoria,
                    Codigo = X.Codigo,
                    Nombre = X.Nombre,
                    CantidadCategoria = X.CantidadCategoria,
                    idCategoria = X.idCategoria,
                    idCategoriaValor = X.idCategoriaValor,
                    FechaCreacion = X.FechaCreacion,
                    FechaModificacion = X.FechaModificacion,
                    UsuarioCreacion = X.UsuarioCreacion,
                    UsuarioModificacion = X.UsuarioModificacion,
                    idEstado = X.idEstado,
                    DetalleRelacionCategoria = db.DetalleRelacionCategoria.Where(p =>p.IdRelacionCategoria == X.idRelacionCategoria & p.Estado==true).ToList(),
                    EstadoRegistro = db.EstadoRegistro.Where(p => p.idEstado == X.idEstado).FirstOrDefault()
                }).ToList();

                return ListaRelacionCategoria;
            }

        }

        #endregion

    }
}
