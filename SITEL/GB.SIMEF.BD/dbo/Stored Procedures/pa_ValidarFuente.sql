



CREATE PROC [dbo].[pa_ValidarFuente]

@IdFuenteRegistro INT

AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para validar fuente

WITH VALIDACION_CTE 
AS
(
SELECT 'solicitud' Nombre, STRING_AGG(Nombre,', ') Listado

FROM Solicitud
WHERE IdEstadoRegistro!=4 AND IdFuenteRegistro=@IdFuenteRegistro

)

SELECT Nombre+' '+Listado lista FROM VALIDACION_CTE WHERE Listado IS NOT NULL