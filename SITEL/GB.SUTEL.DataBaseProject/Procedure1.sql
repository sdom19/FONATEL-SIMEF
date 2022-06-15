Create PROCEDURE [dbo].[pa_getNombreIndicadorByIdSolicitudConstructor]

	@IdSolicitudConstructor UNIQUEIDENTIFIER
	
AS
BEGIN

	SELECT [NombreIndicador]

    FROM [dbo].[Indicador] I

    INNER JOIN [dbo].[Constructor] C
 
    ON I.IdIndicador = C.IdIndicador
	
	INNER JOIN SolicitudConstructor SC
 
	ON C.IdConstructor = SC.IdConstructor
  
	WHERE [IdSolicitudContructor] = @IdSolicitudConstructor


END