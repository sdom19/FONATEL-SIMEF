
CREATE PROC [dbo].[pa_ObtenerSolicitud]
@idSolicitud INT,
@Codigo VARCHAR(30),
@IdEstadoRegistro INT
AS 

BEGIN 
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la Solicitud

DECLARE @consulta VARCHAR(1000)

SET @consulta='SELECT  idSolicitud
      ,Codigo
      ,Nombre
      ,FechaInicio
      ,FechaFin
      ,IdMes
      ,IdAnno
      ,CantidadFormulario
      ,idFuenteRegistro
	  ,IdFrecuenciaEnvio
      ,Mensaje
      ,FechaCreacion
      ,UsuarioCreacion
      ,FechaModificacion
      ,UsuarioModificacion
      ,IdEstadoRegistro
  FROM dbo.Solicitud
  WHERE 1=1 AND IdEstadoRegistro !=4' +
    IIF(@idSolicitud=0,'',' AND idSolicitud='+ CAST(@idSolicitud AS VARCHAR(10))+' ') +
	IIF(@Codigo='','',' AND Codigo='''+ CAST(@Codigo AS VARCHAR(30))+''' ') +
	IIF(@IdEstadoRegistro=0,'',' AND IdEstadoRegistro='+ CAST(@IdEstadoRegistro AS VARCHAR(10))+' ');

EXEC(@consulta)

END