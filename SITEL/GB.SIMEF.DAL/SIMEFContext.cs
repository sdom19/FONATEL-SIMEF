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
        }


        public virtual DbSet<DatoHistorico> DatoHistorico { get; set; }

        public virtual DbSet<DetalleDatoHistoricoColumna> DetalleDatoHistoricoColumna { get; set; }

        public virtual DbSet<DetalleDatoHistoricoFila> DetalleDatoHistoricoFila { get; set; }

        public virtual DbSet<CategoriasDesagregacion> CategoriasDesagregacion { get; set; }
        public virtual DbSet<DetalleCategoriaTexto> DetalleCategoriaTexto { get; set; }
        public virtual DbSet<EstadoRegistro> EstadoRegistro { get; set; }
        public virtual DbSet<Bitacora> Bitacora { get; set; }
        public virtual DbSet<TipoCategoria> TipoCategoria { get; set; }

        public virtual DbSet<DetalleCategoriaFecha> DetalleCategoriaFecha { get; set; }
        public virtual DbSet<DetalleCategoriaNumerico> DetalleCategoriaNumerico { get; set; }

        public virtual DbSet<UsuarioFonatel> UsuarioFonatel { get; set; }

        public virtual DbSet<PlantillaHtml> PlantillaHtml { get; set; }

        public virtual DbSet<Anno> Anno { get; set; }

        public virtual DbSet<ClasificacionIndicadores> ClasificacionIndicadores { get; set; }


        public virtual DbSet<DetalleFormularioWeb> DetalleFormularioWeb { get; set; }
        public virtual DbSet<DetalleFuentesRegistro> DetalleFuentesRegistro { get; set; }
        public virtual DbSet<DetalleIndicadorCategoria> DetalleIndicadorCategoria { get; set; }
        public virtual DbSet<DetalleIndicadorVariables> DetalleIndicadorVariables { get; set; }
        //public virtual DbSet<DetalleRegistroIndicadorCategoria> DetalleRegistroIndicadorCategoria { get; set; }
        //public virtual DbSet<DetalleRegistroIndicadorVariable> DetalleRegistroIndicadorVariable { get; set; }
        public virtual DbSet<DetalleRelacionCategoria> DetalleRelacionCategoria { get; set; }
        
        public virtual DbSet<DetalleSolicitudFormulario> DetalleSolicitudFormulario { get; set; }
        public virtual DbSet<EnvioSolicitudes> EnvioSolicitudes { get; set; }

        //public virtual DbSet<FormulaIndicadorDSF> FormulaIndicadorDSF { get; set; }
        //public virtual DbSet<FormulaIndicadorFecha> FormulaIndicadorFecha { get; set; }
        //public virtual DbSet<FormulaIndicadorMC> FormulaIndicadorMC { get; set; }
        //public virtual DbSet<FormulaNivelCalculoCategoria> FormulaNivelCalculoCategoria { get; set; }
        public virtual DbSet<FormularioWeb> FormularioWeb { get; set; }
        public virtual DbSet<FormulasCalculo> FormulasCalculo { get; set; }
        //public virtual DbSet<FormulasCalculoDetalle> FormulasCalculoDetalle { get; set; }
        //public virtual DbSet<FormulasOperador> FormulasOperador { get; set; }
        public virtual DbSet<FrecuenciaEnvio> FrecuenciaEnvio { get; set; }
        //public virtual DbSet<FuenteIndicador> FuenteIndicador { get; set; }
        public virtual DbSet<FuentesRegistro> FuentesRegistro { get; set; }
        public virtual DbSet<GrupoIndicadores> GrupoIndicadores { get; set; }
        public virtual DbSet<Indicador> Indicador { get; set; }
        
        public virtual DbSet<Mes> Mes { get; set; }
        public virtual DbSet<OperadorArismetico> OperadorArismetico { get; set; }
        //public virtual DbSet<ProgramacionSolicitudes> ProgramacionSolicitudes { get; set; }
        //public virtual DbSet<Registro> Registro { get; set; }
        public virtual DbSet<RegistroIndicadorFonatel> RegistroIndicadorFonatel { get; set; }
        public virtual DbSet<ReglaAtributosValidos> ReglaAtributosValidos { get; set; }
        public virtual DbSet<ReglaComparacionConstante> ReglaComparacionConstante { get; set; }
        public virtual DbSet<ReglaComparacionIndicador> ReglaComparacionIndicador { get; set; }
        public virtual DbSet<ReglaSecuencial> ReglaSecuencial { get; set; }
        public virtual DbSet<ReglaValidacion> ReglaValidacion { get; set; }
        
        public virtual DbSet<DetalleReglaValidacion> DetalleReglaValidacion { get; set; }
        public virtual DbSet<RelacionCategoria> RelacionCategoria { get; set; }
        public virtual DbSet<Solicitud> Solicitud { get; set; }
        public virtual DbSet<SolicitudDetalleFuentes> SolicitudDetalleFuentes { get; set; }
        public virtual DbSet<SolicitudEnvioProgramado> SolicitudEnvioProgramado { get; set; }

        public virtual DbSet<TipoIndicadores> TipoIndicadores { get; set; }
        public virtual DbSet<TipoMedida> TipoMedida { get; set; }
        public virtual DbSet<TipoReglaValidacion> TipoReglaValidacion { get; set; }
       //ublic virtual DbSet<Tippd> TiposDetalleCategoria { get; set; }
        public virtual DbSet<UnidadEstudio> UnidadEstudio { get; set; }
        public virtual DbSet<DefinicionIndicador> DefinicionIndicadores { get; set; }
        public virtual DbSet<ReglaIndicadorSalida> ReglaIndicadorSalida { get; set; }
        public virtual DbSet<ReglaIndicadorEntrada> ReglaIndicadorEntrada { get; set; }
        public virtual DbSet<ReglaIndicadorEntradaSalida> ReglaIndicadorEntradaSalida { get; set; }
    }
}
