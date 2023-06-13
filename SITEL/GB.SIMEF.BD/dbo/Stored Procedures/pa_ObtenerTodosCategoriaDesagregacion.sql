CREATE PROC [dbo].[pa_ObtenerTodosCategoriaDesagregacion]
@idCategoriaDesagregacion INT,
@codigo VARCHAR(30),
@IdEstadoRegistro INT,
@idTipoCategoria INT 
AS 

BEGIN 
-- =============================================
-- Author: Michael Hernández Cordero
-- Create date: 18-04-2023
-- Description:	Procedimiento utilizado para obtener Todas las Categorias de Desagregacion

DECLARE @consulta VARCHAR(1000)

SET @consulta='SELECT idCategoriaDesagregacion
      ,Codigo
      ,NombreCategoria
      ,CantidadDetalleDesagregacion
      ,idTipoDetalle
      ,IdTipoCategoria
      ,FechaCreacion
      ,UsuarioCreacion
      ,FechaModificacion
      ,UsuarioModificacion
      ,IdEstadoRegistro 
    FROM CategoriaDesagregacion
    WHERE 1=1 '+
    IIF(@idCategoriaDesagregacion=0,'',' AND idCategoriaDesagregacion='+ CAST(@idCategoriaDesagregacion AS VARCHAR(10))+' ') +
	IIF(@IdEstadoRegistro=0,'',' AND IdEstadoRegistro='+  CAST(@IdEstadoRegistro AS VARCHAR(10))+' ') +
	IIF(@idTipoCategoria=0,'',' AND idTipoCategoria='+ CAST(@idTipoCategoria AS VARCHAR(10))+' ') +
	
	IIF(@codigo='','',' AND UPPER(Codigo)='''+ UPPER( @codigo)+'''') 

EXEC(@consulta)

END