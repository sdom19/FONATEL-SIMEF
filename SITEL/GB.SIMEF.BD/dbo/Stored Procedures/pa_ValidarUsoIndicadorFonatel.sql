
CREATE PROC [dbo].[pa_ValidarUsoIndicadorFonatel]
	@pIdIndicador INT
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para validar uso del indicador 

WITH VALIDACION_CTE
AS
(
	SELECT 'Formularios' Nombre, STRING_AGG(TRIM(fw.Nombre),', ') Listado
	FROM FormularioWeb fw
	INNER JOIN DetalleFormularioWeb dfw
		ON fw.IdFormularioWeb = dfw.IdFormularioWeb
	INNER JOIN Indicador ind
		ON ind.idIndicador = dfw.idIndicador
	WHERE (dfw.Estado = 1 AND fw.IdEstadoRegistro != 4)
		AND ind.idIndicador = @pIdIndicador

	UNION

	SELECT 'Fórmulas' Nombre, STRING_AGG(TRIM(fc.Nombre),', ') Listado
	FROM FormulaCalculo fc
	INNER JOIN Indicador ind
		ON ind.idIndicador = fc.idIndicador
	WHERE fc.IdEstadoRegistro != 4 AND ind.idIndicador = @pIdIndicador
)

SELECT Nombre+': '+Listado lista FROM VALIDACION_CTE WHERE Listado IS NOT NULL