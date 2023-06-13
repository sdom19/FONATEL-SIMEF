CREATE PROC [dbo].[pa_ObtenerListadoIndicadorXFormulario]
@idFormulario INT
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el listado del indicador por formulario

SELECT STRING_AGG(UPPER(C.Nombre),', ') Listado FROM FormularioWeb A
INNER JOIN DetalleFormularioWeb B
ON A.IdFormularioWeb=B.IdFormularioWeb
INNER JOIN Indicador C
ON C.idIndicador=B.idIndicador
WHERE a.IdFormularioWeb=@idFormulario AND b.Estado=1;