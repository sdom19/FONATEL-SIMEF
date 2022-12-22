using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;
using GB.SIMEF.Entities;

namespace GB.SIMEF.DAL
{
    public partial class BDHistoricoContext : DbContext
    {
        public BDHistoricoContext()
           : base("name=BDHistorico")
        {
        }

        public virtual DbSet<DatoHistorico> DatoHistorico { get; set; }

        public virtual DbSet<DetalleDatoHistoricoColumna> DetalleDatoHistoricoColumna { get; set; }

        public virtual DbSet<DetalleDatoHistoricoFila> DetalleDatoHistoricoFila { get; set; }


    }
}
