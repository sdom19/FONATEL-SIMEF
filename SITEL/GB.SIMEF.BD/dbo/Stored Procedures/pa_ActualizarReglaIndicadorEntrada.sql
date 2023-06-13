-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar reglas de indicador de entrada
-- ============================================

CREATE PROCEDURE [dbo].[pa_ActualizarReglaIndicadorEntrada]
	   @IdCompara INT
      ,@IdDetalleReglaValidacion INT
	  ,@IdDetalleIndicador INT
	  ,@IdIndicador INT
AS

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.ReglaComparacionIndicador AS TARGET
			USING (VALUES( @IdCompara,@IdDetalleReglaValidacion,@IdDetalleIndicador,@IdIndicador))AS SOURCE 
						 (  IdCompara,IdDetalleReglaValidacion,IdDetalleIndicador, IdIndicador)
										ON TARGET.IdReglaComparacionIndicador=SOURCE.IdCompara
										WHEN NOT MATCHED THEN
											INSERT ( IdDetalleReglaValidacion
													,IdDetalleIndicadorvariable
													,IdIndicador)
											VALUES(
													SOURCE.IdDetalleReglaValidacion, 
													SOURCE.IdDetalleIndicador,
													SOURCE.IdIndicador
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											 IdDetalleReglaValidacion = SOURCE.IdDetalleReglaValidacion
											,IdDetalleIndicadorVariable = SOURCE.IdDetalleIndicador
											,IdIndicador = SOURCE.IdIndicador;
	COMMIT TRAN
	SELECT IdReglaComparacionIndicador
      ,[IdDetalleReglaValidacion]
	  ,[IdDetalleIndicadorVariable]
      ,[IdIndicador]
  FROM [dbo].[ReglaComparacionIndicador]
  WHERE IdReglaComparacionIndicador=@IdCompara;
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