-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Carga información SIGITEL
-- ============================================
CREATE PROCEDURE [dbo].[pa_CargaInformacionSigitel]
AS

SELECT DISTINCT a.IdIndicador,
m.Codigo CodigoIndicador,
UPPER(a.NombreIndicador) NombreIndicador,
CONCAT(c.IdCategoriaDesagregacion,0) IdColumna,
c.NombreCategoria NombreColumna,
CAST(b.Valor AS VARCHAR(1000)) ValorColumna,
b.NumeroFila,
f.IdMes,
f.Nombre Mes,
g.IdAnno,
g.Nombre Anno,
i.IdSolicitud,
n.IdGrupoIndicador,
n.Nombre NombreGrupo,
o.IdClasificacionIndicador,
o.Nombre NombreClasificacion,
i.Codigo CodigoSolicitud,
i.Nombre NombreSolicitud,
j.Fuente NombreFuente,
k.IdFormularioWeb,
CAST(k.Codigo AS VARCHAR(30)) CodigoFormulario,
CAST(k.Nombre AS VARCHAR(300)) NombreFormulario,
CONCAT(g.Nombre,'-',f.Nombre) AnnoMes,
0 VariableDato,
m.VisualizaSigitel EstadoIndicador,

0  IdVariable,
b.IdCategoriaDesagregacion IdCategoriaDesagregacion
FROM IndicadorResultado a
INNER JOIN IndicardorResultadoDetalleCategoria b
ON a.IdIndicadorResultado=b.IdIndicadorResultado
INNER JOIN Indicador m
ON m.IdIndicador=a.IdIndicador
INNER JOIN GrupoIndicador n
ON n.IdGrupoIndicador=m.IdGrupoIndicador
INNER JOIN ClasificacionIndicador o
ON o.IdClasificacionIndicador=m.IdClasificacionIndicador
INNER JOIN CategoriaDesagregacion c
ON b.IdCategoriaDesagregacion=c.IdCategoriaDesagregacion
INNER JOIN Mes f
ON f.IdMes=a.idMes
INNER JOIN Anno g
ON g.IdAnno=a.IdAnno
INNER JOIN IndicadorResultadoSolicitud h
ON h.IdIndicadorResultado=a.IdIndicadorResultado
INNER JOIN Solicitud i
ON i.IdSolicitud=h.IdSolicitud
INNER JOIN FuenteRegistro j
ON i.IdFuenteRegistro=j.IdFuenteRegistro
INNER JOIN FormularioWeb k
ON k.IdFormularioWeb=h.IdFormularioWEb
  

UNION

SELECT DISTINCT a.IdIndicador,
m.Codigo CodigoIndicador,
UPPER(a.NombreIndicador) NombreIndicador,
CONCAT(ROW_NUMBER ()   
    OVER (PARTITION BY a.IdIndicador,d.IdDetalleIndicadorVariable ORDER BY d.NombreVariable) ,1)IdColumna,
d.NombreVariable NombreColumna,
CAST(d.Valor AS VARCHAR(1000)) ValorColumna,
d.NumeroFila,
f.IdMes,
f.Nombre Mes,
g.IdAnno,
g.Nombre Anno,
i.IdSolicitud,
n.IdGrupoIndicador,
n.Nombre NombreGrupo,
o.IdClasificacionIndicador,
o.Nombre NombreClasificacion,
i.Codigo CodigoSolicitud,
i.Nombre NombreSolicitud,
j.Fuente NombreFuente,
k.IdFormularioWeb,
CAST(k.Codigo AS VARCHAR(30)) CodigoFormulario,
CAST(k.Nombre AS VARCHAR(300)) NombreFormulario,
CONCAT(g.Nombre,'-',f.Nombre) AnnoMes,
1 VariableDato,
m.VisualizaSigitel EstadoIndicador,
d.IdDetalleIndicadorVariable IdVariable,
0 IdCategoriaDesagregacion

FROM IndicadorResultado a
INNER JOIN Indicador m
ON m.IdIndicador=a.IdIndicador
INNER JOIN GrupoIndicador n
ON n.IdGrupoIndicador=m.IdGrupoIndicador
INNER JOIN ClasificacionIndicador o
ON o.IdClasificacionIndicador=m.IdClasificacionIndicador
INNER JOIN IndicadorResultadoDetalleVariable d
ON d.IdIndicadorResultado=a.IdIndicadorResultado
INNER JOIN Mes f
ON f.IdMes=a.idMes
INNER JOIN Anno g
ON g.IdAnno=a.IdAnno
INNER JOIN IndicadorResultadoSolicitud h
ON h.IdIndicadorResultado=a.IdIndicadorResultado
INNER JOIN Solicitud i
ON i.IdSolicitud=h.IdSolicitud
INNER JOIN FuenteRegistro j
ON i.IdFuenteRegistro=j.IdFuenteRegistro
INNER JOIN FormularioWeb k
ON k.IdFormularioWeb=h.IdFormularioWeb