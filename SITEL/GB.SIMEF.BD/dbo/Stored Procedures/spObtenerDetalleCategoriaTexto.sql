
CREATE procedure [dbo].[spObtenerDetalleCategoriaTexto]
@idCategoriaDetalle int,
@idCategoria int,
@codigo int,
@Etiqueta varchar(300)
as 

begin 
declare @consulta varchar(2000)

set @consulta='SELECT idCategoriaDetalle'+
      ',idCategoria'+
      ',Codigo'+
      ',Etiqueta'+
      ',Estado'+
  ' FROM DetalleCategoriaTexto '+
  ' where Estado=1 '+
    IIF(@idCategoria=0,'',' and idCategoria='+ cast(@idCategoria as varchar(10))+' ') +
	IIF(@idCategoriaDetalle=0,'',' and idCategoriaDetalle='+ cast(@idCategoriaDetalle as varchar(10))+' ') +
	IIF(@codigo=0,'',' and Codigo='+ cast(@codigo as varchar(10))+'') +
	IIF(@Etiqueta='','',' and upper(Etiqueta)='''+@Etiqueta+'''') +
    ' order by Codigo'
exec(@consulta)

end