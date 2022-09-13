
CREATE procedure [dbo].[spActualizarIndicadorFonatel]
	   @pIdIndicador int
		,@pCodigo varchar(30)
		,@pNombre varchar(350)
		,@pIdTipoIndicador int
		,@pIdClasificacion int
		,@pIdGrupo int
		,@pDescripcion varchar(2000)
		,@pCantidadVariableDato int
		,@pCantidadCategoriasDesagregacion int
		,@pIdUnidadEstudio int
		,@pIdTipoMedida int
		,@pIdFrecuencia int
		,@pInterno bit
		,@pSolicitud bit
		,@pFuente varchar(300)
		,@pNotas varchar(3000)
		--,@pFechaCreacion datetime
		,@pUsuarioCreacion varchar(100)
		--,@pFechaModificacion datetime
		,@pUsuarioModificacion varchar(100)
		,@pVisualizaSigitel bit
		,@pIdEstado int
as

BEGIN TRY
	BEGIN TRAN;
		MERGE dbo.Indicador AS TARGET
			USING (VALUES( 
						   @pIdIndicador
							,@pCodigo
							,@pNombre
							,@pIdTipoIndicador
							,@pIdClasificacion
							,@pIdGrupo
							,@pDescripcion
							,@pCantidadVariableDato
							,@pCantidadCategoriasDesagregacion 
							,@pIdUnidadEstudio
							,@pIdTipoMedida
							,@pIdFrecuencia
							,@pInterno
							,@pSolicitud
							,@pFuente
							,@pNotas
							--,@pFechaCreacion
							,@pUsuarioCreacion
							--,@pFechaModificacion
							,@pUsuarioModificacion
							,@pVisualizaSigitel
							,@pIdEstado
						  ))AS SOURCE (
											idIndicador
											  ,Codigo
											  ,Nombre
											  ,IdTipoIndicador
											  ,IdClasificacion
											  ,idGrupo
											  ,Descripcion
											  ,CantidadVariableDato
											  ,CantidadCategoriasDesagregacion
											  ,IdUnidadEstudio
											  ,idTipoMedida
											  ,IdFrecuencia
											  ,Interno
											  ,Solicitud
											  ,Fuente
											  ,Notas
											  --,FechaCreacion
											  ,UsuarioCreacion
											  --,FechaModificacion
											  ,UsuarioModificacion
											  ,VisualizaSigitel
											  ,idEstado
												)
										ON TARGET.idIndicador = SOURCE.idIndicador 
										WHEN NOT MATCHED THEN
											INSERT ( 
											  Codigo
											  ,Nombre
											  ,IdTipoIndicador
											  ,IdClasificacion
											  ,idGrupo
											  ,Descripcion
											  ,CantidadVariableDato
											  ,CantidadCategoriasDesagregacion
											  ,IdUnidadEstudio
											  ,idTipoMedida
											  ,IdFrecuencia
											  ,Interno
											  ,Solicitud
											  ,Fuente
											  ,Notas
											  ,FechaCreacion
											  ,UsuarioCreacion
											  --,FechaModificacion
											  ,UsuarioModificacion
											  ,VisualizaSigitel
											  ,idEstado
													)
											VALUES (
													SOURCE.Codigo
													,SOURCE.Nombre
													,SOURCE.IdTipoIndicador
													,SOURCE.IdClasificacion
													,SOURCE.idGrupo
													,SOURCE.Descripcion
													,SOURCE.CantidadVariableDato
													,SOURCE.CantidadCategoriasDesagregacion
													,SOURCE.IdUnidadEstudio
													,SOURCE.idTipoMedida
													,SOURCE.IdFrecuencia
													,SOURCE.Interno
													,SOURCE.Solicitud
													,SOURCE.Fuente
													,SOURCE.Notas
													,getdate()
													,SOURCE.UsuarioCreacion
													--,SOURCE.FechaModificacion
													,SOURCE.UsuarioModificacion
													,SOURCE.VisualizaSigitel
													,SOURCE.idEstado
													)
										WHEN MATCHED THEN
											UPDATE SET 
											Codigo = SOURCE.Codigo
											,Nombre = SOURCE.Nombre
											,IdTipoIndicador = SOURCE.IdTipoIndicador
											,IdClasificacion = SOURCE.IdClasificacion
											,idGrupo = SOURCE.idGrupo
											,Descripcion = SOURCE.Descripcion
											,CantidadVariableDato = SOURCE.CantidadVariableDato
											,CantidadCategoriasDesagregacion = SOURCE.CantidadCategoriasDesagregacion
											,IdUnidadEstudio = SOURCE.IdUnidadEstudio
											,idTipoMedida = SOURCE.idTipoMedida
											,IdFrecuencia = SOURCE.IdFrecuencia
											,Interno = SOURCE.Interno
											,Solicitud = SOURCE.Solicitud
											,Fuente = SOURCE.Fuente
											,Notas = SOURCE.Notas
											--,FechaCreacion = SOURCE.FechaCreacion
											,UsuarioCreacion = SOURCE.UsuarioCreacion
											,FechaModificacion = getdate()
											,UsuarioModificacion = SOURCE.UsuarioModificacion
											,VisualizaSigitel = SOURCE.VisualizaSigitel
											,idEstado = SOURCE.idEstado;
	COMMIT TRAN

	SELECT top 1
		[idIndicador]
      ,[Codigo]
      ,[Nombre]
      ,[IdTipoIndicador]
      ,[IdClasificacion]
      ,[idGrupo]
      ,[Descripcion]
      ,[CantidadVariableDato]
      ,[CantidadCategoriasDesagregacion]
      ,[IdUnidadEstudio]
      ,[idTipoMedida]
      ,[IdFrecuencia]
      ,[Interno]
      ,[Solicitud]
      ,[Fuente]
      ,[Notas]
      ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[VisualizaSigitel]
      ,[idEstado]
	FROM [dbo].[Indicador]


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