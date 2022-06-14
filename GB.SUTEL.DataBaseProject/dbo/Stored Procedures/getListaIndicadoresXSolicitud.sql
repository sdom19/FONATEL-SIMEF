-- =============================================
-- Author:		Grupo Babel
-- Create date: 10/03/2015
-- Description:	Consulta indicadores para la solicitud  
-- =============================================
CREATE PROCEDURE [dbo].[getListaIndicadoresXSolicitud]
	@IdOperador int,
	@IdIndicador uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

			SELECT A.IdIndicador,
					E.IdOperador,
					B.IdConstructor
			  FROM Indicador A
		INNER JOIN Constructor B 
				ON B.IdIndicador = A.IdIndicador 
		INNER JOIN SolicitudConstructor E ON B.IdConstructor = e.IdConstructor
		INNER JOIN SolicitudIndicador F ON E.IdSolicitudIndicador = F.IdSolicitudIndicador 
			   AND F.IdSolicitudIndicador =  @IdIndicador
			 WHERE A.Borrado = 0 AND E.IdOperador = @IdOperador AND E.Borrado = 0

		
END
