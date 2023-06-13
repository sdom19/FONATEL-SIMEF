CREATE PROCEDURE [dbo].[pa_ObtenerFormularioWeb]
	@idFormulario INT,
	@IdEstadoRegistro INT,
	@codigo VARCHAR(30),
	@idfrecuenciaEnvio INT
AS
BEGIN
-- =============================================
-- Author:		Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el formulario web 
	DECLARE @consulta VARCHAR(1000)

	SET @consulta='SELECT idFormularioWeb
		  ,Codigo
		  ,Nombre
		  ,Descripcion
		  ,CantidadIndicador
		  ,idFrecuenciaEnvio
		  ,FechaCreacion
		  ,UsuarioCreacion
		  ,FechaModificacion
		  ,UsuarioModificacion
		  ,IdEstadoRegistro
	  FROM FormularioWeb
	  WHERE IdEstadoRegistro!=4'+
	  IIF(@idFormulario=0,'',' AND idFormularioweb='+ CAST(@idFormulario AS VARCHAR(10))+' ') +
	  IIF(@IdEstadoRegistro=0,'',' AND IdEstadoRegistro='+ CAST(@IdEstadoRegistro AS VARCHAR(10))+' ')+
	 -- IIF(@idfrecuenciaEnvio=0,'',' AND idfrecuenciaEnvio='+ CAST(@idfrecuenciaEnvio AS VARCHAR(10))+' ') +
	  IIF(@codigo='','',' AND UPPER(Codigo)='''+ UPPER( @codigo)+'''') 

EXEC(@consulta)
END