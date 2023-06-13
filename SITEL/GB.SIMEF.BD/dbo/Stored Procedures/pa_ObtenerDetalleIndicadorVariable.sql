CREATE PROCEDURE [dbo].[pa_ObtenerDetalleIndicadorVariable]
    @pIdDetalleIndicador INT,
	@pIdIndicador INT
AS
BEGIN
-- =============================================
-- Author:		Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el detalle indicador variable
DECLARE @consulta VARCHAR(250)

SET @consulta = 'SELECT idDetalleIndicadorVariable,
    idIndicador,
    NombreVariable,
    Descripcion,
	Estado
    FROM DetalleIndicadorVariable
    WHERE Estado=1 '+
    IIF(@pIdDetalleIndicador=0,'', 'AND idDetalleIndicadorVariable=' + CAST(@pIdDetalleIndicador AS VARCHAR(10)) + ' ') +
	IIF(@pIdIndicador=0,'', 'AND idIndicador=' + CAST(@pIdIndicador AS VARCHAR(10)) + ' ')
EXEC(@consulta)
END