
CREATE PROCEDURE [dbo].[pa_ClonarArbol] 
				@p_IdConstructor uniqueidentifier,
				@p_Criterio  varchar(50),
				@p_IdOperadorOriginal varchar(20),
				@p_IdOperadorClonar varchar(20)

AS
Begin
Begin Tran
Begin try

/******************************************************************************
******************************************************************************/
--Inicio de clonación de Tabla Constructor Criterio Detalle Agrupación (Árboles)

declare @noentrar tinyint
set @noentrar = 0;

declare @IdDetalleAgrupacionClonado int
set @IdDetalleAgrupacionClonado = 0;

declare @IdAgrupacionClonado int
set @IdAgrupacionClonado = 0;

CREATE TABLE #PadreClonacionTempTable(
id int primary key identity(1,1) not null,
padreOne  uniqueidentifier not null, 
padreTwo uniqueidentifier not null)

DECLARE select_cursor CURSOR LOCAL FOR  
SELECT IdConstructorCriterio,IdConstructorDetallePadre, DA.DescDetalleAgrupacion,DA.IdAgrupacion
FROM ConstructorCriterioDetalleAgrupacion CCDA
inner join [dbo].[DetalleAgrupacion] as DA on da.IdAgrupacion = CCDA.IdAgrupacion and 
DA.IdDetalleAgrupacion = CCDA.IdDetalleAgrupacion AND DA.IdOperador = CCDA.IdOperador
where CCDA.Borrado = 0 AND da.Borrado = 0 AND IdConstructor = @p_IdConstructor and IdCriterio = @p_Criterio and CCDA.IdOperador = @p_IdOperadorOriginal
order by IdCriterio,ccda.IdOperador,UltimoNivel,Orden;

--Declare variables
declare @IdPadreOne uniqueidentifier;
declare @IdPadreTwo uniqueidentifier;
declare @DescDetalleAgrupacion varchar(250);
declare @DescDetalleAgrupacionHexa varbinary(250);
declare @IdAgrupacion int;

declare @IdAutoGuid uniqueidentifier;
set @IdAutoGuid = NEWID();

OPEN select_cursor   
FETCH NEXT FROM select_cursor into @IdPadreOne,@IdPadreTwo,@DescDetalleAgrupacion,@IdAgrupacion

