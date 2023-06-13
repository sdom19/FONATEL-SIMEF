 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para eliminar argumentos de formula
-- ============================================
CREATE PROCEDURE [dbo].[pa_EliminarArgumentoDeFormula]
	@pIdFormula INT
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRY
		BEGIN TRAN;
			DECLARE @IDsABorrar TABLE (IdVariableDatoCriterio INT, IdDefinicionFecha INT)

			DELETE FROM ArgumentoFormula
			OUTPUT deleted.IdFormulaVariableDatoCriterio, deleted.IdFormulaDefinicionFecha INTO @IDsABorrar
			WHERE IdFormulaCalculo = @pIdFormula

			DELETE fv FROM FormulaVariableDatoCriterio fv
			INNER JOIN @IDsABorrar bb ON bb.IdVariableDatoCriterio = fv.IdFormulaVariableDatoCriterio
			
			DELETE ff FROM FormulaDefinicionFecha ff
			INNER JOIN @IDsABorrar bb ON bb.IdDefinicionFecha = ff.IdFormulaDefinicionFecha
			
			UPDATE FormulaCalculo
			SET Formula = null
			WHERE IdFormulaCalculo = @pIdFormula

			SELECT 1 AS BIT
		COMMIT TRAN

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
END