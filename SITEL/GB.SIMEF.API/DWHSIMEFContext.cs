using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;


namespace SIMEF.API.Models
{
    public partial class DWHSIMEFContext : DbContext
    {
        public DWHSIMEFContext()
        {

        }
        public DWHSIMEFContext(DbContextOptions<DWHSIMEFContext> options) : base(options)
        { 
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Connection.DWHSIMEF);
            }
        }

        public virtual DbSet<DimDefinicionIndicador> DimDefinicionIndicador { get; set; }
        public virtual DbSet<DimGrupoIndicadores> DimGrupoIndicador { get; set; }
        public virtual DbSet<DimTipoIndicadores> DimTipoIndicadores { get; set; }
        public virtual DbSet<DimTablaIndicadores> DimTablaIndicadores { get; set; }
        public virtual DbSet<DimDetalleIndicadorVariables> DimDetalleIndicadorVariables { get; set; }

    }
}
