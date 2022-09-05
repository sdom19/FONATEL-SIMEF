create procedure [dbo].[spObtenerListadoIndicadoresXFormulario]
@idFormulario int
as
SELECT UPPER(C.Nombre) Listado FROM FormularioWeb A
INNER JOIN DetalleFormularioWeb B
ON A.idFormulario=B.idFormulario
INNER JOIN Indicador C
ON C.idIndicador=B.idIndicador
where a.idFormulario=@idFormulario