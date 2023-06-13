CREATE PROC [dbo].[pa_ObtenerIndicadorFonatel]
@pIdIndicador INT,
@pCodigo VARCHAR(30),
@pIdTipoIndicador INT,
@pIdClasificacion INT,
@pIdGrupo INT,
@pIdGraficoInforme INT,
@pIdUnidadEstudio INT,
@pIdTipoMedida INT,
@pIdFrecuencia INT,
@pIdEstadoRegistro INT
AS 

BEGIN 
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener los indicadores

DECLARE @consulta VARCHAR(1000)

SET @consulta='SELECT [idIndicador]
      ,[Codigo]
      ,[Nombre]
      ,[IdTipoIndicador]
      ,[IdClasificacionIndicador]
      ,[idGrupoIndicador]
      ,[Descripcion]
      ,[CantidadVariableDato]
      ,[CantidadCategoriaDesagregacion]
      ,[IdUnidadEstudio]
      ,[idTipoMedida]
      ,[IdFrecuenciaEnvio]
      ,[Interno]
      ,[Solicitud]
      ,[Fuente]
      ,[Nota]
      ,[FechaCreacion]
      ,[UsuarioCreacion]
      ,[FechaModificacion]
      ,[UsuarioModificacion]
      ,[VisualizaSigitel]
      ,[IdEstadoRegistro]
	  ,[IdGraficoInforme]
   FROM [Indicador]
   WHERE IdEstadoRegistro!=4 '+
    IIF(@pIdIndicador=0,'',' AND idIndicador='+ CAST(@pIdIndicador AS VARCHAR(10))+' ') +
	IIF(@pIdTipoIndicador=0,'',' AND IdTipoIndicador='+ CAST(@pIdTipoIndicador AS VARCHAR(10))+' ') +
	IIF(@pIdClasificacion=0,'',' AND IdClasificacionIndicador='+ CAST(@pIdClasificacion AS VARCHAR(10))+' ') +
	IIF(@pIdGrupo=0,'',' AND idGrupoIndicador='+ CAST(@pIdGrupo AS VARCHAR(10))+' ') +
	IIF(@pIdGraficoInforme=0,'',' AND IdGraficoInforme='+ CAST(@pIdGraficoInforme AS VARCHAR(10))+' ') +
	IIF(@pIdUnidadEstudio=0,'',' AND IdUnidadEstudio='+ CAST(@pIdUnidadEstudio AS VARCHAR(10))+' ') +
	IIF(@pIdTipoMedida=0,'',' AND idTipoMedida='+ CAST(@pIdTipoMedida AS VARCHAR(10))+' ') +
	IIF(@pIdFrecuencia=0,'',' AND IdFrecuenciaEnvio='+ CAST(@pIdFrecuencia AS VARCHAR(10))+' ') +
	IIF(@pIdEstadoRegistro=0,'',' AND IdEstadoRegistro='+ CAST(@pIdEstadoRegistro AS VARCHAR(10))+' ') +
	IIF(@pCodigo='','',' AND UPPER(TRIM(Codigo))='''+ UPPER(TRIM(@pCodigo))+'''') 

EXEC(@consulta)

END