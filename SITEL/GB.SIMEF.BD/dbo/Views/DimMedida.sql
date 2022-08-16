

CREATE view [dbo].[DimMedida]
as
select 0 idMedida, 'No Definido' Medida
union
select idMedida, Nombre Medida 
--into DWSIMEF.dbo.DimMedida
from TipoMedida