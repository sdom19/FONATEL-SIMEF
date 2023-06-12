using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;
using GB.SIMEF.Entities;

namespace GB.SIMEF.DAL
{
    public partial class SIMEFContext:DbContext
    {
        public SIMEFContext()
           : base("name=SIMEFEntities")
        {
            base.Configuration.ProxyCreationEnabled = false;
        }




        public virtual DbSet<CategoriaDesagregacion> CategoriasDesagregacion { get; set; }

        public virtual DbSet<TipoDetalleCategoria> TipoDetalleCategoria { get; set; }
        public virtual DbSet<DetalleCategoriaTexto> DetalleCategoriaTexto { get; set; }
        public virtual DbSet<EstadoRegistro> EstadoRegistro { get; set; }
        public virtual DbSet<Bitacora> Bitacora { get; set; }
        public virtual DbSet<TipoCategoria> TipoCategoria { get; set; }

        public virtual DbSet<DetalleCategoriaFecha> DetalleCategoriaFecha { get; set; }
        public virtual DbSet<DetalleCategoriaNumerico> DetalleCategoriaNumerico { get; set; }


        public virtual DbSet<PlantillaHtml> PlantillaHtml { get; set; }

        public virtual DbSet<Anno> Anno { get; set; }

        public virtual DbSet<ClasificacionIndicador> ClasificacionIndicadores { get; set; }


        public virtual DbSet<DetalleFormularioWeb> DetalleFormularioWeb { get; set; }
        public virtual DbSet<DetalleFuenteRegistro> DetalleFuentesRegistro { get; set; }
        public virtual DbSet<DetalleIndicadorCategoria> DetalleIndicadorCategoria { get; set; }
        public virtual DbSet<DetalleIndicadorVariable> DetalleIndicadorVariables { get; set; }
        
        
        
        public virtual DbSet<DetalleRegistroIndicadorFonatel> DetalleRegistroIndcadorFonatel { get; set; }
        //public virtual DbSet<DetalleRegistroIndicadorVariable> DetalleRegistroIndicadorVariable { get; set; }
        public virtual DbSet<DetalleRelacionCategoria> DetalleRelacionCategoria { get; set; }
        
        public virtual DbSet<DetalleSolicitudFormulario> DetalleSolicitudFormulario { get; set; }
        public virtual DbSet<EnvioSolicitud> EnvioSolicitud { get; set; }

        public virtual DbSet<RelacionCategoriaAtributo> RelacionCategoriaAtributo { get; set; }
        //public virtual DbSet<FormulaIndicadorFecha> FormulaIndicadorFecha { get; set; }
        //public virtual DbSet<FormulaIndicadorMC> FormulaIndicadorMC { get; set; }
        public virtual DbSet<FormulaNivelCalculoCategoria> FormulaNivelCalculoCategoria { get; set; }
        public virtual DbSet<FormularioWeb> FormularioWeb { get; set; }
        public virtual DbSet<FormulaCalculo> FormulaCalculo { get; set; }
        //public virtual DbSet<FormulasCalculoDetalle> FormulasCalculoDetalle { get; set; }
        //public virtual DbSet<FormulasOperador> FormulasOperador { get; set; }
        public virtual DbSet<FrecuenciaEnvio> FrecuenciaEnvio { get; set; }
        //public virtual DbSet<FuenteIndicador> FuenteIndicador { get; set; }
        public virtual DbSet<FuenteRegistro> FuentesRegistro { get; set; }
        public virtual DbSet<GrupoIndicador> GrupoIndicadores { get; set; }
        public virtual DbSet<GraficoInforme> GraficoInforme { get; set; }
        public virtual DbSet<Indicador> Indicador { get; set; }
        
        public virtual DbSet<Mes> Mes { get; set; }
        public virtual DbSet<OperadorAritmetico> OperadorArismetico { get; set; }
        //public virtual DbSet<DetalleRegistroIndicadorVariableFonatel> DetalleRegistroIndicadorVariableFonatel { get; set; }

        //public virtual DbSet<DetalleRegistroIndicadorCategoriaFonatel> DetalleRegistroIndicadorCategoriaFonatel { get; set; }
        public virtual DbSet<RegistroIndicadorFonatel> RegistroIndicadorFonatel { get; set; }
        public virtual DbSet<ReglaAtributoValido> ReglaAtributosValidos { get; set; }
        public virtual DbSet<ReglaComparacionConstante> ReglaComparacionConstante { get; set; }
        //public virtual DbSet<ReglaComparacionIndicador> ReglaComparacionIndicador { get; set; }
        public virtual DbSet<ReglaSecuencial> ReglaSecuencial { get; set; }
        public virtual DbSet<ReglaValidacion> ReglaValidacion { get; set; }
        
        public virtual DbSet<DetalleReglaValidacion> DetalleReglaValidacion { get; set; }
        public virtual DbSet<RelacionCategoria> RelacionCategoria { get; set; }
        public virtual DbSet<Solicitud> Solicitud { get; set; }
        public virtual DbSet<SolicitudDetalleFuentes> SolicitudDetalleFuentes { get; set; }
        public virtual DbSet<SolicitudEnvioProgramado> SolicitudEnvioProgramado { get; set; }

        public virtual DbSet<TipoIndicador> TipoIndicadores { get; set; }
        public virtual DbSet<TipoMedida> TipoMedida { get; set; }
        public virtual DbSet<TipoReglaValidacion> TipoReglaValidacion { get; set; }
        public virtual DbSet<UnidadEstudio> UnidadEstudio { get; set; }
        public virtual DbSet<DefinicionIndicador> DefinicionIndicadores { get; set; }
        public virtual DbSet<ReglaIndicadorSalida> ReglaIndicadorSalida { get; set; }
        public virtual DbSet<ReglaIndicadorEntrada> ReglaIndicadorEntrada { get; set; }
        public virtual DbSet<ReglaIndicadorEntradaSalida> ReglaIndicadorEntradaSalida { get; set; }
        public virtual DbSet<RelacionCategoriaId> RelacionCategoriaId { get; set; }

        public virtual DbSet<AcumulacionFormula> AcumulacionFormula { get; set; }
        public virtual DbSet<FormulaCalculoTipoFecha> FormulaCalculoTipoFecha { get; set; }
        public virtual DbSet<FormulaCalculoUnidadMedida> FormulaCalculoUnidadMedida { get; set; }
    }
}
