

CREATE PROCEDURE [dbo].[pa_EjecucionMotor]
    @p_Opcion int,
	@p_Periodo int,
	@p_Usuario varchar(20),
	@p_Anio int,
	@p_Fecha Datetime,
	@p_IdEjecucion int
AS
BEGIN

---declare @Anio int;
---set @Anio = (select YEAR(GETDATE()));
if(@p_opcion=1) 
begin
 insert into EjecucionMotor values (@p_Periodo,@p_Anio,@p_Usuario,@p_Fecha,0);
end
else if (@p_opcion=2)
begin
 select 
	  idejecucion ,
	 anioEjecucion,
	 usuarioEjecucion,
	 FechaRegistro,
	 Ejecutado,
	  case periodoEjecucion 
	  when 1 then 'PRIMER TRIMESTRE'
	  when 2 then 'SEGUNDO TRIMESTRE'
	  when 3 then 'TERCER TRIMESTRE'
	  when 4 then 'CUARTO TRIMESTRE'
	  when 5 then 'ANUAL'
  end periodoEjecucion,
  PeriodoEjecucion as idPeriodoEjecucion
    from EjecucionMotor
 where Ejecutado = 0
 order by anioEjecucion, idPeriodoEjecucion asc
end
 else if (@p_opcion=3)
 begin
select 
	  idejecucion ,
	 anioEjecucion,
	 usuarioEjecucion,
	 FechaRegistro,
	 Ejecutado,
	  case periodoEjecucion 
	  when 1 then 'PRIMER TRIMESTRE'
	  when 2 then 'SEGUNDO TRIMESTRE'
	  when 3 then 'TERCER TRIMESTRE'
	  when 4 then 'CUARTO TRIMESTRE'
	  when 5 then 'ANUAL'
  end periodoEjecucion
    from EjecucionMotor
 where Ejecutado = 1
 end
 else if (@p_opcion=4)
  begin
  update EjecucionMotor set Ejecutado = 3 where  idejecucion =@p_IdEjecucion
  end
  else if (@p_opcion=5)
  begin
  update EjecucionMotor set Ejecutado = 1 where  idejecucion =@p_IdEjecucion
  end
END