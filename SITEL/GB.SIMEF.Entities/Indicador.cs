//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GB.SIMEF.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Indicador")]
    public partial class Indicador
    {
     
        public Indicador()
        {
            //this.DetalleFormularioWeb = new HashSet<DetalleFormularioWeb>();
            //this.DetalleIndicadorCategoria = new HashSet<DetalleIndicadorCategoria>();
            //this.DetalleIndicadorVariables = new HashSet<DetalleIndicadorVariables>();
            //this.DefinicionIndicadores = new HashSet<DefinicionIndicadores>();
            //this.ReglaValidacion = new HashSet<ReglaValidacion>();
        }

        [Key]
        public int idIndicador { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int IdTipoIndicador { get; set; }
        public int IdClasificacion { get; set; }
        public int idGrupo { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> CantidadVariableDato { get; set; }
        public Nullable<int> CantidadCategoriasDesagregacion { get; set; }
        public Nullable<int> IdUnidadEstudio { get; set; }
        public int idTipoMedida { get; set; }
        public int IdFrecuencia { get; set; }
        public Nullable<bool> Interno { get; set; }
        public bool Solicitud { get; set; }
        public string Fuente { get; set; }
        public string Notas { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool VisualizaSigitel { get; set; }
        public int idEstado { get; set; }

        public virtual ClasificacionIndicadores ClasificacionIndicadores { get; set; }

        public virtual ICollection<DetalleFormularioWeb> DetalleFormularioWeb { get; set; }

        public virtual ICollection<DetalleIndicadorCategoria> DetalleIndicadorCategoria { get; set; }

        public virtual ICollection<DetalleIndicadorVariables> DetalleIndicadorVariables { get; set; }
        public virtual EstadoRegistro EstadoRegistro { get; set; }
        public virtual FrecuenciaEnvio FrecuenciaEnvio { get; set; }
        public virtual GrupoIndicadores GrupoIndicadores { get; set; }

        public virtual ICollection<DefinicionIndicadores> DefinicionIndicadores { get; set; }
        public virtual TipoMedida TipoMedida { get; set; }
        public virtual UnidadEstudio UnidadEstudio { get; set; }
        public virtual TipoIndicadores TipoIndicadores { get; set; }

        public virtual ICollection<ReglaValidacion> ReglaValidacion { get; set; }

        #region Variables que no forman parte del contexto
        [NotMapped]
        public string id { get; set; }
        [NotMapped]
        public int nuevoEstado { get; set; }
        #endregion
    }
}
