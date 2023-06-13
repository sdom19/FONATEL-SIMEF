CREATE PROCEDURE [dbo].[pa_ObtenerFrecuenciaEnvio]
	@idFrecuencia INT
AS
BEGIN
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la frecuencia de envio

DECLARE @consulta VARCHAR(150)

SET @consulta = 'SELECT idFrecuenciaEnvio, 
	Nombre, 
	CantidadDia, 
	CantidadMes,
	Estado 
	FROM FrecuenciaEnvio 
	WHERE Estado=1'+
	IIF(@idFrecuencia=0,'', 'AND idFrecuenciaEnvio=' + CAST(@idFrecuencia AS VARCHAR(10)) + ' ')
EXEC(@consulta)
END