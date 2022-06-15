using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GB.SUTEL.Entities;
using System.ComponentModel.DataAnnotations;

namespace GB.SUTEL.UI.Models

{
    public class ConstructorViewModel
    {
        public int idDireccion { get; set; }
        public Constructor constructor { get; set; }
         
         public List<Constructor> listaConstructores { get; set; }
       
        public List<Frecuencia> listaFrecuencia { get; set; }
        public List<Frecuencia> listaDesglose { get; set; }
        public List<Direccion> listaDireccion { get; set; }

        #region criterio

        public ConstructorCriterio criterio { get; set; }

        public String idCriterio { get; set; }
        public String nombreCriterio { get; set; }
        public List<Operador> listaOperadores { get; set; }
        public List<Criterio> listaCriterios { get; set; }
        public string idOperador { get; set; }
        public string nombreOperador { get; set; }
        #endregion

        #region detalleAgrupacion
        public DetalleAgrupacion detalleAgrupacion { get; set; }
        public DetalleAgrupacion detalleAgrupacionPadre { get; set; }
        public List<DetalleAgrupacion> listaDetallesAgrupacion { get; set; }
        public List<DetalleAgrupacion> listaDetallesAgrupacionPadre { get; set; }
        public int idTipoValor { get; set; }
        public List<TipoValor> listaTipoValor { get; set; }
        public Regla regla { get; set; }
        public List<TipoNivelDetalle> listaTipoNivelDetalle { get; set; }
        #endregion

    }
}