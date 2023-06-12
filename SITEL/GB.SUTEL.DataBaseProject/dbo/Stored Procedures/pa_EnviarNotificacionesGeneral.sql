﻿CREATE PROCEDURE [dbo].[pa_EnviarNotificacionesGeneral]
@IdSolicitud int
AS

  DECLARE @NombreSolicitud varchar(250); --Nombre de la solicitud
  DECLARE @FechaInicio varchar(250); -- Fecha inicio de la solicitud
  DECLARE @FechaFinal varchar(250); -- Fecha final de la solicitud
  DECLARE @CorreoUsuario varchar(60); --- Correo del usuario
  DECLARE @Operadores varchar(max) = ''; -- Total de todos los operadores insertados
  DECLARE @OperadorCursor varchar(max); --Variable para cursor de operadores
  DECLARE @NombreUsuario varchar(250); -- Nombre de los usuarios de la solicitud


/*Consultas*/
	SELECT @NombreSolicitud = Descripcion from SolicitudGeneral where @IdSolicitud = Idsolicitud;
	SELECT @FechaInicio = CONVERT(varchar(10), FechaInicio, 3) from SolicitudGeneral where @IdSolicitud = Idsolicitud;
	SELECT @FechaFinal = CONVERT(varchar(10), FechaFinal, 3) from SolicitudGeneral where @IdSolicitud = Idsolicitud;
	SELECT @CorreoUsuario = CorreoUsuario from Usuario where IdUsuario = (select IdUsuario from SolicitudGeneral where @IdSolicitud = Idsolicitud);
