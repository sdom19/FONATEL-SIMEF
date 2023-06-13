/****** Script for SelectTopNRows command from SSMS  ******/

CREATE PROCEDURE [dbo].[pa_ObtenerReglaAtributoValido]
@IdDetalleReglaValidacion INT 
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la regla de atributo válido

SELECT
  0 IdReglaAtributoValido
  ,IdDetalleReglaValidacion
    ,IdCategoriaDesagregacion
	, 0 IdDetalleCategoriaTexto
       ,STRING_AGG([IdDetalleCategoriaTexto],',') idAtributoString 
  FROM [ReglaAtributoValido]
 WHERE IdDetalleReglaValidacion=@IdDetalleReglaValidacion
	GROUP BY 
      
       IdCategoriaDesagregacion
	  ,IdDetalleReglaValidacion