
CREATE PROCEDURE [SITEL].[pa_ActualizarRegistroIndicadorDetalleCategoria]
AS
BEGIN
-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 27-03-2023
-- Description:	Procedimiento utilizado en el ETL para pasar la estructura de [Formulario categorias] en registro indicador
-- =============================================


WITH DetalleRegistroIndicador AS
(
	SELECT DISTINCT 
		
		
	a.IdEnvioSolicitud IdSolicitud,c.IdFormularioWeb,d.IdIndicador  FROM EnvioSolicitud a
	INNER JOIN  Solicitud b
	ON b.IdSolicitud=a.IdSolicitud
	and b.IdEstadoRegistro=2
	INNER JOIN  DetalleSolicitudFormulario c
	ON c.IdSolicitud=a.IdSolicitud
	INNER JOIN  DetalleFormularioWeb d
	ON d.IdFormularioWeb=c.IdFormularioWeb
	and d.Estado=1
	INNER JOIN  FormularioWeb f
	ON f.IdFormularioWeb=d.IdFormularioWEb
	and f.IdEstadoRegistro=2
	INNER JOIN  Indicador g
	ON d.IdIndicador=g.IdIndicador
	and g.IdEstadoRegistro=2
	WHERE a.Enviado=0


),
CTECategoriaIndicador AS
(
	SELECT DISTINCT c.IdSolicitud, c.IdFormularioWEb IdFormulario,c.IdIndicador,NombreCategoria,a.idCategoriaDesagregacion idCategoria, a.IdDetalleCategoriaTexto, b.IdTipoDetalleCategoria IdTipoCategoria, b.CantidadDetalleDesagregacion
	, h.FechaMinima, h.FechaMaxima, i.Minimo,i.Maximo, dt.Etiqueta

	FROM DetalleRegistroIndicador c

	INNER JOIN  DetalleIndicadorCategoria a
	on c.IdIndicador=a.IdIndicador
	and a.Estado=1
	INNER JOIN  CategoriaDesagregacion b
	on b.IdCategoriaDesagregacion=a.idCategoriaDesagregacion
	and b.IdEstadoRegistro=2
	INNER JOIN  Indicador d
	on d.IdIndicador=c.IdIndicador
	and d.IdEstadoRegistro=2
	FULL OUTER JOIN DetalleCategoriaFecha h
	on h.idCategoriaDesagregacion=b.idCategoriaDesagregacion
	FULL OUTER JOIN DetalleCategoriaNumerico i
	on i.idCategoriaDesagregacion=b.idCategoriaDesagregacion
	LEFT JOIN DetalleCategoriaTexto dt ON b.IdCategoriaDesagregacion = dt.IdCategoriaDesagregacion AND a.IdDetalleCategoriaTexto = dt.IdDetalleCategoriaTexto AND dt.Estado = 1
	WHERE c.IdSolicitud is not null and c.IdFormularioweb is not null
),


CTECategoriaIndicadorDetalle AS
(
	SELECT
		a.IdSolicitud,
		a.IdFormulario,
		a.IdIndicador,
		a.NombreCategoria,
		a.idCategoria,
		5 IdTipoCategoria, 
		a.CantidadDetalleDesagregacion,
		--(Select STRING_AGG(       CONVERT( NVARCHAR(MAX),  '<option value='''+x.Etiqueta+'''>'+x.Etiqueta+'</option>'),'') from DetalleCategoriaTexto x where
		--	X.Estado=1 and  x.idCategoriaDesagregacion=a.idCategoria  group by x.idCategoriaDesagregacion ) AS JSON,
		STRING_AGG(CONVERT(NVARCHAR(MAX), '<option value='''+a.Etiqueta+'''>'+a.Etiqueta+'</option>'),'') AS JSON,
		NULL	Minimo,
		NULL	Maximo
	from CTECategoriaIndicador a
	where CantidadDetalleDesagregacion>0
	group by a.IdSolicitud, a.IdFormulario, a.IdIndicador, a.NombreCategoria, a.idCategoria, a.CantidadDetalleDesagregacion
	union
	select 
		a.IdSolicitud,
		a.IdFormulario,
		a.IdIndicador,
		a.NombreCategoria,
		a.idCategoria,
		a.IdTipoCategoria, 
		a.CantidadDetalleDesagregacion,
		NULL AS JSON,
		CASE a.IdTipoCategoria When 1 then cast(Minimo as varchar(100)) Else cast(FechaMinima as Varchar(100)) end Minimo,
		CASE a.IdTipoCategoria When 1 then Cast( Maximo as varchar(100)) Else Cast (FechaMaxima as varchar(100)) end Maximo 
	from CTECategoriaIndicador a
	where CantidadDetalleDesagregacion=0

)

select distinct 	
		cast(concat(a.IdSolicitud ,a.IdFormulario,a.IdIndicador )as bigint) IdDetalleRegistroIndicadorFonatel,
		cast(concat(a.IdSolicitud ,a.IdFormulario,a.IdIndicador,idCategoria )as bigint) IdDetalleRegistroIndicadorCategoriaFonatel,
		
		a.IdSolicitud,
		a.IdFormulario,
		a.IdIndicador,
		NombreCategoria,
		idCategoria,
		IdTipoCategoria, 
		CantidadDetalleDesagregacion, 
		cast(JSON as varchar(max)) JSON,
		Minimo,
		Maximo

from CTECategoriaIndicadorDetalle a
INNER JOIN  DetalleSolicitudFormulario f
on f.IdFormularioweb=a.IdFormulario



end