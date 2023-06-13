
CREATE PROC [dbo].[pa_ObtenerFuenteRegistro]
@idFuenteRegistro INT,
@IdEstadoRegistro INT
AS 

BEGIN 
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la fuente registro

DECLARE @consulta VARCHAR(1000)

SET @consulta='SELECT idFuenteRegistro
      ,Fuente
      ,CantidadDestinatario
      ,FechaCreacion
      ,UsuarioCreacion
      ,FechaModificacion
      ,UsuarioModificacion
      ,IdEstadoRegistro
  FROM dbo.FuenteRegistro
   WHERE IdEstadoRegistro!=4 AND idFuenteRegistro>0 '+
    IIF(@idFuenteRegistro=0,'',' AND idFuenteRegistro='+ CAST(@idFuenteRegistro AS VARCHAR(10))+' ') +
	IIF(@IdEstadoRegistro=0,'',' AND IdEstadoRegistro='+ CAST(@IdEstadoRegistro AS VARCHAR(10))+' ');

EXEC(@consulta)

END