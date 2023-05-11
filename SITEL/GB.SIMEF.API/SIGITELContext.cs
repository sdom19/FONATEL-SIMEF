using System;
using System.ComponentModel.DataAnnotations.Schema;
using GB.SIMEF.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;


namespace SIMEF.API.Models
{
    public partial class SIGITELContext : DbContext
    {
        public SIGITELContext()
        {
        }

        public SIGITELContext(DbContextOptions<SIGITELContext> options)
            : base(options)
        {
        }


        public virtual DbSet<DefinicionIndicador> DefinicionIndicador { get; set; }
        public virtual DbSet<GrupoIndicador> GrupoIndicador { get; set; }
        public virtual DbSet<TipoIndicador> TipoIndicador { get; set; }
        public virtual DbSet<TablaIndicador> TablaIndicador { get; set; }
        public virtual DbSet<DetalleIndicadorVariable> DetalleIndicadorVariable { get; set; }
        public virtual DbSet<IndicadorResultado> IndicadorResultado { get; set; }
    }
}
