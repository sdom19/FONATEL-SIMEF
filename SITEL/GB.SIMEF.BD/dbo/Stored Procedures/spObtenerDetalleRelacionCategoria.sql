
create procedure [dbo].[spObtenerDetalleRelacionCategoria]
@idDetalleRelacionCategoria int,
@IdRelacionCategoria int,
@idCategoriaAtributo int,
@CategoriaAtributoValor varchar(300)

as 

begin 
declare @consulta varchar(2000)

set @consulta='SELECT idDetalleRelacionCategoria'+
      ',IdRelacionCategoria'+
      ',idCategoriaAtributo'+
      ',CategoriaAtributoValor'+
      ',Estado'+
  ' FROM DetalleRelacionCategoria '+
  ' where Estado=1 '+
    IIF(@IdRelacionCategoria=0,'',' and IdRelacionCategoria='+ cast(@IdRelacionCategoria as varchar(10))+' ') +
	IIF(@idDetalleRelacionCategoria=0,'',' and idDetalleRelacionCategoria='+ cast(@idDetalleRelacionCategoria as varchar(10))+' ') +
	IIF(@idCategoriaAtributo=0,'',' and idCategoriaAtributo='+ cast(@idCategoriaAtributo as varchar(10))+'') +
	IIF(@CategoriaAtributoValor='','',' and upper(CategoriaAtributoValor)='''+@CategoriaAtributoValor+'''') +
    ' order by idCategoriaAtributo'
exec(@consulta)

end