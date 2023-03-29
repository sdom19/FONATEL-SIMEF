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
    using System.Text;
    using GB.SIMEF.Resources;

    [Table("CategoriaDesagregacion")]
    public class CategoriaDesagregacion
    {
        public CategoriaDesagregacion()
        {
            this.DetalleCategoriaNumerico = new DetalleCategoriaNumerico();
            this.DetalleCategoriaFecha = new DetalleCategoriaFecha();
            this.DetalleCategoriaTexto = new List<DetalleCategoriaTexto>();
            this.EstadoRegistro = new EstadoRegistro();
            this.id = string.Empty;
            this.TieneDetalle = false;
            this.IndicadorAsociados = string.Empty;

        }
        [Key]


        public int idCategoriaDesagregacion { get; set; }
        [MaxLength(30)]
        public string Codigo { get; set; }
        [MaxLength(300)]
        public string NombreCategoria { get; set; }
        public int CantidadDetalleDesagregacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public int idEstadoRegistro { get; set; }

        public int IdTipoDetalleCategoria { get; set; }

        public int IdTipoCategoria { get; set; }




        #region Varibles que no forman parte del contexto


        [NotMapped]
        public virtual TipoCategoria TipoCategoria { get; set; }
        [NotMapped]
   
        public bool EsParcial { get; set; }
        public virtual List<DetalleCategoriaTexto> DetalleCategoriaTexto { get; set; }
        [NotMapped]
        public virtual EstadoRegistro EstadoRegistro { get; set; }
        [NotMapped]
        public bool TieneDetalle { get; set; }

        [NotMapped]
        public string id { get; set; }
        [NotMapped]

        public virtual DetalleCategoriaNumerico DetalleCategoriaNumerico { get; set; }
        [NotMapped]

        public virtual DetalleCategoriaFecha DetalleCategoriaFecha { get; set; }

        [NotMapped]

        public string IndicadorAsociados { get; set; }

        
        public override string ToString()
        {
            StringBuilder json = new StringBuilder();
            json.Append("{\"Código\":\"").Append(this.Codigo).Append("\",");
            json.Append("\"Nombre de categoria\":\"").Append(this.NombreCategoria).Append("\",");
            json.Append("\"Tipo de categoría\":\"").Append(this.TipoCategoria.Nombre).Append("\",");
            switch (this.IdTipoDetalleCategoria)
            {
                case (int)Constantes.TipoDetalleCategoriaEnum.Fecha:
                    json.Append("\"Tipo de detalle\":\"").Append(Enum.GetName(typeof(Constantes.TipoDetalleCategoriaEnum), (int)Constantes.TipoDetalleCategoriaEnum.Fecha)).Append("\",");
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Alfanumerico:
                    json.Append("\"Tipo de detalle\":\"").Append(Enum.GetName(typeof(Constantes.TipoDetalleCategoriaEnum), (int)Constantes.TipoDetalleCategoriaEnum.Alfanumerico)).Append("\",");
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Numerico:
                    json.Append("\"Tipo de detalle\":\"").Append(Enum.GetName(typeof(Constantes.TipoDetalleCategoriaEnum), (int)Constantes.TipoDetalleCategoriaEnum.Numerico)).Append("\",");
                    break;
                case (int)Constantes.TipoDetalleCategoriaEnum.Texto:
                    json.Append("\"Tipo de detalle\":\"").Append(Enum.GetName(typeof(Constantes.TipoDetalleCategoriaEnum), (int)Constantes.TipoDetalleCategoriaEnum.Texto)).Append("\",");
                    break;
            }
            
            json.Append("\"Tiene detalle\":").Append(this.TieneDetalle ? "\"Sí\"" : "\"No\"").Append(",");
            
            json.Append("\"Cantidad de detalles\":\"").Append(this.CantidadDetalleDesagregacion).Append("\",");
            
            switch((int)this.EstadoRegistro.IdEstadoRegistro)
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


        #endregion

    }
}