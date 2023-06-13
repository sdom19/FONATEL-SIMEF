
CREATE PROC [dbo].[pa_ValidarRegla]

@IdReglaValidacion INT

AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para validar regla

WITH VALIDACION_CTE 
AS
(
SELECT 'Indicador' Nombre, STRING_AGG(b.Nombre,', ') Listado

FROM ReglaValidacion a
INNER JOIN Indicador b 
ON a.IdIndicador=b.IdIndicador 
AND b.IdEstadoRegistro!=4
WHERE a.IdEstadoRegistro!=4 AND a.IdReglaValidacion=@IdReglaValidacion

)

SELECT Nombre+': '+Listado lista FROM VALIDACION_CTE WHERE Listado IS NOT NULL