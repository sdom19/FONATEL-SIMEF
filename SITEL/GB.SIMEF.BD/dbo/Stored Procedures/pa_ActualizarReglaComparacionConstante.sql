
-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar reglas de comparación de constantes
-- ============================================
CREATE PROCEDURE [dbo].[pa_ActualizarReglaComparacionConstante]
	   @idReglaComparacionConstante INT
      ,@IdDetalleReglaValidacion INT
	  ,@Constante VARCHAR(100)

AS

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.ReglaComparacionConstante AS TARGET
			USING (VALUES( @idReglaComparacionConstante,@IdDetalleReglaValidacion,@Constante))AS SOURCE 
						 (  idReglaComparacionConstante,IdDetalleReglaValidacion,Constante)
										ON TARGET.idReglaComparacionConstante=SOURCE.idReglaComparacionConstante
										WHEN NOT MATCHED THEN
											INSERT (
													IdDetalleReglaValidacion,
													Constante)
											VALUES(
													SOURCE.IdDetalleReglaValidacion, 
													SOURCE.Constante
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											 IdDetalleReglaValidacion = SOURCE.IdDetalleReglaValidacion,
											 Constante = SOURCE.Constante;
	COMMIT TRAN
	SELECT [idReglaComparacionConstante]
      ,[idDetalleReglaValidacion]
      ,[Constante]
  FROM [dbo].[ReglaComparacionConstante]
  WHERE idReglaComparacionConstante=@idReglaComparacionConstante;
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