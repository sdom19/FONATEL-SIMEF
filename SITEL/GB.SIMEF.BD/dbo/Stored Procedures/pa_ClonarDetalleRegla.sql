-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para clonar detalle de regla
-- ============================================
CREATE PROCEDURE [dbo].[pa_ClonarDetalleRegla] 
	@pIdReglaAClonar INT,
	@pIdReglaDestino INT
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Detalles Regla Validacion
	INSERT INTO DetalleReglaValidacion

	SELECT 
		IdRegla = @pIdReglaDestino
		,IdTipoReglaValidacion
		,IdOperadorAritmetico
		,IdDetalleIndicadorVariable
		,IdIndicador
		,Estado
	FROM DetalleReglaValidacion div
	WHERE div.IdReglaValidacion = @pIdReglaAClonar AND div.Estado = 1

-- REGLA CONTRA OTRO INDICADOR DE ENTRADA
INSERT INTO ReglaComparacionIndicador
           (IdDetalleReglaValidacion
           ,IdDetalleIndicadorVariable
           ,IdIndicador)
 SELECT DISTINCT
       c.IdDetalleReglaValidacion
      ,a.IdDetalleIndicadorVariable
      ,a.IdIndicador
  FROM ReglaComparacionIndicador a
  INNER JOIN DetalleReglaValidacion b
  ON a.IdDetalleReglaValidacion=b.IdDetalleReglaValidacion
  INNER JOIN DetalleReglaValidacion c
  ON c.IdReglaValidacion=@pIdReglaDestino AND
 c.estado =1 AND c.IdTipoReglaValidacion=2 AND c.IdOperadorAritmetico=b.IdOperadorAritmetico AND b.Estado =1
  WHERE b.IdReglaValidacion=@pIdReglaAClonar AND b.IdTipoReglaValidacion = 2 


  -- REGLA CONTRA CONSTANTE
INSERT INTO ReglaComparacionConstante
           (IdDetalleReglaValidacion
           ,Constante)
 SELECT DISTINCT
       c.IdDetalleReglaValidacion
      ,a.Constante

  FROM ReglaComparacionConstante a
  INNER JOIN DetalleReglaValidacion b
  ON a.IdDetalleReglaValidacion=b.IdDetalleReglaValidacion
  INNER JOIN DetalleReglaValidacion c
  ON c.IdReglaValidacion=@pIdReglaDestino AND
  c.estado =1 AND c.IdTipoReglaValidacion =3 AND c.IdOperadorAritmetico=b.IdOperadorAritmetico AND b.Estado =1
  WHERE b.IdReglaValidacion=@pIdReglaAClonar AND b.IdTipoReglaValidacion = 3 

    -- REGLA CONTRA ATRIBUTOS VALIDOS
INSERT INTO ReglaAtributoValido
           (IdDetalleReglaValidacion
           ,IdCategoriaDesagregacion
		   ,IdDetalleCategoriaTexto)
 SELECT DISTINCT
       c.IdDetalleReglaValidacion
      ,a.IdCategoriaDesagregacion
	  ,a.IdDetalleCategoriaTexto

  FROM ReglaAtributoValido a
  INNER JOIN DetalleReglaValidacion b
  ON a.IdDetalleReglaValidacion=b.IdDetalleReglaValidacion
  INNER JOIN DetalleReglaValidacion c
  ON c.IdReglaValidacion=@pIdReglaDestino AND
  c.estado =1 AND c.IdTipoReglaValidacion =4 AND c.IdOperadorAritmetico=b.IdOperadorAritmetico AND b.Estado =1
  WHERE b.IdReglaValidacion=@pIdReglaAClonar AND b.IdTipoReglaValidacion = 4 

  --  -- REGLA SECUENCIAL
INSERT INTO ReglaSecuencial
           (IdDetalleReglaValidacion
           ,IdCategoriaDesagregacion)
 SELECT DISTINCT
       c.IdDetalleReglaValidacion
      ,a.IdCategoriaDesagregacion
  FROM ReglaSecuencial a
  INNER JOIN DetalleReglaValidacion b
  ON a.IdDetalleReglaValidacion=b.IdDetalleReglaValidacion
  INNER JOIN DetalleReglaValidacion c
  ON c.IdReglaValidacion=@pIdReglaDestino AND
 c.estado =1 AND c.IdTipoReglaValidacion=5 AND c.IdOperadorAritmetico=b.IdOperadorAritmetico AND b.Estado =1
  WHERE b.IdReglaValidacion=@pIdReglaAClonar AND b.IdTipoReglaValidacion = 5 

    --  -- REGLA INDICADOR SALIDA
INSERT INTO ReglaIndicadorSalida
           (IdDetalleReglaValidacion
		   ,IdDetalleIndicadorVariable
           ,IdIndicador)
 SELECT DISTINCT
       c.IdDetalleReglaValidacion
	   ,a.IdDetalleIndicadorVariable
      ,a.IdIndicador
  FROM ReglaIndicadorSalida a
  INNER JOIN DetalleReglaValidacion b
  ON a.IdDetalleReglaValidacion=b.IdDetalleReglaValidacion
  INNER JOIN DetalleReglaValidacion c
  ON c.IdReglaValidacion=@pIdReglaDestino AND
 c.estado =1 AND c.IdTipoReglaValidacion=6 AND c.IdOperadorAritmetico=b.IdOperadorAritmetico AND b.Estado =1
  WHERE b.IdReglaValidacion=@pIdReglaAClonar AND b.IdTipoReglaValidacion = 6 
END

  -- REGLA CONTRA OTRO INDICADOR DE ENTRADA SALIDA
INSERT INTO ReglaComparacionEntradaSalida
           (IdDetalleReglaValidacion
           ,IdDetalleIndicadorVariable
           ,IdIndicador)
 SELECT DISTINCT
       c.IdDetalleReglaValidacion
      ,a.IdDetalleIndicadorVariable
      ,a.IdIndicador
  FROM ReglaComparacionEntradaSalida a
  INNER JOIN DetalleReglaValidacion b
  ON a.IdDetalleReglaValidacion=b.IdDetalleReglaValidacion
  INNER JOIN DetalleReglaValidacion c
  ON c.IdReglaValidacion=@pIdReglaDestino AND
 c.estado =1 AND c.IdTipoReglaValidacion=7 AND c.IdOperadorAritmetico=b.IdOperadorAritmetico AND b.Estado =1
  WHERE b.IdReglaValidacion=@pIdReglaAClonar AND b.IdTipoReglaValidacion = 7