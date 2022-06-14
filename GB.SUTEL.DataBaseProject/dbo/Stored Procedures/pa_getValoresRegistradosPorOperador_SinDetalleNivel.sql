
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pa_getValoresRegistradosPorOperador_SinDetalleNivel] 
	-- Add the parameters for the stored procedure here

	@IdOperador VARCHAR(20),
	@IdTipoValor INT,
	@IdDetalleAgrupacion INT

	
AS
BEGIN

  SELECT  TOP 12 Valor
  
  FROM [dbo].[RegistroIndicador] RI 

    INNER JOIN  

  [dbo].[DetalleRegistroIndicador] DRI

  ON RI.IdRegistroIndicador = DRI.IdRegistroIndicador

  INNER JOIN   [ConstructorCriterioDetalleAgrupacion] CCDA 
  
  ON DRI.IdConstructorCriterio = CCDA.IdConstructorCriterio

  WHERE UltimoNivel = 1 

  AND IdOperador = @IdOperador

  AND IdDetalleAgrupacion = @IdDetalleAgrupacion

  AND CCDA.IdTipoValor = @IdTipoValor

  AND IdCanton IS NULL

  AND IdGenero IS NULL

  AND IdProvincia IS NULL    

  ORDER BY Anno DESC, NumeroDesglose DESC  

END