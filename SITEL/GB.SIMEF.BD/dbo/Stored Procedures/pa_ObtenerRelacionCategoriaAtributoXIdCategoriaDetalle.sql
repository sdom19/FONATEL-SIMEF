CREATE PROCEDURE [dbo].[pa_ObtenerRelacionCategoriaAtributoXIdCategoriaDetalle]
@IdCategoriaDetalle INT
AS
BEGIN
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la relación de categoria atributo por IdCategoriaDetalle


SELECT * FROM RelacionCategoriaAtributo WHERE IdCategoriaDesagregacionAtributo = @IdCategoriaDetalle
END