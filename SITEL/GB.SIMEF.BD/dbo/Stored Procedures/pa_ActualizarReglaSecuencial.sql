
-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar reglas secuencial
-- ============================================
CREATE PROCEDURE [dbo].[pa_ActualizarReglaSecuencial]
	   @IdCompara INT
	  ,@IdCategoria INT
      ,@IdDetalleReglaValidacion INT
AS

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.ReglaSecuencial AS TARGET
			USING (VALUES( @IdCompara,@IdCategoria,@IdDetalleReglaValidacion))AS SOURCE 
						 (  IdCompara,IdCategoria,IdDetalleReglaValidacion)
										ON TARGET.IdReglaSecuencial=SOURCE.IdCompara
										WHEN NOT MATCHED THEN
											INSERT ( IdCategoriaDesagregacion,
													IdDetalleReglaValidacion)
											VALUES(
													SOURCE.IdCategoria,
													SOURCE.IdDetalleReglaValidacion 
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											 IdCategoriaDesagregacion = SOURCE.IdCategoria
											,IdDetalleReglaValidacion = SOURCE.IdDetalleReglaValidacion;
	COMMIT TRAN
	SELECT [IdReglaSecuencial]
      ,[IdCategoriaDesagregacion]
      ,[IdDetalleReglaValidacion]
  FROM [dbo].[ReglaSecuencial]
  WHERE IdReglaSecuencial=@IdCompara;
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