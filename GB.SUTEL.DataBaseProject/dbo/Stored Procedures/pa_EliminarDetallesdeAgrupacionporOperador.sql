


-- =============================================
-- Author:		<>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[pa_EliminarDetallesdeAgrupacionporOperador]
	-- Add the parameters for the stored procedure here
	@IdConstructor UNIQUEIDENTIFIER,
	@IdCriterio  VARCHAR(50),
	@IdOperador VARCHAR(20)
AS
BEGIN
	
Begin Tran
Begin try


SELECT IdConstructorCriterio ConstructorCriterio  INTO EliminarDetallesdeAgrupacion 
	   from [dbo].[ConstructorCriterioDetalleAgrupacion] (NOLOCK)
	   where IdConstructor=@IdConstructor AND
	   IdCriterio=@IdCriterio AND
	   UltimoNivel=1 AND
	   IdOperador=@IdOperador

UPDATE Regla SET Borrado=1 
FROM  EliminarDetallesdeAgrupacion A
WHERE IdConstructorCriterio=ConstructorCriterio 

UPDATE NivelDetalleReglaEstadistica SET Borrado=1 
FROM  EliminarDetallesdeAgrupacion A
WHERE IdConstructorCriterioDetalleAgrupacion=ConstructorCriterio 

update [dbo].[ConstructorCriterioDetalleAgrupacion] set Borrado=1 WHERE IdConstructor=@IdConstructor AND IdCriterio=@IdCriterio AND IdOperador=@IdOperador

DROP TABLE EliminarDetallesdeAgrupacion;

Commit Tran;
SELECT 'TRUE' AS RESPUESTA
End try
Begin catch
	Rollback Tran;	
	SELECT 'FALSE' AS RESPUESTA
	--select ERROR_MESSAGE() as ErrorMessage;
End catch


END