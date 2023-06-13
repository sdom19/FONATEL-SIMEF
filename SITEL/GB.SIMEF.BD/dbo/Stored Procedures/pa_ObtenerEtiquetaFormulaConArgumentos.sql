
CREATE PROCEDURE [dbo].[pa_ObtenerEtiquetaFormulaConArgumentos]
	@pIdFormulaCalculo INT
AS
BEGIN
-- =============================================
-- Author:		Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la etiqueta de formulario con argumentos
	SET NOCOUNT ON;

    SELECT 
		fc.Formula AS PredicadoSQL -- reutilizar columna
		,af.OrdenEnFormula
		,af.Etiqueta
	FROM ArgumentoFormula af
	INNER JOIN FormulaCalculo fc ON fc.IdFormulaCalculo = af.IdFormulaCalculo
	WHERE 
		fc.IdFormulaCalculo = @pIdFormulaCalculo
		 AND fc.Formula IS NOT NULL
	ORDER BY af.OrdenEnFormula ASC
END