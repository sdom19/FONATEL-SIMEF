
CREATE PROCEDURE [SITEL].[pa_ActualizarRegistroIndicadorDetalleVariable]
AS 
BEGIN
-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 27-03-2023
-- Description:	Procedimiento utilizado en el ETL para pasar la estructura de [formulario variables] en registro indicador
-- =============================================
SELECT DISTINCT
    CAST(CONCAT(g.IdEnvioSolicitud ,c.IdFormularioWeb,a.IdIndicador )AS BIGINT) IdDetalleRegistroIndicadorFonatel,
    CAST(CONCAT(g.IdEnvioSolicitud, d.IdFormularioWeb, a.IdIndicador,a.IdDetalleIndicadorVariable) AS BIGINT) IdDetalleRegistroIndicadorVariableFonatel,
	g.IdEnvioSolicitud IdSolicitud, 
	c.IdFormularioWeb,
	c.IdIndicador,
	a.IdDetalleIndicadorVariable idVariable,
	a.NombreVariable, 
	a.Descripcion

	FROM
	DetalleIndicadorVariable a
	INNER JOIN DetalleFormularioWeb c
	ON c.IdIndicador=a.IdIndicador
	and c.Estado=1
	INNER JOIN FormularioWeb d
	ON d.IdFormularioWeb=c.IdFormularioWeb
	AND d.IdEstadoRegistro=2
	INNER JOIN DetalleSolicitudFormulario f
	ON f.IdFormularioWeb=d.IdFormularioWeb
	and f.Estado=1
	INNER JOIN EnvioSolicitud g 
	ON g.IdSolicitud=f.IdSolicitud
	AND g.Enviado=0
	INNER JOIN Solicitud h ON
	h.IdSolicitud=g.IdSolicitud
	AND h.IdEstadoRegistro=2
	INNER JOIN Indicador i ON
	i.IdIndicador=a.IdIndicador
	AND i.IdEstadoRegistro=2
	WHERE a.Estado=1
END