CREATE TRIGGER Ejecucion_Insert
on EjecucionMotor 

AFTER INSERT
  AS
  BEGIN
set nocount on;

DECLARE 
	
	@id int, 
	@periodo int, 
	@anio int, 
	@eje int, 
	@user varchar(500),
	@fecha datetime,
	@insert varchar(500)

 Select @id=         idejecucion              FROM inserted
 Select @periodo=    periodoEjecucion         FROM inserted
 Select @anio=       anioEjecucion            FROM inserted
 Select @eje=        Ejecutado                FROM inserted
 Select @user=        usuarioEjecucion       FROM inserted
 Select @fecha=       FechaRegistro           FROM inserted
 Select @insert=     CONCAT('ID Ejecuci�n: ',@id,' Periodo de ejecuci�n: ',@periodo,' A�o de ejecuci�n : ',@anio,' Fecha de Ejecucion',@fecha,' Ejecutado: ',@eje)

BEGIN
		INSERT INTO Bitacora (Pantalla, Accion,Usuario,Descripcion,FechaBitacora,RegistroAnterior,RegistroNuevo) VALUES ('Calidad Formulas',2,@user,'Proceso de creaci�n en Programaci�n de ejecuci�n (F�rmulas)',GETDATE(),' ',@insert)
END
END


