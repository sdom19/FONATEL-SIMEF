
CREATE procedure [dbo].[spActualizarDetalleCategoriaTexto]
	   @idCategoriaDetalle int 
      ,@idCategoria int 
      ,@Codigo int 
      ,@Etiqueta varchar(300)
      ,@Estado bit  
as

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.DetalleCategoriaTexto AS TARGET
			USING (VALUES( @idCategoriaDetalle 
						  ,@idCategoria 
						  ,@Codigo 
						  ,@Etiqueta
						  ,@Estado))AS SOURCE (idCategoriaDetalle 
												,idCategoria 
												,Codigo 
												,Etiqueta
												,Estado )
										ON TARGET.idCategoria =SOURCE.idCategoria and
										TARGET.Codigo =SOURCE.Codigo 
										WHEN NOT MATCHED THEN
											INSERT ( 
												 
													idCategoria 
													,Codigo 
													,Etiqueta
													,Estado )
											VALUES(
												 
												 
													SOURCE.idCategoria 
													,SOURCE.Codigo 
													,upper(SOURCE.Etiqueta)
													,SOURCE.Estado  )
										WHEN MATCHED THEN
											UPDATE SET 
											idCategoria=SOURCE.idCategoria,
											Etiqueta=upper(SOURCE.Etiqueta),
											CODIGO=SOURCE.Codigo,
											estado=source.estado;
	COMMIT TRAN
	select idCategoriaDetalle 
		  ,idCategoria 
		  ,Codigo 
		  ,Etiqueta
		  ,Estado 
	from dbo.DetalleCategoriaTexto
	where Estado=1 and idCategoria=@idCategoria

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