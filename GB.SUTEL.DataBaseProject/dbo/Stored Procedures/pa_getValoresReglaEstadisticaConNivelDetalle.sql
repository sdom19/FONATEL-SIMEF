-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pa_getValoresReglaEstadisticaConNivelDetalle]
	-- Add the parameters for the stored procedure here

	 @IdConstructorCriterio UNIQUEIDENTIFIER
	,@IdNivelDetalle INT
	
AS
BEGIN
	

	SELECT ValorLimiteInferior
	     , ValorLimiteSuperior	

	
	FROM [dbo].[NivelDetalleReglaEstadistica]

    WHERE  IdConstructorCriterioDetalleAgrupacion =   @IdConstructorCriterio 

    AND IdNivelDetalle = @IdNivelDetalle

	AND Borrado = 0

	ORDER BY IdGenerico
	


END