
CREATE PROC [dbo].[pa_ObtenerFuenteRegistroDestinatario]
@idDetalleFuente INT,
@idFuenteRegistro INT
AS 

BEGIN 
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la fuente registro destinatario

DECLARE @consulta VARCHAR(1000)

SET @consulta='SELECT idDetalleFuenteRegistro
      ,idFuenteRegistro
      ,NombreDestinatario
      ,CorreoElectronico
      ,Estado
	  ,IdUsuario
	  ,CorreoEnviado
  FROM [dbo].[DetalleFuenteRegistro]
  WHERE Estado=1 '+
    IIF(@idFuenteRegistro=0,'',' AND idFuenteRegistro='+ CAST(@idFuenteRegistro AS VARCHAR(10))+' ')+
	IIF(@idDetalleFuente =0,'',' AND idDetalleFuenteRegistro='+ CAST(@idDetalleFuente  AS VARCHAR(10))+' ');

EXEC(@consulta)

END