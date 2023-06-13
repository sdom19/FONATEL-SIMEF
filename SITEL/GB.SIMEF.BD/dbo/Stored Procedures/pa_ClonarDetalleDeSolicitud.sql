-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para clonar detalle de solicitud
-- ============================================
CREATE PROCEDURE [dbo].[pa_ClonarDetalleDeSolicitud] 
	@pIdSolicitudAClonar INT,
	@pIdSolicitudDestino INT
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Detalles Variable dato
	INSERT INTO DetalleSolicitudFormulario
	SELECT 
		IdSolicitud = @pIdSolicitudDestino
		,IdFormularioWeb
		,Estado
	FROM DetalleSolicitudFormulario div
	WHERE div.IdSolicitud = @pIdSolicitudAClonar AND div.Estado = 1

END