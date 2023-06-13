
CREATE PROC [dbo].[pa_ObtenerNombreVariableDetalleReglaValidacion]
	@IdBusqueda INT,
	@Tipo INT
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el nombre variable detalle y regla validación

IF (@Tipo=1)
BEGIN

	SELECT DISTINCT a.NombreCategoria NombreVariable 
	FROM CategoriaDesagregacion a
	INNER JOIN ReglaAtributoValido b
	ON a.IdCategoriaDesagregacion=b.IdCategoriaDesagregacion
	WHERE b.IdDetalleReglaValidacion=@IdBusqueda

END

ELSE IF(@Tipo = 2)
BEGIN

	SELECT DISTINCT a.NombreCategoria NombreVariable 
	FROM CategoriaDesagregacion a
	INNER JOIN ReglaSecuencial b
	ON a.IdCategoriaDesagregacion=b.IdCategoriaDesagregacion
	WHERE b.IdDetalleReglaValidacion=@IdBusqueda

END

ELSE IF (@Tipo=0)

BEGIN
	SELECT NombreVariable FROM DetalleIndicadorVariable WHERE IdDetalleIndicadorVariable=@IdBusqueda;
END