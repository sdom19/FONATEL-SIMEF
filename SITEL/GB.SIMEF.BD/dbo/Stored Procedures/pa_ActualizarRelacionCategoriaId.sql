-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para actualizar relación entre categorias de ID
-- ============================================
CREATE PROCEDURE [dbo].[pa_ActualizarRelacionCategoriaId]
@IdRelacion INT,
@IdCategoriaId VARCHAR(100),
@IdEstadoRegistro INT,
@OpcionEliminar BIT
AS


BEGIN TRAN;
MERGE RelacionCategoriaId AS TARGET
USING (VALUES(@IdRelacion,@IdCategoriaid,@IdEstadoRegistro))	
AS SOURCE(IdRelacion,IdCategoriaDesagregacion,IdEstadoRegistro)
ON SOURCE.IdRelacion = TARGET.IdRelacionCategoriaId AND 
SOURCE.idCategoriaDesagregacion = TARGET.idCategoriaDesagregacion

WHEN NOT MATCHED BY TARGET THEN
	INSERT (idRelacionCategoriaId,idCategoriaDesagregacion,IdEstadoRegistro)VALUES(IdRelacion,IdCategoriaDesagregacion,IdEstadoRegistro)
WHEN MATCHED THEN
UPDATE SET IdEstadoRegistro = SOURCE.IdEstadoRegistro;
	 --DELETE;

COMMIT TRAN
	SELECT 
	 IdRelacionCategoria,
	 Codigo,
	 Nombre,
	 CantidadCategoria,
	 idCategoriaDesagregacion,
	 IdEstadoRegistro, 
	 FechaCreacion, 
	 UsuarioCreacion, 
	 FechaModificacion, 
	 UsuarioModificacion, 
	 CantidadFila
	 FROM RelacionCategoria 
	WHERE IdRelacionCategoria=@IdRelacion