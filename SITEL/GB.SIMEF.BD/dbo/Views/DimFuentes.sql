
CREATE view [dbo].[DimFuentes]
as
select 0 idFuente, 'No Definido' Medida
union
select  idFuente, Fuente 
--into DWSIMEF.dbo.DimFuentes
from FuentesRegistro