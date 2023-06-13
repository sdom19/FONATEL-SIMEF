

CREATE PROC [dbo].[pa_ObtenerListadoTipoReglaXReglaValidacion]

@idRegla INT

AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el listado del tipo de regla por regla validación

SELECT ISNULL(STRING_AGG(c.Nombre,', '),'No Registrado')
FROM ReglaValidacion a
INNER JOIN DetalleReglaValidacion b
ON a.IdReglaValidacion=b.IdReglaValidacion
INNER JOIN TipoReglaValidacion c
ON c.IdTipoReglaValidacion=b.IdTipoReglaValidacion
WHERE a.IdReglaValidacion=@idRegla AND b.Estado = 1