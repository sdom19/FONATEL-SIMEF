 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para eliminar detalle de registro de inidcador categoria valor
-- ============================================
CREATE PROCEDURE [dbo].[pa_EliminarDetalleRegistroIndicadorCategoriaValor]
(
	  @idSolicitud INT=0,
	  @idFormulario INT=0,
	  @idIndicador INT=0,
	  @idCategoria INT=0
)
AS

BEGIN TRY

DECLARE @Consulta VARCHAR(1000)

SET @Consulta ='
DELETE
  FROM DetalleRegistroIndicadorCategoriaValor
  where 1=1'+
  IIF(@idSolicitud=0,'', 'and IdSolicitud=' + CAST(@idSolicitud AS VARCHAR(10)) + ' ')+
  IIF(@idFormulario=0,'', 'and IdFormulario=' + CAST(@idFormulario AS VARCHAR(10)) + ' ')+
  IIF(@idIndicador=0,'', 'and IdIndicador=' + CAST(@idIndicador AS VARCHAR(10)) + ' ')+
  IIF(@idCategoria=0,'', 'and idCategoria=' + CAST(@idCategoria AS VARCHAR(10)) + ' ');

  EXEC(@consulta);

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		BEGIN
			SELECT   
				ERROR_NUMBER() AS ErrorNumber  
			   ,ERROR_MESSAGE() AS ErrorMessage; 
			ROLLBACK TRANSACTION;
		END
END CATCH