-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar Detalle de categoria desde formulario web
-- =============================================
CREATE  PROCEDURE [dbo].[pa_ActualizarDetalleFormularioWeb]
	   @idDetalle INT,
	   @idFormulario INT,
	   @idIndicador INT,
	   @TituloHojas VARCHAR(300),
	   @NotasInformante VARCHAR(3000),
	   @NotasEncargado VARCHAR(3000),
	   @Estado BIT
AS

BEGIN TRY

	BEGIN TRAN;
		MERGE dbo.DetalleFormularioWeb AS TARGET
			USING (VALUES( @idDetalle,
						  @IdFormulario,
						  @IdIndicador,
						  @TituloHojas,
						  @NotasInformante,
						  @NotasEncargado,
						  @Estado))AS SOURCE ( IdDetalle,
									  IdFormulario,
									  IdIndicador,
									  TituloHojas,
									  NotasInformante,
									  NotasEncargado,
									  Estado)
										ON TARGET.IdDetalleFormularioWEb=SOURCE.IdDetalle
										WHEN NOT MATCHED THEN
											INSERT (
												IdFormularioWEb,
												IdIndicador,
												TituloHoja,
												NotaInformante,
												NotaEncargado,
												Estado)
											VALUES( 
												SOURCE.IdFormulario,
												SOURCE.IdIndicador,
												SOURCE.TituloHojas,
												SOURCE.NotasInformante,
												SOURCE.NotasEncargado,
												SOURCE.Estado)
										WHEN MATCHED THEN
											UPDATE SET
												IdFormularioWeb=SOURCE.IdFormulario,
												IdIndicador=SOURCE.IdIndicador,
												TituloHoja=SOURCE.TituloHojas,
												NotaInformante=SOURCE.NotasInformante,
												NotaEncargado=SOURCE.NotasEncargado,
												Estado=SOURCE.Estado;
	COMMIT TRAN


	IF(@Estado=0)
	BEGIN
		UPDATE FormularioWeb SET IdEstadoRegistro=1 FROM DetalleFormularioWeb 
		WHERE  FormularioWeb.IdFormularioWeb = @IdFormulario

	END 

	SELECT TOP (1000) [IdDetalleFormularioWeb]
      ,[IdFormularioWeb]
      ,[IdIndicador]
      ,[TituloHoja]
      ,[NotaInformante]
      ,[NotaEncargado]
      ,[Estado]
	FROM [DetalleFormularioWeb]
    WHERE Estado=1 AND IdIndicador=@idIndicador AND IdFormularioWeb=@idFormulario;
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