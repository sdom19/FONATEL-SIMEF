-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Solicitud de envío programado
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarEnvioSolicitudEnvioProgramado]

AS
WITH cteEnvio
AS
(
SELECT distinct
	a.IdSolicitud idSolicitud, 
	a.FechaCiclo Fecha, 
	b.CantidadDia, 
	a.CantidadRepeticion  
FROM  SolicitudEnvioProgramado a
INNER JOIN FrecuenciaEnvio b ON 
b.IdFrecuenciaEnvio=a.IdFrecuenciaEnvio
INNER JOIN Solicitud c ON 
c.IdSolicitud=a.IdSolicitud
AND c.IdEstadoRegistro=2
LEFT JOIN EnvioSolicitud P
ON P.IdSolicitud=C.IdSolicitud
AND P.Enviado=1
WHERE FORMAT(GETDATE(),'yyyy-MM-dd')=FORMAT(a.FechaCiclo,'yyyy-MM-dd') AND a.Estado=1 AND P.IdSolicitud IS NULL

UNION

SELECT distinct
	a.IdSolicitud idSolicitud, 
	a.FechaCiclo Fecha, 
	b.CantidadDia, 
	a.CantidadRepeticion

FROM  SolicitudEnvioProgramado a
INNER JOIN FrecuenciaEnvio b ON 
b.IdFrecuenciaEnvio=a.IdFrecuenciaEnvio
INNER JOIN Solicitud c ON 
c.IdSolicitud=a.IdSolicitud
AND c.IdEstadoRegistro=2
LEFT JOIN EnvioSolicitud P
ON P.IdSolicitud=C.IdSolicitud
AND P.Enviado=1
AND   FORMAT( P.Fecha,'yyyy-MM-dd')=DATEADD(DAY,b.CantidadDia, a.FechaCiclo)
where FORMAT( GETDATE(),'yyyy-MM-dd')=DATEADD(DAY,b.CantidadDia, a.FechaCiclo) 
AND a.Estado=1 AND P.IdSolicitud IS NULL
)



--UNION

--SELECT distinct
--	a.IdSolicitud idSolicitud, 
--	a.FechaCiclo Fecha, 
--	b.CantidadDia, 
--	a.CantidadRepeticion 
--FROM  SolicitudEnvioProgramado a
--INNER JOIN FrecuenciaEnvio b ON 
--b.IdFrecuenciaEnvio=a.IdFrecuenciaEnvio
--INNER JOIN Solicitud c ON 
--c.IdSolicitud=a.IdSolicitud
--AND c.IdEstadoRegistro=2
--INNER JOIN EnvioSolicitud d
--ON d.IdSolicitud=a.IdSolicitud
--AND d.Enviado=1
--WHERE FORMAT( DATEADD( DAY,b.CantidadDia,d.Fecha),'yyyy-MM-dd')= FORMAT( GETDATE(),'yyyy-MM-dd')

--)



INSERT INTO EnvioSolicitud 
			(
           [Fecha]
           ,[IdSolicitud]
           ,[Enviado]
           ,[EnvioProgramado]
           ,[MensajeError])

SELECT 
	GETDATE(),
	idSolicitud,
	0,
	1,
	'Pendiente Envio'

FROM cteEnvio