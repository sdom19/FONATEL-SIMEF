


CREATE PROC [dbo].[pa_ObtenerTipoDetalleCategoria]

@idTipoCategoria INT
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el  tipo de detalle categoria

DECLARE @consulta VARCHAR(200);
SET @consulta=
 'SELECT  idTipoDetalleCategoria 
      ,Nombre 
      ,Estado 
	  ,TipoSQL
 FROM dbo.TipoDetalleCategoria 
 WHERE estado=1 '+
 IIF(@idTipoCategoria=0,'',' AND idTipoDetalleCategoria='+ CAST(@idTipoCategoria AS VARCHAR(10))+' ') ;
 EXEC(@consulta)