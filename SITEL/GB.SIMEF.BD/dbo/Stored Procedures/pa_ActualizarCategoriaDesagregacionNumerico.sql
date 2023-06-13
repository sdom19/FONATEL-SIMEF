-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Categorias de Desagregación Numérico
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarCategoriaDesagregacionNumerico]
	   @idCategoria INT
      ,@Minimo FLOAT
      ,@Maximo FLOAT
      ,@Estado BIT
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.DetalleCategoriaNumerico AS TARGET
			USING (VALUES(  @idCategoria,@Minimo ,@Maximo ,@Estado  ))AS SOURCE 
						 (  idCategoria,Minimo ,Maximo ,Estado )
										ON TARGET.idCategoriaDesagregacion=SOURCE.idCategoria
										WHEN NOT MATCHED THEN
											INSERT ( 
													 idCategoriaDesagregacion
													,Minimo
													,Maximo
													,Estado)
											VALUES(
													SOURCE.idCategoria
													,SOURCE.Minimo
													,SOURCE.Maximo
													,SOURCE.Estado
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											Minimo=SOURCE.Minimo,
											Maximo=SOURCE.Maximo;
	COMMIT TRAN
	SELECT  idDetalleCategoriaNumerico 
		   ,idCategoriaDesagregacion
		   ,Minimo
		   ,Maximo
		   ,Estado
  FROM dbo.DetalleCategoriaNumerico
  WHERE idCategoriaDesagregacion=@idCategoria;
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