/*Cursor*/
	 DECLARE Contador CURSOR FOR Select CorreoUsuario 
	 FROM Usuario U INNER JOIN SolicitudOperador SO on U.IdUsuario = SO.IdUsuario 
	 WHERE IdSolicitud = @IdSolicitud and U.Activo = 1;

  OPEN Contador;

		FETCH NEXT FROM Contador into @OperadorCursor
		WHILE @@FETCH_STATUS = 0
		BEGIN
			select @OperadorCursor
			 set @Operadores = @Operadores  + @OperadorCursor + ';';
		FETCH NEXT FROM Contador into @OperadorCursor
		END;

  IF((select COUNT(@OperadorCursor)) >= 1)
  BEGIN
	  DECLARE @Correo varchar(max) ='<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns:v="urn:schemas-microsoft-com:vml"><head> <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/> <meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0;"/> <meta name="viewport" content="width=600,initial-scale=2.3,user-scalable=no"> <link href="https://fonts.googleapis.com/css?family=Work+Sans:300,400,500,600,700" rel="stylesheet"> <link href="https://fonts.googleapis.com/css?family=Quicksand:300,400,700" rel="stylesheet"> <title>SUTEL</title> <style type="text/css"> footer{bottom: 0; width: 100%;}#TablaPrincipal{}body{height: auto; width: 100%; background-color: #ffffff; margin: 0; padding: 0; -webkit-font-smoothing: antialiased; mso-margin-top-alt: 0px; mso-margin-bottom-alt: 0px; mso-padding-alt: 0px 0px 0px 0px;}p, h1, h2, h3, h4{margin-top: 0; margin-bottom: 0; padding-top: 0; padding-bottom: 0;}span.preheader{display: none; font-size: 1px;}html{width: 100%; min-height: 100%; position: relative;}table{font-size: 14px; border: 0;}/* ----------- responsivity ----------- */ @media only screen and (max-width: 640px){/*------ top header ------ */ .main-header{font-size: 20px !important;}.main-section-header{font-size: 28px !important;}.show{display: block !important;}.hide{display: none !important;}.align-center{text-align: center !important;}.no-bg{background: none !important;}/*----- main image -------*/ .main-image img{width: 440px !important; height: auto !important;}/*======divider======*/ .divider img{width: 440px !important;}/*-------- container --------*/ .container590{width: 440px !important;}.container580{width: 400px !important;}.main-button{width: 220px !important;}/*-------- secions ----------*/ .section-img img{width: 320px !important; height: auto !important;}.team-img img{width: 100% !important; height: auto !important;}}@media only screen and (max-width: 479px){/*------ top header ------ */ .main-header{font-size: 18px !important;}.main-section-header{font-size: 26px !important;}/*======divider======*/ .divider img{width: 280px !important;}/*-------- container --------*/ .container590{width: 280px !important;}.container590{width: 280px !important;}.container580{width: 260px !important;}/*-------- secions ----------*/ .section-img img{width: 280px !important; height: auto !important;}}</style></head><body class="respond" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0"> <table style="display:none!important;"> <tr> <td> <div style="overflow:hidden;display:none;font-size:1px;color:#ffffff;line-height:1px;font-family:Arial;maxheight:0px;max-width:0px;opacity:0;"></div></td></tr></table> <table border="0" width="100%" cellpadding="0" cellspacing="0" bgcolor="ffffff"> <tr> <td align="center"> <table border="0" align="center" width="590" cellpadding="0" cellspacing="0" class="container590"> <tr> <td height="25" style="font-size: 25px; line-height: 25px;">&nbsp;</td></tr><tr> <td align="center"> <table border="0" align="center" width="590" cellpadding="0" cellspacing="0" class="container590"> <tr> <td align="center" height="70" style="height:70px;"> <a href="#" style="display: block; border-style: none !important; border: 0 !important;"><img width="500" border="0" style="display: block; width: 500px;" src="https://sitel.sutel.go.cr/Content/Images/logos/logo-Sutel_11_3.png" alt="LogoSutel"/></a> </td></tr></table> </td></tr><tr> <td height="25" style="font-size: 25px; line-height: 25px;">&nbsp;</td></tr></table> </td></tr></table> <table border="0" width="100%" cellpadding="0" cellspacing="0" bgcolor="ffffff" class="bg_color"> <tr> <td align="center"> <table border="0" align="center" width="590" cellpadding="0" cellspacing="0" class="container590"> <tr> <td align="center" style="color: #343434; font-size: 24px; font-family: Quicksand, Calibri, sans-serif; font-weight:700;letter-spacing: 3px; line-height: 35px;" class="main-header"> <div style="line-height: 35px"> <span style="color: #919090;">Sistema de Indicadores de Telecomunicaciones</span> </div></td></tr><tr> <td height="10" style="font-size: 10px; line-height: 10px;">&nbsp;</td></tr><tr> <td align="center"> <table border="0" width="40" align="center" cellpadding="0" cellspacing="0" bgcolor="eeeeee"> <tr> <td height="2" style="font-size: 2px; line-height: 2px;">&nbsp;</td></tr></table> </td></tr><tr> <td height="20" style="font-size: 20px; line-height: 20px;">&nbsp;</td></tr><tr> <td align="left"> <table border="0" width="590" align="center" cellpadding="0" cellspacing="0" class="container590" style=""> <tr> <td align="left" style="color: #888888; font-size: 16px; font-family: "Work Sans", Calibri, sans-serif; line-height: 24px;"> <p style="line-height: 24px; margin-bottom:15px;"> Estimado Señor, </p><p style="line-height: 24px;margin-bottom:15px;"> Se le informa que la solicitud de información de <strong>'+@NombreSolicitud+'</strong> está abierto entre las fechas <strong>'+@FechaInicio+'</strong> al <strong>'+@FechaFinal+'</strong>. Por favor diríjase al sistema de SITEL y descargue el archivo para su complementación y remisión. <br/><br/> </p><br/><br/> <table border="0" align="center" width="180" cellpadding="0" cellspacing="0" bgcolor="#017688" style="margin-bottom:20px;"> <tr> <td height="10" style="font-size: 10px; line-height: 10px;">&nbsp;</td></tr><tr> <td align="center" style="color: #ffffff; font-size: 14px; font-family: "Work Sans", Calibri, sans-serif; line-height: 22px; letter-spacing: 2px;"> <div style="line-height: 22px;"> <a href="https://sitel.sutel.go.cr/" style="color: #ffffff; text-decoration: none;">Ir al sitio</a> </div></td></tr><tr> <td height="10" style="font-size: 10px; line-height: 10px;">&nbsp;</td></tr></table> </td></tr></table> </td></tr></table> </td></tr><tr> <td height="40" style="font-size: 40px; line-height: 40px;">&nbsp;</td></tr></table> <footer id="Footer"> <table border="0" width="100%" cellpadding="0" cellspacing="0" > <tr></tr><tr> <td align="center"> <table border="0" align="center" width="590" cellpadding="0" cellspacing="0" class="container590"> <tr> <td> <table border="0" align="left" cellpadding="0" cellspacing="0" style="border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;" class="container590"> <tr> <td align="left" style="color: #aaaaaa; font-size: 14px; font-family: "Work Sans", Calibri, sans-serif; line-height: 24px;"> <div style="line-height: 24px;"> <span style="font-size:11px; color:#888888; font-family:Helvetica, Arial, sans-serif;line-height:200%;"> <strong><a href="mailto:'+@CorreoUsuario+'" style="color:#000">'+@CorreoUsuario+' </a></strong>| 4000-0000 </span> </div></td></tr></table> <table border="0" align="left" width="5" cellpadding="0" cellspacing="0" style="border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;" class="container590"> <tr> <td height="20" width="5" style="font-size: 20px; line-height: 20px;">&nbsp;</td></tr></table> <table border="0" align="right" cellpadding="0" cellspacing="0" style="border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt;" class="container590"> <tr> <td align="center"> <table align="center" border="0" cellpadding="0" cellspacing="0"> <tr> <td align="center"> <div style="width:90%"> <span style="font-size:13px;color:#181818;font-family:Helvetica, Arial, sans-serif;line-height:200%; color: #888888;">Por favor, no responder a este correo ya que no es monitoreado.<br/></span> </div></td></tr></table> </td></tr></table> </td></tr></table> <div align="center" style="width:90%; font-size:13px;color:#181818;font-family:Helvetica, Arial, sans-serif;line-height:200%; color: #888888;"> <p> <strong>Advertencia:</strong> Una vez que el periodo expire no se podrá cargar dicho documento. </p></div></td></tr><tr> <td height="25" style="font-size: 25px; line-height: 25px;">&nbsp;</td></tr></table> </footer> </body></html>'
	  exec msdb.dbo.sp_send_dbmail
								   @profile_name = 'SCI_Notificaciones',
								   @recipients = @Operadores,
								   @subject = @NombreSolicitud,
								   @body =  @Correo,
								   @body_format= 'HTML'
  END
  CLOSE Contador
  DEALLOCATE Contador

 /*Inicio del segundo correo*/	

	/*Consultas*/
		SELECT @NombreUsuario = U.NombreUsuario FROM Usuario as U INNER JOIN SolicitudGeneral as SG on SG.IdUsuario = U.IdUsuario WHERE @IdSolicitud = IdSolicitud and U.Activo = 1;
		SET @CorreoUsuario = (SELECT U.CorreoUsuario FROM Usuario as U INNER JOIN SolicitudGeneral as SG on SG.IdUsuario = U.IdUsuario WHERE @IdSolicitud = IdSolicitud and U.Activo = 1);
		SELECT @NombreSolicitud = Descripcion from SolicitudGeneral where  @Idsolicitud = IdSolicitud ;
		SELECT @FechaInicio = CONVERT(varchar(10), FechaInicio, 3) from SolicitudGeneral where @Idsolicitud = IdSolicitud;
		SELECT @FechaFinal = CONVERT(varchar(10), FechaFinal, 3) from SolicitudGeneral where @Idsolicitud = IdSolicitud;
		SET @Operadores = ''

	/*Cursor*/
		DECLARE Contador2 CURSOR FOR Select DISTINCT (O.NombreOperador) 
		from Operador O INNER JOIN Usuario U on O.IdOperador = U.IdOperador INNER JOIN SolicitudOperador SO on U.IdUsuario = SO.IdUsuario 
		where @Idsolicitud = Idsolicitud; 

	OPEN Contador2;
		FETCH NEXT FROM Contador2 into @OperadorCursor
		WHILE @@FETCH_STATUS = 0
		BEGIN
			select @OperadorCursor
			 set @Operadores = @Operadores  + @OperadorCursor + '<br /><br />';
		FETCH NEXT FROM Contador2 into @OperadorCursor
		END;

	IF((select COUNT(@CorreoUsuario)) >= 1)
    BEGIN
		set @Correo = '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns:v="urn:schemas-microsoft-com:vml"><head> <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/> <meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0;"/> <meta name="viewport" content="width=600,initial-scale=2.3,user-scalable=no"> <link href="https://fonts.googleapis.com/css?family=Work+Sans:300,400,500,600,700" rel="stylesheet"> <link href="https://fonts.googleapis.com/css?family=Quicksand:300,400,700" rel="stylesheet"> <title>SUTEL</title> <style type="text/css"> footer{bottom: 0; width: 100%;}#TablaPrincipal{}body{height: auto; width: 100%; background-color: #ffffff; margin: 0; padding: 0; -webkit-font-smoothing: antialiased; mso-margin-top-alt: 0px; mso-margin-bottom-alt: 0px; mso-padding-alt: 0px 0px 0px 0px;}p, h1, h2, h3, h4{margin-top: 0; margin-bottom: 0; padding-top: 0; padding-bottom: 0;}span.preheader{display: none; font-size: 1px;}html{width: 100%; min-height: 100%; position: relative;}table{font-size: 14px; border: 0;}/* ----------- responsivity ----------- */ @media only screen and (max-width: 640px){/*------ top header ------ */ .main-header{font-size: 20px !important;}.main-section-header{font-size: 28px !important;}.show{display: block !important;}.hide{display: none !important;}.align-center{text-align: center !important;}.no-bg{background: none !important;}/*----- main image -------*/ .main-image img{width: 440px !important; height: auto !important;}/*======divider======*/ .divider img{width: 440px !important;}/*-------- container --------*/ .container590{width: 440px !important;}.container580{width: 400px !important;}.main-button{width: 220px !important;}/*-------- secions ----------*/ .section-img img{width: 320px !important; height: auto !important;}.team-img img{width: 100% !important; height: auto !important;}}@media only screen and (max-width: 479px){/*------ top header ------ */ .main-header{font-size: 18px !important;}.main-section-header{font-size: 26px !important;}/*======divider======*/ .divider img{width: 280px !important;}/*-------- container --------*/ .container590{width: 280px !important;}.container590{width: 280px !important;}.container580{width: 260px !important;}/*-------- secions ----------*/ .section-img img{width: 280px !important; height: auto !important;}}</style></head><body class="respond" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0"> <table style="display:none!important;"> <tr> <td> <div style="overflow:hidden;display:none;font-size:1px;color:#ffffff;line-height:1px;font-family:Arial;maxheight:0px;max-width:0px;opacity:0;"></div></td></tr></table> <table border="0" width="100%" cellpadding="0" cellspacing="0" bgcolor="ffffff"> <tr> <td align="center"> <table border="0" align="center" width="590" cellpadding="0" cellspacing="0" class="container590"> <tr> <td height="25" style="font-size: 25px; line-height: 25px;">&nbsp;</td></tr><tr> <td align="center"> <table border="0" align="center" width="590" cellpadding="0" cellspacing="0" class="container590"> <tr> <td align="center" height="70" style="height:70px;"> <a href="#" style="display: block; border-style: none !important; border: 0 !important;"><img width="500" border="0" style="display: block; width: 500px;" src="https://sitel.sutel.go.cr/Content/Images/logos/logo-Sutel_11_3.png" alt="LogoSutel"/></a> </td></tr></table> </td></tr><tr> <td height="25" style="font-size: 25px; line-height: 25px;">&nbsp;</td></tr></table> </td></tr></table> <table border="0" width="100%" cellpadding="0" cellspacing="0" bgcolor="ffffff" class="bg_color"> <tr> <td align="center"> <table border="0" align="center" width="590" cellpadding="0" cellspacing="0" class="container590"> <tr> <td align="center" style="color: #343434; font-size: 24px; font-family: Quicksand, Calibri, sans-serif; font-weight:700;letter-spacing: 3px; line-height: 35px;" class="main-header"> <div style="line-height: 35px"> <span style="color: #919090;">Sistema de Indicadores de Telecomunicaciones</span> </div></td></tr><tr> <td height="10" style="font-size: 10px; line-height: 10px;">&nbsp;</td></tr><tr> <td align="center"> <table border="0" width="40" align="center" cellpadding="0" cellspacing="0" bgcolor="eeeeee"> <tr> <td height="2" style="font-size: 2px; line-height: 2px;">&nbsp;</td></tr></table> </td></tr><tr> <td height="20" style="font-size: 20px; line-height: 20px;">&nbsp;</td></tr><tr> <td align="left"> <table border="0" width="590" align="center" cellpadding="0" cellspacing="0" class="container590" style=""> <tr> <td align="left" style="color: #888888; font-size: 16px; font-family: "Work Sans", Calibri, sans-serif; line-height: 24px;"> <p style="line-height: 24px; margin-bottom:15px;"> Estimado '+@NombreUsuario+', </p><p style="line-height: 24px;margin-bottom:15px;"> Se creó correctamente la solicitud de información de <strong>'+@NombreSolicitud+'</strong>. La ventana está abierta entre las fechas <strong>'+@FechaInicio+'</strong> al <strong>'+@FechaFinal+'</strong> y fue notificada a los siguientes operadores: <br/><br/> </p><div style="text-align: center;"> <table style="text-align: center; width:100%" border="1" id="TablaPrincipal"> '+@Operadores+' </table> </div><br/><br/><!--table border="0" align="center" width="180" cellpadding="0" cellspacing="0" bgcolor="5caad2" style="margin-bottom:20px;"> <tr> <td height="10" style="font-size: 10px; line-height: 10px;">&nbsp;</td></tr><tr> <td align="center" style="color: #ffffff; font-size: 14px; font-family: "Work Sans", Calibri, sans-serif; line-height: 22px; letter-spacing: 2px;"> <div style="line-height: 22px;"> <a href="https://sitel.sutel.go.cr/" style="color: #ffffff; text-decoration: none;">Ir al sitio</a> </div></td></tr><tr> <td height="10" style="font-size: 10px; line-height: 10px;">&nbsp;</td></tr></table--> </td></tr></table> </td></tr></table> </td></tr><tr> <td height="40" style="font-size: 40px; line-height: 40px;">&nbsp;</td></tr></table></body></html>'
		exec msdb.dbo.sp_send_dbmail
										  @profile_name = 'SCI_Notificaciones',
										  @recipients = @CorreoUsuario,
										  @subject = @NombreSolicitud,
										  @body =  @Correo,
										  @body_format= 'HTML'

	END

CLOSE Contador2
DEALLOCATE Contador2

update SolicitudGeneral set NotificacionEnviada = 1 where IdSolicitud = @IdSolicitud;