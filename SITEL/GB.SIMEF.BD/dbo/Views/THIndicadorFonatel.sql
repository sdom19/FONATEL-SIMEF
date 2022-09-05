CREATE view THIndicadorFonatel
as
select 
a.idIndicador,
a.idGrupo,
a.idTipoMedida,
Concat(j.IdMes,j.IdAnno) idPeriodo,
j.idFuente,
a.IdFrecuencia,
a.IdTipoIndicador,
a.IdClasificacion,
a.IdUnidadEstudio,
a.idEstado,
b.idCantegoria idVariable,
c.idCategoria IdCategoria,
d.idFormulario,
f.idsolicitud,
0 Valor

from Indicador a
inner join DetalleIndicadorVariable b
on a.idindicador=b.idindicador 
inner join DetalleIndicadorCategoria c
on c.idIndicador=a.idIndicador
inner join DetalleFormularioWeb d
on d.idIndicador=a.idIndicador
inner join DetalleSolicitudFormulario f
on f.IdFormulario=d.idFormulario
inner join Solicitud j
on j.idSolicitud=f.idsolicitud