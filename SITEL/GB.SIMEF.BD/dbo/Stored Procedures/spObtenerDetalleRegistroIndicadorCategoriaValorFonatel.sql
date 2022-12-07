CREATE procedure [SITEL].[spObtenerDetalleRegistroIndicadorCategoriaValorFonatel]
	  @idSolicitud int=0,
	  @idFormulario int=0,
	  @idIndicador int=0,
	  @idCategoria int=0
AS

Declare @Consulta varchar(1000)

set @Consulta ='
SELECT IdSolicitud
      ,IdFormulario
      ,IdIndicador
      ,idCategoria
      ,NumeroFila
      ,Valor
  FROM SITELP.FONATEL.DetalleRegistroIndicadorCategoriaValorFonatel
  where 1=1'+
  IIF(@idSolicitud=0,'', 'and IdSolicitud=' + cast(@idSolicitud as varchar(10)) + ' ')+
  IIF(@idFormulario=0,'', 'and IdFormulario=' + cast(@idFormulario as varchar(10)) + ' ')+
  IIF(@idIndicador=0,'', 'and IdIndicador=' + cast(@idIndicador as varchar(10)) + ' ')+
  IIF(@idCategoria=0,'', 'and idCategoria=' + cast(@idCategoria as varchar(10)) + ' ');

  exec(@consulta);