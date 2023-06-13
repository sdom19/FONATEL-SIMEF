
CREATE PROCEDURE [SITEL].[pa_ActualizarRegistroIndicadorDetalle]
AS
BEGIN

-- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 27-03-2023
-- Description:	Procedimiento utilizado en el ETL para pasar la estructura de [formulario detalle] en registro indicador
-- =============================================

SELECT DISTINCT 
CAST(CONCAT(p.IdEnvioSolicitud ,b.IdFormularioWeb,g.IdIndicador )AS BIGINT) IdDetalleRegistroIndicadorFonatel,
CAST(CONCAT(p.IdEnvioSolicitud ,b.IdFormularioWeb) AS BIGINT) idRegistroIndicadorFonatel,
 p.IdEnvioSolicitud IdSolicitud
,b.IdFormularioWeb
,g.IdIndicador

--,ROW_NUMBER ( ) OVER(partition by a.IdSolicitud order by a.IdSolicitud) IdDetalleRegistroIndicador

,d.TituloHoja
,d.NotaEncargado
,d.NotaInformante
,g.Codigo CodigoIndicador 
,g.Nombre NombreIndicador
,0 CantidadFilas
FROM Solicitud a
INNER JOIN EstadoRegistro h
ON h.IdEstadoRegistro=a.IdEstadoRegistro


INNER JOIN Mes e 
ON e.IdMes=a.IdMes
INNER JOIN Anno f 
ON f.IdAnno=a.IdAnno


INNER JOIN DetalleSolicitudFormulario b
ON a.IdSolicitud=b.IdSolicitud
INNER JOIN FormularioWeb c

ON c.IdFormularioWeb=b.IdFormularioWeb
AND c.IdEstadoRegistro=2

INNER JOIN DetalleFormularioWeb d ON 
d.IdFormularioWeb=c.IdFormularioWeb
AND d.Estado=1

INNER JOIN Indicador g ON
d.IdIndicador=g.IdIndicador
AND g.IdEstadoRegistro=2

INNER JOIN EnvioSolicitud p
ON p.IdSolicitud=b.IdSolicitud
AND  p.Enviado=0




END