CREATE TRIGGER Solicitud_Delete
on SolicitudGeneral

AFTER DELETE
  AS
  BEGIN
set nocount on;

DECLARE 
	
	@id             int, 
	@iduser         int, 
	@descripcion    varchar(250), 
	@inicio         date, 
	@final          date,
	@path           varchar(max),
	@user           varchar(30),
	@delete         varchar(max)

Select @id=              IdSolicitud      FROM deleted
Select @iduser=          IdUsuario        FROM deleted
Select @descripcion=     Descripcion      FROM deleted
Select @inicio=          FechaInicio      FROM deleted
Select @final=           FechaFinal       FROM deleted
Select @path=            Path             FROM deleted
SELECT @user=            AccesoUsuario    FROM Usuario   WHERE @iduser=IdUsuario

Select @delete=          CONCAT('ID Solicitud: ',@id,' Descripción: ',@descripcion,' Fecha de inicio: ',@inicio,' Fecha Final: ',@final,' Path: ',@path)
 

BEGIN
		INSERT INTO Bitacora (Pantalla, Accion,Usuario,Descripcion,FechaBitacora,RegistroAnterior,RegistroNuevo) VALUES ('Solicitud General Index',4,@user,'Proceso de eliminación en: Solicitud General Index',GETDATE(),@delete,'')
END
END

