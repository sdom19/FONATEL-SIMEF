
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[pa_ExtraerGenero] 
	
AS
BEGIN

	SELECT *
    FROM  Genero
	WHERE   Genero.Descripcion != 'Todos' and Genero.Descripcion != 'No Aplica'	  				  
 
END