
CREATE PROC [dbo].[pa_ObtenerRelacionCategoriaAtributo]
@idRelacion INT,
@idCategoriaId VARCHAR(50)
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la relación de categoria atributo


WITH CTEDetalleRelacionCategoria
AS
(
	SELECT d.IdCategoriaDesagregacion,d.IdDetalleRelacionCategoria,d.IdRelacionCategoria,d.Estado 
	FROM DetalleRelacionCategoria d
	WHERE IdRelacionCategoria=@idRelacion
	AND Estado=1



)
SELECT DISTINCT c.idRelacionCategoriaId,
	   c.idCategoriaDesagregacion,
	   d.IdCategoriaDesagregacion IdCategoriaDesagregacionAtributo,
	   ISNULL(e.IdDetalleCategoriaTextoAtributo,0) IdDetalleCategoriaTextoAtributo

FROM RelacionCategoria a
INNER JOIN RelacionCategoriaId c
ON c.idRelacionCategoriaId=a.IdRelacionCategoria
INNER JOIN CTEDetalleRelacionCategoria d
ON a.IdRelacionCategoria=a.IdRelacionCategoria
FULL OUTER JOIN RelacionCategoriaAtributo e
ON e.IdCategoriaDesagregacionAtributo=d.IdCategoriaDesagregacion
AND e.idRelacionCategoriaId=c.idRelacionCategoriaId
AND e.idCategoriaDesagregacion=c.idCategoriaDesagregacion
WHERE a.IdRelacionCategoria=@idRelacion
AND c.idCategoriaDesagregacion=@idCategoriaId