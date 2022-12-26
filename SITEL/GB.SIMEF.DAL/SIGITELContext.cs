using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;
using GB.SIMEF.Entities;

namespace GB.SIMEF.DAL
{
    public partial class SIGITELContext : DbContext
    {
        public SIGITELContext()
           : base("name=SIGITELEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<SIGITELContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
