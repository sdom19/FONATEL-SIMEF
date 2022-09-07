



Create procedure [dbo].[spValidarFormulario]

@idFormulario int

as

WITH VALIDACION_CTE 
AS
(
select 'Solicitudes de Información' Nombre, STRING_AGG(Nombre,', ') Listado

from Solicitud a
inner join DetalleSolicitudFormulario b
on a.IdSolicitud=b.IdSolicitud
where a.idEstado!=4 and b.IdFormulario=@idFormulario

)

select Nombre+': '+Listado lista from VALIDACION_CTE where Listado is not null