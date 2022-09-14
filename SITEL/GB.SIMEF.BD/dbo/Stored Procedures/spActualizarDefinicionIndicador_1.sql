Create procedure dbo.spActualizarDefinicionIndicador
	   @idDefinicion int
      ,@Fuente varchar(3000)
      ,@Notas varchar(3000)
      ,@idIndicador int
      ,@idEstado int
      ,@Definicion varchar(3000)	  
as

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.DefinicionIndicador AS TARGET
			USING (VALUES( 
				 @idDefinicion 
				,UPPER(@Fuente) 
				,UPPER(@Notas) 
				,@idIndicador
				,@idEstado 
				,UPPER(@Definicion) 
			))AS SOURCE (
				 idDefinicion 
				,Fuente 
				,Notas 
				,idIndicador
				,idEstado 
				,Definicion 
			)
			ON TARGET.idDefinicion=SOURCE.idDefinicion 
										WHEN NOT MATCHED THEN
											INSERT ( 
													 idDefinicion 
												    ,Fuente 
													,Notas 
													,idIndicador
													,idEstado 
													,Definicion )
											VALUES(
												 
													 source.idDefinicion 
												    ,source.Fuente 
													,source.Notas 
													,source.idIndicador
													,source.idEstado 
													,source.Definicion )
										WHEN MATCHED THEN
											UPDATE SET 
													idDefinicion= source.idDefinicion
												   ,Fuente=source.Fuente 
												   ,Notas=source.Notas
												   ,idIndicador=source.idIndicador
												   ,idEstado= source.idEstado
												   ,Definicion=source.Definicion;
	COMMIT TRAN
	
	SELECT  idDefinicion
      ,Fuente
      ,Notas
      ,idIndicador
      ,idEstado
      ,Definicion
  FROM SIMEF.dbo.DefinicionIndicador
  where idDefinicion=@idDefinicion


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