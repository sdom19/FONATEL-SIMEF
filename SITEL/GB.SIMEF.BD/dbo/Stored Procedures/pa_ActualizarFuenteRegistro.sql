-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar fuente de registro
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarFuenteRegistro]
	   @IdFuenteRegistro INT
      ,@Fuente VARCHAR(500)
	  ,@CantidadDestinatario INT
	  ,@UsuarioCreacion VARCHAR(100)
      ,@UsuarioModificacion VARCHAR(100)
      ,@Estado INT
AS

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.FuenteRegistro AS TARGET
			USING (VALUES(  @IdFuenteRegistro,@Fuente,@CantidadDestinatario ,@UsuarioCreacion ,@UsuarioModificacion ,@Estado  ))AS SOURCE 
						 (  IdFuenteRegistro,Fuente,CantidadDestinatario,UsuarioCreacion ,UsuarioModificacion ,Estado )
										ON TARGET.IdFuenteRegistro=SOURCE.IdFuenteRegistro
										AND TARGET.IdFuenteRegistro!=0
										WHEN NOT MATCHED THEN
											INSERT ( 
													 Fuente
													,CantidadDestinatario
													,FechaCreacion
													,UsuarioCreacion
													,IdEstadoRegistro
												   )
											VALUES(
													 UPPER(SOURCE.Fuente)
													,SOURCE.CantidadDestinatario
													,GETDATE()
													,SOURCE.UsuarioCreacion
													,SOURCE.estado
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											Fuente= UPPER(SOURCE.Fuente),
											CantidadDestinatario=SOURCE.CantidadDestinatario,
											FechaModificacion=GETDATE(),
											UsuarioModificacion=SOURCE.UsuarioModificacion,
											IdEstadoRegistro=SOURCE.estado;
	COMMIT TRAN





	--if @Estado=4 
	--begin
	--	update SITELP.dbo.Usuario set Borrado=1, activo=0 
	--	from DetalleFuenteRegistro b
	--	where  b.IdFuenteRegistro=@IdFuenteRegistro and b.IdUsuario=SITELP.dbo.Usuario.IdUsuario
	--end 



	IF(@IdFuenteRegistro=0)
	BEGIN
		SET @IdFuenteRegistro=SCOPE_IDENTITY();
	END


	SELECT [IdFuenteRegistro]
      ,[Fuente]
      ,[CantidadDestinatario]
      ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[IdEstadoRegistro]
  FROM [dbo].[FuenteRegistro]
  WHERE IdFuenteRegistro=@IdFuenteRegistro;
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