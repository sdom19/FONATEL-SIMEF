CREATE procedure [dbo].[spObtenerBitacora]
@fechaDesde varchar(12),
@fechaHasta varchar(12)
as 
declare @consulta varchar(1000);
set @consulta='Select top 100 idBitacora
      ,Fecha
      ,Usuario
      ,Pantalla
      ,Accion
      ,Codigo
 from bitacora' +
IIF(@fechaDesde='','',' where Fecha between '''+@fechaDesde +''' and '''+@fechaHasta+'''')+
' order by fecha desc' 


exec(@consulta)