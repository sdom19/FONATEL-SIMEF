
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================


CREATE PROCEDURE [dbo].[pa_InsertarArchivoExcel]
	-- Add the parameters for the stored procedure here
	 @ArchivoExcelBytes VARBINARY(MAX)
	,@IdArchivoExcel INT
	
AS
BEGIN

DECLARE  @result BIT;

	BEGIN TRANSACTION
		BEGIN TRY

			UPDATE ArchivoExcel
			SET 
			ArchivoExcelBytes = @ArchivoExcelBytes
			,FechaHoraGeneracionAutomatica = GETDATE()
			,ArchivoExcelGenerado = 1
			WHERE IdArchivoExcel = @IdArchivoExcel;
			
			SET @result = 1;
			COMMIT TRANSACTION
		END TRY
	
	BEGIN CATCH

		SET @result = 0;
		ROLLBACK TRANSACTION;

		--select ERROR_MESSAGE() as ErrorMessage;
	END CATCH

	SELECT @result AS Resultado


	

END