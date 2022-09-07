create PROCEDURE [dbo].[spObtenerFrecuenciasEnvio]
	@idFrecuencia int
as
BEGIN
declare @consulta varchar(150)

set @consulta = 'select idFrecuencia, 
	Nombre, 
	CantidadDias, 
	Estado 
	from FrecuenciaEnvio 
	where Estado=1'+
	IIF(@idFrecuencia=0,'', 'and idFrecuencia' + cast(@idFrecuencia as varchar(10)) + ' ')
exec(@consulta)
END