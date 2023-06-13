-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Indicador FONATEL
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarIndicadorFonatel]
	   @pIdIndicador INT
		,@pCodigo VARCHAR(30)
		,@pNombre VARCHAR(350)
		,@pIdTipoIndicador INT
		,@pIdClasificacionIndicador INT
		,@pIdGraficoInforme INT
		,@pIdGrupoIndicador INT
		,@pDescripcion VARCHAR(2000)
		,@pCantidadVariableDato INT
		,@pCantidadCategoriaDesagregacion INT
		,@pIdUnidadEstudio INT
		,@pIdTipoMedida INT
		,@pIdFrecuencia INT
		,@pInterno BIT
		,@pSolicitud BIT
		,@pFuente VARCHAR(300)
		,@pNota VARCHAR(3000)
		--,@pFechaCreacion datetime
		,@pUsuarioCreacion VARCHAR(100)
		--,@pFechaModificacion datetime
		,@pUsuarioModificacion VARCHAR(100)
		,@pVisualizaSigitel BIT
		,@pIdEstadoRegistro INT
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.Indicador AS TARGET
			USING (VALUES( 
						   @pIdIndicador
							,@pCodigo
							,@pNombre
							,@pIdTipoIndicador
							,@pIdClasificacionIndicador
							,@pIdGraficoInforme
							,@pIdGrupoIndicador
							,@pDescripcion
							,@pCantidadVariableDato
							,@pCantidadCategoriaDesagregacion 
							,@pIdUnidadEstudio
							,@pIdTipoMedida
							,@pIdFrecuencia
							,@pInterno
							,@pSolicitud
							,@pFuente
							,@pNota
							--,@pFechaCreacion
							,@pUsuarioCreacion
							--,@pFechaModificacion
							,@pUsuarioModificacion
							,@pVisualizaSigitel
							,@pIdEstadoRegistro
						  ))AS SOURCE (
											idIndicador
											  ,Codigo
											  ,Nombre
											  ,IdTipoIndicador
											  ,IdClasificacionIndicador
											  ,IdGraficoInforme
											  ,idGrupoIndicador
											  ,Descripcion
											  ,CantidadVariableDato
											  ,CantidadCategoriaDesagregacion
											  ,IdUnidadEstudio
											  ,idTipoMedida
											  ,IdFrecuencia
											  ,Interno
											  ,Solicitud
											  ,Fuente
											  ,Nota
											  --,FechaCreacion
											  ,UsuarioCreacion
											  --,FechaModificacion
											  ,UsuarioModificacion
											  ,VisualizaSigitel
											  ,IdEstadoRegistro
												)
										ON TARGET.idIndicador = SOURCE.idIndicador 
										AND TARGET.idIndicador >0
										WHEN NOT MATCHED THEN
											INSERT ( 
											  Codigo
											  ,Nombre
											  ,IdTipoIndicador
											  ,IdClasificacionIndicador
											  ,IdGraficoInforme
											  ,idGrupoIndicador
											  ,Descripcion
											  ,CantidadVariableDato
											  ,CantidadCategoriaDesagregacion
											  ,IdUnidadEstudio
											  ,idTipoMedida
											  ,IdFrecuenciaEnvio
											  ,Interno
											  ,Solicitud
											  ,Fuente
											  ,Nota
											  ,FechaCreacion
											  ,UsuarioCreacion
											  --,FechaModificacion
											  ,UsuarioModificacion
											  ,VisualizaSigitel
											  ,IdEstadoRegistro
													)
											VALUES (
													SOURCE.Codigo
													,SOURCE.Nombre
													,SOURCE.IdTipoIndicador
													,SOURCE.IdClasificacionIndicador
													,SOURCE.IdGraficoInforme
													,SOURCE.idGrupoIndicador
													,SOURCE.Descripcion
													,SOURCE.CantidadVariableDato
													,SOURCE.CantidadCategoriaDesagregacion
													,SOURCE.IdUnidadEstudio
													,SOURCE.idTipoMedida
													,SOURCE.IdFrecuencia
													,SOURCE.Interno
													,SOURCE.Solicitud
													,SOURCE.Fuente
													,SOURCE.Nota
													,GETDATE()
													,SOURCE.UsuarioCreacion
													--,SOURCE.FechaModificacion
													,SOURCE.UsuarioModificacion
													,SOURCE.VisualizaSigitel
													,SOURCE.IdEstadoRegistro
													)
										WHEN MATCHED THEN
											UPDATE SET 
											Codigo = SOURCE.Codigo
											,Nombre = SOURCE.Nombre
											,IdTipoIndicador = SOURCE.IdTipoIndicador
											,IdClasificacionIndicador = SOURCE.IdClasificacionIndicador
											,IdGraficoInforme = SOURCE.IdGraficoInforme
											,idGrupoIndicador = SOURCE.idGrupoIndicador
											,Descripcion = SOURCE.Descripcion
											,CantidadVariableDato = SOURCE.CantidadVariableDato
											,CantidadCategoriaDesagregacion = SOURCE.CantidadCategoriaDesagregacion
											,IdUnidadEstudio = SOURCE.IdUnidadEstudio
											,idTipoMedida = SOURCE.idTipoMedida
											,IdFrecuenciaEnvio = SOURCE.IdFrecuencia
											,Interno = SOURCE.Interno
											,Solicitud = SOURCE.Solicitud
											,Fuente = SOURCE.Fuente
											,Nota = SOURCE.Nota
											--,FechaCreacion = SOURCE.FechaCreacion
											--,UsuarioCreacion = SOURCE.UsuarioCreacion
											,FechaModificacion = GETDATE()
											,UsuarioModificacion = SOURCE.UsuarioModificacion
											,VisualizaSigitel = SOURCE.VisualizaSigitel
											,IdEstadoRegistro = SOURCE.IdEstadoRegistro;
	COMMIT TRAN
  	IF(@pIdIndicador=0)
	BEGIN
		SET @pIdIndicador=SCOPE_IDENTITY();
		SET IDENTITY_INSERT dbo.DetalleIndicadorVariable ON;
		INSERT INTO DetalleIndicadorVariable (IdDetalleIndicadorVariable, IdIndicador,NombreVariable,Descripcion, Estado) VALUES(0,@pIdIndicador,'No definida','No definida',0);
		SET IDENTITY_INSERT dbo.DetalleIndicadorVariable OFF;
	END

	IF (@pIdEstadoRegistro = 3 OR @pIdEstadoRegistro = 4)
	BEGIN
		UPDATE FormularioWeb 
		SET IdEstadoRegistro = 3, UsuarioModificacion = @pUsuarioModificacion
		FROM DetalleFormularioWeb 
		WHERE FormularioWeb.IdFormularioWeb = DetalleFormularioWeb.IdFormularioWeb
		AND DetalleFormularioWeb.IdIndicador = @pIdIndicador
		AND IdEstadoRegistro != 4
		
		UPDATE Solicitud 
		SET IdEstadoRegistro = 3, UsuarioModificacion = @pUsuarioModificacion
		FROM DetalleSolicitudFormulario,	
		DetalleFormularioWeb 
		WHERE DetalleFormularioWeb.IdFormularioWeb=DetalleSolicitudFormulario.IdFormularioWeb AND
		DetalleFormularioWeb.IdIndicador=@pIdIndicador

		UPDATE FormulaCalculo 
		SET IdEstadoRegistro = 3, UsuarioModificacion = @pUsuarioModificacion
		WHERE IdIndicador = @pIdIndicador and IdEstadoRegistro != 4

		IF (@pIdEstadoRegistro = 3)
		BEGIN
			UPDATE ReglaValidacion 
			SET IdEstadoRegistro = 3, UsuarioModificacion = @pUsuarioModificacion
			WHERE IdIndicador = @pIdIndicador and IdEstadoRegistro != 4
		END
	END

	SELECT TOP 1
		[idIndicador]
      ,[Codigo]
      ,[Nombre]
      ,[IdTipoIndicador]
      ,[IdClasificacionIndicador]
	  ,[IdGraficoInforme]
      ,[idGrupoIndicador]
      ,[Descripcion]
      ,[CantidadVariableDato]
      ,[CantidadCategoriaDesagregacion]
      ,[IdUnidadEstudio]
      ,[idTipoMedida]
      ,[IdFrecuenciaEnvio]
      ,[Interno]
      ,[Solicitud]
      ,[Fuente]
      ,[Nota]
      ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[VisualizaSigitel]
      ,[IdEstadoRegistro]
	FROM [dbo].[Indicador]
	WHERE [idIndicador] = @pIdIndicador

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