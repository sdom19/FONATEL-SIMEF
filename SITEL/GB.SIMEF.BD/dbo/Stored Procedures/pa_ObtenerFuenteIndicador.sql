CREATE PROCEDURE [dbo].[pa_ObtenerFuenteIndicador]
	@pIdFuenteIndicador INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- =============================================
    -- Author: Michael Hernández Cordero
    -- Create date: 18-04-2023
    -- Description:	Procedimiento utilizado para obtener la fuente indicador
	SET NOCOUNT ON;

	DECLARE @consulta VARCHAR(150)

	SET @consulta = 
    'SELECT 
		IdFuenteIndicador,
		Fuente,
		Estado 
	FROM FuenteIndicador 
	WHERE Estado=1 ' + 
	IIF(@pIdFuenteIndicador=0,'', 'AND IdFuenteIndicador=' + CAST(@pIdFuenteIndicador AS VARCHAR(10)) + ' ')

	 EXEC(@consulta)
END