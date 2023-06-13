 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener categorías de fórmulas nivel de cálculo
-- ============================================
CREATE PROCEDURE [dbo].[pa_ObtenerCategoriaDeFormulaNivelCalculo]
	@pIdFormula INT,
	@pIdIndicador INT
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
	FROM dbo.CategoriaDesagregacion cd
	INNER JOIN dbo.DetalleIndicadorCategoria dic ON dic.idCategoriaDesagregacion = cd.IdCategoriaDesagregacion
	INNER JOIN dbo.Indicador i ON i.IdIndicador = dic.IdIndicador
	INNER JOIN dbo.FormulaNivelCalculoCategoria fnc ON fnc.IdCategoriaDesagregacion = cd.IdCategoriaDesagregacion

	WHERE i.IdIndicador = @pIdIndicador
		AND fnc.IdFormulaCalculo = @pIdFormula
		AND dic.Estado = 1 -- habilitado
END