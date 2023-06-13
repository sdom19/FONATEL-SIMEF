
CREATE PROCEDURE [dbo].[pa_ObtenerListaIndicadorXFormulario]
	@idFormulario INT
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el listado del indicador por formulario

SELECT 
c.idIndicador, 
	c.Codigo, 
	c.Nombre,
	c.IdTipoIndicador,
	c.IdClasificacionIndicador,
	c.IdGrupoIndicador,
	c.Descripcion,
	c.CantidadVariableDato,
	c.CantidadCategoriaDesagregacion,
	c.IdUnidadEstudio, 
	c.idTipoMedida, 
	c.IdFrecuenciaEnvio,
	c.Interno,
	c.Solicitud,
	c.Fuente,
	c.Nota,
	c.FechaCreacion,
	c.UsuarioCreacion,
	c.FechaModificacion,
	c.UsuarioModificacion,
	c.VisualizaSigitel,
	c.IdEstadoRegistro,
	c.IdGraficoInforme
FROM FormularioWeb A
	INNER JOIN DetalleFormularioWeb B
	ON A.IdFormularioWeb=B.IdFormularioWeb
	INNER JOIN Indicador C
	ON C.idIndicador=B.idIndicador
	WHERE a.IdFormularioWeb=@idFormulario AND b.Estado=1 AND C.IdEstadoRegistro != 4;