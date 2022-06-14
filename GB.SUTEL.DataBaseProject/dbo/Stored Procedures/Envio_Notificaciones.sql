
-- =============================================
-- Author:		David Melendez
-- Create date: 02 de marzo de 2012
-- Description:	Procedimiento para envio de comentario vía email
-- =============================================
--155745A4-71A9-4A3B-B378-621DB06AB7F3
CREATE PROCEDURE [dbo].[Envio_Notificaciones] 
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
    
	SELECT @destinatarios = COALESCE(@destinatarios + '; ', '') + A.CorreoUsuario  FROM Usuario A
	INNER JOIN Operador B ON A.IdOperador = B.IdOperador 
	WHERE B.IdOperador IN (SELECT IdOperador FROM SolicitudConstructor  WHERE IdSolicitudIndicador = @IdSolicitudIndicador)
	AND A.UsuarioInterno = 0
	

    --set @lbody = '<table style="width: 100%;background:#f2f3f3; font-family: Verdana, Arial, Helvetica, sans-serif; ">
				--	<tr>
				--		<th colspan=2 style="border:solid #5477b2 1px;background:#0b3d91;color:#fff;font-weight:bold;font-family: Verdana, Arial, Helvetica, sans-serif;">Comentario</th>
				--	</tr>
				--	<tr>
				--		<td style="color:#7f8485;font-weight:bold;width:10%;font-family: Verdana, Arial, Helvetica, sans-serif;">Cliente:</td>
				--		<td style="color:#7f8485;font-family: Verdana, Arial, Helvetica, sans-serif;">'+@nombre+'&nbsp;'+@apellidos+'</td>
				--	</tr>
				--	<tr>
				--		<td style="color:#7f8485;font-weight:bold;width:15%;font-family: Verdana, Arial, Helvetica, sans-serif;">E-mail:</td>
				--		<td style="color:#7f8485;text-decoration:none;font-family: Verdana, Arial, Helvetica, sans-serif;">'+@email+'</td>
				--	</tr>
				--	<tr>
				--		<td style="color:#7f8485;font-weight:bold;width:10%;font-family: Verdana, Arial, Helvetica, sans-serif;">Teléfono:</td>
				--		<td style="color:#7f8485;text-decoration:none;font-family: Verdana, Arial, Helvetica, sans-serif;">'+@telefono+'</td>
				--	</tr>
				--	<tr>
				--		<td style="color:#7f8485;font-weight:bold;width:10%;font-family: Verdana, Arial, Helvetica, sans-serif;">Ciudad:</td>
				--		<td style="color:#7f8485;font-family: Verdana, Arial, Helvetica, sans-serif;">'+@ciudad+'</td>
				--	</tr>
				--	<tr>
				--		<td style="color:#7f8485;font-weight:bold;width:10%;font-family: Verdana, Arial, Helvetica, sans-serif;">País:</td>
				--		<td style="color:#7f8485;font-family: Verdana, Arial, Helvetica, sans-serif;">'+@pais+'</td>
				--	</tr>
				--	<tr>
				--		<td style="color:#7f8485;font-weight:bold;width:10%;font-family: Verdana, Arial, Helvetica, sans-serif;">Tipo de cliente:</td>
				--		<td style="color:#7f8485;font-family: Verdana, Arial, Helvetica, sans-serif;">'+@tipoRemitente+'</td>
				--	</tr>
				--	<tr>
				--		<td style="color:#7f8485;font-weight:bold;width:10%;font-family: Verdana, Arial, Helvetica, sans-serif;">Tipo de comentario:</td>
				--		<td style="color:#7f8485;font-family: Verdana, Arial, Helvetica, sans-serif;">'+@tipoComentario+'</td>
				--	</tr>
				--	<tr>
				--		<td style="color:#7f8485;font-weight:bold;width:10%;font-family: Verdana, Arial, Helvetica, sans-serif;">Comentario:</td>
				--		<td ></td>
				--	</tr>
				--	<tr>
				--		<td colspan=2 style="width:80%;color:#7f8485;top:5pt; text-indent: 3.5em;font-family: Verdana, Arial, Helvetica, sans-serif;">
				--			<p>'+@comentario+'
				--			</p>
				--		</td>
				--	</tr>
				--	<tr>
				--		<td>
				--		</td>
				--	</tr>
				--	<tr>
				--		<td colspan=2 style="border:solid #5477b2 1px;background:#0b3d91;height:10pt;font-family: Verdana, Arial, Helvetica, sans-serif;">
				--		</td>
				--	</tr>

				--</table>'
		
	  set @lbody = @html;
	  		
			EXEC msdb.dbo.sp_send_dbmail
			 @profile_name='SUTEL' 
			,@recipients = @destinatarios
			,@copy_recipients = ''	
			,@body_format = 'HTML'
			,@body=@lbody
			,@subject=@asunto;
END

