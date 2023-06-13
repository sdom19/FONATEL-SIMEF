

CREATE PROCEDURE [dbo].[pa_ActualizarReglaAtributoValido]
	   @IdCompara INT
	  ,@IdDetalleReglaValidacion INT
      ,@IdCategoria INT
      ,@IdDetalleCategoriaTexto INT
	  ,@OpcionEliminar BIT

AS
-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar regla de validación de atributo
-- =============================================
BEGIN TRY
	IF(@OpcionEliminar=1)
	BEGIN
		DELETE  FROM ReglaAtributoValido WHERE IdDetalleReglaValidacion=@IdDetalleReglaValidacion 
		
	END
	BEGIN TRAN;
		MERGE dbo.ReglaAtributoValido AS TARGET
			USING (VALUES( @IdDetalleReglaValidacion,@IdCategoria,@IdDetalleCategoriaTexto))AS SOURCE 
						 (  IdDetalleReglaValidacion,IdCategoria,IdDetalleCategoriaTexto)
										ON TARGET.IdDetalleReglaValidacion=SOURCE.IdDetalleReglaValidacion
										AND TARGET.IdCategoriaDesagregacion=SOURCE.IdCategoria and 
										TARGET.IdDetalleCategoriaTexto=SOURCE.IdDetalleCategoriaTexto
										
										
										WHEN NOT MATCHED THEN
											INSERT ( [IdDetalleReglaValidacion]
													,[IdCategoriaDesagregacion]
													,[IdDetalleCategoriaTexto])
											VALUES(
													SOURCE.IdDetalleReglaValidacion,
													SOURCE.IdCategoria,
													SOURCE.IdDetalleCategoriaTexto
												  );
	COMMIT TRAN

SELECT
    0 IdReglaAtributoValido 
    ,IdDetalleReglaValidacion
    ,IdCategoriaDesagregacion
	, 0 IdDetalleCategoriaTexto
       ,STRING_AGG(IdDetalleCategoriaTexto,',') idAtributoString
  FROM [ReglaAtributoValido]
 WHERE IdDetalleReglaValidacion=@IdDetalleReglaValidacion
	GROUP BY 
       IdCategoriaDesagregacion
	  ,IdDetalleReglaValidacion
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