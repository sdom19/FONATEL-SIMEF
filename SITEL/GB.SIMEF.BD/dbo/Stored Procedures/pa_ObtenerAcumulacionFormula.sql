 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener acumulación de fórmula
-- ============================================
CREATE PROCEDURE [dbo].[pa_ObtenerAcumulacionFormula]
	-- Add the parameters for the stored procedure here
	@pIdAcumulacionFormula INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @consulta VARCHAR(500)

    SET @consulta = '
		select 
			af.IdAcumulacionFormula, 
			af.Acumulacion, 
			af.Estado 
		from dbo.AcumulacionFormula af 
		where af.Estado=1 
	' + 
	+
    IIF(@pIdAcumulacionFormula=0,'',' and af.IdAcumulacionFormula='+ CAST(@pIdAcumulacionFormula AS VARCHAR(10))+' ')

	EXEC(@consulta)
END