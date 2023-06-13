 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para eliminar detalle de registro de inidcador categoria atributo detalle
-- ============================================
CREATE PROCEDURE [dbo].[pa_EliminarRelacionCategoriaAtributoDetalle]
@IdRelacion INT,
@IdCategoriaId VARCHAR(50)
AS 

BEGIN    
        UPDATE RelacionCategoriaId SET IdEstadoRegistro = 4 WHERE idRelacionCategoriaId = @IdRelacion AND idCategoriaDesagregacion = @IdCategoriaId;

END

 

SELECT 
     idRelacionCategoria,
     Codigo,
     Nombre,
     CantidadCategoria,
     IdCategoriaDesagregacion,
     IdEstadoRegistro, 
     FechaCreacion, 
     UsuarioCreacion, 
     FechaModificacion, 
     UsuarioModificacion, 
     CantidadFila 
     FROM RelacionCategoria 
WHERE IdRelacionCategoria=@IdRelacion