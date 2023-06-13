CREATE PROCEDURE [dbo].[pa_ObtenerVariableSinUsoEnFormula] 
	@pIdIndicador INT,
	@pIdFormula INT
AS
BEGIN
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la variable sin uso en formula

	SET NOCOUNT ON;

    SELECT idDetalleIndicadorVariable,
    idIndicador,
    NombreVariable,
    Descripcion,
	Estado
    FROM DetalleIndicadorVariable
    WHERE Estado = 1 
		AND idIndicador = @pIdIndicador
		AND IdDetalleIndicadorVariable NOT IN (
			SELECT fc.IdDetalleIndicadorVariable FROM FormulaCalculo fc 
			WHERE fc.IdEstadoRegistro <> 4 AND (fc.IdDetalleIndicadorVariable IS NOT NULL) AND fc.IdFormulaCalculo <> @pIdFormula
			)
END