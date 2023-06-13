


CREATE PROC [dbo].[pa_ObtenerTipoCategoria]

@idTipoCategoria INT
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el tipo de categoria

DECLARE @consulta VARCHAR(150);
SET @consulta=
'SELECT 
	idTipoCategoria,
	Nombre,
	Estado
    FROM TipoCategoria
   WHERE estado=1 '+
 IIF(@idTipoCategoria=0,'',' AND idTipoCategoria='+ CAST(@idTipoCategoria AS VARCHAR(10))+' ') ;
 EXEC(@consulta)