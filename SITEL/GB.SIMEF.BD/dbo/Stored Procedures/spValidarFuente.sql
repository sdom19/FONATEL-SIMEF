



CREATE procedure [dbo].[spValidarFuente]

@idFuente int

as

WITH VALIDACION_CTE 
AS
(
select 'Solicitudes de Información' Nombre, STRING_AGG(Nombre,', ') Listado

from Solicitud
where idEstado!=4 and idFuente=@idFuente

)

select Nombre+': '+Listado lista from VALIDACION_CTE where Listado is not null