--EXEC spValidarFormulario 1159
CREATE PROC [dbo].[pa_ValidarFormulario]

@IdFormularioWeb INT

AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para validar formulario


WITH VALIDACION_CTE 
AS
(
		SELECT DISTINCT 1 id, 'Solicitud de Información ' + STRING_AGG((a.Nombre), ', ') Validacion

		FROM Solicitud a
		INNER JOIN DetalleSolicitudFormulario b
		ON a.IdSolicitud=b.IdSolicitud
		WHERE a.IdEstadoRegistro!=4 AND b.IdFormularioWeb=@IdFormularioWeb

		UNION

		SELECT DISTINCT 2 id, (', fecha del último envío de la solicitud ' + CONVERT (VARCHAR, MAX(Fecha) ,3) ) Validacion

		FROM Solicitud a
		INNER JOIN DetalleSolicitudFormulario b
		ON a.IdSolicitud=b.IdSolicitud
		INNER JOIN EnvioSolicitud E
		ON a.IdSolicitud = E.IdSolicitud
		WHERE a.IdEstadoRegistro!=4 AND b.IdFormularioWeb= @IdFormularioWeb --AND E.Fecha = (select MAX(fecha) FROM EnvioSolicitud WHERE IdSolicitud= b.IdSolicitud AND Enviado = E.Enviado) 
		AND E.Enviado = 1
		--AND b.Estado = 1

		UNION

		SELECT DISTINCT 3 id, ( ', para la Fuente de Registro '+ STRING_AGG(c.Fuente, ', ')) Validacion
		FROM Solicitud a
		INNER JOIN DetalleSolicitudFormulario b
		ON a.IdSolicitud=b.IdSolicitud
		INNER JOIN FuenteRegistro c
		ON a.IdFuenteRegistro=c.IdFuenteRegistro
		WHERE b.IdFormularioWeb = @IdFormularioWeb
		AND a.IdEstadoRegistro != 4
		--AND b.Estado = 1
		--AND c.IdEstadoRegistro != 4

)

SELECT STRING_AGG(Validacion, '') Validacion FROM VALIDACION_CTE HAVING STRING_AGG(Validacion, '') IS NOT NULL
--order by id