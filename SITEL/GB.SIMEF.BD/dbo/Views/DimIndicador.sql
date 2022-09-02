
CREATE view [dbo].[DimIndicador]
as
select 
Indicador.idIndicador id,
Indicador.Codigo Codigo,
Indicador.Nombre Nombre,
GrupoIndicadores.Nombre Grupo,
ClasificacionIndicadores.Nombre Clasificacion,
UnidadEstudio.Nombre UnidadEstudio,
Case Indicador.Solicitud when 1 then 'SI' else 'NO' end Solicitud
--DetalleIndicadorVariables.NombreVariable Variable,
--CategoriasDesagregacion.NombreCategoria Atributo,
--DetalleRelacionCategoria.CategoriaAtributoValor AtributoValor

from Indicador

inner join TipoIndicadores 
on TipoIndicadores.IdTipoIdicador=Indicador.idTipoMedida

inner join ClasificacionIndicadores 
on ClasificacionIndicadores.idClasificacion=Indicador.IdClasificacion

inner join GrupoIndicadores 
on GrupoIndicadores.idGrupo=Indicador.idGrupo


inner join UnidadEstudio 
on UnidadEstudio.idUnidad=Indicador.IdUnidadEstudio


inner join DetalleIndicadorVariables
on Indicador.idIndicador= DetalleIndicadorVariables.idIndicador

--inner join DetalleIndicadorCategoria
--on Indicador.idIndicador= DetalleIndicadorCategoria.idIndicador

--inner join DetalleRelacionCategoria
--on DetalleIndicadorCategoria.idCategoriaRelacion=DetalleRelacionCategoria.IdRelacionCategoria
--and DetalleIndicadorCategoria.idDetalleRelacion=DetalleRelacionCategoria.idDetalleRelacionCategoria

--inner join RelacionCategoria 
--on RelacionCategoria.idRelacionCategoria=DetalleRelacionCategoria.IdRelacionCategoria

--inner join CategoriasDesagregacion
--on CategoriasDesagregacion.idCategoria=RelacionCategoria.idCategoria