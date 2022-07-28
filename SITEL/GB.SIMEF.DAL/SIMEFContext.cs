using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;


namespace GB.SIMEF.DAL
{
    public class SIMEFContext:DbContext
    {
        public SIMEFContext()
           : base("")
        {
        }
    }
}
