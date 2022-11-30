using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;
using GB.SIMEF.Entities;

namespace GB.SIMEF.DAL
{
    public partial class SITELContext:DbContext
    {
        public SITELContext()
           : base("name=SITELEntities")
        {
        }

        public DbSet<Usuario> Usuario { get; set; }


    }
}
