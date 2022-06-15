CREATE TABLE [dbo].[InidicadorUmbralPeso] (
    [IdUmbralPeso]              INT            IDENTITY (1, 1) NOT NULL,
    [IdIndicador]               NVARCHAR (50)  NULL,
    [Umbral]                    DECIMAL (5, 2) NULL,
    [PesoRelativo]              DECIMAL (5, 2) NULL,
    [FechaUltimaModificacion]   DATETIME       NULL,
    [UsuarioUltimaModificacion] VARCHAR (40)   NULL,
    PRIMARY KEY CLUSTERED ([IdUmbralPeso] ASC),
    CONSTRAINT [FK__Inidicado__IdInd__19FFD4FC] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador]),
    UNIQUE NONCLUSTERED ([IdIndicador] ASC)
);


GO
create TRIGGER [dbo].[BitacoraUmbralUpdate]
ON [dbo].[InidicadorUmbralPeso] 

			AFTER UPDATE
			AS
BEGIN
			SET NOCOUNT ON

  DECLARE 
	
	@id        nvarchar(50),
	@umbralold decimal(5,2),
	@umbral    decimal(5,2),
	@pesold    decimal(5,2),
	@peso      decimal(5,2),
	@us        varchar(40),
	@old       varchar(1000),
	@new       varchar(1000)

 Select @umbralold=       Umbral					FROM deleted
 Select @pesold=          PesoRelativo				FROM deleted
 Select @umbral=          Umbral					FROM inserted
 Select @id=              IdIndicador				FROM inserted
 Select @peso=            PesoRelativo				FROM inserted
 Select @us=              UsuarioUltimaModificacion FROM inserted
 Select @old=             CONCAT('ID Indicador: ',@id,'Umbral: ',@umbralold,'Peso Relativo: ',@pesold)
 Select @new=             CONCAT('ID Indicador: ',@id,'Umbral: ',@umbral,'Peso Relativo: ',@peso)
  
BEGIN
		INSERT INTO Bitacora (Pantalla, Accion,Usuario,Descripcion,FechaBitacora,RegistroAnterior,RegistroNuevo)
        VALUES('Calidad Umbrales', 3,@us,'Proceso de edición de Umbrales y Pesos Relativos Calidad',getdate(),@old,@new)

END
END
GO
create TRIGGER [dbo].[BitacoraUmbralInsert]
ON [dbo].[InidicadorUmbralPeso]
 
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