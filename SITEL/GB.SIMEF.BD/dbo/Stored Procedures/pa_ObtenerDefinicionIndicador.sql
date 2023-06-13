 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener definción de Indicador
-- ============================================
CREATE PROCEDURE [dbo].[pa_ObtenerDefinicionIndicador]
@idIndicador INT,
@IdEstadoRegistro INT
AS 

BEGIN 
DECLARE @consulta VARCHAR(1000)

SET @consulta='select 
	a.idIndicador idDefinicionIndicador
	,b.Fuente
	,b.Nota
	,isnull(b.IdEstadoRegistro,0) IdEstadoRegistro
	,b.Definicion
	from Indicador a
	left join DefinicionIndicador b
	on a.idIndicador=b.idDefinicionIndicador
	and b.IdEstadoRegistro!=4
	where a.IdEstadoRegistro=2 
	and a.idClasificacionIndicador not in (1,4)'+
	IIF(@idIndicador=0,'',' and a.idIndicador='+ CAST(@idIndicador AS VARCHAR(10))+' ') +
	IIF(@IdEstadoRegistro=0,'',' and b.IdEstadoRegistro='+ CAST(@IdEstadoRegistro AS VARCHAR(10))+' ');

EXEC(@consulta)

END