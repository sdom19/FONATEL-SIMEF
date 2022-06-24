


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pa_getValoresRegistradosPorOperador_Provincia] 


    @IdOperador VARCHAR(20),
	@IdTipoValor INT,
	@IdDetalleAgrupacion INT

	
AS
BEGIN

  SELECT  TOP 84 IdProvincia, Valor
  
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
   
  AND IdProvincia IS NOT NULL
  
  ORDER BY Anno DESC, NumeroDesglose DESC, IdProvincia 
  END