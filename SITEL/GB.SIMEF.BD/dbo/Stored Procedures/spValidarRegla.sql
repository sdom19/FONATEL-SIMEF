



CREATE procedure [dbo].[spValidarRegla]

@idRegla int

as

WITH VALIDACION_CTE 
AS
(
select 'Indicadores' Nombre, STRING_AGG(b.Nombre,', ') Listado

from ReglaValidacion a
inner join Indicador b 
on a.IdIndicador=b.IdIndicador 
and b.IdEstado!=4
where a.idEstado!=4 and a.IdRegla=@idRegla

)

select Nombre+': '+Listado lista from VALIDACION_CTE where Listado is not null