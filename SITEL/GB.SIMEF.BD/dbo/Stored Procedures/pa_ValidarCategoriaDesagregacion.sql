CREATE PROC [dbo].[pa_ValidarCategoriaDesagregacion]

@IdCategoriaDesagregacion INT

AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para validar la categoria de desagregación

WITH CTEIndicador
AS
(
	SELECT DISTINCT TRIM(a.Nombre) Nombre FROM Indicador a
	INNER JOIN DetalleIndicadorCategoria b
	ON a.idIndicador=b.idIndicador
	WHERE IdEstadoRegistro!=4 AND b.IdCategoriaDesagregacion=@IdCategoriaDesagregacion
	and b.Estado=1
),
formulario
AS
(
	SELECT DISTINCT TRIM(fw.Nombre) Nombre
	FROM FormularioWeb fw
	INNER JOIN DetalleFormularioWeb dfw
		ON fw.IdFormularioWeb = dfw.IdFormularioWeb
	INNER JOIN Indicador ind
		ON ind.idIndicador = dfw.idIndicador
		AND dfw.Estado = 1 OR fw.IdEstadoRegistro != 4
	INNER JOIN Indicador a ON
	ind.idIndicador = a.IdIndicador
	AND dfw.IdIndicador = a.IdIndicador
	INNER JOIN DetalleIndicadorCategoria b
	ON a.idIndicador=b.idIndicador
	and b.Estado=1
	WHERE a.IdEstadoRegistro!=4 AND b.IdCategoriaDesagregacion=@IdCategoriaDesagregacion
)

SELECT 'Indicadores: '+STRING_AGG(i.Nombre,', ') listado  FROM CTEIndicador i WHERE Nombre IS NOT NULL 
UNION ALL 
SELECT 'Formularios: '+ STRING_AGG(f.Nombre,', ') listado FROM Formulario f WHERE Nombre IS NOT NULL;