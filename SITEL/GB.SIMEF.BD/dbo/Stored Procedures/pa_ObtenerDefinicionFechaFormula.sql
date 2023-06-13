 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener definción fecha formula
-- ============================================
CREATE PROCEDURE [dbo].[pa_ObtenerDefinicionFechaFormula]
	@pIdFormulaCalculo INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		af.IdArgumentoFormula,
		af.IdFormulaTipoArgumento,
		af.IdFormulaDefinicionFecha,
		af.IdFormulaVariableDatoCriterio,
		af.IdFormulaCalculo,
		af.PredicadoSQL,
		af.OrdenEnFormula,
		af.Etiqueta,

		ff.IdFormulaDefinicionFecha,
		ff.FechaInicio,
		ff.FechaFinal,
		ff.IdUnidadMedida,
		ff.IdTipoFechaInicio,
		ff.IdTipoFechaFinal,
		ff.IdCategoriaDesagregacionInicio,
		ff.IdCategoriaDesagregacionFinal,
		ff.IdIndicador
	FROM ArgumentoFormula af 
	INNER JOIN FormulaDefinicionFecha ff ON af.IdFormulaDefinicionFecha = ff.IdFormulaDefinicionFecha
	WHERE 
		af.IdFormulaCalculo = ISNULL(@pIdFormulaCalculo, af.IdFormulaCalculo)
END