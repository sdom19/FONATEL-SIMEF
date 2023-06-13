 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para envío de correo
-- ============================================
CREATE PROCEDURE [dbo].[pa_EnvioCorreo]
 @Para VARCHAR(300),	
 @CC VARCHAR(300),
 @titulo VARCHAR(100),
 @html NVARCHAR(MAX)	
AS

BEGIN TRY 
EXEC msdb.dbo.sp_send_dbmail
	@profile_name ='SCI_Notificaciones',
	@recipients=@Para,
	@copy_recipients=@CC,
	@subject=@titulo,
	@body=@html, 
	@body_format='HTML';
	SELECT 1;
END TRY 
BEGIN CATCH
	SELECT 0;
END CATCH