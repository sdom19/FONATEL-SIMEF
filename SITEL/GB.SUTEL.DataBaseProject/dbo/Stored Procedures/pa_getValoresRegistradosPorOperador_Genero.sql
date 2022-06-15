
-- =============================================
-- Author:		<Author,Kevin Solís>
-- Create date: <05/04/2016,,>
-- Description:	<Se utiliza para obtener una lista de valores numéricos registrados por un operador. 
--               Esto con el fin de aplicar reglas automáticas(+-2 desviación estandar) a las plantillas excel.>
-- =============================================
CREATE PROCEDURE [dbo].[pa_getValoresRegistradosPorOperador_Genero] 
	-- Add the parameters for the stored procedure here

	@IdOperador VARCHAR(20),
	@IdTipoValor INT,
	@IdDetalleAgrupacion INT

	
AS
BEGIN

  SELECT TOP 24 IdGenero, Valor
  
  FROM [dbo].[RegistroIndicador] RI 

    INNER JOIN  

  [dbo].[DetalleRegistroIndicador] DRI

  ON RI.IdRegistroIndicador = DRI.IdRegistroIndicador

  INNER JOIN   [ConstructorCriterioDetalleAgrupacion] CCDA 
  
  ON DRI.IdConstructorCriterio = CCDA.IdConstructorCriterio

  WHERE UltimoNivel = 1

  AND IdOperador =  @IdOperador

  AND IdDetalleAgrupacion = @IdDetalleAgrupacion

  AND CCDA.IdTipoValor = @IdTipoValor
   
  AND IdGenero IS NOT NULL
  
  ORDER BY Anno DESC, NumeroDesglose DESC, IdGenero  
  


END