WHILE @@FETCH_STATUS = 0  
BEGIN   	 
     --Insert in the table
	Insert into #PadreClonacionTempTable values (@IdAutoGuid,@IdPadreOne);

	set @DescDetalleAgrupacionHexa = Convert(varbinary(250),@DescDetalleAgrupacion);
	
	if (select count(*) from DetalleAgrupacion where DescDetalleAgrupacion = @DescDetalleAgrupacion and DescHexa=@DescDetalleAgrupacionHexa and IdAgrupacion= @IdAgrupacion and IdOperador = @p_IdOperadorClonar and Borrado = 0 ) > 0
	begin
	    set @IdDetalleAgrupacionClonado = (select top 1 IdDetalleAgrupacion from DetalleAgrupacion where DescDetalleAgrupacion = @DescDetalleAgrupacion and  IdAgrupacion=@IdAgrupacion and DescHexa=@DescDetalleAgrupacionHexa and IdOperador = @p_IdOperadorClonar and Borrado = 0) 
		set @IdAgrupacionClonado =@IdAgrupacion ---(select top 1 IdAgrupacion from DetalleAgrupacion where DescDetalleAgrupacion = @DescDetalleAgrupacion and IdAgrupacion=@IdAgrupacion and IdOperador = @p_IdOperadorClonar and Borrado = 0) 

		merge ConstructorCriterioDetalleAgrupacion as [mainConstructorCriterioDetalleAgrupacion]
		using 
		(
			select *
			from ConstructorCriterioDetalleAgrupacion
			where [borrado] = 0 and [IdConstructor] = @p_IdConstructor and  [IdConstructorCriterio] = @IdPadreOne and IdCriterio = @p_Criterio and IdOperador = @p_IdOperadorOriginal
		)as [cloneConstructorCriterioDetalleAgrupacion] (
             		[IdConstructorCriterio]
					,[IdConstructor]
					,[IdCriterio]
					,[IdOperador]
					,[IdAgrupacion]
					,[IdConstructorDetallePadre]
					,[IdTipoValor]
					,[IdNivelDetalle]
					,[IdNivel]
					,[UltimoNivel]
					,[Borrado]
					,[IdDetalleAgrupacion]
					,[Orden]
					,[UsaReglaEstadisticaConNivelDetalle]
					,[UsaReglaEstadistica]
					,[OrdenCorregido]
					,[IdNivelDetalleGenero]
				)
		on 1=0
		when not matched then 
		insert
			(
			   [IdConstructorCriterio]
			  ,[IdConstructor]
			  ,[IdCriterio]
			  ,[IdOperador]
			  ,[IdAgrupacion]
			  ,[IdConstructorDetallePadre]
			  ,[IdTipoValor]
			  ,[IdNivelDetalle]
			  ,[IdNivel]
			  ,[UltimoNivel]
			  ,[Borrado]
			  ,[IdDetalleAgrupacion]
			  ,[Orden]
			  ,[UsaReglaEstadisticaConNivelDetalle]
			  ,[UsaReglaEstadistica]
			  ,[OrdenCorregido]
			  ,[IdNivelDetalleGenero]
			)
			values
			(             
			   @IdAutoGuid
			  ,@p_IdConstructor
			  ,[cloneConstructorCriterioDetalleAgrupacion].[IdCriterio]
			  ,@p_IdOperadorClonar
			  ,@IdAgrupacionClonado
			  ,(select padreOne 
				from #PadreClonacionTempTable 
				where padreTwo = [cloneConstructorCriterioDetalleAgrupacion].[IdConstructorDetallePadre] 
				and [cloneConstructorCriterioDetalleAgrupacion].[IdConstructorDetallePadre] is not null
			  )
			  ,[cloneConstructorCriterioDetalleAgrupacion].[IdTipoValor]
			  ,[cloneConstructorCriterioDetalleAgrupacion].[IdNivelDetalle]
			  ,[cloneConstructorCriterioDetalleAgrupacion].[IdNivel]
			  ,[cloneConstructorCriterioDetalleAgrupacion].[UltimoNivel]
			  ,[cloneConstructorCriterioDetalleAgrupacion].[Borrado]
			  ,@IdDetalleAgrupacionClonado
			  ,[cloneConstructorCriterioDetalleAgrupacion].[Orden]
			  ,[cloneConstructorCriterioDetalleAgrupacion].[UsaReglaEstadisticaConNivelDetalle]
			  ,[cloneConstructorCriterioDetalleAgrupacion].[UsaReglaEstadistica]
			  ,[cloneConstructorCriterioDetalleAgrupacion].[OrdenCorregido]
			  ,[cloneConstructorCriterioDetalleAgrupacion].[IdNivelDetalleGenero]
			);	  

	end
	else
	begin
	    set @noentrar = 1;
	    rollback;
		--FETCH NEXT FROM select_cursor into @IdPadreOne,@IdPadreTwo,@DescDetalleAgrupacion*/
		break;
	end
	FETCH NEXT FROM select_cursor into @IdPadreOne,@IdPadreTwo,@DescDetalleAgrupacion,@IdAgrupacion
	set @IdAutoGuid = NEWID();
	
END   

CLOSE select_cursor;
DEALLOCATE select_cursor;

--Fin de clonación de Tabla Constructor Criterio Detalle Agrupación (Árboles)
/******************************************************************************
******************************************************************************/
--Inicio de clonación de Tabla Regla

if @noentrar = 0
begin 

	merge Regla as [mainRegla]
		using 
		(
			select *
			from Regla as RE
			where RE.IdConstructorCriterio
			in(
				select IdConstructorCriterio
				from ConstructorCriterioDetalleAgrupacion
				where ConstructorCriterioDetalleAgrupacion.IdConstructor = @p_IdConstructor 
				and IdCriterio = @p_Criterio and IdOperador = @p_IdOperadorOriginal and borrado = 0
			)
		)as [cloneRegla] (
             		 [IdConstructorCriterio]
					,[ValorLimiteInferior]
					,[ValorLimiteSuperior]
					,[FechaCreacionRegla]
					,[Borrado]
				)
		on 1=0
		when not matched then
		insert
			(
			   [IdConstructorCriterio]
			  ,[ValorLimiteInferior]
			  ,[ValorLimiteSuperior]
			  ,[FechaCreacionRegla]
			  ,[Borrado]
			)
			values
			(             
				(
					select padreOne 
					from #PadreClonacionTempTable 
					where padreTwo = [cloneRegla].[IdConstructorCriterio]				
				)		    
			   ,[cloneRegla].[ValorLimiteInferior]
			   ,[cloneRegla].[ValorLimiteSuperior]
			   ,getdate()
			   ,[cloneRegla].[Borrado]
			);
end	

--select @noentrar as 'Resultado'
	
--Fin de clonación de Tabla Regla
DROP TABLE #PadreClonacionTempTable;
Commit Tran;
SELECT 'TRUE' AS RESPUESTA
End try
Begin catch    
    set @noentrar = 1;
	SELECT 'FALSE' AS RESPUESTA
    --select @noentrar as 'Resultado' 
	Rollback Tran;
	--select ERROR_MESSAGE() as ErrorMessage;
End catch

--Select new Id Constructor
--select top 1 * from Constructor where IdConstructor = @IdConstructorLookup;

END