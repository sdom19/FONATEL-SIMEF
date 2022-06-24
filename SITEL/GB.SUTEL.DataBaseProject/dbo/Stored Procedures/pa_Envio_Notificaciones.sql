
-- =============================================
-- Author:		David Melendez
-- Create date: 02 de marzo de 2012
-- Description:	Procedimiento para envio de comentario vía email
-- =============================================
--155745A4-71A9-4A3B-B378-621DB06AB7F3
CREATE PROCEDURE [dbo].[pa_Envio_Notificaciones] 
	-- Add the parameters for the stored procedure here
	@IdSolicitudIndicador uniqueidentifier, 
	@Asunto varchar(65),
	@html varchar(max)
AS
BEGIN
	 --Para el envio del Correo
	Declare @lbody varchar(MAX)
	Declare @destinatarios varchar(max)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
	SELECT @destinatarios = COALESCE(@destinatarios + '; ', '') + A.CORREOUSUARIO  FROM USUARIO A
	INNER JOIN OPERADOR B ON A.IDOPERADOR = B.IDOPERADOR 
	WHERE B.IDOPERADOR IN (SELECT IDOPERADOR FROM SOLICITUDCONSTRUCTOR  WHERE IDSOLICITUDINDICADOR = @IdSolicitudIndicador)
	AND A.USUARIOINTERNO = 0 AND A.Activo = 1
	

   
		
	  set @lbody = @html;
	  		
			EXEC msdb.dbo.sp_send_dbmail
			 @profile_name='SCI_Notificaciones' 
			,@recipients = @destinatarios
			,@copy_recipients = 'correositel@sutel.go.cr'	
			,@body_format = 'HTML'
			,@body=@lbody
			,@subject=@asunto;
END