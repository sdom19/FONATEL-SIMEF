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
    using GB.SIMEF.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    [Table("RelacionCategoria")]
    public partial class RelacionCategoria
    {
        public RelacionCategoria()
        {
            this.DetalleRelacionCategoria = new List<DetalleRelacionCategoria>();
            this.RelacionCategoriaId = new List<RelacionCategoriaId> ();
        }

        [Key]
        public int IdRelacionCategoria { get; set; }

        [MaxLength(10, ErrorMessage = "Máximo de caracteres permitido *")]
        public string Codigo { get; set; }
        [MaxLength(10, ErrorMessage = "Máximo de caracteres permitido *")]
        public string Nombre { get; set; }
        //VALIDAR EL RANGO
        public int CantidadCategoria { get; set; }
        public int idCategoria { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public int idEstado { get; set; }
        public int CantidadFilas { get; set; }


        #region Variables que no estan en la entiendad
        [NotMapped]
        public virtual EstadoRegistro EstadoRegistro { get; set; }

        [NotMapped]
        public bool TieneDetalle { get; set; }

        [NotMapped]

        public List<DetalleRelacionCategoria> DetalleRelacionCategoria { get; set; }

        [NotMapped]
        public string id { get; set; }

        [NotMapped]
        public string  detalleid { get; set; }

        [NotMapped]

        public List<RelacionCategoriaId> RelacionCategoriaId { get; set; }

        [NotMapped]

        public CategoriasDesagregacion CategoriasDesagregacionid;

        #endregion

        public override string ToString()
        {
            StringBuilder json = new StringBuilder();
            json.Append("{\"Código\":\"").Append(this.Codigo).Append("\",");
            json.Append("\"Nombre de la relación\":\"").Append(this.Nombre).Append("\",");
            json.Append("\"Cantidad de atributos\":\"").Append(this.CantidadCategoria).Append("\",");
            

            json.Append("\"ID de la Categoría\":").Append(this.idCategoria).Append(",");

            json.Append("\"Cantidad de filas\":\"").Append(this.CantidadFilas).Append("\",");

            switch ((int)this.EstadoRegistro.IdEstadoRegistro)
            {
                case (int)Constantes.EstadosRegistro.Desactivado:
                    json.Append("\"Estado\":\"").Append(Enum.GetName(typeof(Constantes.EstadosRegistro), (int)Constantes.EstadosRegistro.Desactivado)).Append("\"}");
                    break;
                case (int)Constantes.EstadosRegistro.Activo:
                    json.Append("\"Estado\":\"").Append(Enum.GetName(typeof(Constantes.EstadosRegistro), (int)Constantes.EstadosRegistro.Activo)).Append("\"}");
                    break;
                case (int)Constantes.EstadosRegistro.Eliminado:
                    json.Append("\"Estado\":\"").Append(Enum.GetName(typeof(Constantes.EstadosRegistro), (int)Constantes.EstadosRegistro.Eliminado)).Append("\"}");
                    break;
                case (int)Constantes.EstadosRegistro.EnProceso:
                    json.Append("\"Estado\":\"").Append(Enum.GetName(typeof(Constantes.EstadosRegistro), (int)Constantes.EstadosRegistro.EnProceso)).Append("\"}");
                    break;
            }

            string resultado = json.ToString();

            return resultado;
        }
    }
}
