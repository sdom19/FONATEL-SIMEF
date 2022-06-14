CREATE TRIGGER Umbral_Insert
ON InidicadorUmbralPeso
 
  AFTER INSERT
  AS

BEGIN
  SET NOCOUNT ON

  DECLARE 
	@id			nvarchar(50),
	@umbralold  decimal(5,2),
	@umbral		decimal(5,2),
	@pesold		decimal(5,2),
	@peso		decimal(5,2),
	@us			varchar(40),
	@old		varchar(1000),
	@new		varchar(1000)

 Select @umbralold=       Umbral                    FROM deleted
 Select @pesold=          PesoRelativo              FROM deleted
 Select @umbral=          Umbral                    FROM inserted
 Select @id=              IdIndicador               FROM inserted
 Select @peso=            PesoRelativo              FROM inserted
 Select @us=              UsuarioUltimaModificacion FROM inserted
 Select @new=             CONCAT('ID Indicador: ',@id,'Umbral: ',@umbral,'Peso Relativo: ',@peso)
  
BEGIN
		INSERT INTO Bitacora (Pantalla, Accion,Usuario,Descripcion,FechaBitacora,RegistroAnterior,RegistroNuevo)
        VALUES('Calidad Umbrales', 2,@us,'Proceso de creación de Umbrales y Pesos Relativos Calidad',getdate(),' ',@new)

END
END