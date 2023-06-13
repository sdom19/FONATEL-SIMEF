CREATE PROCEDURE [dbo].[pa_ObtenerVariable]
	@pIdIndicador INT
AS
BEGIN
DECLARE @consulta VARCHAR(250)
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la variable

SET @consulta = 'SELECT idDetalleIndicadorVariable,
    idIndicador,
    NombreVariable,
    Descripcion,
	Estado 
    FROM DetalleIndicadorVariable
    WHERE Estado=1 '+
	IIF(@pIdIndicador=0,'', 'AND idIndicador=' + CAST(@pIdIndicador AS VARCHAR(10)) + ' ')
EXEC(@consulta)
END