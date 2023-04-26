using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities.DTO
{
    public class JobMotorFormulaDTO
    {
        public Guid idJob { get; set; }
        public DateTime lanzado { get; set; }
        public string estado { get; set; }

        public List<TareaJobMotorDTO> tareas { get; set; }
    }
}
