-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar formula de variale dato criterio
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarFormulaVariableDatoCriterio]
       @pIdFormulaVariableDatoCriterio INT,
	   @pIdFuenteIndicador INT,
	   @pIdIndicador VARCHAR(250),
	   @pIdDetalleIndicadorVariable INT,
	   @pIdCriterio VARCHAR(250),
	   @pidCategoriaDesagregacion INT,
	   @pIdDetalleCategoriaTexto INT,
	   @pIdAcumulacionFormula INT,
	   @pEsValorTotal BIT
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.FormulaVariableDatoCriterio AS TARGET
			USING (VALUES( 
				@pIdFormulaVariableDatoCriterio, 
				@pIdFuenteIndicador,
				@pIdIndicador,
				@pIdDetalleIndicadorVariable, 
				@pIdCriterio,
				@pidCategoriaDesagregacion,
				@pIdDetalleCategoriaTexto,
				@pIdAcumulacionFormula,
				@pEsValorTotal
			))AS SOURCE (
				IdFormulaVariableDatoCriterio, 
				IdFuenteIndicador,
				IdIndicador,
				IdDetalleIndicadorVariable,
				IdCriterio,
				idCategoriaDesagregacion,
				IdDetalleCategoriaTexto,
				IdAcumulacionFormula,
				EsValorTotal
			)
			ON TARGET.IdFormulaVariableDatoCriterio = SOURCE.IdFormulaVariableDatoCriterio 
										WHEN NOT MATCHED THEN
											INSERT (  
												    IdFuenteIndicador,
													IdIndicador,
													IdDetalleIndicadorVariable,
													IdCriterio,
													idCategoriaDesagregacion,
													IdDetalleCategoriaTexto,
													IdAcumulacionFormula,
													EsValorTotal
													)
											VALUES( 
													SOURCE.IdFuenteIndicador,
													SOURCE.IdIndicador,
													SOURCE.IdDetalleIndicadorVariable,
													SOURCE.IdCriterio,
													SOURCE.idCategoriaDesagregacion,
													SOURCE.IdDetalleCategoriaTexto,
													SOURCE.IdAcumulacionFormula,
													SOURCE.EsValorTotal
												    )
										WHEN MATCHED THEN
											UPDATE SET 
												    IdFuenteIndicador = SOURCE.IdFuenteIndicador,
													IdIndicador = SOURCE.IdIndicador,
													IdDetalleIndicadorVariable = SOURCE.IdDetalleIndicadorVariable,
													IdCriterio = SOURCE.IdCriterio,
													idCategoriaDesagregacion = SOURCE.idCategoriaDesagregacion,
													IdDetalleCategoriaTexto = SOURCE.IdDetalleCategoriaTexto,
													IdAcumulacionFormula = SOURCE.IdAcumulacionFormula,
													EsValorTotal = SOURCE.EsValorTotal;
	COMMIT TRAN

	SELECT  
		IdFormulaVariableDatoCriterio, 
		IdFuenteIndicador,
		IdIndicador,
		IIF (IdDetalleIndicadorVariable IS NULL, 0, IdDetalleIndicadorVariable) AS IdDetalleIndicadorVariable, 
		IIF (IdCriterio IS NULL, '', IdCriterio) AS IdCriterio,
		IIF (idCategoriaDesagregacion IS NULL, 0, idCategoriaDesagregacion) AS idCategoriaDesagregacion,
		IIF (IdDetalleCategoriaTexto IS NULL, 0, IdDetalleCategoriaTexto) AS IdDetalleCategoriaTexto,
		IdAcumulacionFormula,
		EsValorTotal
  FROM dbo.FormulaVariableDatoCriterio
  WHERE IdFormulaVariableDatoCriterio = @pIdFormulaVariableDatoCriterio OR IdFormulaVariableDatoCriterio = SCOPE_IDENTITY()

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		BEGIN
			SELECT   
				ERROR_NUMBER() AS ErrorNumber  
			   ,ERROR_MESSAGE() AS ErrorMessage; 
			ROLLBACK TRANSACTION;
		END
END CATCH