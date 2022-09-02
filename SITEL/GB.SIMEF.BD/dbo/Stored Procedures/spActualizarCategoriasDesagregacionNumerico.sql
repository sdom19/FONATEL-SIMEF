
create procedure [dbo].[spActualizarCategoriasDesagregacionNumerico]
	   @idCategoria int
      ,@Minimo int
      ,@Maximo int
      ,@Estado bit
as

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.DetalleCategoriaNumerico AS TARGET
			USING (VALUES(  @idCategoria,@Minimo ,@Maximo ,@Estado  ))AS SOURCE 
						 (  idCategoria,Minimo ,Maximo ,Estado )
										ON TARGET.idCategoria=SOURCE.idCategoria
										WHEN NOT MATCHED THEN
											INSERT ( 
													 idCategoria
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
	SELECT  idCategoriaDetalle
		   ,idCategoria
		   ,Minimo
		   ,Maximo
		   ,Estado
  FROM dbo.DetalleCategoriaNumerico
  where idCategoria=@idCategoria;
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