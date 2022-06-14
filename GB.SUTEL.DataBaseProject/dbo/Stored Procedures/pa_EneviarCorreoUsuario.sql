-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pa_EneviarCorreoUsuario]
	-- Add the parameters for the stored procedure here
	@codOper varchar(20), 
	@AcUsu varchar(60),
	@NomUsu varchar(60),
	@CorUsu	varchar(60)
AS
BEGIN

	Declare @html nvarchar(max)
	select @html = '<!DOCTYPE html><html><body>
<h3>Bienvenido a sitel.sutel.go.cr</h3>
<p>Bienvenido '+@NomUsu+', es un gusto informarle, que ya se encuentra creado su usuario para el sistema SITEL, 
por favor proceda a ingresar a la dirección sitel.sutel.go.cr y solicite un cambio de contraseña, mediate la opción "¿Olvidó su contraseña?".</p>
<p>Su usuario es '+@Acusu+'</p></body></html>'

   --Insertar en tabla usuarios

SELECT 
       [IdOperador]
      ,[AccesoUsuario]
      ,[NombreUsuario]      
      ,[CorreoUsuario]     
  FROM [dbo].[Usuario]
  where ([CorreoUsuario] = @CorUsu and [AccesoUsuario] = @AcUsu)


	--Ejecutar envio de correo
	exec [dbo].pa_SendEmail 
	@CorUsu
  ,'Creación de Usuario'
  ,@html
  ,'SUTEL';

END