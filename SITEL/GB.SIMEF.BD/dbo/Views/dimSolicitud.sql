
create view dimSolicitud

as

select a.idSolicitud,
	   a.Codigo,
	   a.Nombre Solicitud,
	   b.Fuente,
	   c.Nombre Mes,
	   d.Nombre Anno
--into DWSIMEF.dbo.DimSolicitud
from Solicitud a

inner join FuentesRegistro b
on b.idFuente=a.idFuente
inner join Mes c
on c.idMes=a.IdMes
inner join Anno d
on d.idAnno=a.IdAnno