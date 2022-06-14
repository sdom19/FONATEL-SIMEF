

-- =============================================
-- Author:		<>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pa_EditarIdConstructorDetallePadreporOperador]
	-- Add the parameters for the stored procedure here
	@IdConstructor UNIQUEIDENTIFIER,
	@IdCriterio  VARCHAR(50),
	@IdOperador VARCHAR(20)

AS
BEGIN
	
Begin Tran
Begin try

 UPDATE [dbo].[ConstructorCriterioDetalleAgrupacion] SET IdConstructorDetallePadre = NULL
 WHERE IdConstructor=@IdConstructor AND IdCriterio=@IdCriterio AND IdOperador=@IdOperador and Borrado=0

Commit Tran;
SELECT 'TRUE' AS RESPUESTA
End try
Begin catch
	Rollback Tran;	
	SELECT 'FALSE' AS RESPUESTA
	--select ERROR_MESSAGE() as ErrorMessage;
End catch


END