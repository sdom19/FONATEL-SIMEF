 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener detalle de categoria
-- ============================================
CREATE PROCEDURE [dbo].[pa_ObtenerDetalleDeCategoria]
	@pIdIndicador INT, 
	@pIdCategoria INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
	dic.IdDetalleIndicadorCategoria,
	dic.idIndicador,
	dic.idCategoriaDesagregacion, 
	cd.Codigo, 
	cd.NombreCategoria, 
	dic.IdDetalleIndicadorCategoria, 
	dct.Etiqueta AS Etiquetas, 
	dic.Estado
	FROM DetalleIndicadorCategoria dic

	INNER JOIN CategoriaDesagregacion cd
	ON cd.IdCategoriaDesagregacion = dic.idCategoriaDesagregacion

	INNER JOIN DetalleCategoriaTexto dct
	ON dic.idCategoriaDesagregacion = dct.IdCategoriaDesagregacion
	AND dic.IdDetalleCategoriaTexto = dct.IdDetalleCategoriaTexto

	WHERE 
		dic.idIndicador = @pIdIndicador 
		AND dic.idCategoriaDesagregacion = @pIdCategoria 
		AND dic.Estado = 1
		AND cd.IdEstadoRegistro = 2 -- Estado: Activo
		AND dct.Estado = 1 -- true: activo lógico
END