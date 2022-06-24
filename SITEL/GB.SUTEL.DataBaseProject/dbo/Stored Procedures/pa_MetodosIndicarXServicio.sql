 CREATE PROCEDURE pa_MetodosIndicarXServicio
	 @Opcion int,
     @IdIndicador nvarchar(50),
     @Umbral decimal(5,2) = 0,
     @PesoRelativo decimal(5,2) = 0,
	 @usuario varchar(30),
	 @fecha varchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
IF (@Opcion = 1) 
  BEGIN	
	insert into InidicadorUmbralPeso values (@IdIndicador,@Umbral,@PesoRelativo,getdate(),@usuario)
  END
ELSE IF (@Opcion = 2)
BEGIN
	update InidicadorUmbralPeso set Umbral = @Umbral , PesoRelativo = @PesoRelativo ,FechaUltimaModificacion = getdate(), UsuarioUltimaModificacion= @usuario where  IdIndicador = @IdIndicador	
END
ELSE IF (@Opcion = 3)
BEGIN
	select* from InidicadorUmbralPeso where IdIndicador = @IdIndicador
END	
END