-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar detalles registro fuente
-- =============================================
CREATE PROCEDURE [dbo].[pa_ActualizarFuenteRegistroDetalle]
	   @IdDetalleFuenteRegistro INT
	  ,@IdFuenteRegistro INT
      ,@Nombre VARCHAR(300)
	  ,@CorreoElectronico VARCHAR(300)
	  ,@Estado BIT
	  ,@IdUsuario INT 
	  ,@CorreoEnviado BIT = 0
AS

BEGIN TRY

	IF EXISTS(SELECT 1 FROM DetalleFuenteRegistro WHERE IdDetalleFuenteRegistro = @IdDetalleFuenteRegistro)
	BEGIN
		DECLARE @CorreoElectronicoExistente VARCHAR(300)
		SELECT @CorreoElectronicoExistente = CorreoElectronico FROM DetalleFuenteRegistro WHERE IdDetalleFuenteRegistro = @IdDetalleFuenteRegistro
		IF(TRIM(UPPER(@CorreoElectronicoExistente)) != TRIM(UPPER(@CorreoElectronico)))
		BEGIN
			SET @CorreoEnviado = 0;
		END
	END

	BEGIN TRAN;
		MERGE dbo.DetalleFuenteRegistro AS TARGET
			USING (VALUES( @IdDetalleFuenteRegistro, @IdFuenteRegistro,@Nombre,@CorreoElectronico,@Estado, @IdUsuario, @CorreoEnviado ))AS SOURCE 
						 (  IdDetalleFuenteRegistro,IdFuenteRegistro,Nombre,CorreoElectronico,Estado, IdUsuario, CorreoEnviado )
										ON TARGET.IdDetalleFuenteRegistro=SOURCE.IdDetalleFuenteRegistro
										WHEN NOT MATCHED THEN
											INSERT ( 
													IdFuenteRegistro,
													 NombreDestinatario,
													 CorreoElectronico,
													 Estado,
													 IdUsuario,
													 CorreoEnviado
												   )
											VALUES(
													 SOURCE.IdFuenteRegistro
													,UPPER(SOURCE.Nombre)
													,UPPER(SOURCE.CorreoElectronico)

													,Estado
													,IdUsuario
													,0
												  )
										WHEN MATCHED THEN
											UPDATE SET 
											NombreDestinatario=UPPER(SOURCE.Nombre)
											,CorreoElectronico=UPPER(SOURCE.CorreoElectronico)
											,Estado=SOURCE.Estado
											,CorreoEnviado = SOURCE.CorreoEnviado;

	COMMIT TRAN


		IF(@Estado=0)
	BEGIN
		UPDATE FuenteRegistro SET IdEstadoRegistro=1 FROM DetalleFuenteRegistro 
		WHERE FuenteRegistro.IdFuenteRegistro = @IdFuenteRegistro

	END 

	SELECT [IdDetalleFuenteRegistro]
      ,[IdFuenteRegistro]
      ,[NombreDestinatario]
      ,[CorreoElectronico]
      ,[Estado]
	  ,IdUsuario
	  ,CorreoEnviado
  FROM [dbo].[DetalleFuenteRegistro]
  WHERE IdDetalleFuenteRegistro=@IdDetalleFuenteRegistro OR IdDetalleFuenteRegistro=SCOPE_IDENTITY()
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		BEGIN
			SELECT   
				ERROR_NUMBER() AS ErrorNumber  
			   ,ERROR_MESSAGE() AS ErrorMessage; 
			ROLLBACK TRANSACTION;
		END
END CATCH