CREATE PROC [dbo].[pa_ObtenerIndicadoresSalidaParaFormulasCalculo]
	@pIdFormulaCalculo INT
AS

BEGIN
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener los indicadores para formulas de calculo

	SET NOCOUNT ON

    SELECT DISTINCT
    	ind.[IdIndicador]
		,ind.[Codigo]
		,ind.[Nombre]
		,ind.[IdTipoIndicador]
		,ind.[IdClasificacionIndicador]
		,ind.[idGrupoIndicador]
		,ind.[Descripcion]
		,ind.[CantidadVariableDato]
		,ind.[CantidadCategoriaDesagregacion]
		,ind.[IdUnidadEstudio]
		,ind.[idTipoMedida]
		,ind.[IdFrecuenciaEnvio]
		,ind.[Interno]
		,ind.[Solicitud]
		,ind.[Fuente]
		,ind.[Nota]
		,ind.[FechaCreacion]
		,ind.[UsuarioCreacion]
		,ind.[FechaModificacion]
		,ind.[UsuarioModificacion]
		,ind.[VisualizaSigitel]
		,ind.[IdEstadoRegistro]
		,ind.[IdGraficoInforme]
    FROM DetalleIndicadorVariable detalleVar
	INNER JOIN Indicador ind ON ind.IdIndicador = detalleVar.IdIndicador
    WHERE 
		ind.IdEstadoRegistro = 2 -- estado activo
		AND ind.IdClasificacionIndicador IN (2, 3) -- 2: Salida, 3: Entrada/salida
		AND detalleVar.Estado = 1 -- estado true para las variables
		AND detalleVar.IdDetalleIndicadorVariable NOT IN ( -- incluir indicadores donde tengan al menos una variable disponible para utilizar
			SELECT fc.IdDetalleIndicadorVariable FROM FormulaCalculo fc 
			WHERE fc.IdEstadoRegistro <> 4 AND (fc.IdDetalleIndicadorVariable IS NOT NULL) AND fc.IdFormulaCalculo <> @pIdFormulaCalculo
			)
END