-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar relación entre categorias
-- ============================================
CREATE PROCEDURE [dbo].[pa_ActualizarRelacionCategoria]
	   @idRelacionCategoria INT 
      ,@Codigo VARCHAR(30)
      ,@Nombre VARCHAR(300)
      ,@CantidadCategoria INT
	  ,@idCategoria INT
      ,@UsuarioCreacion VARCHAR(100)
      ,@UsuarioModificacion VARCHAR(100)
      ,@IdEstadoRegistro INT 
	  ,@CantidadFila INT
AS

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.RelacionCategoria AS TARGET
			USING (VALUES( @idRelacionCategoria 
						  ,UPPER(@Codigo)
						  ,UPPER(@Nombre) 
						  ,@CantidadCategoria
						  ,@idCategoria 
						  ,UPPER(@UsuarioCreacion)
						  ,UPPER(@UsuarioModificacion)
						  ,@IdEstadoRegistro
						  ,@CantidadFila))AS SOURCE (idRelacionCategoria 
													,Codigo
													,Nombre 
													,CantidadCategoria
													,idCategoria
													,UsuarioCreacion
													,UsuarioModificacion
													,IdEstadoRegistro
													,CantidadFila) 
										ON TARGET.idRelacionCategoria=SOURCE.idRelacionCategoria
										WHEN NOT MATCHED THEN
											INSERT ( Codigo
													,Nombre 
													,CantidadCategoria
													,idCategoriadesagregacion
													,FechaCreacion 
													,UsuarioCreacion
													,UsuarioModificacion
													,IdEstadoRegistro
													,CantidadFila)
											VALUES (
													 SOURCE.Codigo
													,SOURCE.Nombre
													,SOURCE.CantidadCategoria
													,SOURCE.idCategoria
													,GETDATE()
													,SOURCE.UsuarioCreacion
													,SOURCE.UsuarioModificacion
													,SOURCE.IdEstadoRegistro
													,SOURCE.CantidadFila)
											WHEN MATCHED THEN
											UPDATE SET 
											Nombre=SOURCE.Nombre,
											CantidadCategoria=SOURCE.CantidadCategoria,
											idCategoriadesagregacion=SOURCE.idCategoria,
			
											FechaModificacion=GETDATE(),
											UsuarioModificacion=SOURCE.UsuarioModificacion,
											IdEstadoRegistro=SOURCE.IdEstadoRegistro,
											CantidadFila=SOURCE.CantidadFila;
	COMMIT TRAN

	IF(@idRelacionCategoria=0)
	BEGIN
		SET @idRelacionCategoria=SCOPE_IDENTITY();
	END


	SELECT 
	 idRelacionCategoria,
	 Codigo,
	 Nombre,
	 CantidadCategoria,
	 IdCategoriaDesagregacion,
	 IdEstadoRegistro, 
	 FechaCreacion, 
	 UsuarioCreacion, 
	 FechaModificacion, 
	 UsuarioModificacion, 
	 CantidadFila 
	 FROM RelacionCategoria 
	WHERE IdRelacionCategoria=@idRelacionCategoria



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