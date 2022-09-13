CREATE procedure [dbo].[spActualizarFuentesRegistro]
	   @idFuente int
      ,@Fuente varchar(300)
	  ,@CantidadDestinatario int
	  ,@UsuarioCreacion varchar(100)
      ,@UsuarioModificacion varchar(100)
      ,@Estado int
as

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.FuentesRegistro AS TARGET
			USING (VALUES(  @idFuente,@Fuente,@CantidadDestinatario ,@UsuarioCreacion ,@UsuarioModificacion ,@Estado  ))AS SOURCE 
						 (  idFuente,Fuente,CantidadDestinatario,UsuarioCreacion ,UsuarioModificacion ,Estado )
										ON TARGET.idFuente=SOURCE.idFuente
										WHEN NOT MATCHED THEN
											INSERT ( 
													 Fuente
													,CantidadDestinatario
													,FechaCreacion
													,UsuarioCreacion
													,IdEstado
												   )
											VALUES(
													 upper(SOURCE.Fuente)
													,SOURCE.CantidadDestinatario
													,Getdate()
													,SOURCE.UsuarioCreacion
													,source.estado
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											Fuente= upper(SOURCE.Fuente),
											CantidadDestinatario=SOURCE.CantidadDestinatario,
											FechaModificacion=getdate(),
											UsuarioModificacion=source.UsuarioModificacion,
											idEstado=source.estado;
	COMMIT TRAN
	SELECT [idFuente]
      ,[Fuente]
      ,[CantidadDestinatario]
      ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[idEstado]
  FROM [dbo].[FuentesRegistro]
  where idEstado!=4 and Fuente=UPPER(Fuente);
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