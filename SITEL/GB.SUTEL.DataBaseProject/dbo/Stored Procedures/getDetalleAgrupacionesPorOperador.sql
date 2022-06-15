
CREATE PROCEDURE getDetalleAgrupacionesPorOperador 
				@IdOperador varchar(20),
				@IdSolicitudIndicador uniqueidentifier
AS

SELECT DA.IdDetalleAgrupacion as Id_Detalle_Agrupacion,
	   AG.DescAgrupacion  +'/'+ DA.DescDetalleAgrupacion  as Nombre_Detalle_Agrupacion,
	   AG.DescAgrupacion as Nombre_Agrupacion, 
	   OPE.NombreOperador as Nombre_Operador, 
	   CR.DescCriterio as Nombre_Criterio,
	   FR.IdFrecuencia as ID_Frecuencia,	   
	   FR.NombreFrecuencia as Nombre_Frecuencia,
	   FR.CantidadMeses as Cantidad_Meses_Frecuencia,	   
	   DE.IdFrecuencia as ID_Desglose,	
	   DE.NombreFrecuencia as Nombre_Desglose,
	   DE.CantidadMeses as Cantidad_Meses_Desglose,	   	      
	   IND.IdIndicador as ID_Indicador,
	   IND.NombreIndicador as Nombre_Indicador,
	   NIV.DescNivel as Nivel,
	   REG.ValorLimiteInferior as Valor_Inferior,
	   REG.ValorLimiteSuperior as Valor_Superior,
	   TVA.IdTipoValor as Id_Tipo_Valor,
	   TVA.Descripcion as Tipo_Valor,
	   TVA.FORMATO as Valor_Formato,
	   CCDA.IdConstructorCriterio as Id_ConstructorCriterio,
	   CCDALJ.IdConstructorCriterio as Id_Padre_ConstructorCriterio,
	   DALJ.IdDetalleAgrupacion as Id_Padre_Detalle_Agrupacion,
	   DALJ.DescDetalleAgrupacion as Nombre_Padre_Detalle_Agrupacion,
	   SOIN.FechaInicio as Fecha_Inicio,	   
	   SOIN.FechaFin as Fecha_Fin,	   
	   CC.Ayuda as Constructor_Criterio_Ayuda,
	   CR.DescCriterio as Descripcion_Criterio,	   
	   SOCO.IdSolicitudIndicador as Id_Solicitud_Indicador,
	   SOCO.IdSolicitudContructor as Id_Solicitud_Constructor,
	   DIR.Nombre as Nombre_Direccion,
	   SER.DesServicio as Nombre_Servicio,

	   CASE 
			WHEN TND.IdNivelDetalle = 1
				THEN STUFF((
					  SELECT ', ' + Provincia.Nombre
					  FROM Provincia					  
					  FOR XML PATH(''), TYPE).value('.', 'varchar(MAX)'), 1, 1, '')	

			WHEN TND.IdNivelDetalle = 2
				THEN STUFF((
					  SELECT ', ' + Canton.Nombre
					  FROM Canton					  
					  FOR XML PATH(''), TYPE).value('.', 'varchar(MAX)'), 1, 1, '')	

			WHEN TND.IdNivelDetalle = 3
				THEN STUFF((
					  SELECT ', ' + Genero.Descripcion
					  FROM Genero					  
					  FOR XML PATH(''), TYPE).value('.', 'varchar(MAX)'), 1, 1, '')	
					  					
		END AS Tipo_Nivel_Detalle,

		CASE 
			WHEN TND.IdNivelDetalle = 1
				THEN STUFF((
					  SELECT ', ' + CONVERT(varchar,Provincia.IdProvincia)
					  FROM Provincia					  
					  FOR XML PATH(''), TYPE).value('.', 'varchar(MAX)'), 1, 1, '')	

			WHEN TND.IdNivelDetalle = 2
				THEN STUFF((
					  SELECT ', ' + CONVERT(varchar,Canton.IdCanton)
					  FROM Canton					  
					  FOR XML PATH(''), TYPE).value('.', 'varchar(MAX)'), 1, 1, '')	

			WHEN TND.IdNivelDetalle = 3
				THEN STUFF((
					  SELECT ', ' + CONVERT(varchar,Genero.IdGenero)
					  FROM Genero					  
					  FOR XML PATH(''), TYPE).value('.', 'varchar(MAX)'), 1, 1, '')	
					  					
		END AS Id_Tipo_Nivel_Detalle,
		TND.TABLA as Tabla_Tipo_Nivel_Detalle
	   	   
