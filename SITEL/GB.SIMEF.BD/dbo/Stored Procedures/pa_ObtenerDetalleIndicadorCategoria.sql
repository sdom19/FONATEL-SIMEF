 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener detalle de indicador de categoria
-- ============================================
CREATE procedure [dbo].[pa_ObtenerDetalleIndicadorCategoria]
	@pIdIndicador INT,
	@pIdCategoria INT,
	@pDetallesAgrupados BIT
AS
BEGIN
	IF @pDetallesAgrupados = 1
	BEGIN
		SELECT
			0 AS IdDetalleIndicador,
			dic.idIndicador, 
			dic.idCategoriaDesagregacion, 
			cd.Codigo, 
			cd.NombreCategoria, 
			0 AS idCategoriaDetalle, 
			STRING_AGG(dct.Etiqueta,', ' ) AS Etiquetas,
			dic.Estado
		FROM DetalleIndicadorCategoria dic

		INNER JOIN CategoriaDesagregacion cd
		ON cd.IdCategoriaDesagregacion = dic.idCategoriaDesagregacion
		AND cd.IdEstadoRegistro = 2 -- Estado: Activo

		LEFT JOIN DetalleCategoriaTexto dct
		ON dic.idCategoriaDesagregacion = dct.IdCategoriaDesagregacion 
		AND dic.IdDetalleCategoriaTexto = dct.IdDetalleCategoriaTexto
		AND dct.Estado = 1 -- true: activo lógico

		WHERE dic.idIndicador = @pIdIndicador AND dic.Estado = 1
			AND (@pIdCategoria = 0 OR @pIdCategoria = dic.idCategoriaDesagregacion) -- opcional

		GROUP BY dic.idIndicador, dic.idCategoriaDesagregacion, cd.Codigo, cd.NombreCategoria, dic.Estado
	END
	ELSE 
	BEGIN
		SELECT 
			dic.IdDetalleIndicadorCategoria,
			dic.idIndicador,
			dic.idCategoriaDesagregacion, 
			cd.Codigo, 
			cd.NombreCategoria, 
			dic.IdDetalleCategoriaTexto, 
			dct.Etiqueta AS Etiquetas, 
			dic.Estado
		FROM DetalleIndicadorCategoria dic

		INNER JOIN CategoriaDesagregacion cd
		ON cd.IdCategoriaDesagregacion = dic.idCategoriaDesagregacion
		AND cd.IdEstadoRegistro = 2 -- Estado: Activo

		LEFT JOIN DetalleCategoriaTexto dct
		ON dic.idCategoriaDesagregacion = dct.IdCategoriaDesagregacion 
			AND dic.IdDetalleCategoriaTexto = dct.IdDetalleCategoriaTexto
		AND dct.Estado = 1 -- true: activo lógico

		WHERE dic.idIndicador = @pIdIndicador AND dic.Estado = 1
			AND (@pIdCategoria = 0 OR @pIdCategoria = dic.idCategoriaDesagregacion) -- opcional
	END
END