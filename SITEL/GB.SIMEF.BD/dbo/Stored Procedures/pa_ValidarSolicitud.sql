
CREATE  PROC [dbo].[pa_ValidarSolicitud]

@idSolicitud INT 

AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para validar solicitud

WITH VALIDACION_CTE 
AS
(
 	SELECT DISTINCT 1 id, ( 'Fuente de Registro '+ c.Fuente) Validacion
	FROM Solicitud a
	INNER JOIN EnvioSolicitud b
	ON a.IdSolicitud=b.IdSolicitud
	INNER JOIN FuenteRegistro c
	ON a.IdFuenteRegistro=c.IdFuenteRegistro
	WHERE a.IdSolicitud= @idSolicitud
		 
	UNION

	SELECT DISTINCT 2 id, ' con los Formularios Web '+ (c.Nombre) Validacion
	FROM Solicitud a
	INNER JOIN DetalleSolicitudFormulario b
	ON b.IdSolicitud=a.IdSolicitud
	AND b.Estado=1
	INNER JOIN FormularioWeb c
	ON b.IdFormularioWeb=c.IdFormularioWeb
	AND c.IdEstadoRegistro=2
	INNER JOIN EnvioSolicitud e
	ON e.IdSolicitud = a.IdSolicitud
	AND e.Enviado = 1
	WHERE a.IdSolicitud= @idSolicitud

	UNION 

	SELECT DISTINCT 3 id, (' donde la última fecha de envío fue ' +CONVERT(VARCHAR, Fecha ,3) ) Validacion
	FROM Solicitud a
	INNER JOIN EnvioSolicitud b
	ON a.IdSolicitud=b.IdSolicitud
	INNER JOIN FuenteRegistro c
	ON a.IdFuenteRegistro=c.IdFuenteRegistro
	WHERE a.IdSolicitud= @idSolicitud AND b.Fecha = (SELECT MAX(fecha) FROM EnvioSolicitud WHERE IdSolicitud= b.IdSolicitud AND Enviado = b.Enviado) AND b.Enviado = 1
		 

)

SELECT Validacion FROM VALIDACION_CTE WHERE Validacion IS NOT NULL
ORDER BY  id