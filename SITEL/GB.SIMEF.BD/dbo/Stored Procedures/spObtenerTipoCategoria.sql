


create procedure spObtenerTipoCategoria

@idTipoCategoria int
as
declare @consulta varchar(150);
set @consulta=
'select '+
	'idTipoCategoria,'+
	'Nombre,'+
	'Estado'+
 ' from TipoCategoria'+
 ' where estado=1'+
 IIF(@idTipoCategoria=0,'',' and idTipoCategoria='+ cast(@idTipoCategoria as varchar(10))+' ') ;
 exec(@consulta)