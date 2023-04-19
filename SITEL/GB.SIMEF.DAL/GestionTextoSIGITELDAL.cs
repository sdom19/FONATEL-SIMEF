using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;

namespace GB.SIMEF.DAL
{
    public class GestionTextoSIGITELDAL : BitacoraDAL
    {
        private SIGITELSIMEFContext db;

        public List<CatalogoPantallaSIGITEL> ObtenerPantallasSIGITEL()
        {
            List<CatalogoPantallaSIGITEL> data = null;
            using (db = new SIGITELSIMEFContext())
            {
                data = db.CatalogoPantallaSIGITEL.Where(i => i.Estado == true).ToList();
            }
            return data;
        }

        public List<TipoContenidoTextoSIGITEL> ObtenerTipoContenido()
        {
            List<TipoContenidoTextoSIGITEL> data = null;
            using (db = new SIGITELSIMEFContext())
            {
                data = db.TipoContenidoTextoSIGITEL.Where(i => i.Estado == true).ToList();
            }
            return data;
        }

        public List<ContenidoPantallaSIGITEL> ActualizarDatos(ContenidoPantallaSIGITEL obj)
        {
            List<ContenidoPantallaSIGITEL> data = null;
            using (db = new SIGITELSIMEFContext())
            {
                data = db.Database.SqlQuery<ContenidoPantallaSIGITEL>
                    ("EXEC dbo.pa_ActualizarTextoPantallaSIGITEL @IdContenidoPantallaSIGITEL, @IdCatalogoPantallaSIGITEL, @IdTipoContenidoTextoSIGITEL, @Texto, @RutaImagen, @Estado, @Orden",
                    new SqlParameter("@IdContenidoPantallaSIGITEL", obj.IdContenidoPantallaSIGITEL),
                    new SqlParameter("@IdCatalogoPantallaSIGITEL", obj.IdCatalogoPantallaSIGITEL),
                    new SqlParameter("@IdTipoContenidoTextoSIGITEL", obj.IdTipoContenidoTextoSIGITEL),
                    new SqlParameter("@Texto", (obj.Texto == null ? string.Empty : obj.Texto)),
                    new SqlParameter("@RutaImagen", (obj.RutaImagen == null ? string.Empty : obj.RutaImagen)),
                    new SqlParameter("@Estado", obj.Estado),
                    new SqlParameter("@Orden", obj.Orden)
                    ).ToList();

                data = PrepararObjetoListado(data);
            }
            return data;
        }

        public List<ContenidoPantallaSIGITEL> ObtenerDatos(ContenidoPantallaSIGITEL obj)
        {
            List<ContenidoPantallaSIGITEL> data = null;
            using (db = new SIGITELSIMEFContext())
            {
                data = db.Database.SqlQuery<ContenidoPantallaSIGITEL>
                    ("EXEC dbo.pa_ObtenerTextoPantallaSIGITEL @IdCatalogoPantallaSIGITEL",
                    new SqlParameter("@IdCatalogoPantallaSIGITEL", obj.IdCatalogoPantallaSIGITEL)
                    ).ToList();

                data = PrepararObjetoListado(data);
            }
            return data;
        }

        private List<ContenidoPantallaSIGITEL> PrepararObjetoListado(List<ContenidoPantallaSIGITEL> data)
        {
            var result = data.Select(x => new ContenidoPantallaSIGITEL
            {
                IdContenidoPantallaSIGITEL = x.IdContenidoPantallaSIGITEL,
                IdCatalogoPantallaSIGITEL = x.IdCatalogoPantallaSIGITEL,
                IdTipoContenidoTextoSIGITEL = x.IdTipoContenidoTextoSIGITEL,
                Texto = x.Texto,
                RutaImagen = x.RutaImagen,
                Estado = x.Estado,
                Orden = x.Orden,
                TipoContenidoTextoSIGITEL = db.TipoContenidoTextoSIGITEL.Where(t => t.IdTipoContenidoTextoSIGITEL == x.IdTipoContenidoTextoSIGITEL).FirstOrDefault(),
                Pantalla = db.CatalogoPantallaSIGITEL.Where(p => p.IdCatalogoPantallaSIGITEL == x.IdCatalogoPantallaSIGITEL).FirstOrDefault()
            }).ToList();

            return result;
        }
    }
}
