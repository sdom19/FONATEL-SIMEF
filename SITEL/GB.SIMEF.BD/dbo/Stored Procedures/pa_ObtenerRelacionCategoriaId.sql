CREATE PROCEDURE [dbo].[pa_ObtenerRelacionCategoriaId]

@idRelacion INT,
@idCategoriaId INT

AS
BEGIN
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la relación de categoria Id

	DECLARE @consulta VARCHAR(1000);

	SET @consulta='SELECT 
	idRelacionCategoriaId,
	idCategoriaDesagregacion
	FROM RelacionCategoriaId 
	WHERE 1=1  ' +
	 IIF(@idRelacion=0,'',' AND idRelacionCategoriaId,='+ CAST(@idRelacion AS VARCHAR(10))+' ') +
	 IIF(@idCategoriaId=0,'',' AND idCategoriaDesagregacion='+ CAST(@idCategoriaId AS VARCHAR(10))+' ')

	EXEC(@consulta)

END