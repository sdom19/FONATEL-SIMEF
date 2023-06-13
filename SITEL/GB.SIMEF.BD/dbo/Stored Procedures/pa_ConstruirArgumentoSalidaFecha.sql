 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para construir argumento de salida fecha
-- ============================================
CREATE PROCEDURE [dbo].[pa_ConstruirArgumentoSalidaFecha]

 @UnidadMedida INT,

 @IdTipoFechaInicio INT,
 @FechaInicio DATETIME,
 @IdCategoriaInicio INT,

 @IdTipoFechaFinal INT,
 @FechaFinal DATETIME,
 @IdCategoriaFinal INT,

 @IndicadorReferencia INT,
 @indicadorSalida INT

 AS

DECLARE @numeroFila INT,
@IdResultadoIndicador UNIQUEIDENTIFIER;

SELECT TOP 1 @IdResultadoIndicador= a.IdIndicadorResultado 
FROM IndicadorResultado a
INNER JOIN IndicardorResultadoDetalleCategoria b
ON a.IdIndicadorResultado=b.IdIndicadorResultado
WHERE a.IdIndicador=@IndicadorReferencia ORDER BY a.FechaCreacion DESC;


WITH FechaInicio AS
(
	SELECT @FechaInicio AS FechaInicio, @numeroFila numeroFila
	WHERE @IdTipoFechaInicio=1
	
	UNION

	SELECT TOP 1 CAST(b.Valor AS DATETIME) FechaInicio, @numeroFila numeroFila
	FROM IndicadorResultado a
    INNER JOIN IndicardorResultadoDetalleCategoria b
    ON a.IdIndicadorResultado =b.IdIndicadorResultado 
	WHERE b.IdCategoriaDesagregacion=CASE @IdTipoFechaInicio WHEN 2 THEN @IdCategoriaInicio ELSE 0 END
	AND  a.IdIndicador=@IndicadorReferencia
	AND NumeroFila=@numeroFila AND a.IdIndicadorResultado =@IdResultadoIndicador
	
	UNION
	
	SELECT GETDATE() FechaInicio, @numeroFila numeroFila
	WHERE @IdTipoFechaInicio=3
),
FechaFin AS
(
	SELECT @FechaFinal AS FechaFin, @numeroFila numeroFila
	WHERE @IdTipoFechaFinal=1

	UNION

	SELECT TOP 1 CAST(b.Valor AS DATETIME) FechaFin, @numeroFila numeroFila
    FROM IndicadorResultado a
    INNER JOIN IndicardorResultadoDetalleCategoria b
    ON a.IdIndicadorResultado =b.IdIndicadorResultado 
	WHERE b.IdCategoriaDesagregacion=CASE @IdTipoFechaFinal WHEN 2 THEN @IdCategoriaFinal ELSE 0 END
	AND  @IdTipoFechaFinal=2 AND a.IdIndicador=@IndicadorReferencia
	AND NumeroFila=@numeroFila AND a.IdIndicadorResultado=@IdResultadoIndicador
	
	UNION

	SELECT GETDATE() FechaFin, @numeroFila numeroFila
	WHERE @IdTipoFechaFinal=3
)
SELECT distinct case @UnidadMedida 
WHEN 1 THEN DATEDIFF(DAY, a.FechaInicio, b.FechaFin ) 
WHEN 2 THEN DATEDIFF(MONTH, a.FechaInicio, b.FechaFin  ) 
ELSE DATEDIFF(YEAR, a.FechaInicio, b.FechaFin  ) END 
Valor, c.NumeroFila
FROM  FechaInicio a,  FechaFin b, IndicardorResultadoDetalleCategoria c
INNER JOIN IndicadorResultado d
ON c.IdIndicadorResultado=d.IdIndicadorResultado
WHERE d.IdIndicador=@indicadorSalida