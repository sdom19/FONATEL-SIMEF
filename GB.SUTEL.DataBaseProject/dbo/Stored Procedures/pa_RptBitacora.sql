
-- ============================================================================================
-- Author:		<Javier Muñoz>
-- Create date: <20052015>
-- Description:	<Provee información para el reporte denominado Bitacora>
-- ============================================================================================
CREATE PROCEDURE [dbo].[pa_RptBitacora] 
	--PARÁMETROS
	-- LISTA DE CÓDIGOS DE Pantallas SEPARADOS POR COMAS (,)
	@pPantallas VARCHAR(MAX) = '', 
	-- LISTA DE CÓDIGOS DE Usuarios SEPARADOS POR COMAS (,)
	@pUsuarios VARCHAR(MAX) = '', 
	-- LISTA DE Acciones  SEPARADAS POR COMAS (,)
	@pAcciones  VARCHAR(MAX) = '',   	
	-- FECHA INICIAL
	@pFechaInicial  VARCHAR(MAX) = '',   	
	-- FECHA FINAL
	@pFechaFinal  VARCHAR(MAX) = ''   

AS	
BEGIN
	SET NOCOUNT ON;

	--VERIFICAR PARÁMETROS
	SET @pPantallas = ISNULL(@pPantallas, '')
	SET @pUsuarios = ISNULL(@pUsuarios, '')
	SET @pAcciones = ISNULL(@pAcciones, '')
	SET @pFechaInicial = ISNULL(@pFechaInicial, '')
	SET @pFechaFinal = ISNULL(@pFechaFinal, '')		
	
	SELECT 
		BT.PANTALLA,
		ACC.NOMBRE as ACCION,
		BT.USUARIO,
		BT.DESCRIPCION,		
		convert(VARCHAR(10), BT.FECHABITACORA,103)+' '+convert(VARCHAR(8), BT.FECHABITACORA,108) as FECHABITACORA,
		BT.REGISTROANTERIOR,
		BT.REGISTRONUEVO
	FROM dbo.BITACORA as BT with(nolock)

	INNER JOIN dbo.ACCION as ACC with(nolock)
	ON BT.ACCION = ACC.IDACCION

	INNER JOIN dbo.PANTALLA as PAN with(nolock)
	ON BT.PANTALLA = PAN.NOMBRE	

	INNER JOIN dbo.Usuario as USU with(nolock)
	on bt.Usuario = USU.AccesoUsuario

	where 

	(1=(CASE WHEN LEN(@pUsuarios)<1 THEN 1 ELSE 0 END)  OR bt.Usuario IN (SELECT AccesoUsuario FROM [dbo].[Usuario] WHERE NombreUsuario in (@pUsuarios))) and
	(1=(CASE WHEN LEN(@pPantallas)<1 THEN 1 ELSE 0 END) OR bt.Pantalla IN (SELECT Nombre FROM [dbo].[Pantalla] WHERE IdPantalla in  (@pPantallas))) and
	(1=(CASE WHEN LEN(@pAcciones)<1 THEN 1 ELSE 0 END)  OR acc.Nombre IN (SELECT Nombre FROM [dbo].[Accion] WHERE IdAccion in (@pAcciones))) and
	
	((CASE WHEN @pFechaInicial='' AND @pFechaFinal='' THEN BT.FECHABITACORA end <> '') or 
	(CASE WHEN @pFechaInicial!='' AND @pFechaFinal!='' THEN BT.FECHABITACORA end >= @pFechaInicial) AND
	(CASE WHEN @pFechaInicial!='' AND @pFechaFinal!='' THEN BT.FECHABITACORA end <= DATEADD(day,1,@pFechaFinal)))

	
END