-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar envío de solicitud de tabla
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarEnvioSolicitudTabla]
			@IdEnvio INT,
            @IdSolicitud INT
           ,@Enviado INT
           ,@EnvioProgramado BIT
           ,@MensajeError  VARCHAR(2000)
		   ,@EjecutarJob BIT
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.EnvioSolicitud AS TARGET
			USING (VALUES(  
			@IdEnvio
			,@IdSolicitud
           ,@Enviado 
           ,@EnvioProgramado 
           ,@MensajeError ))AS SOURCE (
		    IdEnvio
		   ,IdSolicitud 
           ,Enviado 
           ,EnvioProgramado 
           ,MensajeError )
										ON TARGET.IdEnvioSolicitud=SOURCE.IdEnvio
										AND target.idSolicitud=SOURCE.idSolicitud
										WHEN NOT MATCHED THEN
											INSERT ( 
													  
														IdSolicitud 
														,Enviado 
														,EnvioProgramado 
														,MensajeError
														,fecha)
											VALUES(
												 
												    SOURCE.IdSolicitud 
														,SOURCE.Enviado 
														,SOURCE.EnvioProgramado 
														,SOURCE.MensajeError
														,GETDATE())
										WHEN MATCHED THEN
											UPDATE SET 
											Enviado =SOURCE.Enviado,
											MensajeError=SOURCE.MensajeError,
											fecha=GETDATE();
	COMMIT TRAN


	IF(@IdEnvio=0)
	BEGIN
		SET @IdEnvio=SCOPE_IDENTITY();
	END
	IF(@EjecutarJob=1)
	BEGIN
		 
		EXEC msdb.dbo.sp_start_job N'jb_EnvioSolicitudNoProgramado' ;  
	 
	END 



	SELECT [IdEnvioSolicitud]
      ,[Fecha]
      ,[IdSolicitud]
      ,[Enviado]
      ,[EnvioProgramado]
      ,[MensajeError]
  FROM [dbo].[EnvioSolicitud]
  WHERE IdEnvioSolicitud= @IdEnvio
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