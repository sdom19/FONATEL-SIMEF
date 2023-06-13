


CREATE PROC [dbo].[pa_ObtenerReglaValidacion]

@idRegla INT,
@Codigo VARCHAR(30),
@idIndicador INT
AS
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener la regla de validación 

DECLARE @consulta VARCHAR(2000);
SET @consulta=
  'SELECT idReglaValidacion 
      ,Codigo 
      ,Nombre 
      ,Descripcion 
      ,idIndicador
      ,FechaCreacion 
      ,UsuarioCreacion 
      ,FechaModificacion 
      ,UsuarioModificacion 
      ,IdEstadoRegistro 
  FROM dbo.ReglaValidacion 
  WHERE IdEstadoRegistro!=4 '+
 IIF(@idRegla=0,'',' AND idReglaValidacion='+ CAST(@idRegla AS VARCHAR(10))+' ')+
 IIF(@idIndicador=0,'',' AND idIndicador='+ CAST(@idIndicador AS VARCHAR(10))+' ')+
 IIF(@Codigo='','',' AND Codigo='''+@Codigo+''' ')
 EXEC(@consulta)