CREATE PROCEDURE [dbo].[pa_SolicitudesVencidas]
as
BEGIN
DECLARE @iXml xml;
DECLARE @P_CORREO nvarchar(max)
DECLARE @P_OPERADOR nvarchar(max)
DECLARE @P_DIRECCION int
DECLARE @P_FORMULARIO nvarchar(500)
DECLARE @P_FECHA_INICIAL datetime
DECLARE @P_FECHA_FINAL datetime
DECLARE @MAX INT
DECLARE @COUNT INT
DECLARE @Asunto varchar(65)
DECLARE	@html varchar(max)
DECLARE @CANTIDAD_REGISTROS int

DECLARE @TABLA_SOLICITUD_VENCIDA TABLE(
	ID  int identity,
	NOMBRE_OPERADOR nvarchar(150),
	DIRECCION int,
	NOMBRE_FORMULARIO nvarchar(500),
	FECHA_INICIO datetime,
	FECHA_FIN datetime,
	ESTADO nvarchar(2)
)

DECLARE @TABLA_SOLICITUDES TABLE(
 ID  int identity,
 FORMULARIO nvarchar(500)
)

set @Asunto='Sistema de notificaciones de vencimiento de solicitudes SITEL'
insert into @TABLA_SOLICITUD_VENCIDA(NOMBRE_OPERADOR,DIRECCION,NOMBRE_FORMULARIO,FECHA_INICIO,FECHA_FIN,ESTADO)( 
SELECT O.NOMBREOPERADOR,SCO.IDDIRECCION,SCO.DESCFORMULARIO,SCO.FECHAINICIO,SCO.FECHAFIN,SCO.ESTADO  FROM OPERADOR O,(
select DISTINCT SC.IDOPERADOR,R.IDDIRECCION,R.DESCFORMULARIO, R.FECHAINICIO, R.FECHAFIN,case when  SC.IDESTADO<4 then 'NO' else 'SI'end as ESTADO from SOLICITUDCONSTRUCTOR SC,(
select SI.IDSOLICITUDINDICADOR, SI.IDDIRECCION,SI.DESCFORMULARIO, SI.FECHAINICIO,SI.FECHAFIN
 from SOLICITUDINDICADOR SI
where convert(date,SI.FECHAFIN) =convert(date,DATEADD(day, -1,GETDATE()))) as r
where r.IDSOLICITUDINDICADOR=sc.IDSOLICITUDINDICADOR
) AS SCO
WHERE SCO.IDOPERADOR=O.IDOPERADOR
)


insert into @TABLA_SOLICITUDES(FORMULARIO)(Select distinct s.NOMBRE_FORMULARIO from @TABLA_SOLICITUD_VENCIDA  s)

SELECT @MAX = @@IDENTITY
	SET @COUNT = 1
	select @CANTIDAD_REGISTROS=Count(*) from   @TABLA_SOLICITUD_VENCIDA
	

WHILE @COUNT <= @MAX 
BEGIN
  --Formulario
   select @P_FORMULARIO=FORMULARIO from @TABLA_SOLICITUDES where ID=@COUNT
   --Datos en el formulario
   select top 1 @P_FECHA_INICIAL=FECHA_INICIO,
				@P_FECHA_FINAL=FECHA_FIN,
				@P_DIRECCION=DIRECCION from @TABLA_SOLICITUD_VENCIDA where NOMBRE_FORMULARIO=@P_FORMULARIO;
   --correo
   select @P_CORREO=CORREO from DIRECCION where IDDIRECCION=@P_DIRECCION
   --Lista de operadores
  SELECT @iXml = (
		select '<TR><TD style="padding-left:5px;border-bottom:solid #000 1px;">'+ id + '</TD><TD style="padding-left:5px;border-bottom:solid #000 1px;">'+estado+'</TD><TR>' from (
		select  cu.NOMBRE_OPERADOR as 'id', CU.ESTADO AS 'estado' from @TABLA_SOLICITUD_VENCIDA cu
		where cu.NOMBRE_FORMULARIO=@P_FORMULARIO
		) as r FOR XML PATH);

		SELECT  @P_OPERADOR=@iXml.value('.','nvarchar(max)');

		

	set @html='	<img src="http://i58.tinypic.com/20p3kvp.png" style="max-height: 55px;"/>
	<table style=" background-color:#eeeeee; word-wrap:break-word; width:100%!important;line-height:100%!important ">
		<tbody>
			<tr>
				<td style="font-family:Helvetica,Arial,sans-serif; font-size:12px; color:#000; padding:25px 35px 25px 35px;">
					<table style="border-collapse:collapse; width:100%;background-color:#fff;">
						<tr>
							<td style="font-family:Helvetica,Arial,sans-serif;   font-size:15px;padding:
								15px 5px 5px 15px; color:#0072c6;  text-decoration:none; border:0; text-align:left"><b>Sistema de Indicadores de Telecomunicaciones (SITEL)</b>
							</td>
						</tr>
						<tr>
							<td style="text-align:left;padding-top:25px;padding-left:15px;">
										Se le informa que la solicitud <strong>'
							 set @html=@html+@P_FORMULARIO;
							 set @html=@html+'</strong>  del <strong>'
							 set @html=@html+convert(VARCHAR(10), @P_FECHA_INICIAL,103);
							 set @html=@html+'</strong> al <strong>'
							 set @html=@html+ convert(VARCHAR(10),@P_FECHA_FINAL,103);
							 set @html=@html+'</strong> ha terminado su per&iacute;odo de registro de indicadores.
							</td>
						</tr>
						<tr>
							<td style="text-align:left;padding-bottom:15px;padding-top:15px;padding-left:15px;">
								La siguiente informaci&oacute;n muestra el estado de la entrega por parte de los siguientes operadores:
							</td>
						</tr>
						<tr>
							<td style="text-align:left;padding-left:15px;">
								<table>
								
								<tr><td style="background-color:#00A3E0; color:#fff;padding: 10px 10px 10px 10px;">Operador</TD><td style="background-color:#00A3E0; color:#fff;padding: 10px 10px 10px 10px;">Entreg&oacute;</td></tr>';
								set @html=@html+@P_OPERADOR;
								set @html=@html+'
								</table>
							</td>
						</tr>
				<tr>
					<td style="text-align:left;padding-left:15px;font-size:11px;">
					<hr>
					Por favor, no responder a este correo ya que no es monitoreado.

					</td>
				</tr>
			<tr>
				<td style="text-align:left;padding-left:15px;font-size:11px;">
				 Cualquier duda o consulta diríjase al correo: <a>sitel@sutel.go.cr</a>
				</td>
			</tr>
		</table>
	</td>
	</tr>
	</tbody>
</table>'

		if( @CANTIDAD_REGISTROS>0)
		begin
	  		
			EXEC msdb.dbo.sp_send_dbmail
			 @profile_name='SCI_Notificaciones' 
			,@recipients = @P_CORREO
			,@copy_recipients = ''	
			,@body_format = 'HTML'
			,@body=@html
			,@subject=@asunto;
        end
	set @COUNT= @COUNT+1;
END
END