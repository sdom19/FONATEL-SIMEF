-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Categorias de Desagregación
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarCategoriaDesagregacion]
	   @idCategoria INT 
      ,@Codigo VARCHAR(30)
      ,@NombreCategoria VARCHAR(2000)
      ,@CantidadDetalleDesagregacion INT
      ,@idTipoDetalle INT
      ,@IdTipoCategoria INT  
      ,@UsuarioCreacion VARCHAR(100)
      ,@UsuarioModificacion VARCHAR(100)
      ,@IdEstadoRegistro INT 
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.CategoriaDesagregacion AS TARGET
			USING (VALUES( @idCategoria 
						  ,UPPER(@Codigo)
						  ,UPPER(@NombreCategoria)
						  ,@CantidadDetalleDesagregacion 
						  ,@idTipoDetalle 
					      ,@IdTipoCategoria   
						  ,UPPER(@UsuarioCreacion)
						  ,UPPER(@UsuarioModificacion)
						  ,@IdEstadoRegistro   ))AS SOURCE (idCategoria 
													,Codigo
													,NombreCategoria 
													,CantidadDetalleDesagregacion 
													,IdTipoDetalleCategoria
													,IdTipoCategoria   
													,UsuarioCreacion
													,UsuarioModificacion
													,IdEstadoRegistro )
										ON TARGET.idCategoriaDesagregacion=SOURCE.idCategoria
										AND TARGET.idCategoriaDesagregacion>0
										WHEN NOT MATCHED THEN
											INSERT ( 
													 Codigo
													,NombreCategoria 
													,CantidadDetalleDesagregacion 
													,IdTipoDetalleCategoria
													,IdTipoCategoria
													,FechaCreacion
													,UsuarioCreacion
													,IdEstadoRegistro)
											VALUES(
												 
												   SOURCE.Codigo
												  ,SOURCE.NombreCategoria 
												  ,SOURCE.CantidadDetalleDesagregacion 
												  ,SOURCE.IdTipoDetalleCategoria 
												  ,SOURCE.IdTipoCategoria 
												  ,GETDATE()
												  ,SOURCE.UsuarioCreacion
												  ,SOURCE.IdEstadoRegistro )
										WHEN MATCHED THEN
											UPDATE SET 
											NombreCategoria=UPPER(Source.NombreCategoria),
											CantidadDetalleDesagregacion=SOURCE.CantidadDetalleDesagregacion,
											IdTipoDetalleCategoria=SOURCE.IdTipoDetalleCategoria,
											IdTipoCategoria  =SOURCE.IdTipoCategoria,
											FechaModificacion=GETDATE(),
											UsuarioModificacion=UPPER(SOURCE.UsuarioModificacion),
											IdEstadoRegistro=SOURCE.IdEstadoRegistro;
	COMMIT TRAN


	IF(@idCategoria=0)
	BEGIN
		SET @idCategoria=SCOPE_IDENTITY();
		SET IDENTITY_INSERT dbo.DetalleCategoriaTexto ON;
		INSERT INTO DetalleCategoriaTexto (IdCategoriaDesagregacion,idDetalleCategoriaTexto,Codigo,Etiqueta,Estado) 
		SELECT IdCategoriaDesagregacion,0, 0 Codigo,'N/A',0 estado FROM CategoriaDesagregacion
		WHERE IdCategoriaDesagregacion=@idCategoria;
		SET IDENTITY_INSERT dbo.DetalleCategoriaTexto OFF;
	END 

	IF(@IdEstadoRegistro=4 OR @IdEstadoRegistro=3)
	BEGIN
		UPDATE RelacionCategoria SET IdEstadoRegistro=4 FROM DetalleRelacionCategoria 
		WHERE RelacionCategoria.IdRelacionCategoria=DetalleRelacionCategoria.IdRelacionCategoria
		AND (DetalleRelacionCategoria.IdCategoriaDesagregacion=@idCategoria) OR (RelacionCategoria.IdCategoriaDesagregacion=@idCategoria)
		
		UPDATE RelacionCategoria SET IdEstadoRegistro=4  
		WHERE RelacionCategoria.IdCategoriaDesagregacion=@idCategoria

		UPDATE Indicador SET IdEstadoRegistro=3 FROM DetalleIndicadorCategoria
		WHERE Indicador.idIndicador=DetalleIndicadorCategoria.IdIndicador
		AND DetalleIndicadorCategoria.IdCategoriaDesagregacion=@idCategoria

		UPDATE FormularioWeb SET IdEstadoRegistro=3 FROM DetalleFormularioWeb, DetalleIndicadorCategoria,Indicador
		WHERE FormularioWeb.IdFormularioWeb = DetalleFormularioWeb.IdFormularioWeb
		AND DetalleFormularioWeb.IdIndicador = Indicador.IdIndicador
		AND Indicador.idIndicador=DetalleIndicadorCategoria.IdIndicador
		AND DetalleIndicadorCategoria.IdCategoriaDesagregacion=@idCategoria;

		UPDATE ReglaValidacion SET IdEstadoRegistro=3 FROM DetalleIndicadorCategoria
		WHERE DetalleIndicadorCategoria.IdIndicador=ReglaValidacion.IdIndicador
		AND DetalleIndicadorCategoria.IdCategoriaDesagregacion=@idCategoria

		UPDATE FormulaCalculo SET IdEstadoRegistro=3 FROM DetalleIndicadorCategoria
		WHERE DetalleIndicadorCategoria.IdIndicador=FormulaCalculo.IdIndicador
		AND DetalleIndicadorCategoria.IdCategoriaDesagregacion=@idCategoria

	END 


	SELECT TOP (1000) IdCategoriaDesagregacion
      ,[Codigo]
      ,[NombreCategoria]
      ,[CantidadDetalleDesagregacion]
      ,[IdTipoDetalleCategoria]
      ,[IdTipoCategoria]
      ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[IdEstadoRegistro]
  FROM [CategoriaDesagregacion]
  WHERE IdCategoriaDesagregacion=@idCategoria
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