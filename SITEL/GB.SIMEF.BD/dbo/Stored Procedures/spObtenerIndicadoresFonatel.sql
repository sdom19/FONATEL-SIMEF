create procedure [dbo].[spObtenerIndicadoresFonatel]
@pIdIndicador int,
@pCodigo varchar(30),
@pIdTipoIndicador int,
@pIdClasificacion int,
@pIdGrupo int,
@pIdUnidadEstudio int,
@pIdTipoMedida int,
@pIdFrecuencia int,
@pIdEstado int
as 

begin 
declare @consulta varchar(1000)

set @consulta='SELECT '+
      '[idIndicador]'+
      ',[Codigo]'+
      ',[Nombre]'+
      ',[IdTipoIndicador]'+
      ',[IdClasificacion]'+
      ',[idGrupo]'+
      ',[Descripcion]'+
      ',[CantidadVariableDato]'+
      ',[CantidadCategoriasDesagregacion]'+
      ',[IdUnidadEstudio]'+
      ',[idTipoMedida]'+
      ',[IdFrecuencia]'+
      ',[Interno]'+
      ',[Solicitud]'+
      ',[Fuente]'+
      ',[Notas]'+
      ',[FechaCreacion]'+
      ',[UsuarioCreacion]'+
      ',[FechaModificacion]'+
      ',[UsuarioModificacion]'+
      ',[VisualizaSigitel]'+
      ',[idEstado]'+
  ' FROM [Indicador]'+
  ' where idEstado!=4 '+
    IIF(@pIdIndicador=0,'',' and idIndicador='+ cast(@pIdIndicador as varchar(10))+' ') +
	IIF(@pIdTipoIndicador=0,'',' and IdTipoIndicador='+ cast(@pIdTipoIndicador as varchar(10))+' ') +
	IIF(@pIdClasificacion=0,'',' and IdClasificacion='+ cast(@pIdClasificacion as varchar(10))+' ') +
	IIF(@pIdGrupo=0,'',' and idGrupo='+ cast(@pIdGrupo as varchar(10))+' ') +
	IIF(@pIdUnidadEstudio=0,'',' and IdUnidadEstudio='+ cast(@pIdUnidadEstudio as varchar(10))+' ') +
	IIF(@pIdTipoMedida=0,'',' and idTipoMedida='+ cast(@pIdTipoMedida as varchar(10))+' ') +
	IIF(@pIdFrecuencia=0,'',' and IdFrecuencia='+ cast(@pIdFrecuencia as varchar(10))+' ') +
	IIF(@pIdEstado=0,'',' and idEstado='+ cast(@pIdEstado as varchar(10))+' ') +
	IIF(@pCodigo='','',' and UPPER(TRIM(Codigo))='''+ UPPER(TRIM(@pCodigo))+' ') 

exec(@consulta)

end