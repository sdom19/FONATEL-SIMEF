-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar formula de cálculo
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarFormulaCalculo]
		@pIdFormulaCalculo INT,
		@pCodigo VARCHAR(30),
		@pNombre VARCHAR(300),
		@pIdIndicador INT,
		@pIdDetalleIndicadorVariable INT,
		@pFechaCalculo DATETIME,
		@pDescripcion VARCHAR(1500),
		@pIdFrecuenciaEnvio INT,
		@pNivelCalculoTotal BIT,
		@pUsuarioModificacion VARCHAR(100),
		@pUsuarioCreacion VARCHAR(100),
		@pIdEstadoRegistro INT
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.FormulaCalculo AS TARGET
			USING (VALUES(  @pIdFormulaCalculo,
							@pCodigo,
							@pNombre,
							@pIdIndicador,
						    @pIdDetalleIndicadorVariable,
							@pFechaCalculo,
							@pDescripcion,
							@pNivelCalculoTotal,
							@pIdFrecuenciaEnvio,
							@pUsuarioModificacion,
							@pUsuarioCreacion,
							@pIdEstadoRegistro 
						  ))AS SOURCE (
											IdFormulaCalculo,
											Codigo,
											Nombre,
											IdIndicador,
											IdDetalleIndicadorVariable,
											FechaCalculo,
											Descripcion,
											NivelCalculoTotal,
											IdFrecuenciaEnvio,
											UsuarioModificacion,
											UsuarioCreacion,
										    IdEstadoRegistro 
												)
										ON TARGET.IdFormulaCalculo = SOURCE.IdFormulaCalculo 
										WHEN NOT MATCHED THEN
											INSERT ( 
													Codigo,
													Nombre,
													IdIndicador,
													IdDetalleIndicadorVariable,
													FechaCalculo,
													Descripcion,
													NivelCalculoTotal,
													IdFrecuenciaEnvio,
													FechaCreacion,
													UsuarioCreacion,
													IdEstadoRegistro 
													)
											VALUES (
													SOURCE.Codigo,
													SOURCE.Nombre,
													SOURCE.IdIndicador,
													SOURCE.IdDetalleIndicadorVariable,
													SOURCE.FechaCalculo,
													SOURCE.Descripcion,
													SOURCE.NivelCalculoTotal,
													SOURCE.IdFrecuenciaEnvio,
													GETDATE(),
													SOURCE.UsuarioCreacion,
													SOURCE.IdEstadoRegistro 
													)
										WHEN MATCHED THEN
											UPDATE SET 
											Codigo = SOURCE.Codigo
											,Nombre = SOURCE.Nombre
											,IdIndicador = SOURCE.IdIndicador
											,IdDetalleIndicadorVariable = SOURCE.IdDetalleIndicadorVariable
											,FechaCalculo = SOURCE.FechaCalculo
											,Descripcion = SOURCE.Descripcion
											,NivelCalculoTotal = SOURCE.NivelCalculoTotal
											,IdFrecuenciaEnvio = SOURCE.IdFrecuenciaEnvio
											,FechaModificacion = GETDATE()
											,UsuarioModificacion = SOURCE.UsuarioModificacion
											,IdEstadoRegistro = SOURCE.IdEstadoRegistro;
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
	WHERE IdFormulaCalculo = @pIdFormulaCalculo OR IdFormulaCalculo = SCOPE_IDENTITY()

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