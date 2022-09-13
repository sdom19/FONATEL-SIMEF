
CREATE procedure [dbo].[spObtenerFuentesRegistros]
@idFuentesRegistro int,
@idEstado int
as 

begin 
declare @consulta varchar(1000)

set @consulta='SELECT idFuente
      ,Fuente
      ,CantidadDestinatario
      ,FechaCreacion
      ,UsuarioCreacion
      ,FechaModificacion
      ,UsuarioModificacion
      ,idEstado
  FROM dbo.FuentesRegistro'+
  ' where idEstado!=4 '+
    IIF(@idFuentesRegistro=0,'',' and idFuente='+ cast(@idFuentesRegistro as varchar(10))+' ') +
	IIF(@idEstado=0,'',' and idEstado='+ cast(@idEstado as varchar(10))+' ');

exec(@consulta)

end