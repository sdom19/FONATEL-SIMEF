 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener detalle de categoria texto
-- ============================================
CREATE procedure [dbo].[pa_ObtenerDetalleCategoriaTexto]
@idCategoriaDetalle INT,
@idCategoria INT,
@codigo INT,
@Etiqueta VARCHAR(300)
AS 

BEGIN 
DECLARE @consulta VARCHAR(2000)

SET @consulta='SELECT idCategoriaDesagregacion 
      ,IdDetalleCategoriaTexto
      ,Codigo
      ,Etiqueta
      ,Estado
  FROM DetalleCategoriaTexto 
   where Estado=1 '+
    IIF(@idCategoria=0,'',' and idCategoriaDesagregacion='+ CAST(@idCategoria AS VARCHAR(10))+' ') +
	IIF(@idCategoriaDetalle=0,'',' and IdDetalleCategoriaTexto='+ CAST(@idCategoriaDetalle AS VARCHAR(10))+' ') +
	IIF(@codigo=0,'',' and Codigo='+ CAST(@codigo AS VARCHAR(10))+'') +
	IIF(@Etiqueta='','',' and upper(Etiqueta)='''+@Etiqueta+'''') +
    ' order by Codigo'
EXEC(@consulta)

END