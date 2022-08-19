


CREATE procedure [dbo].[spObtenerReglasValidacion]

@idRegla int,
@Codigo varchar(30),
@idIndicador int,
@IdTipo int,
@idEstado int 
as
declare @consulta varchar(2000);
set @consulta=
  'SELECT idRegla '+
     ' ,Codigo '+
     ' ,Nombre '+
     ' ,IdTipo '+
     ' ,Descripcion '+
     ' ,idOperador '+
     ' ,idIndicador '+
     ' ,FechaCreacion '+
     ' ,UsuarioCreacion '+
     ' ,FechaModificacion '+
     ' ,UsuarioModificacion '+
     ' ,idEstado '+
 ' FROM dbo.ReglaValidacion '+
 ' where idEstado!=4 '+
 IIF(@idRegla=0,'',' and idRegla='+ cast(@idRegla as varchar(10))+' ')+
 IIF(@idIndicador=0,'',' and idIndicador='+ cast(@idIndicador as varchar(10))+' ')+
 IIF(@IdTipo=0,'',' and IdTipo='+ cast(@IdTipo as varchar(10))+' ')+
 IIF(@Codigo='','',' and Codigo='''+@Codigo+''' ')+
 IIF(@idEstado=0,'',' and idEstado='+ cast(@idEstado as varchar(10))+' ');
 exec(@consulta)