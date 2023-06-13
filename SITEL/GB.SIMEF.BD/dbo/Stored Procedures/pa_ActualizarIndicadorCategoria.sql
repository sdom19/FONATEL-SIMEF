
-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar categoria de Indicador
-- =============================================

CREATE PROCEDURE [dbo].[pa_ActualizarIndicadorCategoria]
	   @pIdDetalleIndicador INT
      ,@pIdIndicador INT 
      ,@pidCategoriaDesagregacion INT
      ,@pIdDetalleCategoriaTexto INT
      ,@pEstado BIT  
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.DetalleIndicadorCategoria AS TARGET
			USING (VALUES(   @pIdDetalleIndicador
							,@pIdIndicador
							,@pidCategoriaDesagregacion
							,@pIdDetalleCategoriaTexto
							,@pEstado  ))AS SOURCE (  
												 IdDetalleIndicador 
												,IdIndicador
												,idCategoriaDesagregacion 
												,IdDetalleCategoriaTexto
												,Estado )
										ON TARGET.IdIndicador  = SOURCE.IdIndicador 
										AND TARGET.idCategoriaDesagregacion  = SOURCE.idCategoriaDesagregacion 
										AND TARGET.IdDetalleCategoriaTexto  = SOURCE.IdDetalleCategoriaTexto
										
										WHEN NOT MATCHED THEN
											INSERT ( 
												     IdIndicador
												    ,idCategoriaDesagregacion 
												    ,IdDetalleCategoriaTexto
												    ,Estado )
											VALUES(
																						 
													SOURCE.IdIndicador 
													,SOURCE.idCategoriaDesagregacion 
													,SOURCE.IdDetalleCategoriaTexto
													,SOURCE.Estado)
										WHEN MATCHED THEN
											UPDATE SET 
											IdIndicador=SOURCE.IdIndicador,
											idCategoriaDesagregacion=SOURCE.idCategoriaDesagregacion,
											IdDetalleCategoriaTexto=SOURCE.IdDetalleCategoriaTexto,
											Estado=SOURCE.Estado;
	
	COMMIT TRAN

	SELECT IdDetalleIndicadorCategoria
		  ,IdIndicador
		  ,idCategoriaDesagregacion
		  ,IdDetalleCategoriaTexto
		  ,Estado
  FROM [dbo].[DetalleIndicadorCategoria]
	WHERE @pIdIndicador=IdIndicador

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