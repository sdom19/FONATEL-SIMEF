
-- =============================================
-- Author:		<>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pa_EliminarConstructorCriterio]
	-- Add the parameters for the stored procedure here
	@IdConstructor UNIQUEIDENTIFIER,
	@IdCriterio  VARCHAR(50)

AS
BEGIN
	
Begin Tran
Begin try

 DELETE FROM [dbo].[ConstructorCriterio] 
 WHERE IdConstructor=@IdConstructor AND IdCriterio=@IdCriterio 

Commit Tran;
SELECT 'TRUE' AS RESPUESTA
End try
Begin catch
	Rollback Tran;	
	SELECT 'FALSE' AS RESPUESTA
	--select ERROR_MESSAGE() as ErrorMessage;
End catch


END