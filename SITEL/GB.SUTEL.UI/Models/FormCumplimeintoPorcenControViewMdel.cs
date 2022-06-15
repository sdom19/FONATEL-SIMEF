using GB.SUTEL.BL.UmbralesPesosRelativos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Models
{
    public partial class FormCumplimeintoPorcenControViewMdel
    {
        public int IdParamFormulas { get; set; }
        public int IdServicio { get; set; }
        public string IdIndicador { get; set; }
        public string FormulaPorcentaje { get; set; }
        public string FormulaCumplimiento { get; set; }
        public string Criterios { get; set; }
        public DateTime FechaUltimaActualizacion { get; set; }
        public string Usuario { get; set; }
        public List<string> FromArray { get; set; }
        public List<string> ArrayIF { get; set; }
        public List<string> ArrayVerdadero { get; set; }
        public List<string> ArrayFalso { get; set; }

        #region Configuracion
        public List<SelectListItem> LstDireccion()
        {
            List<SelectListItem> _LisDireccion = new List<SelectListItem>();

            var GetLsDireccion = new UmbralesPesosRelativosBL().GetLisDirecciones();
            if (GetLsDireccion != null)
            {
                _LisDireccion.Add(new SelectListItem { Text = "Seleccione", Value = "0" });
                foreach (var item in GetLsDireccion)
                {
                    _LisDireccion.Add(new SelectListItem { Text = item.Nombre, Value = item.IdDireccion.ToString() });
                }
            }

            return _LisDireccion;
        }

        public List<SelectListItem> LstServicios()
        {
            List<SelectListItem> _LisServicios = new List<SelectListItem>();

            var GetLsServicios = new UmbralesPesosRelativosBL().GetLisServicio();
            if (GetLsServicios != null)
            {
                _LisServicios.Add(new SelectListItem { Text = "Seleccione", Value = "0" });
                foreach (var item in GetLsServicios)
                {
                    _LisServicios.Add(new SelectListItem { Text = item.DesServicio, Value = item.IdServicio.ToString() });
                }
            }

            return _LisServicios;
        }

#endregion
    }
}