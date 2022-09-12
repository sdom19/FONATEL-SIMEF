CREATE procedure [dbo].[spActualizarRelacionCategoria]
	   @idRelacionCategoria int 
      ,@Codigo varchar(30)
      ,@Nombre varchar(300)
      ,@CantidadCategoria int
	  ,@idCategoria int
	  ,@idCategoriaValor varchar(300)
      ,@UsuarioCreacion varchar(100)
      ,@UsuarioModificacion varchar(100)
      ,@idEstado int 
as

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.RelacionCategoria AS TARGET
			USING (VALUES( @idRelacionCategoria 
						  ,upper(@Codigo)
						  ,upper(@Nombre) 
						  ,@CantidadCategoria
						  ,@idCategoria 
					      ,@idCategoriaValor   
						  ,upper(@UsuarioCreacion)
						  ,upper(@UsuarioModificacion)
						  ,@idEstado   ))AS SOURCE (idRelacionCategoria 
													,Codigo
													,Nombre 
													,CantidadCategoria
													,idCategoria
													,idCategoriaValor  
													,UsuarioCreacion
													,UsuarioModificacion
													,idEstado ) 
										ON TARGET.idRelacionCategoria=SOURCE.idRelacionCategoria
										WHEN NOT MATCHED THEN
											INSERT ( Codigo
													,Nombre 
													,CantidadCategoria
													,idCategoria
													,IdCategoriaValor
													,FechaCreacion 
													,UsuarioCreacion
													,UsuarioModificacion
													,idEstado )
											VALUES (
													 Source.Codigo
													,Source.Nombre
													,Source.CantidadCategoria
													,Source.idCategoria
													,Source.idCategoriaValor
													,getdate()
													,Source.UsuarioCreacion
													,Source.UsuarioModificacion
													,Source.idEstado)
											WHEN MATCHED THEN
											UPDATE SET 
											Nombre=Source.Nombre,
											CantidadCategoria=Source.CantidadCategoria,
											idCategoria=Source.idCategoria,
											idCategoriaValor  =Source.idCategoriaValor,
											FechaModificacion=getdate(),
											UsuarioModificacion=Source.UsuarioModificacion,
											idEstado=Source.idEstado;
	COMMIT TRAN
	SELECT [idRelacionCategoria]
      ,[Codigo]
      ,[Nombre]
      ,[CantidadCategoria]
      ,[idCategoria]
      ,[idCategoriaValor]
      ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[idEstado]
  FROM [dbo].[RelacionCategoria]
where idEstado!=4



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