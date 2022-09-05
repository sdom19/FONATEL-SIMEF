CREATE procedure [dbo].[spActualizarFuentesRegistroDetalle]
	   @idDetalleFuente int
	  ,@idFuente int
      ,@Nombre varchar(300)
	  ,@CorreoElectronico varchar(300)
	  ,@Estado bit
as

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.DetalleFuentesRegistro AS TARGET
			USING (VALUES( @idDetalleFuente, @idFuente,@Nombre,@CorreoElectronico,@Estado ))AS SOURCE 
						 (  idDetalleFuente,idFuente,Nombre,CorreoElectronico,Estado )
										ON TARGET.idDetalleFuente=SOURCE.idDetalleFuente
										WHEN NOT MATCHED THEN
											INSERT ( 
													idFuente,
													 NombreDestinatario,
													 CorreoElectronico,
													 Estado
												   )
											VALUES(
													 SOURCE.idFuente
													,upper(SOURCE.Nombre)
													,upper(SOURCE.CorreoElectronico)

													,1
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											NombreDestinatario=upper(SOURCE.Nombre)
											,CorreoElectronico=upper(SOURCE.CorreoElectronico)
											,Estado=SOURCE.Estado;

	COMMIT TRAN
	SELECT [idDetalleFuente]
      ,[idFuente]
      ,[NombreDestinatario]
      ,[CorreoElectronico]
      ,[Estado]
  FROM [dbo].[DetalleFuentesRegistro]
  where Estado=1 and idFuente=@idFuente
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