
 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para construir argumento de salida variable
-- ============================================
CREATE PROCEDURE [dbo].[pa_ConstruirArgumentoSalidaVariable]



@IndicadorSalida INT,
@IdAcumulacion INT,
@IdVariable INT,
@IdCategoria INT,
@IdCategoriaDetalle INT,
@IndicadorReferencia INT
AS

DECLARE @FechaUltimoRegistro DATE;
SELECT TOP 1 @FechaUltimoRegistro = FechaCreacion FROM IndicadorResultado
WHERE IdIndicador = @IndicadorSalida
ORDER BY FechaCreacion DESC 


IF @FechaUltimoRegistro IS NULL AND @IdAcumulacion <> 0
BEGIN
    SET @IdAcumulacion = 0;
END; 


WITH MainQuery
AS (
    SELECT ISNULL(SUM(a.Valor),0) Valor, b.NumeroFila, a.IdIndicadorResultadoDetalleVariable, b.FechaCreacion
    FROM dbo.IndicadorResultado r
    INNER JOIN dbo.IndicadorResultadoDetalleVariable a
    ON r.IdIndicadorResultado=a.IdIndicadorResultado
    INNER JOIN dbo.IndicardorResultadoDetalleCategoria b
    ON a.IdIndicadorResultado=b.IdIndicadorResultado
    AND a.NumeroFila=b.NumeroFila
    INNER JOIN DetalleCategoriaTexto c
    ON b.IdCategoriaDesagregacion=c.IdCategoriaDesagregacion
    AND UPPER(TRIM(c.Etiqueta)) LIKE UPPER('%'+TRIM(b.valor)+'%')      
WHERE r.IdIndicador=@IndicadorReferencia AND a.IdDetalleIndicadorVariable=@IdVariable
   AND b.IdCategoriaDesagregacion=ISNULL(@IdCategoria, b.IdCategoriaDesagregacion) /* valor total y por detalle */
    AND c.IdDetalleCategoriaTexto=ISNULL(@IdCategoriaDetalle, c.IdDetalleCategoriaTexto) /* valor total y por detalle */
    GROUP BY b.NumeroFila, a.idIndicadorResultadoDetalleVariable, b.FechaCreacion
) 


SELECT ISNULL(b.Valor,0) Valor, a.NumeroFila, a.idIndicadorResultadoDetalleVariable
FROM MainQuery a
LEFT JOIN MainQuery b
ON a.IdIndicadorResultadoDetalleVariable=b.IdIndicadorResultadoDetalleVariable
AND a.NumeroFila=b.NumeroFila
AND  FORMAT(a.FechaCreacion,'MM-yy') = 
    CASE @IdAcumulacion
    /* acumulacion mensual */
    WHEN 3 THEN FORMAT(DATEADD(MONTH, DATEDIFF(MONTH, -1, CAST(@FechaUltimoRegistro AS VARCHAR(100)))-1, -1),'MM-yy')
    /* acumulacion anual */
    WHEN 2 THEN FORMAT(DATEADD(YEAR, DATEDIFF(YEAR, -1, CAST(@FechaUltimoRegistro AS VARCHAR(100)))-1, -1),'MM-yy')
   /* acumulación total */
    ELSE FORMAT(b.FechaCreacion,'MM-yy')
    END
WHERE a.NumeroFila IS NOT NULL
ORDER BY a.NumeroFila