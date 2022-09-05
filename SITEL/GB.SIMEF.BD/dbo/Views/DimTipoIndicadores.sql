
CREATE VIEW DimTipoIndicadores
AS
select 0 IdTipoIdicador, 'No Definido' tipoIndicador
--into DWSIMEF.dbo.DimTipoIndicadores
union
select IdTipoIdicador, Nombre tipoIndicador


from TipoIndicadores