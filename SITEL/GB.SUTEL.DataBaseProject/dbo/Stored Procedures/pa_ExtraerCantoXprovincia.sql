
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[pa_ExtraerCantoXprovincia] 
	@IdProvincia int
AS
BEGIN

	SELECT *
    FROM  Canton
	WHERE IdProvincia = @IdProvincia and  CANTON.NOMBRE != 'Todos' and CANTON.NOMBRE != 'No Aplica'	  				  
 
END