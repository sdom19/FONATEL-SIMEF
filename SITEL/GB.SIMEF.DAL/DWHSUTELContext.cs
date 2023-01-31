using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class DWHSUTELContext : DbContext
    {
        public DWHSUTELContext()
           : base("name=DWHSUTELEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DWHSUTELContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
