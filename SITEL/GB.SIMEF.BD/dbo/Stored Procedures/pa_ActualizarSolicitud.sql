CREATE PROCEDURE [dbo].[pa_ActualizarSolicitud]
	   @idSolicitud INT 
      ,@Codigo VARCHAR(30)
      ,@Nombre VARCHAR(300)
	  ,@FechaInicio DATE
	  ,@FechaFin DATE
      ,@idMes INT
	  ,@idAnno INT
	  ,@CantidadFormularios INT
	  ,@idFuente INT
	  ,@IdFrecuenciaEnvio INT
	  ,@Mensaje varchar (3000)
      ,@UsuarioCreacion VARCHAR(100)
      ,@UsuarioModificacion VARCHAR(100)
      ,@IdEstadoRegistro INT 
AS
-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar solicitud
-- ============================================
BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.Solicitud AS TARGET
			USING (VALUES( @idSolicitud
						  ,UPPER(@Codigo)
						  ,UPPER(@Nombre)
						  ,@FechaInicio
						  ,@FechaFin
						  ,@idMes
						  ,@idAnno
						  ,@CantidadFormularios
						  ,@idFuente
						  ,@IdfrecuenciaEnvio
					      ,@Mensaje
						  ,UPPER(@UsuarioCreacion)
						  ,UPPER(@UsuarioModificacion)
						  ,@IdEstadoRegistro   ))AS SOURCE (idSolicitud 
													,Codigo
													,Nombre 
													,FechaInicio
													,FechaFin
													,idMes
													,idAnno
													,CantidadFormulario
													,idFuente
													,IdFrecuenciaEnvio
													,Mensaje												
													,UsuarioCreacion
													,UsuarioModificacion
													,IdEstadoRegistro ) 
										ON TARGET.idSolicitud=SOURCE.idSolicitud
										WHEN NOT MATCHED THEN
											INSERT ( Codigo
													,Nombre 
													,FechaInicio
													,FechaFin
													,idMes
													,idAnno
													,CantidadFormulario
													,idFuenteRegistro
													,IdFrecuenciaEnvio
													,Mensaje
													,FechaCreacion 
													,UsuarioCreacion
													,IdEstadoRegistro )
											VALUES (
													 SOURCE.Codigo
													,SOURCE.Nombre
													,SOURCE.FechaInicio
													,SOURCE.FechaFin
													,SOURCE.idMes
													,SOURCE.idAnno
													,SOURCE.CantidadFormulario
													,SOURCE.idFuente
													,SOURCE.IdFrecuenciaEnvio
													,SOURCE.Mensaje
													,GETDATE()
													,SOURCE.UsuarioCreacion
													,SOURCE.IdEstadoRegistro)
											WHEN MATCHED THEN
											UPDATE SET 
											Nombre=SOURCE.Nombre
											,FechaInicio=SOURCE.FechaInicio
											,FechaFin=SOURCE.FechaFin
											,idMes=SOURCE.idMes
											,idAnno=SOURCE.idAnno
											,CantidadFormulario=SOURCE.CantidadFormulario
											,idFuenteRegistro=SOURCE.idFuente
											,IdFrecuenciaEnvio=SOURCE.IdFrecuenciaEnvio
											,Mensaje=SOURCE.Mensaje
											,FechaModificacion=GETDATE(),
											UsuarioModificacion=SOURCE.UsuarioModificacion,
											IdEstadoRegistro=SOURCE.IdEstadoRegistro;
	COMMIT TRAN
	SELECT TOP 1
		[idSolicitud]
      ,[Codigo]
      ,[Nombre]
      ,[FechaInicio]
      ,[FechaFin]
      ,[idMes]
      ,[idAnno]
	  ,[CantidadFormulario]
	  ,[idFuenteRegistro]
	  ,[IdFrecuenciaEnvio]
	  ,[Mensaje]
	  ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[IdEstadoRegistro]
  FROM [dbo].[Solicitud]
WHERE [idSolicitud] = @idSolicitud OR [idSolicitud] = SCOPE_IDENTITY() 

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