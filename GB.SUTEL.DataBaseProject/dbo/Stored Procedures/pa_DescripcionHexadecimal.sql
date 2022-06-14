
-- =============================================
-- Author:		<>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pa_DescripcionHexadecimal]
	-- Add the parameters for the stored procedure here
	@IdOperadorCopia VARCHAR(20),
	@IdOperadorOriginal VARCHAR(20),
	@descAgrupacion NVARCHAR(600),
	@desDetalleAgrupacion VARCHAR(250)

AS
BEGIN
		Declare @DescH varbinary (250)
Begin Tran
Begin try

 set @DescH = (SELECT TOP 1 [DescHexa]
  FROM [dbo].[DetalleAgrupacion] WHERE 
  IdAgrupacion=(SELECT  [IdAgrupacion]  FROM [dbo].[Agrupacion] WHERE DescAgrupacion=@descAgrupacion) 
  AND IdOperador=@IdOperadorCopia AND DescDetalleAgrupacion=@desDetalleAgrupacion 
  AND DescHexa = (SELECT TOP 1 [DescHexa]
  FROM [dbo].[DetalleAgrupacion] 
  WHERE IdAgrupacion=(SELECT  [IdAgrupacion]  FROM [dbo].[Agrupacion] WHERE DescAgrupacion=@descAgrupacion) AND IdOperador=@IdOperadorOriginal AND DescDetalleAgrupacion=@desDetalleAgrupacion))

Commit Tran;
SELECT 'TRUE' AS RESPUESTA
End try
Begin catch
	Rollback Tran;	
	SELECT 'FALSE' AS RESPUESTA
	--select ERROR_MESSAGE() as ErrorMessage;
End catch


END