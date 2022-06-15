
CREATE PROCEDURE pa_LogErrores
	-- Add the parameters for the stored procedure here
	@Indicador varchar(20),
	@tipo varchar(4),
	@Usuario varchar(20),
	@FormulaAntiguaCumpli varchar(max),
	@FormulaNuevaCumpli varchar(max),
	@FormulaAntiguaPor varchar(max),
	@FormulaNuevaPor varchar(max)
AS
BEGIN
	insert into 
	 LogFormulas values
	  (
	 @Indicador,
	 getdate(),
	 @Usuario,
	 @FormulaAntiguaCumpli,
	 @FormulaNuevaCumpli,
	 @FormulaAntiguaPor,
	 @FormulaNuevaPor,
	 @tipo
	 );
END