
create procedure [dbo].[spActualizarDetalleRelacionCategoria]
	  @idDetalleRelacionCategoria int
      ,@IdRelacionCategoria int 
      ,@idCategoriaAtributo int 
      ,@CategoriaAtributoValor varchar(300)
      ,@Estado bit  
as

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.DetalleRelacionCategoria AS TARGET
			USING (VALUES(
							@idDetalleRelacionCategoria
						   ,@IdRelacionCategoria 
						  ,@idCategoriaAtributo 
						  ,@CategoriaAtributoValor
						  ,@Estado))AS SOURCE (  
												idDetalleRelacionCategoria
												,IdRelacionCategoria 
												,idCategoriaAtributo 
												,CategoriaAtributoValor
												,Estado )
										ON TARGET.idDetalleRelacionCategoria =SOURCE.idDetalleRelacionCategoria
										

										WHEN NOT MATCHED THEN
											INSERT (
											 		 									 
													IdRelacionCategoria 
													,idCategoriaAtributo
													,CategoriaAtributoValor
													,Estado )
											VALUES(
																						 
													SOURCE.IdRelacionCategoria 
													,SOURCE.idCategoriaAtributo 
													,upper(SOURCE.CategoriaAtributoValor)
													,SOURCE.Estado  )
										WHEN MATCHED THEN
											UPDATE SET 
											IdRelacionCategoria=SOURCE.IdRelacionCategoria,
											idCategoriaAtributo=SOURCE.idCategoriaAtributo,
											CategoriaAtributoValor=upper(SOURCE.CategoriaAtributoValor),
											Estado=source.Estado;
	COMMIT TRAN
	select idDetalleRelacionCategoria 
		  ,IdRelacionCategoria
		  ,idCategoriaAtributo
		  ,CategoriaAtributoValor
		  ,Estado 
	from dbo.DetalleRelacionCategoria
	where Estado=1 and IdRelacionCategoria=@IdRelacionCategoria

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

select * from DetalleRelacionCategoria