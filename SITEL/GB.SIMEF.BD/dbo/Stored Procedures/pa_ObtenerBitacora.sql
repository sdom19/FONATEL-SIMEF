 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener bitacora
-- ============================================
CREATE PROCEDURE [dbo].[pa_ObtenerBitacora]
@fechaDesde VARCHAR(12),
@fechaHasta VARCHAR(12),
@Pantalla VARCHAR(600),
@Acciones VARCHAR(200),
@Usuario VARCHAR(600)
AS 


SET @Pantalla=REPLACE(@Pantalla,',',''',''');

SET @Usuario=REPLACE(@Usuario,',',''',''');


DECLARE @consulta VARCHAR(MAX);
SET @consulta='Select top 2000 idBitacora
      ,[Fecha]
      ,[Usuario]
      ,[Pantalla]
      ,[Accion]
      ,[Codigo]
      ,isnull([ValorInicial],'''') ValorInicial
      ,isnull([ValorAnterior],'''')ValorAnterior
      ,isnull([ValorActual],'''') ValorActual

 from bitacora where 1=1 ' +
IIF(@fechaDesde='','',' and Fecha between '''+@fechaDesde +''' and '''+@fechaHasta+'''')+
IIF(@Usuario='','',' and Usuario  in ('''+@usuario +''')')+
IIF(@Pantalla='','',' and Pantalla  in ('''+@Pantalla +''')')+
IIF(@Acciones='','',' and Accion  in ('+@Acciones+')')+
' order by fecha desc' 


EXEC(@consulta)