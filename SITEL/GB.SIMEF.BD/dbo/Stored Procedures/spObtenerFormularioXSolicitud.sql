CREATE procedure [dbo].[spObtenerFormularioXSolicitud]
@idSolicitud int
as
SELECT STRING_AGG(UPPER(C.Nombre),', ') Listado FROM Solicitud A
INNER JOIN DetalleSolicitudFormulario B
ON A.IdSolicitud=b.IdSolicitud
and b.Estado=1
INNER JOIN FormularioWeb c
ON b.IdFormulario=c.IdFormulario
and c.IdEstado!=4
where a.IdSolicitud=@idSolicitud