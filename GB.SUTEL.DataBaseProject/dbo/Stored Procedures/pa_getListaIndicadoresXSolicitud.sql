-- =============================================
-- Author:		Grupo Babel
-- Create date: 10/03/2015
-- Description:	Consulta indicadores para la solicitud  
-- =============================================
CREATE PROCEDURE [dbo].[pa_getListaIndicadoresXSolicitud]
	@IdOperador varchar(50),
	@IdIndicador uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

			SELECT A.IDINDICADOR,
					E.IDOPERADOR,
					B.IDCONSTRUCTOR
			  FROM INDICADOR A
		INNER JOIN CONSTRUCTOR B 
				ON B.IDINDICADOR = A.IDINDICADOR 
		INNER JOIN SOLICITUDCONSTRUCTOR E ON B.IDCONSTRUCTOR = e.IDCONSTRUCTOR
		INNER JOIN SOLICITUDINDICADOR F ON E.IDSOLICITUDINDICADOR = F.IDSOLICITUDINDICADOR 
			   AND F.IDSOLICITUDINDICADOR =  @IdIndicador
			 WHERE A.BORRADO = 0 AND E.IDOPERADOR = @IdOperador AND E.BORRADO = 0

		
END