
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	< Este procedimiento se encarga de insertar
-- IdSolicitudIndicador, IdOperador, FechaHoraSolicitud en la tabla dbo.ArchivoExcel>
-- =============================================

create PROCEDURE [dbo].[pa_InsertarSolicitudArchivoExcel]

	  @IdSolicitudIndicador UNIQUEIDENTIFIER,
	  @IdOperador varchar(20)
AS
BEGIN
	
	DECLARE  @result BIT;
	DECLARE  @count BIT;

	BEGIN TRANSACTION
		BEGIN TRY
			 
			 SET @count = (select count(IdSolicitudIndicador) from  ArchivoExcel where IdSolicitudIndicador = @IdSolicitudIndicador and IdOperador = @IdOperador);

			 if @count <= 0 begin
				INSERT INTO ArchivoExcel (IdSolicitudIndicador,IdOperador, FechaHoraSolicitud) VALUES (@IdSolicitudIndicador, @IdOperador,GETDATE())
			 end

			 SET @result = 1;
			 COMMIT TRANSACTION
		END TRY
	
	BEGIN CATCH
		SET @result = 0;
		ROLLBACK TRANSACTION;

		--select ERROR_MESSAGE() as ErrorMessage;
	END CATCH

	SELECT @result AS Resultado

END