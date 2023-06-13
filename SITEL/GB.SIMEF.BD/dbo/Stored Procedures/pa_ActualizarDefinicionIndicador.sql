-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Definición del Indicador
-- =============================================
CREATE PROCEDURE  [dbo].[pa_ActualizarDefinicionIndicador]
       @Fuente VARCHAR(3000)
      ,@Notas VARCHAR(3000)
      ,@idIndicador INT
      ,@IdEstadoRegistro INT
      ,@Definicion VARCHAR(3000)	  
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.DefinicionIndicador AS TARGET
			USING (VALUES( 
				 UPPER(@Fuente) 
				,UPPER(@Notas) 
				,@idIndicador
				,@IdEstadoRegistro 
				,UPPER(@Definicion) 
			))AS SOURCE (
				 Fuente 
				,Nota
				,idIndicador
				,IdEstadoRegistro 
				,Definicion 
			)
			ON TARGET.idDefinicionIndicador=SOURCE.idIndicador 
										WHEN NOT MATCHED THEN
											INSERT (  
												     Fuente 
													,Nota 
													,idDefinicionIndicador
													,IdEstadoRegistro 
													,Definicion )
											VALUES(
												 
												    SOURCE.Fuente 
													,SOURCE.Nota
													,SOURCE.idIndicador
													,SOURCE.IdEstadoRegistro 
													,SOURCE.Definicion )
										WHEN MATCHED THEN
											UPDATE SET 
												    Fuente=SOURCE.Fuente 
												   ,Nota=SOURCE.Nota
												   ,idDefinicionIndicador=SOURCE.idIndicador
												   ,IdEstadoRegistro= SOURCE.IdEstadoRegistro
												   ,Definicion=SOURCE.Definicion;
	COMMIT TRAN
	IF(@IdEstadoRegistro=4)
	BEGIN
		UPDATE Indicador SET VisualizaSigitel=0 WHERE IdIndicador=@idIndicador

	END 

	SELECT  
       Fuente
      ,Nota
      ,idDefinicionIndicador
      ,IdEstadoRegistro
      ,Definicion
  FROM dbo.DefinicionIndicador
  WHERE idDefinicionIndicador=@idIndicador
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