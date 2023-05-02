
create procedure [dbo].[spObtenerSolicitudes]
@idSolicitud int,
@Codigo varchar(30),
@idEstado int
as 

begin 
declare @consulta varchar(1000)

set @consulta='SELECT  idSolicitud
      ,Codigo
      ,Nombre
      ,FechaInicio
      ,FechaFin
      ,IdMes
      ,IdAnno
      ,CantidadFormularios
      ,idFuente
      ,IdFrecuenciaEnvio
      ,Mensaje
      ,FechaCreacion
      ,UsuarioCreacion
      ,FechaModificacion
      ,UsuarioModificacion
      ,IdEstado
  FROM dbo.Solicitud
  where 1=1 '+
    IIF(@idSolicitud=0,'',' and idSolicitud='+ cast(@idSolicitud as varchar(10))+' ') +
	IIF(@Codigo='','',' and Codigo='''+ cast(@Codigo as varchar(10))+''' ') +
	IIF(@idEstado=0,'',' and idEstado='+ cast(@idEstado as varchar(10))+' ');

exec(@consulta)

end