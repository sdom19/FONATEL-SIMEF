﻿
CREATE PROCEDURE [dbo].[SyncOperadores]
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SCI.DBO.Operador (IdOperador, NombreOperador, Estado)
	SELECT IDREGULADO,Nombre,1 FROM  REGULADOS.DBO.OPERADORES 
	WHERE Estado = 'Activo-OFICIAL'
	ORDER BY CODIGOOPERADOR
    
	--if @@ERROR > 0
		

END

