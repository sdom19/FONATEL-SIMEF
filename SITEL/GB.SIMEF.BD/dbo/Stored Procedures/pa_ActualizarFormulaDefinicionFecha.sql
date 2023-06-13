CREATE PROCEDURE [dbo].[pa_ActualizarFormulaDefinicionFecha]
       @pIdFormulaDefinicionFecha INT,
	   @pFechaInicio DATETIME,
	   @pFechaFinal DATETIME,
	   @pIdUnidadMedida INT,
	   @pIdTipoFechaInicio INT,
	   @pIdTipoFechaFinal INT,
	   @pIdCategoriaInicio INT,
	   @pIdCategoriaFinal INT,
	   @pIdIndicador INT
AS

BEGIN TRY
-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar fecha de definición de formula 
-- =============================================
	BEGIN TRAN;
		MERGE dbo.FormulaDefinicionFecha AS TARGET
			USING (VALUES( 
				@pIdFormulaDefinicionFecha, 
				@pFechaInicio,
				@pFechaFinal,
				@pIdUnidadMedida,
				@pIdTipoFechaInicio,
				@pIdTipoFechaFinal,
				@pIdCategoriaInicio,
				@pIdCategoriaFinal,
				@pIdIndicador
			))AS SOURCE (
				IdFormulaDefinicionFecha, 
				FechaInicio,
				FechaFinal,
				IdUnidadMedida,
				IdTipoFechaInicio,
				IdTipoFechaFinal,
				IdCategoriaInicio,
				IdCategoriaFinal,
				IdIndicador
			)
			ON TARGET.IdFormulaDefinicionFecha = SOURCE.IdFormulaDefinicionFecha 
										WHEN NOT MATCHED THEN
											INSERT (  
												    FechaInicio,
													FechaFinal,
													IdUnidadMedida,
													IdTipoFechaInicio,
													IdTipoFechaFinal,
													IdCategoriaDesagregacionInicio,
													IdCategoriaDesagregacionFinal,
													IdIndicador
													)
											VALUES( 
													SOURCE.FechaInicio,
													SOURCE.FechaFinal,
													SOURCE.IdUnidadMedida,
													SOURCE.IdTipoFechaInicio,
													SOURCE.IdTipoFechaFinal,
													SOURCE.IdCategoriaInicio,
													SOURCE.IdCategoriaFinal,
													SOURCE.IdIndicador
												    )
										WHEN MATCHED THEN
											UPDATE SET 
												    FechaInicio = SOURCE.FechaInicio,
													FechaFinal = SOURCE.FechaFinal,
													IdUnidadMedida = SOURCE.IdUnidadMedida,
													IdTipoFechaInicio = SOURCE.IdTipoFechaInicio,
													IdTipoFechaFinal = SOURCE.IdTipoFechaFinal,
													IdCategoriaDesagregacionInicio = SOURCE.IdCategoriaInicio,
													IdCategoriaDesagregacionFinal = SOURCE.IdCategoriaFinal,
													IdIndicador = SOURCE.IdIndicador;
	COMMIT TRAN

	SELECT  
		IdFormulaDefinicionFecha, 
		FechaInicio,
		FechaFinal,
		IdUnidadMedida,
		IdTipoFechaInicio,
		IdTipoFechaFinal,
		IIF (IdCategoriaDesagregacionInicio IS NULL, 0, IdCategoriaDesagregacionInicio) AS IdCategoriaDesagregacionInicio,
		IIF (IdCategoriaDesagregacionFinal IS NULL, 0, IdCategoriaDesagregacionFinal) AS IdCategoriaDesagregacionFinal,
		IdIndicador
  FROM dbo.FormulaDefinicionFecha
  WHERE IdFormulaDefinicionFecha = @pIdFormulaDefinicionFecha OR IdFormulaDefinicionFecha = SCOPE_IDENTITY()


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