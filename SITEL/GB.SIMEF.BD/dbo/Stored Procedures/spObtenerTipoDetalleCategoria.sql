


CREATE procedure [dbo].[spObtenerTipoDetalleCategoria]

@idTipoCategoria int
as
declare @consulta varchar(150);
set @consulta=
 'SELECT  idTipoCategoria '+
      ',Nombre '+
      ',Estado '+
' FROM dbo.TipoDetalleCategoria '+
' where estado=1 '+
 IIF(@idTipoCategoria=0,'',' and idTipoCategoria='+ cast(@idTipoCategoria as varchar(10))+' ') ;
 exec(@consulta)