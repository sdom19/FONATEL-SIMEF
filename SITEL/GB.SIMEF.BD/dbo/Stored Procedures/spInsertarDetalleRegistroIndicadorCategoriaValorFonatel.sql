CREATE procedure [dbo].[spInsertarDetalleRegistroIndicadorCategoriaValorFonatel]
(
	   @lst TypeDetalleRegistroIndicadorCategoriaValorFonatel READONLY
)
AS
BEGIN TRY

INSERT INTO SITELP.FONATEL.DetalleRegistroIndicadorCategoriaValorFonatel(IdSolicitud,IdFormulario,IdIndicador,idCategoria,NumeroFila,Valor)
SELECT IdSolicitud,IdFormulario,IdIndicador,idCategoria,NumeroFila,Valor FROM @lst;

SELECT IdSolicitud,IdFormulario,IdIndicador,idCategoria,NumeroFila,Valor FROM SITELP.FONATEL.DetalleRegistroIndicadorCategoriaValorFonatel;

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