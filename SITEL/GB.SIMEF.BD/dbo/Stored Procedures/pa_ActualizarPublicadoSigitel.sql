-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar publicado de indicador
-- =============================================
CREATE PROCEDURE  [dbo].[pa_ActualizarPublicadoSigitel]
   @pIdIndicador INT
  ,@pUsuarioModificacion VARCHAR(100)
  ,@pVisualizaSigitel BIT

AS

BEGIN TRAN;
UPDATE dbo.Indicador
   SET 
       UsuarioModificacion = @pUsuarioModificacion
      ,VisualizaSigitel =@pVisualizaSigitel
	  ,FechaModificacion=GETDATE()
 WHERE IdIndicador=@pIdIndicador
COMMIT TRAN;


 SELECT TOP (1000) IdIndicador
      ,Codigo
      ,Nombre
      ,IdTipoIndicador
      ,IdClasificacionIndicador
      ,IdGrupoIndicador
      ,Descripcion
      ,CantidadVariableDato
      ,CantidadCategoriaDesagregacion
      ,IdUnidadEstudio
      ,IdTipoMedida
      ,IdFrecuenciaEnvio
      ,Interno
      ,Solicitud
      ,Fuente
      ,Nota
      ,FechaCreacion
      ,UsuarioCreacion
      ,FechaModificacion
      ,UsuarioModificacion
      ,VisualizaSigitel
      ,IdEstadoRegistro
	  ,IdGraficoInforme
  FROM dbo.Indicador
   WHERE IdIndicador=@pIdIndicador