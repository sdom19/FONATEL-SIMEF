-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Detalle de categoria de manera manual
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarDetalleCategoriaTexto]
	   @idCategoriaDetalle INT
      ,@idCategoria INT 
      ,@Codigo INT 
      ,@Etiqueta VARCHAR(300)
      ,@Estado BIT  
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.DetalleCategoriaTexto AS TARGET
			USING (VALUES( @idCategoriaDetalle 
						  ,@idCategoria 
						  ,@Codigo 
						  ,@Etiqueta
						  ,@Estado))AS SOURCE (idCategoriaDetalle 
												,idCategoria 
												,Codigo 
												,Etiqueta
												,Estado )
										ON TARGET.idCategoriaDesagregacion =SOURCE.idCategoria AND
										--TARGET.Codigo =SOURCE.Codigo 
										TARGET.idDetalleCategoriaTexto>0 AND 
										TARGET.idDetalleCategoriaTexto=SOURCE.idCategoriaDetalle
										WHEN NOT MATCHED THEN
											INSERT ( 
												 
													idCategoriaDesagregacion 
													,Codigo 
													,Etiqueta
													,Estado )
											VALUES(
												 
												 
													SOURCE.idCategoria 
													,SOURCE.Codigo 
													,UPPER(SOURCE.Etiqueta)
													,SOURCE.Estado  )
										WHEN MATCHED THEN
											UPDATE SET 
											idCategoriaDesagregacion=SOURCE.idCategoria,
											Etiqueta=UPPER(SOURCE.Etiqueta),
											CODIGO=SOURCE.Codigo,
											estado=SOURCE.estado;
	COMMIT TRAN
	SELECT idDetalleCategoriaTexto
		  ,idCategoriaDesagregacion
		  ,Codigo 
		  ,Etiqueta
		  ,Estado 
	FROM   dbo.DetalleCategoriaTexto NOLOCK
	WHERE idDetalleCategoriaTexto=@idCategoriaDetalle OR idDetalleCategoriaTexto=SCOPE_IDENTITY()

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