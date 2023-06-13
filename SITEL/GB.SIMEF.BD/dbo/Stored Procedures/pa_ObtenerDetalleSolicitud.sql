
CREATE PROC [dbo].[pa_ObtenerDetalleSolicitud]

@pidSolicitud INT

AS

BEGIN 
-- =============================================
-- Author:		Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el detalle de solicitud
DECLARE @consulta VARCHAR(2000)

SET @consulta='SELECT a.IdSolicitud
      ,a.IdFormularioWeb
      ,a.Estado
  FROM DetalleSolicitudFormulario a
  INNER JOIN FormularioWeb b
  ON a.IdFormularioWeb=b.IdFormularioWeb
  AND b.IdEstadoRegistro!=4
  WHERE Estado=1 '+
    IIF(@pidSolicitud=0,'',' AND a.IdSolicitud='+ CAST(@pidSolicitud AS VARCHAR(10))+' ')
EXEC(@consulta)

END