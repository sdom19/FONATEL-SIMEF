
CREATE procedure [dbo].[spObtenerFuentesRegistrosDestinatarios]
@idDetalleFuente int,
@idFuentesRegistro int
as 

begin 
declare @consulta varchar(1000)

set @consulta='SELECT [idDetalleFuente]
      ,[idFuente]
      ,[NombreDestinatario]
      ,[CorreoElectronico]
      ,[Estado]
  FROM [dbo].[DetalleFuentesRegistro]'+
  ' where Estado=1 '+
    IIF(@idFuentesRegistro=0,'',' and idFuente='+ cast(@idFuentesRegistro as varchar(10))+' ')+
	IIF(@idDetalleFuente =0,'',' and idDetalleFuente='+ cast(@idDetalleFuente  as varchar(10))+' ');

exec(@consulta)

end