-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar relación entre categorias de atributo
-- ============================================
CREATE PROCEDURE [dbo].[pa_ActualizarRelacionCategoriaAtributo]
        @idRelacion INT,
        @IdCategoriaId VARCHAR(50),
        @IdcategoriaAtributo INT,
        @IdcategoriaAtributoDetalle INT

AS

MERGE RelacionCategoriaAtributo AS TARGET
USING (VALUES(  @idRelacion,@IdCategoriaId,@IdcategoriaAtributo,@IdcategoriaAtributoDetalle))	
AS SOURCE(idRelacion,IdCategoriaId,IdcategoriaAtributo,IdcategoriaAtributoDetalle)
ON SOURCE.IdRelacion = TARGET.IdRelacioncategoriaid AND 
SOURCE.IdCategoriaId = TARGET.IdCategoriadesagregacion AND
SOURCE.IdcategoriaAtributo =TARGET.IdcategoriaDesagregacionAtributo

WHEN NOT MATCHED BY TARGET THEN
	INSERT (idRelacionCategoriaId,idCategoriaDesagregacion,IdCategoriaDesagregacionAtributo,IdDetalleCategoriaTextoAtributo)
	VALUES(idRelacion,IdCategoriaId,IdcategoriaAtributo,IdcategoriaAtributoDetalle)
WHEN MATCHED THEN
UPDATE SET IdDetalleCategoriaTextoAtributo = SOURCE.IdcategoriaAtributoDetalle;
	 --DELETE;

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