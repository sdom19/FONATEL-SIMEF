
CREATE PROCEDURE [dbo].[pa_VerificarSiFormulaEjecuto]
	@pIdFormula INT
AS
BEGIN
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para verificar si la formula se ejecuto

	SET NOCOUNT ON;

    -- lógica para verificar si la fórmula fue ejecutada
	-- retornar las siguientes columnas (mantener los nombres)
	-- NombreIndicador, NombreVariableDato, FechaEjecucion

	SELECT
		ir.IdIndicadorResultado,
		fc.Nombre AS NombreIndicador,
		div.NombreVariable AS NombreVariableDato,
		ir.FechaCreacion AS FechaEjecucion
	FROM FormulaCalculo fc
	INNER JOIN Indicador i ON i.IdIndicador = fc.IdIndicador
	INNER JOIN DetalleIndicadorVariable div ON div.IdIndicador = fc.IdIndicador
	INNER JOIN IndicadorResultadoDetalleVariable ird ON ird.IdDetalleIndicadorVariable = div.IdDetalleIndicadorVariable
	INNER JOIN IndicadorResultado ir ON ir.IdIndicadorResultado = ird.IdIndicadorResultado
	WHERE fc.IdFormulaCalculo = @pIdFormula
END