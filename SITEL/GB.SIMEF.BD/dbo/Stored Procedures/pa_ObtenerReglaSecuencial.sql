/****** Script for SelectTopNRows command from SSMS  ******/


CREATE PROCEDURE [dbo].[pa_ObtenerReglaSecuencial]
@IdDetalleReglaValidacion INT 
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la regla de Secuencial 

SELECT [IdReglaSecuencial]
      ,[IdDetalleReglaValidacion]
      ,[IdCategoriaDesagregacion]
  FROM [dbo].[ReglaSecuencial]
  WHERE IdDetalleReglaValidacion=@IdDetalleReglaValidacion