-- =============================================
-- Author:		Grupo Babel
-- Create date: 10/03/2015
-- Description:	Consulta indicadores para la solicitud  
-- =============================================
CREATE PROCEDURE [dbo].[getListaIndicadoresSolicitud]
	@IdOperador int,
	@IdDireccion int,
	@IdFrecuencia int,
	@Filtrado bit,
	@DescripcionIndicador varchar(250)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@Filtrado = 1 AND @IdOperador = 0)
	BEGIN
		SELECT A.IdIndicador,
				   B.IdConstructor, 
				   A.NombreIndicador,
				   D.NombreFrecuencia 
			  FROM Indicador A
		INNER JOIN Constructor B 
				ON B.IdIndicador = A.IdIndicador 
		INNER JOIN INDICADORDIRECCION C
				ON C.IdIndicador = A.IdIndicador AND C.IdDireccion = @IdDireccion
		 LEFT JOIN Frecuencia D 
				ON D.IdFrecuencia = B.IdFrecuencia AND B.IdFrecuencia = @IdFrecuencia
			 WHERE A.Borrado = 0 AND
				   B.Borrado = 0 AND
				   A.NombreIndicador LIKE '%'+ @DescripcionIndicador + '%'
	END
	ELSE IF(@Filtrado = 1 AND @IdOperador = 1) 
	BEGIN
		SELECT A.IdIndicador, 
				   A.NombreIndicador,
				   B.IdConstructor,  
				   D.NombreFrecuencia
			  FROM Indicador A
		INNER JOIN Constructor B 
				ON B.IdIndicador = A.IdIndicador 
		INNER JOIN SolicitudConstructor F 
				ON B.IdConstructor = F.IdConstructor AND F.IdOperador = @IdOperador
		INNER JOIN INDICADORDIRECCION C 
				ON C.IdIndicador = A.IdIndicador AND C.IdDireccion = @IdDireccion
		 LEFT JOIN Frecuencia D 
				ON D.IdFrecuencia = B.IdFrecuencia AND B.IdFrecuencia = @IdFrecuencia
			 WHERE A.Borrado = 0 AND B.Borrado = 0 AND
				   A.NombreIndicador LIKE '%'+ @DescripcionIndicador + '%'
	END
	ELSE
	BEGIN

		--listado completo
		IF(@IdOperador = 0 OR @IdOperador is null)
		BEGIN
			SELECT A.IdIndicador,
				   B.IdConstructor, 
				   A.NombreIndicador,
				   D.NombreFrecuencia 
			  FROM Indicador A
		INNER JOIN Constructor B 
				ON B.IdIndicador = A.IdIndicador 
		INNER JOIN INDICADORDIRECCION C
				ON C.IdIndicador = A.IdIndicador AND C.IdDireccion = @IdDireccion
		 INNER JOIN Frecuencia D 
				ON D.IdFrecuencia = B.IdFrecuencia AND B.IdFrecuencia = @IdFrecuencia
			 WHERE A.Borrado = 0 AND
				   B.Borrado = 0
		END
		ELSE
		BEGIN
			SELECT A.IdIndicador, 
				   A.NombreIndicador,
				   B.IdConstructor,  
				   D.NombreFrecuencia
			  FROM Indicador A
		INNER JOIN Constructor B 
				ON B.IdIndicador = A.IdIndicador 
		INNER JOIN SolicitudConstructor F 
				ON B.IdConstructor = F.IdConstructor AND F.IdOperador = @IdOperador
		INNER JOIN INDICADORDIRECCION C 
				ON C.IdIndicador = A.IdIndicador AND C.IdDireccion = @IdDireccion
		INNER JOIN Frecuencia D 
				ON D.IdFrecuencia = B.IdFrecuencia AND B.IdFrecuencia = @IdFrecuencia
			 WHERE A.Borrado = 0 AND B.Borrado = 0
		END
		END
END
