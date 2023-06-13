
--EXEC spObtenerReglaValidacionApi @idCategoriaDesagregacion = 2323, @Valor = '10101',@tipoRegla = 4
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la regla de validación API

CREATE PROC [dbo].[pa_ObtenerReglaValidacionApi]
@idIndicadorActual INT = NULL,
@tipoRegla INT = NULL,
@idVariable INT = NULL,
@idCategoriaDesagregacion INT = NULL,
@idTipoCategoria INT = 0,
@Valor VARCHAR(2000) = NULL,
@idDetalleReglaValidacion INT = NULL
AS
BEGIN	
	
	IF @tipoRegla = 4 --Atributos validos
	BEGIN
		
		SELECT 
			dt.IdDetalleCategoriaTexto, dt.idCategoriaDesagregacion, dt.Codigo, dt.Etiqueta, dt.Estado
		FROM [RelacionCategoria] r 
		  INNER JOIN [RelacionCategoriaAtributo] ca ON r.IdRelacionCategoria = ca.idRelacionCategoriaId
		  INNER JOIN [DetalleCategoriaTexto] dt ON dt.IdDetalleCategoriaTexto = ca.IdDetalleCategoriaTextoAtributo
		  INNER JOIN ReglaAtributoValido ra ON ra.idCategoriaDesagregacion = @idCategoriaDesagregacion
				AND ra.IdDetalleCategoriaTexto = dt.idCategoriaDesagregacion 
				--AND ra.IdCategoriaDesagregacion = dt.idCategoriaDesagregacion 
				AND ra.IdDetalleReglaValidacion = @idDetalleReglaValidacion
		WHERE r.idCategoriaDesagregacion = @idCategoriaDesagregacion 
			AND r.IdEstadoRegistro = 2 
			AND dt.Estado = 1 
			AND ca.idCategoriaDesagregacion = @Valor

	END
	ELSE IF @tipoRegla = 5 --Regla secuencial
	BEGIN
		IF(@idTipoCategoria IN(2,3)) --Texto, Alfanumerico
		BEGIN
			SELECT t.idCategoriaDesagregacion, t.Etiqueta, t.Codigo 
			FROM DetalleCategoriaTexto t WHERE t.idCategoriaDesagregacion = @idCategoriaDesagregacion
			AND t.Estado = 1
		END
		ELSE IF(@idTipoCategoria = 1) --Numerico
		BEGIN
			SELECT t.idCategoriaDesagregacion, t.Minimo AS ValorMinimo, t.Maximo AS ValorMaximo
			FROM DetalleCategoriaNumerico t WHERE t.idCategoriaDesagregacion = @idCategoriaDesagregacion
			AND t.Estado = 1
		END
		ELSE IF(@idTipoCategoria = 4) --Fecha
		BEGIN
			SELECT t.idCategoriaDesagregacion, t.FechaMinima, t.FechaMaxima 
			FROM DetalleCategoriaFecha t WHERE t.idCategoriaDesagregacion = @idCategoriaDesagregacion
			AND t.Estado = 1
		END
	END
	ELSE IF @tipoRegla = 6 --Contra indicador salida
	BEGIN
		SELECT c.IdIndicador, Operador FROM DetalleReglaValidacion d
		INNER JOIN ReglaIndicadorSalida c
		ON d.IdDetalleReglaValidacion = c.IdDetalleReglaValidacion
		INNER JOIN OperadorAritmetico o
		ON d.IdOperadorAritmetico = o.IdOperadorAritmetico
		WHERE d.IdIndicador = @idIndicadorActual  
		AND d.IdDetalleIndicadorVariable = @idVariable
		AND IdTipoReglaValidacion = @tipoRegla
		AND d.Estado = 1
	END
	ELSE IF @tipoRegla = 7 --Contra indicador entrada salida
	BEGIN
		SELECT c.IdIndicador, Operador, c.IdDetalleIndicadorVariable AS idVariableComparar 
		FROM DetalleReglaValidacion d
		INNER JOIN ReglaComparacionEntradaSalida c
		ON d.IdDetalleReglaValidacion = c.IdDetalleReglaValidacion
		INNER JOIN OperadorAritmetico o
		ON d.IdOperadorAritmetico = o.IdOperadorAritmetico
		WHERE d.IdIndicador = @idIndicadorActual  
		AND d.IdDetalleIndicadorVariable = @idVariable
		AND IdTipoReglaValidacion = @tipoRegla
		AND d.Estado = 1
	END
END