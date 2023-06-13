CREATE PROCEDURE [dbo].[pa_ObtenerRelacionCategoria]

@idRelacionCategoria INT,
@codigo VARCHAR(30),
@idCategoria INT,
@IdEstadoRegistro INT

AS
BEGIN
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la relación de categoria

	DECLARE @consulta VARCHAR(1000);

	SET @consulta='SELECT 
	 idRelacionCategoria,
	 Codigo,
	 Nombre,
	 CantidadCategoria,
	 idCategoriaDesagregacion,
	 IdEstadoRegistro, 
	 FechaCreacion, 
	 UsuarioCreacion, 
	 FechaModificacion, 
	 UsuarioModificacion, 
	 CantidadFila 
	 FROM RelacionCategoria 
	 WHERE IdEstadoRegistro!=4 ' +

	--¿Se necesita un if?
	 IIF(@idRelacionCategoria=0,'',' AND idRelacionCategoria='+ CAST(@idRelacionCategoria AS VARCHAR(10))+' ') +
	 IIF(@idCategoria=0,'',' AND idCategoriaDesagregacion='+ CAST(@idCategoria AS VARCHAR(10))+' ') +
	 IIF(@IdEstadoRegistro=0,'',' AND IdEstadoRegistro='+ CAST(@IdEstadoRegistro AS VARCHAR(10))+' ') +
	 IIF(@codigo='','',' AND UPPER(Codigo)='''+ UPPER( @codigo)+'''') 

	EXEC(@consulta)

END