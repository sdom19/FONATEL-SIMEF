CREATE PROC [dbo].[pa_ObtenerFormularioXSolicitud]
@idSolicitud INT
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el formulario por solicitud

SELECT STRING_AGG(UPPER(C.Nombre),', ') Listado FROM Solicitud A
INNER JOIN DetalleSolicitudFormulario B
ON A.IdSolicitud=b.IdSolicitud
AND b.Estado=1
INNER JOIN FormularioWeb c
ON b.IdFormularioWeb=c.IdFormularioWEb
AND c.IdEstadoRegistro!=4
WHERE a.IdSolicitud=@idSolicitud