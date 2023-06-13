
CREATE PROC [dbo].[pa_ResultadoIndicadorDetalleCategoriaSalida]

 @idFormula INT,
 @idResultado UNIQUEIDENTIFIER
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para el resultado indicador detalle categoria salida




WITH IndicadorSalida_NivelCalculo
AS
(
	SELECT   a.IdIndicador,b.idCategoriaDesagregacion, a.IdFormulaCalculo
	FROM formulaCalculo a
	INNER JOIN Indicador c
	ON c.IdIndicador=a.IdIndicador
	INNER JOIN DetalleIndicadorCategoria b
	ON a.IdIndicador=b.IdIndicador
	AND b.Estado=1
	WHERE a.IdFormulaCalculo=@idFormula
	AND a.NivelCalculoTotal=1
	UNION
	SELECT a.IdIndicador,b.idCategoriaDesagregacion, a.IdFormulaCalculo
	FROM formulaCalculo a
	INNER JOIN Indicador c
	ON c.IdIndicador=a.IdIndicador
	INNER JOIN  DetalleIndicadorCategoria b
	ON a.IdIndicador=b.IdIndicador
	AND b.Estado=1
	INNER JOIN  FormulaNivelCalculoCategoria d
	ON d.IdFormulaCalculo=a.IdFormulaCalculo
	AND d.IdCategoriaDesagregacion=b.idCategoriaDesagregacion
	WHERE a.IdFormulaCalculo=@idFormula
	AND a.NivelCalculoTotal=0

),

IndicadorEntrada_DetalleCategoria
AS
(
	SELECT a.idCategoriaDesagregacion,c.IdIndicador IndicadorEntrada, a.IdIndicador IndicadorSalida, d.IdIndicadorResultado ResultadoIndicadorEntrada
	FROM IndicadorSalida_NivelCalculo a
	INNER JOIN  ArgumentoFormula b
	ON a.IdFormulaCalculo=b.IdFormulaCalculo 
	INNER JOIN  FormulaVariableDatoCriterio c
	ON b.IdFormulaVariableDatoCriterio=c.IdFormulaVariableDatoCriterio
	AND (c.IdCategoriaDesagregacion=a.idCategoriaDesagregacion OR c.IdCategoriaDesagregacion IS NULL)
	INNER JOIN  IndicadorResultado d
	ON c.IdIndicador=CAST(d.IdIndicador AS VARCHAR(100))
	WHERE d.IdIndicadorResultado =(SELECT TOP 1 t.IdIndicadorResultado 
	FROM IndicadorResultado t 
	INNER JOIN IndicardorResultadoDetalleCategoria x
	ON x.IdIndicadorResultado=t.IdIndicadorResultado 

	WHERE CAST(t.IdIndicador AS VARCHAR(100))=c.IdIndicador AND t.Estado=1
	 ORDER BY t.FechaCreacion DESC
	)
),


ResultadoIndicadorCategoria
AS
(
	SELECT DISTINCT @idResultado idResultado, b.IdCategoriaDesagregacion,b.TipoCategoria,b.Valor,b.NumeroFila,c.IndicadorEntrada IndicadorReferencia, 1 Estado, GETDATE() FechaCreacion
	FROM IndicadorResultado a
	INNER JOIN IndicardorResultadoDetalleCategoria b
	ON a.IdIndicadorResultado=b.IdIndicadorResultado
	INNER JOIN IndicadorEntrada_DetalleCategoria c
	ON c.idCategoriaDesagregacion=b.IdCategoriaDesagregacion
	AND a.IdIndicadorResultado=c.ResultadoIndicadorEntrada
	WHERE c.IndicadorEntrada =(SELECT TOP 1 IndicadorEntrada FROM IndicadorEntrada_DetalleCategoria)
)

MERGE dbo.IndicardorResultadoDetalleCategoria AS TARGET

USING( SELECT * FROM ResultadoIndicadorCategoria) AS SOURCE
ON (SOURCE.idResultado=TARGET.idIndicadorResultado
AND SOURCE.NumeroFila = TARGET.NumeroFila
AND SOURCE.idCategoriaDesagregacion=TARGET.idCategoriaDesagregacion)
WHEN NOT MATCHED
THEN INSERT (
            IdIndicadorResultado
           ,IdCategoriaDesagregacion
           ,NumeroFila
           ,TipoCategoria
           ,Valor
           ,IndicadorReferencia
           ,Estado
           ,FechaCreacion)
     VALUES
	 (
            SOURCE.IdResultado
           ,SOURCE.IdCategoriaDesagregacion
           ,SOURCE.NumeroFila
           ,SOURCE.TipoCategoria
           ,SOURCE.Valor
           ,SOURCE.IndicadorReferencia
           ,SOURCE.Estado
           ,SOURCE.FechaCreacion)
WHEN MATCHED THEN 
	UPDATE SET IdCategoriaDesagregacion=SOURCE.IdCategoriaDesagregacion,
	IndicadorReferencia=SOURCE.IndicadorReferencia,
	Valor=SOURCE.Valor;
	 SELECT 
			IdIndicardorResultadoDetalleCategoria
           ,IdIndicadorResultado
           ,IdCategoriaDesagregacion
           ,NumeroFila
           ,TipoCategoria
           ,Valor
           ,Estado
		   ,IndicadorReferencia
	 FROM dbo.IndicardorResultadoDetalleCategoria
	 WHERE IdIndicadorResultado=@idResultado;