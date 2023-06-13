CREATE PROC [dbo].[pa_ObtenerIndicadorparaGenerarURL]
@idIndicador INT,
@IdEstadoRegistro INT
AS 
BEGIN 
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la URL del indicador 

DECLARE @consulta VARCHAR(1000)

SET @consulta='SELECT a.IdIndicador,
a.Codigo, 
a.Nombre, 
a.IdTipoIndicador,
a.IdClasificacionIndicador,
a.IdGraficoInforme,
a.IdGrupoIndicador, 
a.Descripcion,
a.CantidadVariableDato,
a.CantidadCategoriaDesagregacion,
a.IdUnidadEstudio,
a.IdTipoMedida,
a.IdFrecuenciaEnvio,
a.Interno,
a.Solicitud,
a.Fuente,
a.Nota,
a.FechaCreacion,
a.UsuarioCreacion,
a.FechaModificacion,
a.UsuarioModificacion,
a.VisualizaSigitel,
a.IdEstadoRegistro,
b.IdDefinicionIndicador,
b.Fuente,
b.Nota,
ISNULL(b.IdEstadoRegistro,0) IdEstadoRegistro,
b.Definicion
	FROM Indicador a
	inner join IndicadorResultado c
	on a.IdIndicador=c.IdIndicador
	LEFT JOIN DefinicionIndicador b
	ON a.idIndicador=b.idDefinicionIndicador
	AND b.IdEstadoRegistro!=4
	WHERE a.IdEstadoRegistro=2 
	AND a.VisualizaSigitel=1
	AND a.idClasificacionIndicador NOT IN (1,4)'+
	IIF(@idIndicador=0,'',' AND a.idIndicador='+ CAST(@idIndicador AS VARCHAR(10))+' ') +
	IIF(@IdEstadoRegistro=0,'',' AND b.IdEstadoRegistro='+ CAST(@IdEstadoRegistro AS VARCHAR(10))+' ');

EXEC(@consulta)

END