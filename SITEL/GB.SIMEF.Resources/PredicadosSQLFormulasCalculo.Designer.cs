﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GB.SIMEF.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class PredicadosSQLFormulasCalculo {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PredicadosSQLFormulasCalculo() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GB.SIMEF.Resources.PredicadosSQLFormulasCalculo", typeof(PredicadosSQLFormulasCalculo).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT SUM(PorcCumpl) FROM [CalidadIndicadorCalculo].[dbo].[FactRigurosidadFac] where IdIndicador = &apos;{0}&apos;.
        /// </summary>
        public static string calidadPorcentajeCumplimiento {
            get {
                return ResourceManager.GetString("calidadPorcentajeCumplimiento", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT SUM(PorcentInd) FROM [CalidadIndicadorCalculo].[dbo].[FactRigurosidadFac] where IdIndicador = &apos;{0}&apos;.
        /// </summary>
        public static string calidadPorcentajeIndicador {
            get {
                return ResourceManager.GetString("calidadPorcentajeIndicador", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to declare @FechaUltimoRegistro date = null
        ///declare @IndicadorSalida int = {0}
        ///declare @IdAcumulacion int = {1}
        ///declare @IdVariable int = {2}
        ///declare @IndicadorReferencia int = {3}
        ///declare @IdCategoria int = {4}
        ///declare @IdCategoriaDetalle int = {5}
        ///declare @IdFormula int = {6}
        ///
        ///SELECT top 1 @FechaUltimoRegistro = FechaCreacion from IndicadorResultado
        ///WHERE IdIndicador = @IndicadorSalida
        ///ORDER BY FechaCreacion DESC
        ///
        ///IF @FechaUltimoRegistro is null and @IdAcumulacion &lt;&gt; 0
        ///BEGIN
        ///	SET @IdAcumulacio [rest of string was truncated]&quot;;.
        /// </summary>
        public static string fonatel {
            get {
                return ResourceManager.GetString("fonatel", resourceCulture);
            }
        }
    }
}
