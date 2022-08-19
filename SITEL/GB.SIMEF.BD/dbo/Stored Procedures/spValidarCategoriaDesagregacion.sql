



CREATE procedure [dbo].[spValidarCategoriaDesagregacion]

@idCategoria int

as

WITH VALIDACION_CTE 
AS
(
select 'Relación Categoría ID' Nombre, STRING_AGG(Nombre,', ') Listado

from RelacionCategoria
where idEstado!=4 and idCategoria=@idCategoria

union 
select 'Relación Categoría Atributo' Nombre, STRING_AGG(a.Nombre,', ') Listado

from RelacionCategoria a
inner join DetalleRelacionCategoria b 
on a.idRelacionCategoria=b.IdRelacionCategoria

where idEstado!=4 and b.idCategoriaAtributo=@idCategoria

union 

select 'Indicador Variable' Nombre, STRING_AGG(a.Nombre,', ') Listado  from Indicador a
inner join DetalleIndicadorVariables b
on a.idIndicador=b.idIndicador

where idEstado!=4 and b.idCategoria=@idCategoria
union
select 'Indicador Categoría' Nombre, STRING_AGG(a.Nombre,', ') Listado  from Indicador a
inner join DetalleIndicadorCategoria b
on a.idIndicador=b.idIndicador

where idEstado!=4 and b.idCategoriaRelacion=@idCategoria
)

select Nombre+': '+Listado lista from VALIDACION_CTE where Listado is not null