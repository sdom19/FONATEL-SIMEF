
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pa_EliminarReglaEstadistica]
	-- Add the parameters for the stored procedure here
	@IdConstructor UNIQUEIDENTIFIER

AS
BEGIN
	
Begin Tran
Begin try

 DELETE FROM [dbo].[NivelDetalleReglaEstadistica]
 
 WHERE [IdConstructorCriterioDetalleAgrupacion] IN ( SELECT IdConstructorCriterio
	
													 FROM [dbo].[ConstructorCriterioDetalleAgrupacion]

													 WHERE IdConstructor = @IdConstructor)

Commit Tran;
SELECT 'TRUE' AS RESPUESTA
End try
Begin catch
	Rollback Tran;	
	SELECT 'FALSE' AS RESPUESTA
	--select ERROR_MESSAGE() as ErrorMessage;
End catch


END