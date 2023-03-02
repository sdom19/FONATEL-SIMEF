using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities.DTO
{
    public class JobMotorFormulaDTO
    {
        public Guid id { get; set; }
        public DateTime createAt { get; set; }
        public string status { get; set; }
    }
}
