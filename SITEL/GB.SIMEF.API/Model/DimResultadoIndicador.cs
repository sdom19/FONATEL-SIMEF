﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GB.SIMEF.API.Model
{
    [Table("DimResultadoIndicador", Schema = "FONATEL")]
    public class DimResultadoIndicador
    {
        public DimResultadoIndicador()
        {

        }

        [Key]
        public int IdResultado { get; set; }
        public int IdIndicador { get; set; }
        public int idGrupo { get; set; }
        public string NombreGrupo { get; set; }
        public int idVariable { get; set; }
        public string NombreVariable { get; set; }
        public decimal ValorColumna { get; set; }
        public string AnnoMes { get; set; }

        #region Variables que no forman parte del contexto
        [NotMapped]
        public string AnnoInicio { get; set; }
        [NotMapped]
        public string MesInicio { get; set; }
        [NotMapped]
        public string AnnoFin { get; set; }
        [NotMapped]
        public string MesFin { get; set; }
        [NotMapped]
        public double Total { get; set; }
        #endregion
    }
}
