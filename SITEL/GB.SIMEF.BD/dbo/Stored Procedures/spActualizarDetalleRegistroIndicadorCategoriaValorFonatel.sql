CREATE procedure [dbo].[spActualizarDetalleRegistroIndicadorCategoriaValorFonatel]
	   @IdSolicitud int 
      ,@IdFormulario int
      ,@IdIndicador int
      ,@idCategoria int
	  ,@NumeroFila int
	  ,@Valor varchar(1000)
as

BEGIN TRY
	BEGIN TRAN;
		MERGE SITELP.FONATEL.DetalleRegistroIndicadorCategoriaValorFonatel AS TARGET
			USING (VALUES( @IdSolicitud 
						  ,@IdFormulario
						  ,@IdIndicador
						  ,@idCategoria
						  ,@NumeroFila  
						  ,@Valor))AS SOURCE (IdSolicitud 
											  ,IdFormulario
											  ,IdIndicador
											  ,idCategoria
											  ,NumeroFila  
											  ,Valor) 
										ON TARGET.IdSolicitud =SOURCE.IdSolicitud
										WHEN NOT MATCHED THEN
											INSERT (IdSolicitud 
												  ,IdFormulario
												  ,IdIndicador
												  ,idCategoria
												  ,NumeroFila  
												  ,Valor)
											VALUES (
													 Source.IdSolicitud
													,Source.IdFormulario
													,Source.IdIndicador
													,Source.idCategoria
													,Source.NumeroFila
													,Source.Valor)
											WHEN MATCHED THEN
											UPDATE SET 
											IdSolicitud=Source.IdSolicitud,
											IdFormulario=Source.IdFormulario,
											IdIndicador=Source.IdIndicador,
											idCategoria  =Source.idCategoria,
											NumeroFila=Source.NumeroFila,
											Valor=Source.Valor;
	COMMIT TRAN
	SELECT [IdSolicitud]
      ,[IdFormulario]
      ,[IdIndicador]
      ,[idCategoria]
      ,[NumeroFila]
      ,[Valor]
  FROM SITELP.FONATEL.[DetalleRegistroIndicadorCategoriaValorFonatel];



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