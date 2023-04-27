using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class SIGITELSIMEFContext: DbContext
    {
        public SIGITELSIMEFContext()
          : base("name=SIGITELSIMEFEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<SIGITELContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<CatalogoPantallaSIGITEL> CatalogoPantallaSIGITEL { get; set; }
        public virtual DbSet<TipoContenidoTextoSIGITEL> TipoContenidoTextoSIGITEL { get; set; }
    }
}
