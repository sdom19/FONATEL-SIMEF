CREATE PROCEDURE [dbo].[pa_ObtenerDetalleReglaValidacion]
	@IdDetalleReglaValidacion INT,
	@IdRegla INT,
	@IdTipo INT,
	@IdOperador INT,
	@IdDetalleIndicador INT,
	@IdIndicador INT
AS
BEGIN
-- =============================================
-- Author:		Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el detalle regla validacion

	DECLARE @consulta VARCHAR(1000)		
	SET @consulta='SELECT [IdDetalleReglaValidacion]
      ,[IdReglaValidacion]
      ,[IdTipoReglaValidacion]
      ,[IdOperadorAritmetico]
	  ,[IdDetalleIndicadorVariable]
	  ,[IdIndicador]
      ,[Estado]
		FROM [DetalleReglaValidacion] 
		WHERE Estado!=0'+
		IIF(@IdDetalleReglaValidacion=0,'',' AND IdDetalleReglaValidacion='+ CAST(@IdDetalleReglaValidacion AS VARCHAR(10))+' ') +
		IIF(@IdRegla=0,'',' AND IdReglaValidacion='+ CAST(@IdRegla AS VARCHAR(10))+' ') +
		IIF(@IdTipo=0,'',' AND IdTipoReglaValidacion='+ CAST(@IdTipo AS VARCHAR(10))+' ') +
		IIF(@IdOperador=0,'',' AND IdOperadorAritmetico='+ CAST(@IdOperador AS VARCHAR(10))+' ') +
		IIF(@IdDetalleIndicador=0,'',' AND IdDetalleIndicadorVariable='+ CAST(@IdDetalleIndicador AS VARCHAR(10))+' ') +
		IIF(@IdIndicador=0,'',' AND IdIndicador='+ CAST(@IdIndicador AS VARCHAR(10))+' ') 
EXEC(@consulta)
END