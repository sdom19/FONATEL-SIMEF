-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Detalle de Regla de Validación
-- =============================================

CREATE PROCEDURE [dbo].[pa_ActualizarDetalleReglaValidacion]
	   @pIdDetalleReglaValidacion INT
	  ,@pIdReglaValidacion INT
      ,@pIdTipoReglaValidacion INT
      ,@pIdOperador INT
	  ,@pIdDetalleIndicador INT
	  ,@pIdIndicador INT
      ,@pEstado BIT

AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.DetalleReglaValidacion AS TARGET
			USING (VALUES(  @pIdDetalleReglaValidacion,@pIdReglaValidacion,@pIdTipoReglaValidacion,@pIdOperador, @pIdDetalleIndicador,@pIdIndicador,@pEstado))AS SOURCE 
						 (  IdDetalleReglaValidacion,IdReglaValidacion,IdTipoReglaValidacion,IdOperador, IdDetalleIndicador,IdIndicador, Estado)

										ON TARGET.IdDetalleReglaValidacion=SOURCE.IdDetalleReglaValidacion
										WHEN NOT MATCHED THEN
											INSERT ( [IdReglaValidacion]
													,[IdTipoReglaValidacion]
													,[IdOperadorAritmetico]
													,[IdDetalleIndicadorVariable]
													,[IdIndicador]
													,[Estado]
												   )
											VALUES(
													SOURCE.IdReglaValidacion,
													SOURCE.IdTipoReglaValidacion,
													SOURCE.IdOperador,
													SOURCE.IdDetalleIndicador,
													SOURCE.IdIndicador,
													SOURCE.Estado 
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											IdReglaValidacion = SOURCE.IdReglaValidacion
											,IdTipoReglaValidacion = SOURCE.IdTipoReglaValidacion
											,IdOperadorAritmetico = SOURCE.IdOperador
											,IdDetalleIndicadorVariable = SOURCE.IdDetalleIndicador
											,IdIndicador = SOURCE.IdIndicador
											,Estado = SOURCE.Estado;
	COMMIT TRAN


		IF(@pEstado=0)
		BEGIN
			UPDATE ReglaValidacion SET IdEstadoRegistro=1 FROM DetalleReglaValidacion
			WHERE ReglaValidacion.IdReglaValidacion = @pIdReglaValidacion
		END 


	SELECT IdDetalleReglaValidacion
      ,[IdReglaValidacion]
      ,[IdTipoReglaValidacion]
      ,[IdOperadorAritmetico]
	  ,[IdDetalleIndicadorVariable]
	  ,[IdIndicador]
      ,[Estado]
  FROM [dbo].[DetalleReglaValidacion]
  WHERE IdDetalleReglaValidacion = @pIdDetalleReglaValidacion OR IdDetalleReglaValidacion = SCOPE_IDENTITY();
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