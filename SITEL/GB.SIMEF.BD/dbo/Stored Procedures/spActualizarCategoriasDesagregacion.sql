CREATE procedure [dbo].[spActualizarCategoriasDesagregacion]
	   @idCategoria int 
      ,@Codigo varchar(30)
      ,@NombreCategoria varchar(300)
      ,@CantidadDetalleDesagregacion int
      ,@idTipoDetalle int
      ,@IdTipoCategoria int  
      ,@UsuarioCreacion varchar(100)
      ,@UsuarioModificacion varchar(100)
      ,@idEstado int 
as

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.CategoriasDesagregacion AS TARGET
			USING (VALUES( @idCategoria 
						  ,upper(@Codigo)
						  ,upper(@NombreCategoria)
						  ,@CantidadDetalleDesagregacion 
						  ,@idTipoDetalle 
					      ,@IdTipoCategoria   
						  ,upper(@UsuarioCreacion)
						  ,upper(@UsuarioModificacion)
						  ,@idEstado   ))AS SOURCE (idCategoria 
													,Codigo
													,NombreCategoria 
													,CantidadDetalleDesagregacion 
													,idTipoDetalle 
													,IdTipoCategoria   
													,UsuarioCreacion
													,UsuarioModificacion
													,idEstado )
										ON TARGET.idCategoria=SOURCE.idCategoria
										WHEN NOT MATCHED THEN
											INSERT ( 
													 Codigo
													,NombreCategoria 
													,CantidadDetalleDesagregacion 
													,idTipoDetalle 
													,IdTipoCategoria
													,FechaCreacion
													,UsuarioCreacion
													,idEstado)
											VALUES(
												 
												   Source.Codigo
												  ,Source.NombreCategoria 
												  ,Source.CantidadDetalleDesagregacion 
												  ,Source.idTipoDetalle 
												  ,Source.IdTipoCategoria 
												  ,getdate()
												  ,Source.UsuarioCreacion
												  ,Source.idEstado )
										WHEN MATCHED THEN
											UPDATE SET 
											NombreCategoria=Source.NombreCategoria,
											CantidadDetalleDesagregacion=Source.CantidadDetalleDesagregacion,
											idTipoDetalle=Source.idTipoDetalle,
											IdTipoCategoria  =Source.IdTipoCategoria,
											FechaModificacion=getdate(),
											UsuarioModificacion=Source.UsuarioModificacion,
											idEstado=Source.idEstado;
	COMMIT TRAN
	SELECT TOP (1000) [idCategoria]
      ,[Codigo]
      ,[NombreCategoria]
      ,[CantidadDetalleDesagregacion]
      ,[idTipoDetalle]
      ,[IdTipoCategoria]
      ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[idEstado]
  FROM [CategoriasDesagregacion]
  where idEstado!=4 and Codigo=@Codigo;
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