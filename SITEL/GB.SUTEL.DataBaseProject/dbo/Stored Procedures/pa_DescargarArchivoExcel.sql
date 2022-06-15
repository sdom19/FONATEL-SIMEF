
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[pa_DescargarArchivoExcel]
	
	@IdOperador varchar(20),
	@IdSolicitudIndicador uniqueidentifier
AS
BEGIN
	 SELECT top 1 ArchivoExcelBytes
	 FROM ArchivoExcel	 
	 WHERE IdSolicitudIndicador = @IdSolicitudIndicador and IdOperador = @IdOperador and ArchivoExcelBytes is not null
END