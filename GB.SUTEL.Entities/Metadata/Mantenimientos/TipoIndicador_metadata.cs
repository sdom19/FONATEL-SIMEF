using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities;


namespace GB.SUTEL.Entities
{
    #region Tipo Indicador
    [MetadataType(typeof(TIPOINDICADOR_metadata))]
    public partial class TipoIndicador
    { }
    public class TIPOINDICADOR_metadata
    {
        [Display(Name = "Identificador")]
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public int IdTipoInd { get; set; }

        [Display(Name = "Nombre")]
        [RegularExpression(@"^[A-Za-z0-9 .,áéíóúÁÉÍÓÚñÑ]*$", ErrorMessage = "Los caracteres que digitó son inválidos.")]
         [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        [MaxLength(250, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "tipoIndicadorTamanno")]
        public string DesTipoInd { get; set; }
    }
    #endregion
}
