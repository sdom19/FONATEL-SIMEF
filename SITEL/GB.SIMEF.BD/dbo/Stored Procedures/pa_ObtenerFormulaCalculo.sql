CREATE PROC [dbo].[pa_ObtenerFormulaCalculo]
@IdFormula INT

AS 

BEGIN 
-- =============================================
-- Author:		Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la formula de cálculo

DECLARE @consulta VARCHAR(1000)

SET @consulta='SELECT IdFormulaCalculo
      ,Codigo
      ,Nombre
      ,IdIndicador
      ,IdDetalleIndicadorVariable
      ,Descripcion
      ,IdFrecuenciaEnvio
      ,NivelCalculoTotal
      ,UsuarioModificacion
      ,FechaCreacion
      ,UsuarioCreacion
      ,FechaModificacion
      ,IdEstadoRegistro
	  ,FechaCalculo
	  ,Formula
	  ,IdJob
  FROM dbo.FormulaCalculo
   WHERE IdEstadoRegistro!=4 '+
    IIF(@IdFormula=0,'',' AND IdFormulaCalculo='+ CAST(@IdFormula AS VARCHAR(10))+' ')

EXEC(@consulta)

END