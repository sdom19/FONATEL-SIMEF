
CREATE PROC [dbo].[pa_ObtenerDetalleReglaValidacionXIndicador]
	@idIndicador INT
AS 
BEGIN
-- =============================================
-- Author:		Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el detalle regla validacion por indicador

SELECT 
	det.IdDetalleReglaValidacion,
	det.IdDetalleIndicadorVariable,
	det.IdReglaValidacion,
	det.IdTipoReglaValidacion,
	det.IdOperadorAritmetico,
	det.IdIndicador,
	det.Estado
FROM ReglaValidacion rv
	INNER JOIN DetalleReglaValidacion det ON rv.IdReglaValidacion = det.IdReglaValidacion 
		AND det.Estado = 1 
		AND det.IdIndicador = rv.IdIndicador
WHERE rv.IdIndicador = @idIndicador
AND rv.IdEstadoRegistro = 2

END