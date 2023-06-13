-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar formulario web
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarFormularioWeb]
	   @IdFormularioWeb INT
	   ,@Codigo VARCHAR(10)
       ,@Nombre	VARCHAR(300)
       ,@Descripcion VARCHAR(300)
       ,@CantidadIndicador INT
       ,@IdFrecuenciaEnvio INT
       ,@UsuarioCreacion VARCHAR(100)
       ,@UsuarioModificacion VARCHAR(100)
       ,@IdEstadoRegistro INT
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.FormularioWeb AS TARGET
			USING (VALUES( @IdFormularioWeb
						  ,UPPER(@Codigo)
						  ,UPPER(@Nombre)
						  ,@Descripcion
						  ,@CantidadIndicador
						  ,@IdFrecuenciaEnvio
						  ,UPPER(@UsuarioCreacion)
						  ,UPPER(@UsuarioModificacion)
						  ,@IdEstadoRegistro))AS SOURCE ( IdFormularioWeb
												  ,Codigo
												  ,Nombre
												  ,Descripcion
												  ,CantidadIndicador
												  ,IdFrecuenciaEnvio
												  ,UsuarioCreacion
												  ,UsuarioModificacion
												  ,IdEstadoRegistro	)
										ON TARGET.IdFormularioWeb=SOURCE.IdFormularioWeb
										WHEN NOT MATCHED THEN
											INSERT ( 
												   Codigo
												  ,Nombre
												  ,Descripcion
												  ,CantidadIndicador
												  ,IdFrecuenciaEnvio
												  ,UsuarioCreacion
												  ,FechaCreacion
												  ,IdEstadoRegistro	)
											VALUES( 

												  SOURCE.Codigo
												  ,SOURCE.Nombre
												  ,SOURCE.Descripcion
												  ,SOURCE.CantidadIndicador
												  ,SOURCE.IdFrecuenciaEnvio
												  ,SOURCE.UsuarioCreacion
												  ,GETDATE()
												  ,SOURCE.IdEstadoRegistro )
										WHEN MATCHED THEN
											UPDATE SET 
												  Codigo=SOURCE.Codigo,
												  Nombre=SOURCE.Nombre,
												  Descripcion=SOURCE.Descripcion,
												  CantidadIndicador=SOURCE.CantidadIndicador,
												  IdFrecuenciaEnvio=SOURCE.IdFrecuenciaEnvio,
												  UsuarioModificacion=SOURCE.UsuarioModificacion,
												  FechaModificacion=GETDATE(),
												  IdEstadoRegistro=SOURCE.IdEstadoRegistro;
	COMMIT TRAN


	IF(@IdEstadoRegistro = 4)

	BEGIN
		UPDATE Solicitud SET IdEstadoRegistro = 1 FROM DetalleSolicitudFormulario
		WHERE Solicitud.IdSolicitud = DetalleSolicitudFormulario.IdSolicitud
		AND (DetalleSolicitudFormulario.IdFormularioWeb = @IdFormularioWeb)
		AND Solicitud.IdEstadoRegistro = 2

		UPDATE DetalleSolicitudFormulario SET Estado = 0 WHERE 
		DetalleSolicitudFormulario.IdFormularioWeb = @IdFormularioWeb

	END

	SELECT TOP (1000) [IdFormularioWeb]
      ,[Codigo]
      ,[Nombre]
      ,[Descripcion]
      ,[CantidadIndicador]
      ,[IdFrecuenciaEnvio]
      ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[IdEstadoRegistro]
  FROM [FormularioWeb]
  WHERE IdFormularioWeb=@IdFormularioWeb OR IdFormularioWeb= SCOPE_IDENTITY();
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