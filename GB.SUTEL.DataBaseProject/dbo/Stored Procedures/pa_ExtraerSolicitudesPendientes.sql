
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pa_ExtraerSolicitudesPendientes] 
	
AS
BEGIN

	SELECT IdArchivoExcel, IdSolicitudIndicador, IdOperador
    FROM  ArchivoExcel
	WHERE ArchivoExcelGenerado = 0
 
END