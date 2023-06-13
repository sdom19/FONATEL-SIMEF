-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar reglas de validación
-- ============================================
CREATE PROCEDURE [dbo].[pa_ActualizarReglaValidacion]
	   @IdRegla INT
      ,@Codigo VARCHAR(30)
      ,@Nombre VARCHAR(300)
      ,@Descripcion VARCHAR(3000)
      ,@IdIndicador INT
      ,@UsuarioCreacion VARCHAR(100)
      ,@UsuarioModificacion VARCHAR(100)
      ,@IdEstadoRegistro INT

AS

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.ReglaValidacion AS TARGET
			USING (VALUES(  @IdRegla,@Codigo,@Nombre,@Descripcion,@IdIndicador,@UsuarioCreacion,@UsuarioModificacion,@IdEstadoRegistro))AS SOURCE 
						 (  IdRegla,Codigo,Nombre,Descripcion,IdIndicador,UsuarioCreacion,UsuarioModificacion,IdEstadoRegistro)
										ON TARGET.IdReglaValidacion=SOURCE.IdRegla
										WHEN NOT MATCHED THEN
											INSERT ( 
													 Codigo
													,Nombre
													,Descripcion
													,IdIndicador
													,UsuarioCreacion
													,FechaCreacion
													,IdEstadoRegistro
												   )
											VALUES(
													SOURCE.Codigo,
													SOURCE.Nombre,
													SOURCE.Descripcion,
													SOURCE.IdIndicador,
													SOURCE.UsuarioCreacion,
													GETDATE(),
													SOURCE.IdEstadoRegistro 
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											Codigo = SOURCE.Codigo
											,Nombre = SOURCE.Nombre
											,Descripcion = SOURCE.Descripcion
											,IdIndicador = SOURCE.IdIndicador
											,UsuarioCreacion = SOURCE.UsuarioCreacion
											,FechaModificacion = GETDATE()
											,UsuarioModificacion = SOURCE.UsuarioModificacion									
											,IdEstadoRegistro = SOURCE.IdEstadoRegistro;
	COMMIT TRAN
	SELECT TOP 1
		[IdReglaValidacion]
      ,[Codigo]
      ,[Nombre]
      ,[Descripcion]
      ,[IdIndicador]
      ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[IdEstadoRegistro]
  FROM [dbo].[ReglaValidacion]
  WHERE [IdReglaValidacion] = @IdRegla OR [IdReglaValidacion] = SCOPE_IDENTITY()

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