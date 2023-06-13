-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para clonar detalle de indicador
-- ============================================
CREATE PROCEDURE [dbo].[pa_ClonarDetalleDeIndicador] 
	@pIdIndicadorAClonar INT,
	@pIdIndicadorDestino INT
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Detalles Variable dato
	INSERT INTO DetalleIndicadorVariable
	SELECT 
		IdIndicador = @pIdIndicadorDestino
		,NombreVariable
		,Descripcion
		,Estado
	FROM DetalleIndicadorVariable div
	WHERE div.IdIndicador = @pIdIndicadorAClonar AND div.Estado = 1

	-- Detalles Categoria
	INSERT INTO DetalleIndicadorCategoria
	SELECT
		IdIndicador = @pIdIndicadorDestino
		,idCategoriaDesagregacion
		,IdDetalleCategoriaTexto
		,Estado
	FROM DetalleIndicadorCategoria dic
	WHERE dic.IdIndicador = @pIdIndicadorAClonar AND dic.Estado = 1
END