CREATE PROC [dbo].[pa_ObtenerDetalleRelacionCategoria]
@idDetalleRelacionCategoria INT,
@IdRelacionCategoria INT,
@idCategoriaAtributo INT

AS

BEGIN 
-- =============================================
-- Author:		Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener el detalle relacion categoria
DECLARE @consulta VARCHAR(2000)

SET @consulta='SELECT A.idDetalleRelacionCategoria
      ,A.IdRelacionCategoria
      ,A.idCategoriaDesagregacion
      ,A.Estado 
   FROM DetalleRelacionCategoria A
   INNER JOIN CategoriaDesagregacion C
   ON C.IdCategoriaDesagregacion=A.IdCategoriaDesagregacion
   --AND IdEstadoRegistro=2
   WHERE A.Estado=1 '+
    IIF(@IdRelacionCategoria=0,'',' AND A.IdRelacionCategoria='+ CAST(@IdRelacionCategoria AS VARCHAR(10))+' ') +
	IIF(@idDetalleRelacionCategoria=0,'',' AND A.idDetalleRelacionCategoria='+ CAST(@idDetalleRelacionCategoria AS VARCHAR(10))+' ') +
	IIF(@idCategoriaAtributo=0,'',' AND A.idCategoriaDesagregacion='+ CAST(@idCategoriaAtributo AS VARCHAR(10))+'') ;
EXEC(@consulta)

END