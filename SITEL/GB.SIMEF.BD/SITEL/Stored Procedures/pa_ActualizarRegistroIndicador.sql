CREATE PROCEDURE [SITEL].[pa_ActualizarRegistroIndicador]
AS
BEGIN
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para actualizar el registro del indicador

SELECT DISTINCT
        CONCAT(d.IdEnvioSolicitud ,b.IdFormularioWeb) idRegistroIndicador,
		d.IdEnvioSolicitud idSolicitud,
		b.IdFormularioWeb IdFormulario, 
		c.Codigo CodigoFormulario,
		a.Codigo , 
		a.Nombre, 
		a.IdMes, 
		e.Nombre Mes, 
		a.IdAnno, 
		f.Nombre Anno,  
		a.FechaInicio, 
		a.FechaFin, 
		a.IdFuenteRegistro IdFuente, 
		x.Fuente FuenteNombre,
		a.Mensaje, 
		CAST(c.Nombre AS VARCHAR(300)) Formulario, 
		5 IdEstado, 
		'Pendiente' Estado
FROM Solicitud a
INNER JOIN EstadoRegistro h
ON h.IdEstadoRegistro=a.IdEstadoRegistro
INNER JOIN Mes e 
ON e.IdMes=a.IdMes
INNER JOIN Anno f 
ON f.IdAnno=a.IdAnno
INNER JOIN DetalleSolicitudFormulario b
ON a.IdSolicitud=b.IdSolicitud
INNER JOIN FormularioWeb c
ON c.IdFormularioWeb=b.IdFormularioWeb
AND c.IdEstadoRegistro=2
INNER JOIN EnvioSolicitud d
ON d.IdSolicitud=b.IdSolicitud
--AND d.Enviado=0
INNER JOIN FuenteRegistro x
ON x.IdFuenteRegistro=a.IdFuenteRegistro
END