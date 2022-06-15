-- =============================================
-- Author:		<Author,,Dusting>
-- Create date: <Create Date,,14/04/2015>
-- Description:	<Description,,SendEmail>
-- =============================================
CREATE PROCEDURE [dbo].[SendEmail]
	@to nvarchar(max),@asunto nvarchar(max) = '',@html nvarchar(max) = '', @profile_name nvarchar(max) = 'SUTEL'

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	  		
			EXEC msdb.dbo.sp_send_dbmail
			@profile_name='SUTEL'
			,@recipients = @to
			,@body_format = 'HTML'
			,@body=@html
			,@subject=@asunto;
END
