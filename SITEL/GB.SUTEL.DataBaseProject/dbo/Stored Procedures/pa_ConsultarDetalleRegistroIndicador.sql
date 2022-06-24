-- =============================================
-- Author:		<AYNM>
-- Create date: <12/06/2019>
-- Description:	<para consultar detalle de registro indicador y actualizar historico>
-- =============================================
CREATE PROCEDURE [dbo].[pa_ConsultarDetalleRegistroIndicador]

 @IdDetalleRegistroindicador uniqueidentifier,
 @IdRegistroIndicador uniqueidentifier OUT

AS

BEGIN

SET NOCOUNT OFF;

SET @IdRegistroIndicador =  (SELECT [IdRegistroIndicador]      
                              FROM  [SITEL].[dbo].[DetalleRegistroIndicador]
                              WHERE [IdDetalleRegistroindicador]  = @IdDetalleRegistroindicador)

	SELECT @IdRegistroIndicador

END