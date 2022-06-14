-- =============================================
-- Author:		Grupo Babel
-- Create date: 15 /05/2014
-- Description:	Actualiza la solicitud de constructor 
-- =============================================
CREATE PROCEDURE [dbo].[pa_SolicitudConstructorActualizar]
	@ID_SOLICTUDCONSTRUCTOR NVARCHAR(MAX),
	@ID_ESTADO int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

 
	update SOLICITUDCONSTRUCTOR set IDESTADO=@ID_ESTADO
	where IDSOLICITUDCONTRUCTOR=@ID_SOLICTUDCONSTRUCTOR

	

	select 1
END