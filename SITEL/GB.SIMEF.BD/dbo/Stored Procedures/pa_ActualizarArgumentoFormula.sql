-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Argumento de Formula
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarArgumentoFormula]
       @pIdArgumentoFormula INT,
	   @pIdFormulaTipoArgumento INT,
	   @pIdDefinicionFecha INT,
	   @pIdVariableDatoCriterio INT,
	   @pIdFormula INT,
	   @pPredicadoSQL VARCHAR(8000),
	   @pOrdenEnFormula INT,
	   @pEtiqueta VARCHAR(1000)
AS

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.ArgumentoFormula AS TARGET
			USING (VALUES( 
				@pIdArgumentoFormula,
				@pIdFormulaTipoArgumento,
				@pIdDefinicionFecha,
				@pIdVariableDatoCriterio,
				@pIdFormula,
				@pPredicadoSQL,
				@pOrdenEnFormula,
				@pEtiqueta
			))AS SOURCE (
				IdArgumentoFormula,
				IdFormulaTipoArgumento,
				IdDefinicionFecha,
				IdVariableDatoCriterio,
				IdFormula,
				PredicadoSQL,
				OrdenEnFormula,
				Etiqueta
			)
			ON TARGET.IdArgumentoFormula = SOURCE.IdArgumentoFormula 
										WHEN NOT MATCHED THEN
											INSERT (  
												    IdFormulaCalculo,
													IdFormulaTipoArgumento,
													IdFormulaDefinicionFecha,
													IdFormulaVariableDatoCriterio,
													PredicadoSQL,
													OrdenEnFormula,
													Etiqueta
													)
											VALUES( 
													SOURCE.IdFormula,
													SOURCE.IdFormulaTipoArgumento,
													SOURCE.IdDefinicionFecha,
													SOURCE.IdVariableDatoCriterio,
													SOURCE.PredicadoSQL,
													SOURCE.OrdenEnFormula,
													SOURCE.Etiqueta
												    )
										WHEN MATCHED THEN
											UPDATE SET 
												    IdFormulaCalculo = SOURCE.IdFormula,
													IdFormulaTipoArgumento = SOURCE.IdFormulaTipoArgumento,
													IdFormulaDefinicionFecha = SOURCE.IdDefinicionFecha,
													IdFormulaVariableDatoCriterio = SOURCE.IdVariableDatoCriterio,
													PredicadoSQL = SOURCE.PredicadoSQL,
													OrdenEnFormula = SOURCE.OrdenEnFormula,
													Etiqueta = SOURCE.Etiqueta;
	COMMIT TRAN

	SELECT  
		IdArgumentoFormula,
		IdFormulaTipoArgumento,
		IIF (IdFormulaDefinicionFecha IS NULL, 0, IdFormulaDefinicionFecha) AS IdDefinicionFecha, 
		IIF (IdFormulaVariableDatoCriterio IS NULL, 0, IdFormulaVariableDatoCriterio) AS IdFormulaVariableDatoCriterio, 
		IdFormulaCalculo,
		PredicadoSQL,
		OrdenEnFormula,
		Etiqueta
  FROM dbo.ArgumentoFormula
  WHERE IdArgumentoFormula = @pIdArgumentoFormula OR IdArgumentoFormula = SCOPE_IDENTITY()


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