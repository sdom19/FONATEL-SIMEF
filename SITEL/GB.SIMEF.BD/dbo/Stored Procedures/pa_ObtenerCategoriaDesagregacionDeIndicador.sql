 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener categorías de desagregación de indicador
-- ============================================
CREATE PROCEDURE [dbo].[pa_ObtenerCategoriaDesagregacionDeIndicador]
    @pIdIndicador INT, @pIdTipoDetalle INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;     

	SELECT DISTINCT
        cd.IdCategoriaDesagregacion, 
        cd.Codigo, 
        cd.NombreCategoria, 
        cd.CantidadDetalleDesagregacion,
        cd.FechaCreacion,
        cd.UsuarioCreacion,
        cd.FechaModificacion,
        cd.UsuarioModificacion,
        cd.IdTipoDetalleCategoria,
        cd.IdTipoCategoria,
        cd.IdEstadoRegistro
    FROM CategoriaDesagregacion cd
    INNER JOIN DetalleIndicadorCategoria dic
    ON dic.idCategoriaDesagregacion = cd.IdCategoriaDesagregacion
    INNER JOIN Indicador i
    ON i.IdIndicador = dic.IdIndicador
    WHERE i.IdIndicador = @pIdIndicador
        AND cd.IdTipoDetalleCategoria = ISNULL(@pIdTipoDetalle, cd.IdTipoDetalleCategoria)
        AND dic.Estado = 1 -- habilitado
END