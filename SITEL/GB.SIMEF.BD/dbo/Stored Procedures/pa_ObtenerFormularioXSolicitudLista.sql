CREATE PROC [dbo].[pa_ObtenerFormularioXSolicitudLista]
@idSolicitud INT
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el formulario por solicitud

SELECT  c.IdFormularioWeb
        ,c.Codigo
        ,c.Nombre
        ,c.Descripcion
        ,c.CantidadIndicador
        ,c.IdFrecuenciaEnvio
        ,c.FechaCreacion
        ,c.UsuarioCreacion
        ,c.FechaModificacion
        ,c.UsuarioModificacion
        ,c.IdEstadoRegistro
FROM Solicitud A
INNER JOIN DetalleSolicitudFormulario B
ON A.IdSolicitud=b.IdSolicitud
AND b.Estado=1
INNER JOIN FormularioWeb c
ON b.IdFormularioWeb=c.IdFormularioWeb
AND c.IdEstadoRegistro!=4
WHERE a.IdSolicitud=@idSolicitud