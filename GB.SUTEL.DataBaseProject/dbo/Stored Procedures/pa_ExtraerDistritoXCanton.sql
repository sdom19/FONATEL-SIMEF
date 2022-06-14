
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[pa_ExtraerDistritoXCanton] 
	@IdCanton int
AS
BEGIN

	SELECT *
    FROM  Distrito
	WHERE IdCanton = @IdCanton and  Distrito.NOMBRE != 'Todos' and Distrito.NOMBRE != 'No Aplica'	  				  
 
END