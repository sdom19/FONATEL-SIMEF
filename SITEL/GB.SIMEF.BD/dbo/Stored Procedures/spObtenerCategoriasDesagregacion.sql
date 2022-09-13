CREATE procedure [dbo].[spObtenerCategoriasDesagregacion]
@idCategoria int,
@codigo varchar(30),
@idEstado int,
@idTipoCategoria int 
as 

begin 
declare @consulta varchar(1000)

set @consulta='SELECT idCategoria
      ,Codigo
      ,NombreCategoria
      ,CantidadDetalleDesagregacion
      ,idTipoDetalle
      ,IdTipoCategoria
      ,FechaCreacion
      ,UsuarioCreacion
      ,FechaModificacion
      ,UsuarioModificacion
      ,idEstado 
    FROM CategoriasDesagregacion
    where idEstado!=4 '+
    IIF(@idCategoria=0,'',' and idCategoria='+ cast(@idCategoria as varchar(10))+' ') +
	IIF(@idEstado=0,'',' and idEstado='+ cast(@idEstado as varchar(10))+' ') +
	IIF(@idTipoCategoria=0,'',' and idTipoCategoria='+ cast(@idTipoCategoria as varchar(10))+' ') +
	
	IIF(@codigo='','',' and upper(Codigo)='''+ upper( @codigo)+'''') 

exec(@consulta)

end