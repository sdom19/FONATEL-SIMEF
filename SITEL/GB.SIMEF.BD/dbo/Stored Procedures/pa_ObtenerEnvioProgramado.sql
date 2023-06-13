
CREATE PROC [dbo].[pa_ObtenerEnvioProgramado]

@pidSolicitud INT

AS 

BEGIN 
-- =============================================
-- Author:		Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el envio programado
DECLARE @consulta VARCHAR(2000)

SET @consulta='SELECT IdSolicitudEnvioProgramado
      ,IdSolicitud
	  ,IdFrecuenciaEnvio
	  ,CantidadRepeticion
	  ,FechaCiclo
      ,Estado
   FROM SolicitudEnvioProgramado 
   WHERE Estado=1 '+
    IIF(@pidSolicitud=0,'',' AND IdSolicitud='+ CAST(@pidSolicitud AS VARCHAR(10))+' ')
EXEC(@consulta)

END