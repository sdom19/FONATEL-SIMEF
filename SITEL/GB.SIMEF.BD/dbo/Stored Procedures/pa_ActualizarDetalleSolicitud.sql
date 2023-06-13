-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Detalle de Solicitud
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarDetalleSolicitud]

	   @idSolicitud INT 
      ,@IdFormularioWeb INT
      ,@Estado BIT

AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.DetalleSolicitudFormulario AS TARGET
			USING (VALUES( @idSolicitud
					      ,@IdFormularioWeb
						  ,@Estado  ))AS SOURCE (idSolicitud
												 ,IdFormularioWeb
												 ,Estado )
												 
										ON TARGET.idSolicitud=SOURCE.idSolicitud
										AND target .IdFormularioWeb = SOURCE.IdFormularioWeb
										WHEN NOT MATCHED THEN

											INSERT (idSolicitud
													,IdFormularioWeb
													,Estado )
											VALUES (
													SOURCE.idSolicitud 
													,SOURCE.IdFormularioWeb 
													,SOURCE.Estado)

											WHEN MATCHED THEN

											UPDATE SET 

											Estado=SOURCE.Estado;
	COMMIT TRAN

	SELECT idSolicitud 
		  ,IdFormularioWeb
		  ,Estado 
	FROM dbo.DetalleSolicitudFormulario
	WHERE Estado=1 AND idSolicitud=@idSolicitud

		IF(@Estado=0)
		BEGIN
		UPDATE Solicitud SET IdEstadoRegistro=1 FROM DetalleSolicitudFormulario
		WHERE Solicitud.IdSolicitud =  @idSolicitud
		END 

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