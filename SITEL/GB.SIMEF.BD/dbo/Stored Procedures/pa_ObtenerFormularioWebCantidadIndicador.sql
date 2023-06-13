CREATE PROC [dbo].[pa_ObtenerFormularioWebCantidadIndicador]
@IdFormulario INT
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el formulario web y la cantidad de indicador 

DECLARE @cantidadAct INT = (SELECT COUNT(*) FROM DetalleFormularioWeb df WHERE df.IdFormularioWeb=@IdFormulario AND Estado=1)
DECLARE @cantidadMax INT = (SELECT CantidadIndicador FROM FormularioWeb WHERE IdFormularioWeb=@IdFormulario)
SELECT @cantidadMax-@cantidadAct