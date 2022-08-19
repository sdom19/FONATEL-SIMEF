

CREATE view [dbo].[calendar]
as
select concat(b.idMes, a.idAnno) id, b.Nombre mes, a.Nombre anno, concat(b.Nombre,' ', a.Nombre)MesAño 

from anno a, mes b