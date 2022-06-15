

CREATE PROCEDURE [dbo].[pa_getValoresRegistradosPorOperador_Canton] 
	-- Add the parameters for the stored procedure here

	@IdOperador VARCHAR(20),
	@IdTipoValor INT,
	@IdDetalleAgrupacion INT

	
AS
BEGIN

--- CONSULTA DE LOS DATOS

  SELECT TOP 972 IdCanton, Valor
  
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
   
  AND IdCanton IS NOT NULL

  ORDER BY Anno DESC, NumeroDesglose DESC, IdCanton

END