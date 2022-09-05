
--?

CREATE PROCEDURE [dbo].[spObtenerRelacionCategorias]
-- Cuales parametros van aca? Se necesitan para obtener datos?
@idRelacionCategoria int,
@idCategoria int,
@idEstado int

AS
BEGIN
	declare @consulta varchar(1000);

	set @consulta='SELECT '+
	'idRelacionCategoria,'+
	'Codigo,'+
	'Nombre,'+
	'CantidadCategoria,'+
	'idCategoria,'+
	'idCategoriaValor, '+
	'idEstado, '+
	'FechaCreacion, '+
	'UsuarioCreacion, '+
	'FechaModificacion, '+
	'UsuarioModificacion '+
	'from RelacionCategoria '+
	'where idEstado!=4 ' +

	--¿Se necesita un if?
	 IIF(@idRelacionCategoria=0,'',' and idRelacionCategoria='+ cast(@idRelacionCategoria as varchar(10))+' ') +
	 IIF(@idCategoria=0,'',' and idCategoria='+ cast(@idCategoria as varchar(10))+' ') +
	 IIF(@idEstado=0,'',' and idEstado='+ cast(@idEstado as varchar(10))+' ');
	exec(@consulta)

END