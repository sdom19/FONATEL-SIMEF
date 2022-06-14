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
 Select @insert=     CONCAT('ID Ejecución: ',@id,' Periodo de ejecución: ',@periodo,' Año de ejecución : ',@anio,' Fecha de Ejecucion',@fecha,' Ejecutado: ',@eje)

BEGIN
		INSERT INTO Bitacora (Pantalla, Accion,Usuario,Descripcion,FechaBitacora,RegistroAnterior,RegistroNuevo) VALUES ('Calidad Formulas',2,@user,'Proceso de creación en Programación de ejecución (Fórmulas)',GETDATE(),' ',@insert)
END
END


