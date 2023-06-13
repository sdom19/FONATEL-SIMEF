CREATE  PROC [dbo].[pa_ObtenerVisualizarCategoria]
	@pIdIndicador INT,
	@pidCategoriaDesagregacion INT,
	@pDetallesAgrupados BIT
AS
BEGIN
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener visualizar categoria

		SELECT 
			dic.IdDetalleIndicadorCategoria,
			dic.idIndicador,
			dic.idCategoriaDesagregacion, 
			cd.Codigo, 
			cd.NombreCategoria, 
			dic.IdDetalleCategoriaTexto, 
			dct.Etiqueta AS Etiquetas, 
			dic.Estado,
			cd.IdTipoDetalleCategoria
		FROM DetalleIndicadorCategoria dic

		INNER JOIN CategoriaDesagregacion cd
		ON cd.idCategoriaDesagregacion = dic.idCategoriaDesagregacion
		AND cd.IdEstadoRegistro = 2 -- Estado: Activo

		LEFT JOIN DetalleCategoriaTexto dct
		ON dic.idCategoriaDesagregacion = dct.idCategoriaDesagregacion AND dic.IdDetalleCategoriaTexto = dct.IdDetalleCategoriaTexto
		AND dct.Estado = 1 -- true: activo lógico

		WHERE dic.idIndicador = @pIdIndicador AND dic.Estado = 1
			AND (@pidCategoriaDesagregacion = 0 OR @pidCategoriaDesagregacion = dic.idCategoriaDesagregacion) -- opcional

END