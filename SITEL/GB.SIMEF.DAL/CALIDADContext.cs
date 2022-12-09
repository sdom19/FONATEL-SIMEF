using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;
using GB.SIMEF.Entities;

namespace GB.SIMEF.DAL
{
    public partial class CALIDADContext : DbContext
    {
        public CALIDADContext()
           : base("name=CALIDADEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CALIDADContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
