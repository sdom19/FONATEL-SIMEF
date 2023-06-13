-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Etiqueta de formula
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarEtiquetaFormula]
	@pIdFormulaCalculo INT,
	@pEtiquetaFormula VARCHAR(8000),
	@pUsuarioModificacion VARCHAR(100)
AS
BEGIN

	BEGIN TRY
		BEGIN TRAN;
			UPDATE dbo.FormulaCalculo
			SET 
				Formula = @pEtiquetaFormula,
				UsuarioModificacion = @pUsuarioModificacion,
				FechaModificacion = GETDATE()
			WHERE IdFormulaCalculo = @pIdFormulaCalculo
		COMMIT TRAN

		SELECT TOP (1) IdFormulaCalculo
		  ,Codigo
		  ,Nombre
		  ,IdIndicador
		  ,IdDetalleIndicadorVariable
		  ,Descripcion
		  ,IdFrecuenciaEnvio
		  ,NivelCalculoTotal
		  ,UsuarioModificacion
		  ,FechaCreacion
		  ,UsuarioCreacion
		  ,FechaModificacion
		  ,IdEstadoRegistro
		  ,FechaCalculo
		  ,Formula
		  ,IdJob
		FROM FormulaCalculo
		WHERE IdFormulaCalculo = @pIdFormulaCalculo
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