-- =============================================
-- Author:		<Author,,Dusting>
-- Create date: <Create Date,,14/04/2015>
-- Description:	<Description,,SendEmail>
-- =============================================
CREATE PROCEDURE [dbo].[pa_SendEmail]
	@to nvarchar(max),@asunto nvarchar(max) = '',@html nvarchar(max) = '', @profile_name nvarchar(max) = 'SUTEL'

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	  	
	
	DECLARE @asunto_recipient nvarchar(max) = 'Error al generar archivo Excel'
	DECLARE @asunto_usuario nvarchar(max) = 'Bienvenido: Usuario Operador'
	DECLARE @asunto_recuperacion nvarchar(max) = 'Sutel: Reinicio de contraseña'

	IF ( @asunto = @asunto_recipient) or ( @asunto = @asunto_usuario) or ( @asunto = @asunto_recuperacion)
			
		BEGIN
				EXEC msdb.dbo.sp_send_dbmail
			    @profile_name='SCI_Notificaciones'
			    ,@recipients = @to
			    ,@blind_copy_recipients = 'anayansy.noguera@sutel.go.cr'
			    ,@body_format = 'HTML'
			    ,@body=@html
			    ,@subject=@asunto;
		END
	 
	 ELSE				 
	
		BEGIN
			    EXEC msdb.dbo.sp_send_dbmail
			    @profile_name='SCI_Notificaciones'
			    ,@recipients = @to
			    ,@copy_recipients = 'correositel@sutel.go.cr'	
			    ,@body_format = 'HTML'
			    ,@body=@html
			    ,@subject=@asunto;
		END	
				
END