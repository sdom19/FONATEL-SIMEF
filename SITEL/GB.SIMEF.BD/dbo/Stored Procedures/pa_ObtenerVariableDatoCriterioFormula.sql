
CREATE PROCEDURE [dbo].[pa_ObtenerVariableDatoCriterioFormula] 
	@pIdFormulaCalculo INT
AS
BEGIN
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la Variable Dato Criterio Formula

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

		fv.IdFormulaVariableDatoCriterio,
		fv.IdFuenteIndicador,
		fv.IdIndicador,
		fv.IdDetalleIndicadorVariable,
		fv.IdCriterio,
		fv.IdCategoriaDesagregacion,
		fv.IdDetalleCategoriaTexto,
		fv.IdAcumulacionFormula,
		fv.EsValorTotal
	FROM ArgumentoFormula af 
	INNER JOIN FormulaVariableDatoCriterio fv ON af.IdFormulaVariableDatoCriterio = fv.IdFormulaVariableDatoCriterio
	WHERE 
		af.IdFormulaCalculo = ISNULL(@pIdFormulaCalculo, af.IdFormulaCalculo)
END