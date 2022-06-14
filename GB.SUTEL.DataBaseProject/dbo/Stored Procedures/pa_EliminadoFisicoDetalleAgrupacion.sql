


CREATE PROCEDURE [dbo].[pa_EliminadoFisicoDetalleAgrupacion]

AS
BEGIN
	
	DELETE FROM Regla WHERE Borrado=1;
	DELETE FROM NivelDetalleReglaEstadistica WHERE Borrado=1
	DELETE FROM ConstructorCriterioDetalleAgrupacion WHERE Borrado=1;
END