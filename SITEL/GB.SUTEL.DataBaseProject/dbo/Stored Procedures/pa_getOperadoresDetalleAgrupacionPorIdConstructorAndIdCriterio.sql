

CREATE PROCEDURE [dbo].[pa_getOperadoresDetalleAgrupacionPorIdConstructorAndIdCriterio] 				
				@IdConstructor uniqueidentifier,
				@IdCriterio varchar(50)
AS

SELECT DISTINCT [ConstructorCriterioDetalleAgrupacion].IdOperador, [dbo].[Operador].NombreOperador,[dbo].[Operador].Estado
 FROM [dbo].[ConstructorCriterioDetalleAgrupacion] 
 INNER JOIN [dbo].[Operador]
 ON [Operador].IdOperador=[ConstructorCriterioDetalleAgrupacion].IdOperador
 where [dbo].[ConstructorCriterioDetalleAgrupacion].IdConstructor=@IdConstructor AND [dbo].[ConstructorCriterioDetalleAgrupacion].IdCriterio=@IdCriterio and Borrado=0