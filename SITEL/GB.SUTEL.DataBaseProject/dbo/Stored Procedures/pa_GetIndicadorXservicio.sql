CREATE PROCEDURE [dbo].[pa_GetIndicadorXservicio]
	  @Servicio int,
	  @FechaInic varchar(120),
	  @FechaFin varchar(120),
	  @Usuario varchar(40),
	  @Opcion int,
	  @Direccion int
AS
BEGIN
	SET NOCOUNT ON;
 IF (@Opcion = 1)
 begin
  select 
  Ind.IdIndicador,
  Ind.NombreIndicador,
  Ind.DescIndicador,
  ser.IdServicio,
  ISNULL(ump.Umbral,0) as Umbral,
  ISNULL(ump.PesoRelativo,0) as PesoRelativo,
  ISNULL(ump.UsuarioUltimaModificacion,'') as UsuarioUltimaModificacion,
  ISNULL(CONVERT(VARCHAR(10),ump.FechaUltimaModificacion,105),'') as FechaUltimaModificacion
 from 
 IndicadorDireccion indirec
   inner join Indicador Ind on indirec.IdIndicador = Ind.IdIndicador
   inner join ServicioIndicador Servi on Ind.IdIndicador = Servi.IdIndicador
   inner join Direccion direc on indirec.IdDireccion = direc.IdDireccion
   inner join Servicio ser on Servi.IdServicio = ser.IdServicio
   left  join InidicadorUmbralPeso ump on Ind.IdIndicador = Ump.IdIndicador
 where ser.IdServicio = @Servicio and direc.IdDireccion = @Direccion and Ind.Borrado = 0

 end
 else if  (@Opcion = 2)
 begin
 select 
  Ind.IdIndicador,
  Ind.NombreIndicador,
  Ind.DescIndicador,
  ser.IdServicio,
  ISNULL(ump.Umbral,0) as Umbral,
  ISNULL(ump.PesoRelativo,0) as PesoRelativo,
  ISNULL(ump.UsuarioUltimaModificacion,'') as UsuarioUltimaModificacion,
  ISNULL(CONVERT(VARCHAR(10),ump.FechaUltimaModificacion,105),'') as FechaUltimaModificacion
 from 
 IndicadorDireccion indirec
   inner join Indicador Ind on indirec.IdIndicador = Ind.IdIndicador
   inner join ServicioIndicador Servi on Ind.IdIndicador = Servi.IdIndicador
   inner join Direccion direc on indirec.IdDireccion = direc.IdDireccion
   inner join Servicio ser on Servi.IdServicio = ser.IdServicio
   left  join InidicadorUmbralPeso ump on Ind.IdIndicador = Ump.IdIndicador
 where ser.IdServicio = @Servicio and direc.IdDireccion = @Direccion and Ind.Borrado = 0
  and  ump.FechaUltimaModificacion >= @FechaInic AND ump.FechaUltimaModificacion <= @FechaFin
 end		
 else if  (@Opcion = 3)
 begin
select 
  Ind.IdIndicador,
  Ind.NombreIndicador,
  Ind.DescIndicador,
  ser.IdServicio,
  ISNULL(ump.Umbral,0) as Umbral,
  ISNULL(ump.PesoRelativo,0) as PesoRelativo,
  ISNULL(ump.UsuarioUltimaModificacion,'') as UsuarioUltimaModificacion,
  ISNULL(CONVERT(VARCHAR(10),ump.FechaUltimaModificacion,105),'') as FechaUltimaModificacion
 from 
 IndicadorDireccion indirec
   inner join Indicador Ind on indirec.IdIndicador = Ind.IdIndicador
   inner join ServicioIndicador Servi on Ind.IdIndicador = Servi.IdIndicador
   inner join Direccion direc on indirec.IdDireccion = direc.IdDireccion
   inner join Servicio ser on Servi.IdServicio = ser.IdServicio
   left  join InidicadorUmbralPeso ump on Ind.IdIndicador = Ump.IdIndicador
 where ser.IdServicio = @Servicio and direc.IdDireccion = @Direccion and Ind.Borrado = 0
  and ump.UsuarioUltimaModificacion =@Usuario
  end
END