FROM DetalleAgrupacion AS DA

INNER JOIN Operador AS OPE
ON DA.IdOperador = OPE.IdOperador and 
OPE.Estado = 1

INNER JOIN Agrupacion AS AG
ON DA.IdAgrupacion = AG.IdAgrupacion and 
AG.Borrado = 0

INNER JOIN ConstructorCriterioDetalleAgrupacion as CCDA
ON DA.IdDetalleAgrupacion = CCDA.IdDetalleAgrupacion and 
DA.IdAgrupacion = CCDA.IdAgrupacion and
DA.IdOperador = CCDA.IdOperador

INNER JOIN Nivel as NIV
ON CCDA.IdNivel = NIV.IdNivel and
NIV.Borrado = 0

LEFT JOIN Regla as REG
ON CCDA.IdConstructorCriterio = REG.IdConstructorCriterio and
REG.Borrado = 0

LEFT JOIN TipoValor as TVA
ON CCDA.IdTipoValor = TVA.IdTipoValor

LEFT JOIN TipoNivelDetalle as TND
ON CCDA.IdNivelDetalle = TND.IdNivelDetalle

LEFT JOIN ConstructorCriterioDetalleAgrupacion as CCDALJ
ON CCDA.IdConstructorDetallePadre = CCDALJ.IdConstructorCriterio

LEFT JOIN DetalleAgrupacion as DALJ
ON DALJ.IdDetalleAgrupacion = CCDALJ.IdDetalleAgrupacion and 
DALJ.IdAgrupacion = CCDALJ.IdAgrupacion and
DALJ.IdOperador = CCDALJ.IdOperador

INNER JOIN ConstructorCriterio as CC
ON CCDA.IdCriterio = CC.IdCriterio and 
CCDA.IdConstructor = CC.IdConstructor

INNER JOIN Criterio as CR
ON CC.IdCriterio = CR.IdCriterio and
CR.Borrado = 0

INNER JOIN Constructor as CO
ON CCDA.IdConstructor = CO.IdConstructor and
CO.Borrado = 0

INNER JOIN Direccion as DIR
ON CO.IdDireccion = DIR.IdDireccion

INNER JOIN SolicitudConstructor as SOCO
ON CO.IdConstructor = SOCO.IdConstructor and
SOCO.Borrado = 0 and SOCO.IdOperador = @IdOperador and
SOCO.IdSolicitudIndicador = @IdSolicitudIndicador

INNER JOIN SolicitudIndicador as SOIN
ON SOCO.IdSolicitudIndicador = SOIN.IdSolicitudIndicador and
SOIN.Borrado = 0 and SOIN.Activo = 1 and
SOIN.FechaInicio <= GETDATE() AND SOIN.FechaFin >= GETDATE()

INNER JOIN Servicio as SER
ON SOIN.IdServicio = SER.IdServicio and
SER.Borrado = 0

INNER JOIN Indicador as IND
ON CO.IdIndicador = IND.IdIndicador and
IND.Borrado = 0

INNER JOIN Frecuencia as FR
ON FR.IdFrecuencia = CO.IdFrecuencia and
FR.Borrado = 0

INNER JOIN Frecuencia as DE
ON DE.IdFrecuencia = CO.IdDesglose and
FR.Borrado = 0

WHERE DA.Borrado = 0 and DA.IdOperador = @IdOperador
ORDER BY IND.IdIndicador, CCDALJ.IdConstructorCriterio,
		 DALJ.IdDetalleAgrupacion,DA.IdDetalleAgrupacion,NIV.IdNivel

