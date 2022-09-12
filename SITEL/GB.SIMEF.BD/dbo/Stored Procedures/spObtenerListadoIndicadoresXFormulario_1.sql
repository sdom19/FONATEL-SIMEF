CREATE procedure [dbo].[spObtenerListadoIndicadoresXFormulario]
@idFormulario int
as
SELECT STRING_AGG(UPPER(C.Nombre),', ') Listado FROM FormularioWeb A
INNER JOIN DetalleFormularioWeb B
ON A.idFormulario=B.idFormulario
INNER JOIN Indicador C
ON C.idIndicador=B.idIndicador
where a.idFormulario=@idFormulario