CREATE PROCEDURE [dbo].[spObtenerFormulariosWeb]
	@idFormulario int,
	@idEstado int,
	@codigo varchar(30)
AS
BEGIN
	declare @consulta varchar(1000)

	SET @consulta='SELECT idFormulario
		  ,Codigo
		  ,Nombre
		  ,Descripcion
		  ,CantidadIndicadores
		  ,idFrecuencia
		  ,FechaCreacion
		  ,UsuarioCreacion
		  ,FechaModificacion
		  ,UsuarioModificacion
		  ,IdEstado
	  FROM FormularioWeb
	  where IdEstado!=0'+
	  IIF(@idFormulario=0,'',' and idFormulario='+ cast(@idFormulario as varchar(10))+' ') +
	  IIF(@idEstado=0,'',' and idEstado='+ cast(@idEstado as varchar(10))+' ') +	
	  IIF(@codigo='','',' and upper(Codigo)='''+ upper( @codigo)+'''') 

exec(@consulta)
END