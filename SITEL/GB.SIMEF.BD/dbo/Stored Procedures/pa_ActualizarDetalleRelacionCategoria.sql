-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Detalle de relación de categoría
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarDetalleRelacionCategoria]
	  @idDetalleRelacionCategoria INT
      ,@IdRelacionCategoria INT 
      ,@idCategoriaAtributo INT 
      ,@Estado BIT  
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.DetalleRelacionCategoria AS TARGET
			USING (VALUES(
							@idDetalleRelacionCategoria
						   ,@IdRelacionCategoria 
						  ,@idCategoriaAtributo 
						  ,@Estado))AS SOURCE (  
												idDetalleRelacionCategoria
												,IdRelacionCategoria 
												,idCategoriaAtributo 
												,Estado )
										ON TARGET.idDetalleRelacionCategoria =SOURCE.idDetalleRelacionCategoria
										

										WHEN NOT MATCHED THEN
											INSERT (
											 		 									 
													IdRelacionCategoria 
													,idCategoriaDesagregacion
													,Estado )
											VALUES(
																						 
													SOURCE.IdRelacionCategoria 
													,SOURCE.idCategoriaAtributo 
													,SOURCE.Estado  )
										WHEN MATCHED THEN
											UPDATE SET 
											Estado=SOURCE.Estado;
	COMMIT TRAN

	IF(@Estado=1 AND @idDetalleRelacionCategoria>0)
	BEGIN
		UPDATE RelacionCategoria SET IdEstadoRegistro=1 WHERE IdRelacionCategoria=@IdRelacionCategoria
	END
	DELETE a FROM RelacionCategoriaAtributo a
	INNER JOIN DetalleRelacionCategoria b
	ON 	b.IdRelacionCategoria=a.idRelacionCategoriaId AND b.IdCategoriaDesagregacion=a.IdCategoriaDesagregacionAtributo
	AND Estado=0
	where a.idRelacionCategoriaId=@IdRelacionCategoria AND a.IdCategoriaDesagregacionAtributo=@idCategoriaAtributo

SELECT [IdRelacionCategoria]
      ,[Codigo]
      ,[Nombre]
      ,[CantidadCategoria]
      ,[IdCategoriaDesagregacion]
      ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[IdEstadoRegistro]
      ,[CantidadFila]
  FROM [dbo].[RelacionCategoria]
	WHERE IdRelacionCategoria=@IdRelacionCategoria

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