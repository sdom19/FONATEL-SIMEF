using GB.SUTEL.BL.UmbralesPesosRelativos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.UmbralesPesosRelativos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace GB.SUTEL.UI.Models
{
    public partial class UmbralesPesosRelativosViewModel
    {

        public int IdDireccion { get; set; }
        public int IdServicio { get; set; }
        public string IdIndicador { get; set; }
        public string NombreIndicador { get; set; }
        public decimal Peso { get; set; }
        public decimal Umbral { get; set; }

        public List<ServiIndicadorEnti> LisInicadorServi { get; set; }
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

        public List<SelectListItem> LstUsuarios()
        {
            List<SelectListItem> _LisUsuarios = new List<SelectListItem>();

            var GetLsUsuarios = new UmbralesPesosRelativosBL().GetUser();
            if (GetLsUsuarios != null)
            {
                _LisUsuarios.Add(new SelectListItem { Text = "Seleccione", Value = "0" });
                foreach (var item in GetLsUsuarios)
                {
                    _LisUsuarios.Add(new SelectListItem { Text = item.NombreUsuario, Value = item.AccesoUsuario.ToString() });
                }
            }

            return _LisUsuarios;
        }
    }
}