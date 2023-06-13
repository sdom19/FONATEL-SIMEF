-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar la fecha de Categorias de Desagregación
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarCategoriaDesagregacionFecha]
	   @idCategoria INT
      ,@FechaMinima DATE
      ,@FechaMaxima DATE
      ,@Estado BIT
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.DetalleCategoriafecha AS TARGET
			USING (VALUES(  @idCategoria,@FechaMinima ,@FechaMaxima ,@Estado  ))AS SOURCE 
						 (  idCategoria,FechaMinima ,FechaMaxima ,Estado )
										ON TARGET.idCategoriaDesagregacion=SOURCE.idCategoria
										AND TARGET.idCategoriaDesagregacion>0
										WHEN NOT MATCHED THEN
											INSERT ( 
													 idCategoriaDesagregacion
													,FechaMinima
													,FechaMaxima
													,Estado)
											VALUES(
													SOURCE.idCategoria
													,SOURCE.FechaMinima
													,SOURCE.FechaMaxima
													,SOURCE.Estado
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											FechaMinima=SOURCE.FechaMinima,
											FechaMaxima=SOURCE.FechaMaxima;
	COMMIT TRAN
	SELECT  idDetalleCategoriafecha
		   ,idCategoriaDesagregacion
		   ,FechaMinima
		   ,FechaMaxima
		   ,Estado
  FROM dbo.DetalleCategoriafecha
  WHERE idCategoriaDesagregacion=@idCategoria;
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