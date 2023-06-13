
CREATE PROC [dbo].[pa_ResultadoIndicadorSalida]
@IdFormulaCalculo INT 

AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para el resultado indicador salida

MERGE dbo.IndicadorResultado AS TARGET 
USING 
	 (SELECT   
	 a.IdIndicador,
	 CAST(CONCAT('Frm_',a.codigo,'_Cod_',c.codigo) AS VARCHAR(300)) NombreIndicador,
	 GETDATE() FechaCreacion,
	 'Motor de Cálculo' Usuario,
	 c.IdClasificacionIndicador TipoIndicador,
	 1 Estado,
	 MONTH(GETDATE()) idMes,
	 b.IdAnno
	FROM FormulaCalculo a
	INNER JOIN Indicador c
	ON c.IdIndicador=a.IdIndicador
	AND a.IdEstadoRegistro=2
	INNER JOIN Anno b
	ON b.Nombre=CAST(YEAR(GETDATE()) AS VARCHAR(10))
	WHERE IdFormulaCalculo=@IdFormulaCalculo) AS SOURCE
ON SOURCE.IdIndicador=TARGET.IdIndicador
WHEN MATCHED 
	THEN UPDATE SET NombreIndicador=SOURCE.NombreIndicador,
	FechaCreacion=GETDATE()
WHEN NOT MATCHED
    THEN INSERT 
           (
            IdIndicador
           ,NombreIndicador
           ,FechaCreacion
           ,Usuario
           ,TipoIndicador
           ,Estado
           ,idMes
           ,IdAnno)
    VALUES (
            SOURCE.IdIndicador
           ,SOURCE.NombreIndicador
           ,SOURCE.FechaCreacion
           ,SOURCE.Usuario
           ,SOURCE.TipoIndicador
           ,SOURCE.Estado
           ,SOURCE.idMes
           ,SOURCE.IdAnno);
	SELECT 
			A.IdIndicadorResultado
           ,A.IdIndicador
           ,A.NombreIndicador
           ,A.FechaCreacion
           ,A.Usuario
           ,A.TipoIndicador
           ,A.Estado
           ,A.idMes
           ,A.IdAnno	
    FROM IndicadorResultado A
	INNER JOIN FormulaCalculo B 
	ON A.IdIndicador=B.IdIndicador
	WHERE B.IdFormulaCalculo=@IdFormulaCalculo

/****** Object:  StoredProcedure [dbo].[pa_ValidarFormulario]    Script Date: 15/3/2023 07:28:41 ******/
SET ANSI_NULLS ON