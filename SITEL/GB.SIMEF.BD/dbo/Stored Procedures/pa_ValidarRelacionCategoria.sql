
CREATE PROC [dbo].[pa_ValidarRelacionCategoria]

@idRelacionCategoria INT

AS
BEGIN 
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para validar relación categoria

	WITH VALIDACION_CTE 
	AS
	(
       
	  --SELECT DISTINCT 0 id, ind.Nombre  Listado FROM DetalleIndicadorCategoria dic
	  --INNER JOIN CategoriaDesagregacion cd ON cd.IdCategoriaDesagregacion = dic.IdCategoriaDesagregacion
	  --INNER JOIN RelacionCategoria rc ON rc.IdCategoriaDesagregacion = cd.IdCategoriaDesagregacion
	  --INNER JOIN Indicador ind ON ind.IdIndicador =  dic.IdIndicador
	  --WHERE ind.IdEstadoRegistro != 4 AND  rc.IdRelacionCategoria = @idRelacionCategoria
	  --UNION 
	  SELECT DISTINCT 1 id, Reg.Nombre Listado  FROM CategoriaDesagregacion Cat
	  INNER JOIN RelacionCategoria Rel ON Cat.IdCategoriaDesagregacion = Rel.IdCategoriaDesagregacion
	  INNER JOIN ReglaAtributoValido Ratr ON Cat.IdCategoriaDesagregacion = Ratr.IdCategoriaDesagregacion
	  INNER JOIN DetalleReglaValidacion Det ON  Ratr.IdDetalleReglaValidacion = Det.IdDetalleReglaValidacion
	  INNER JOIN ReglaValidacion Reg ON Reg.IdReglaValidacion = Det.IdReglaValidacion
      WHERE Det.Estado = 1 AND Reg.IdEstadoRegistro != 4 AND 
	  Rel.IdRelacionCategoria=@idRelacionCategoria
	)

	SELECT CONCAT( CASE id WHEN 1 THEN 'la Regla ' END,
	STRING_AGG( CAST(Listado AS NVARCHAR(MAX)),','))  Listado
	 
	FROM  VALIDACION_CTE 
	GROUP BY id
END