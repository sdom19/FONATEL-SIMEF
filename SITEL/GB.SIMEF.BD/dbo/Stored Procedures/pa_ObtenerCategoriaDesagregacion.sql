 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Obtener categorías de desagregación
-- ============================================
CREATE PROCEDURE [dbo].[pa_ObtenerCategoriaDesagregacion]
@idCategoria INT,
@codigo VARCHAR(30),
@IdEstadoRegistro INT,
@idTipoCategoria INT 
AS 

BEGIN 
DECLARE @consulta VARCHAR(1000)

SET @consulta='SELECT idCategoriaDEsagregacion
      ,Codigo
      ,NombreCategoria
      ,CantidadDetalleDesagregacion
      ,IdTipoDetalleCategoria
      ,IdTipoCategoria
      ,FechaCreacion
      ,UsuarioCreacion
      ,FechaModificacion
      ,UsuarioModificacion
      ,IdEstadoRegistro 
    FROM CategoriaDesagregacion
    where IdEstadoRegistro!=4 '+
    IIF(@idCategoria=0,'',' and idCategoriaDEsagregacion='+ CAST(@idCategoria AS VARCHAR(10))+' ') +
	IIF(@IdEstadoRegistro=0,'',' and IdEstadoRegistro='+ CAST(@IdEstadoRegistro AS VARCHAR(10))+' ') +
	IIF(@idTipoCategoria=0,'',' and idTipoCategoria='+ CAST(@idTipoCategoria AS VARCHAR(10))+' ') +
	
	IIF(@codigo='','',' and upper(Codigo)='''+ UPPER( @codigo)+'''')+
	' order by FechaCreacion desc'
EXEC(@consulta)

END