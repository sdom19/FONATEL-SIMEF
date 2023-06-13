-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar reglas de indicador de salida
-- ============================================
CREATE PROCEDURE [dbo].[pa_ActualizarReglaIndicadorSalida]
	   @IdReglaIndicadorSalida INT
      ,@IdDetalleReglaValidacion INT
	  ,@IdDetalleIndicador INT
	  ,@IdIndicador INT

AS

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.ReglaIndicadorSalida AS TARGET
			USING (VALUES( @IdReglaIndicadorSalida,@IdDetalleReglaValidacion,@IdDetalleIndicador,@IdIndicador))AS SOURCE 
						 (  IdReglaIndicadorSalida,IdDetalleReglaValidacion,IdDetalleIndicador,IdIndicador)
										ON TARGET.IdReglaIndicadorSalida=SOURCE.IdReglaIndicadorSalida
										WHEN NOT MATCHED THEN
											INSERT ( IdDetalleReglaValidacion
													,IdDetalleIndicadorVariable
													,IdIndicador)
											VALUES(
													SOURCE.IdDetalleReglaValidacion 
													,SOURCE.IdDetalleIndicador
													,SOURCE.IdIndicador
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											 IdDetalleReglaValidacion = SOURCE.IdDetalleReglaValidacion
											 ,IdDetalleIndicadorVariable = SOURCE.IdDetalleIndicador
											 ,IdIndicador = SOURCE.IdIndicador;
	COMMIT TRAN
	SELECT [IdReglaIndicadorSalida]
      ,[IdDetalleReglaValidacion]
	  ,[IdDetalleIndicadorVariable]
      ,[IdIndicador]
  FROM [dbo].[ReglaIndicadorSalida]
  WHERE IdReglaIndicadorSalida=@IdReglaIndicadorSalida;
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