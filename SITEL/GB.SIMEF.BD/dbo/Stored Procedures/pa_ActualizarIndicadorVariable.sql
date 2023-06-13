
-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Variable del Indicador
-- =============================================

CREATE PROCEDURE [dbo].[pa_ActualizarIndicadorVariable]
	  @pIdDetalleIndicador INT
      ,@pIdIndicador INT 
      ,@pNombreVariable VARCHAR(300)
      ,@pDescripcion VARCHAR(3000)
      ,@pEstado BIT  
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.DetalleIndicadorVariable AS TARGET
			USING (VALUES(
							@pIdDetalleIndicador
						   ,@pIdIndicador 
						  ,@pNombreVariable 
						  ,@pDescripcion
						  ,@pEstado))AS SOURCE (  
												IdDetalleIndicador
												,IdIndicador 
												,NombreVariable 
												,Descripcion
												,Estado )
										ON TARGET.IdDetalleIndicadorVariable = SOURCE.IdDetalleIndicador
										AND TARGET.IdDetalleIndicadorVariable <>0 
									
										
										WHEN NOT MATCHED THEN
											INSERT (
													IdIndicador 
													,NombreVariable
													,Descripcion
													,Estado )
											VALUES(
																						 
													SOURCE.IdIndicador 
													,SOURCE.NombreVariable 
													,SOURCE.Descripcion
													,SOURCE.Estado)
										WHEN MATCHED THEN
											UPDATE SET 
											IdIndicador=SOURCE.IdIndicador,
											NombreVariable=SOURCE.NombreVariable,
											Descripcion=SOURCE.Descripcion,
											Estado=SOURCE.Estado;
	
	COMMIT TRAN

	SELECT IdDetalleIndicadorVariable
		  ,IdIndicador
		  ,NombreVariable
		  ,Descripcion
		  ,Estado 
	from dbo.DetalleIndicadorVariable
	where IdDetalleIndicadorVariable = @pIdDetalleIndicador OR IdDetalleIndicadorVariable = SCOPE_IDENTITY()

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