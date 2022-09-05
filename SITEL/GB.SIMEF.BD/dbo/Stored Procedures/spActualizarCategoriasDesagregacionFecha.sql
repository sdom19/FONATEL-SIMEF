create procedure [dbo].[spActualizarCategoriasDesagregacionFecha]
	   @idCategoria int
      ,@FechaMinima date
      ,@FechaMaxima date
      ,@Estado bit
as

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.DetalleCategoriafecha AS TARGET
			USING (VALUES(  @idCategoria,@FechaMinima ,@FechaMaxima ,@Estado  ))AS SOURCE 
						 (  idCategoria,FechaMinima ,FechaMaxima ,Estado )
										ON TARGET.idCategoria=SOURCE.idCategoria
										WHEN NOT MATCHED THEN
											INSERT ( 
													 idCategoria
													,FechaMinima
													,FechaMaxima
													,Estado)
											VALUES(
													SOURCE.idCategoria
													,SOURCE.FechaMinima
													,SOURCE.FechaMaxima
													,SOURCE.Estado
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											FechaMinima=SOURCE.FechaMinima,
											FechaMaxima=SOURCE.FechaMaxima;
	COMMIT TRAN
	SELECT  idCategoriaDetalle
		   ,idCategoria
		   ,FechaMinima
		   ,FechaMaxima
		   ,Estado
  FROM dbo.DetalleCategoriafecha
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