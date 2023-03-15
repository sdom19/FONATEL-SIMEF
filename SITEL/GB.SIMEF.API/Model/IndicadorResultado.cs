using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GB.SIMEF.API.Model
{
    [Table("IndicadorResultado")]
    public class IndicadorResultado
    {
        public IndicadorResultado()
        {

        }

        [Key]
        public int idIndicadorResultado { get; set; }
        public int IdIndicador { get; set; }
        public string NombreIndicador { get; set; }
        public Boolean EstadoIndicador { get; set; }
        public string CodigoIndicador { get; set; }
        public int IdSolicitud { get; set; }
        public string NombreSolicitud { get; set; }
        public string CodigoSolicitud { get; set; }
        public string NombreFuente { get; set; }
        public int IdFormulario { get; set; }
        public string CodigoFormulario { get; set; }
        public string NombreFormulario { get; set; }
        public int idMes { get; set; }
        public string Mes { get; set; }
        public int idGrupoIndicador { get; set; }
        public string NombreGrupo { get; set; }
        public int IdClasificacion { get; set; }
        public string NombreClasificacion { get; set; }
        public int IdAnno { get; set; }
        public string Anno { get; set; }
        public int NumeroFila { get; set; }
        public int IdColumna { get; set; }
        public string NombreColumna { get; set; }
        public string ValorColumna { get; set; }
        public Boolean VariableDato { get; set; }
        public string AnnoMes { get; set; }
    }
}
