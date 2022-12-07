CREATE procedure [SITEL].[spActualizarDetalleRegistroIndicadorFonatel]
	   @IdSolicitud int=0
      ,@IdFormulario int=0
      ,@IdIndicador int=0
      ,@IdDetalleRegistroIndicador int=0
      ,@TituloHojas varchar(300)=''
      ,@NotasEncargado varchar(3000)=''
	  ,@NotasInformante varchar(3000)=''
	  ,@CodigoIndicador varchar(30)=''
	  ,@NombreIndicador varchar(350)=''
	  ,@CantidadFilas int=0
AS

BEGIN TRY
	BEGIN TRAN;
		MERGE SITELP.FONATEL.DetalleRegistroIndicadorFonatel AS TARGET
			USING (VALUES( @IdSolicitud
						  ,@IdFormulario
						  ,@IdIndicador
						  ,@IdDetalleRegistroIndicador
						  ,@TituloHojas
						  ,@NotasEncargado
						  ,@NotasInformante
						  ,@CodigoIndicador
						  ,@NombreIndicador
						  ,@CantidadFilas))AS SOURCE (IdSolicitud
											  ,IdFormulario
											  ,IdIndicador
											  ,IdDetalleRegistroIndicador
											  ,TituloHojas
											  ,NotasEncargado
											  ,NotasInformante
											  ,CodigoIndicador
											  ,NombreIndicador
											  ,CantidadFilas)  
										ON TARGET.IdSolicitud = SOURCE.IdSolicitud AND TARGET.IdFormulario = SOURCE.IdFormulario AND TARGET.IdIndicador = SOURCE.IdIndicador
										WHEN NOT MATCHED THEN
											INSERT (IdSolicitud
											  ,IdFormulario
											  ,IdIndicador
											  ,IdDetalleRegistroIndicador
											  ,TituloHojas
											  ,NotasEncargado
											  ,NotasInformante
											  ,CodigoIndicador
											  ,NombreIndicador
											  ,CantidadFilas)
											VALUES (
													 Source.IdSolicitud
													,Source.IdFormulario
													,Source.IdIndicador
													,Source.IdDetalleRegistroIndicador
													,Source.TituloHojas
													,Source.NotasEncargado
													,Source.NotasInformante
													,Source.CodigoIndicador
													,Source.NombreIndicador
													,Source.CantidadFilas)
											WHEN MATCHED THEN
											UPDATE SET 
											IdSolicitud=Source.IdSolicitud,
											IdFormulario=Source.IdFormulario,
											IdIndicador=Source.IdIndicador,
											IdDetalleRegistroIndicador  =Source.IdDetalleRegistroIndicador,
											TituloHojas=Source.TituloHojas,
											NotasEncargado=Source.NotasEncargado,
											NotasInformante=Source.NotasInformante,
											CodigoIndicador=Source.CodigoIndicador,
											NombreIndicador=Source.NombreIndicador,
											CantidadFilas=Source.CantidadFilas;
	COMMIT TRAN
	SELECT [IdSolicitud]
      ,[IdFormulario]
      ,[IdIndicador]
      ,[IdDetalleRegistroIndicador]
      ,[TituloHojas]
      ,[NotasEncargado]
	  ,[NotasInformante]
	  ,[CodigoIndicador]
	  ,[NombreIndicador]
	  ,[CantidadFilas]
  FROM SITELP.FONATEL.[DetalleRegistroIndicadorFonatel];



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
