 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener detalle de categoria de formulario web
-- ============================================
CREATE PROCEDURE [dbo].[pa_ObtenerDetalleFormularioWeb]
	@idDetalle INT,
	@idFormulario INT,
	@idIndicador INT
AS
BEGIN
	DECLARE @consulta VARCHAR(1000)		
	SET @consulta='select a.IdDetalleFormularioWeb
		,a.idFormularioWeb
		,a.idIndicador 
		,a.TituloHoja 
		,a.NotaInformante
		,a.NotaEncargado
		,a.Estado
		from DetalleFormularioWeb a
		inner join Indicador b on 
		a.IdIndicador=b.IdIndicador 
		and b.IdEstadoRegistro!=4
		where a.estado!=0 '+
		IIF(@idDetalle=0,'',' and a.IdDetalleFormularioWeb='+ CAST(@idDetalle AS VARCHAR(10))+' ') +
		IIF(@idFormulario=0,'',' and a.idFormularioWeb='+ CAST(@idFormulario AS VARCHAR(10))+' ') +
		IIF(@idIndicador=0,'',' and a.idIndicador='+ CAST(@idIndicador AS VARCHAR(10))+' ')

EXEC(@consulta)
END