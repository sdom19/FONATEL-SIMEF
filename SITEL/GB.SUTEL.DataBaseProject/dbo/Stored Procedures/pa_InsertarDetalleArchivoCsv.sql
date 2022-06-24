
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pa_InsertarDetalleArchivoCsv]
	-- Add the parameters for the stored procedure here
	@Tiempo DATETIME,
	@Frecuencia INT, 
	@Nivel FLOAT, 
	@IdArchivoCsv INT
AS
BEGIN

	DECLARE  @result BIT;

	BEGIN TRANSACTION
		BEGIN TRY
			 
			 INSERT INTO DetalleArchivoCsv (Tiempo,
											Frecuencia,
										    Nivel,
										    IdArchivoCsv)
									 VALUES (
									        @Tiempo,
										    @Frecuencia,
											@Nivel,
											@IdArchivoCsv )			

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