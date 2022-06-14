
CREATE PROCEDURE [dbo].[pa_ParamFormulasTrans]
	 @p_Opcion int=0,
     @p_IdServicio int=0,
     @p_IdIndicador nvarchar(50)='',
	 @p_FormulaPorcentaje varchar(1000)='',
	 @p_FormulaCumplimiento varchar(1000)='',
	 @p_Criterios varchar(1000)='',
	 @p_Usuario varchar(30)='',
	 @p_FromArray varchar(1000),
	 @p_ArrayIf   varchar(1000),
	 @p_ArrayVerdadero varchar(1000),
	 @p_ArrayFalso  varchar(1000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
IF (@p_Opcion = '1') 
  BEGIN	
	declare @p_FormulaAntiguaCumpli varchar(max);
	declare @p_FormulaNuevaCumpli varchar(max);
	declare @p_FormulaAntiguaPor varchar(max);
	declare @p_FormulaNuevaPor varchar(max)


	insert into ParamFormulas
	 values
	 (
	 @p_IdServicio,
	 @p_IdIndicador,
	 @p_FormulaPorcentaje,
	 @p_FormulaCumplimiento,
	 @p_Criterios,
	 @p_FromArray,
	 getdate(),
	 @p_Usuario,
	 'TRIMESTRAL',
	 @p_ArrayIf,
	 @p_ArrayVerdadero,
	 @p_ArrayFalso);
    -------
	set @p_FormulaAntiguaCumpli = (select FormulaCumplimiento from ParamFormulas where IdIndicador =@p_IdIndicador);
	set @p_FormulaNuevaCumpli = @p_FormulaCumplimiento;

	set @p_FormulaAntiguaPor = (select FormulaPorcentaje from ParamFormulas where IdIndicador =@p_IdIndicador);
	set @p_FormulaNuevaPor = @p_FormulaPorcentaje;
	-----
	EXEC pa_LogErrores @p_IdIndicador,'C',@p_Usuario,@p_FormulaAntiguaCumpli,@p_FormulaCumplimiento,@p_FormulaAntiguaPor,@p_FormulaNuevaPor;
    
  END
ELSE IF (@p_Opcion = '2')
BEGIN
	set @p_FormulaAntiguaCumpli = (select FormulaCumplimiento from ParamFormulas where IdIndicador =@p_IdIndicador);
	set @p_FormulaNuevaCumpli = @p_FormulaCumplimiento;

	set @p_FormulaAntiguaPor = (select FormulaPorcentaje from ParamFormulas where IdIndicador =@p_IdIndicador);
	set @p_FormulaNuevaPor = @p_FormulaPorcentaje;

	update ParamFormulas set FormulaPorcentaje = @p_FormulaPorcentaje , FormulaCumplimiento = @p_FormulaCumplimiento ,
	FechaUltimaActualizacion = getdate(),
	Usuario= @p_Usuario, Criterios= @p_Criterios,
	FromArray=@p_FromArray,
	ArrayIf=@p_ArrayIf,
	ArrayVerdadero=@p_ArrayVerdadero,
	ArrayFalso=@p_ArrayFalso    
	where  IdIndicador = @p_IdIndicador;	
	
	EXEC pa_LogErrores @p_IdIndicador,'A',@p_Usuario,@p_FormulaAntiguaCumpli,@p_FormulaCumplimiento,@p_FormulaAntiguaPor,@p_FormulaNuevaPor;	
END
ELSE IF (@p_Opcion = '3')
BEGIN
	select* from ParamFormulas where IdIndicador = @p_IdIndicador
END	
ELSE IF (@p_Opcion = '4')
BEGIN
	select* from ParamFormulas 
	where Periodicidad ='TRIMESTRAL'
	
END	
ELSE IF (@p_Opcion = '5')
BEGIN
	select* from ParamFormulas 
	where Periodicidad ='ANUAL'
END	
END