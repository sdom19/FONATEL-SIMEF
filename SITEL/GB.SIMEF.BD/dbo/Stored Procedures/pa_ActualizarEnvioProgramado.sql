-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar envio programado
-- =============================================
CREATE procedure [dbo].[pa_ActualizarEnvioProgramado]
	   @pIdEnvioProgramado INT 
      ,@pIdSolicitud INT
      ,@pIdFrecuenciaEnvio INT
	  ,@pCantidadRepeticion INT
	  ,@pFechaCiclo DATE
      ,@pEstado BIT
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.SolicitudEnvioProgramado AS TARGET
			USING (VALUES( @pIdEnvioProgramado
						  ,@pIdSolicitud
						  ,@pIdFrecuenciaEnvio
						  ,@pCantidadRepeticion
						  ,@pFechaCiclo
						  ,@pEstado   ))AS SOURCE (IdEnvioProgramado
													,IdSolicitud
													,IdFrecuenciaEnvio
													,CantidadRepiticion
													,FechaCiclo
													,Estado ) 
										ON TARGET.IdSolicitud=SOURCE.IdSolicitud

										WHEN NOT MATCHED THEN

											INSERT ( IdSolicitud
													,IdFrecuenciaEnvio 
													,CantidadRepeticion
													,FechaCiclo
													,Estado)
											VALUES (
													 SOURCE.IdSolicitud
													,SOURCE.IdFrecuenciaEnvio
													,SOURCE.CantidadRepiticion
													,SOURCE.FechaCiclo
													,SOURCE.Estado)
											WHEN MATCHED THEN
											UPDATE SET 											
											 IdFrecuenciaEnvio=SOURCE.IdFrecuenciaEnvio
											,CantidadRepeticion=SOURCE.CantidadRepiticion
											,FechaCiclo=SOURCE.FechaCiclo
											,Estado=SOURCE.Estado;
	COMMIT TRAN
	SELECT IdSolicitudEnvioProgramado
		  ,IdSolicitud
		  ,CantidadRepeticion
		  ,IdFrecuenciaEnvio
		  ,FechaCiclo
		  ,Estado
	from dbo.SolicitudEnvioProgramado
	where IdSolicitudEnvioProgramado= @pIdEnvioProgramado OR IdSolicitudEnvioProgramado=SCOPE_IDENTITY()

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