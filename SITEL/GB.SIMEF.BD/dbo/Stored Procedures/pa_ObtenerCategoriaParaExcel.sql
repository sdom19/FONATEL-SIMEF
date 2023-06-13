 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener categorías para EXCEL
-- ============================================
CREATE PROCEDURE [dbo].[pa_ObtenerCategoriaParaExcel]
@NombreCategoria VARCHAR(3000),
@CategoriaTexto VARCHAR(2000)
AS 

BEGIN 
DECLARE @consulta VARCHAR(5000)

SET @consulta='SELECT b.* FROM CategoriaDesagregacion a
	  INNER JOIN DetalleCategoriaTexto b
	  ON a.IdCategoriaDesagregacion = b.IdCategoriaDesagregacion 
	  WHERE UPPER(a.NombreCategoria) = UPPER('''+@NombreCategoria+''') AND
	  UPPER(b.Etiqueta) = UPPER('''+@CategoriaTexto+''')';

EXEC(@consulta)
END