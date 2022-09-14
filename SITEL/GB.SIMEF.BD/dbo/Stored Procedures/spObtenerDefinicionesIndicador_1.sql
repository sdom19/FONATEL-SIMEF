
CREATE procedure [dbo].[spObtenerDefinicionesIndicador]
@IdDefinicion int,
@idIndicador int,
@idEstado int
as 

begin 
declare @consulta varchar(1000)

set @consulta='select 
	isnull(b.IdDefinicion,0) IdDefinicion 
	,a.idIndicador idIndicador
	,b.Fuente
	,b.Notas
	,isnull(b.idEstado,0) idEstado
	,b.Definicion
	from Indicador a
	left join DefinicionIndicador b
	on a.idIndicador=b.idIndicador
	and b.idEstado!=4
	where a.idEstado=2 
	and a.idClasificacion not in (1,4)'+
    IIF(@IdDefinicion=0,'',' and b.IdDefinicion='+ cast(@IdDefinicion as varchar(10))+' ') +
	IIF(@idIndicador=0,'',' and a.idIndicador='+ cast(@idIndicador as varchar(10))+' ') +
	IIF(@idEstado=0,'',' and b.idEstado='+ cast(@idEstado as varchar(10))+' ');

exec(@consulta)

end