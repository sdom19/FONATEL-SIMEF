
CREATE PROCEDURE [dbo].[pa_ClonarConstructor] 
				@IdConstructor uniqueidentifier				
AS
Begin
Begin Tran
Begin try
--Declare variables
declare @IdConstructorLookup uniqueidentifier;
set @IdConstructorLookup = NEWID();

--declare agregadas por diego
declare @IdConstructorCriterio uniqueidentifier;
set @IdConstructorCriterio = NEWID();

--declare @IdNivelDetalleReglaEstadisticaLookup bigInt ;
--set @IdNivelDetalleReglaEstadisticaLookup = NEWID();

--Inicio de clonación de Tabla Constructor 
merge Constructor as [mainConstructor]
    using 
    (
        select *
		from Constructor
		where IdConstructor = @IdConstructor
    )as [cloneConstructor] (
               [IdConstructor]
			  ,[IdIndicador]
			  ,[IdFrecuencia]
			  ,[IdDesglose]
			  ,[FechaCreacionConstructor]
			  ,[Borrado]
			  ,[IdDireccion]
            )
    on 1=0
    when not matched then 
	insert
        (
            [IdConstructor]
			,[IdIndicador]
			,[IdFrecuencia]
			,[IdDesglose]
			,[FechaCreacionConstructor]
			,[Borrado]
			,[IdDireccion]
        )
        values
        (
             @IdConstructorLookup
			,[cloneConstructor].[IdIndicador]
			,[cloneConstructor].[IdFrecuencia]
			,[cloneConstructor].[IdDesglose]
			,getdate()
			,[cloneConstructor].[Borrado]
			,[cloneConstructor].[IdDireccion]
        );
--Fin de clonación de Tabla Constructor 
/******************************************************
******************************************************/
--Inicio de clonación de Tabla Constructor Criterio
merge ConstructorCriterio as [mainConstructorCriterio]
    using 
    (
        select IdConstructor,IdCriterio,Ayuda
		from ConstructorCriterio
		where IdConstructor = @IdConstructor
    )as [cloneConstructorCriterio] (
             	[IdConstructor]
	            ,[IdCriterio]
	            ,[Ayuda]
            )
    on 1=0
    when not matched then 
	insert
        (
           [IdConstructor]
	      ,[IdCriterio]
	      ,[Ayuda]
        )
        values
        (             
			 @IdConstructorLookup
		    ,[cloneConstructorCriterio].[IdCriterio]
		    ,[cloneConstructorCriterio].[Ayuda]
        );
--Fin de clonación de Tabla Constructor Criterio
/******************************************************************************
******************************************************************************/
--Inicio de clonación de Tabla Constructor Criterio Detalle Agrupación (Árboles)

CREATE TABLE #PadreClonacionTempTable(
id int primary key identity(1,1) not null,
padreOne  uniqueidentifier not null, 
padreTwo uniqueidentifier not null)

DECLARE select_cursor CURSOR LOCAL FOR  
SELECT IdConstructorCriterio,IdConstructorDetallePadre
FROM ConstructorCriterioDetalleAgrupacion
where IdConstructor = @IdConstructor
order by IdCriterio,IdOperador,UltimoNivel,Orden;

--Declare variables
declare @IdPadreOne uniqueidentifier;
declare @IdPadreTwo uniqueidentifier;

declare @IdAutoGuid uniqueidentifier;
set @IdAutoGuid = NEWID();

OPEN select_cursor   
FETCH NEXT FROM select_cursor into @IdPadreOne,@IdPadreTwo

WHILE @@FETCH_STATUS = 0  
BEGIN   	 
     --Insert in the table
	Insert into #PadreClonacionTempTable values (@IdAutoGuid,@IdPadreOne);
	 	   
	merge ConstructorCriterioDetalleAgrupacion as [mainConstructorCriterioDetalleAgrupacion]
    using 
    (
        select *
        from ConstructorCriterioDetalleAgrupacion
        where [IdConstructorCriterio] = @IdPadreOne
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
		  ,@IdConstructorLookup
		  ,[cloneConstructorCriterioDetalleAgrupacion].[IdCriterio]
		  ,[cloneConstructorCriterioDetalleAgrupacion].[IdOperador]
		  ,[cloneConstructorCriterioDetalleAgrupacion].[IdAgrupacion]
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
		  ,[cloneConstructorCriterioDetalleAgrupacion].[IdDetalleAgrupacion]
		  ,[cloneConstructorCriterioDetalleAgrupacion].[Orden]
		  ,[cloneConstructorCriterioDetalleAgrupacion].[UsaReglaEstadisticaConNivelDetalle]
		  ,[cloneConstructorCriterioDetalleAgrupacion].[UsaReglaEstadistica]
		  ,[cloneConstructorCriterioDetalleAgrupacion].[OrdenCorregido]
		  ,[cloneConstructorCriterioDetalleAgrupacion].[IdNivelDetalleGenero]
        );	  

	--******************************************************************************************
    -- CLONACIÓN DE NivelDetalleReglaEstadistica
	--******************************************************************************************
    merge NivelDetalleReglaEstadistica as [mainNivelDetalleReglaEstadistica]
    using 
    (
        select * 
		from NivelDetalleReglaEstadistica
		where IdConstructorCriterioDetalleAgrupacion IN (SELECT [IdConstructorCriterio]
		                                               
													     FROM [dbo].[ConstructorCriterioDetalleAgrupacion] 
														 
														 WHERE [IdConstructorCriterio] = @IdPadreOne
														 
														  AND [UsaReglaEstadisticaConNivelDetalle] = 1)		
			
		-- @IdConstructorCriterio
    )as [cloneNivelDetalleReglaEstadistica] (
             	       [IdNivelDetalleReglaEstadistica]
					  ,[IdConstructorCriterioDetalleAgrupacion]
					  ,[ValorLimiteSuperior]
					  ,[ValorLimiteInferior]
					  ,[IdNivelDetalle]
					  ,[IdGenerico]
					  ,[Borrado]
            )

    on 1=0
    when not matched then 
	insert
        (             	       
					   [IdConstructorCriterioDetalleAgrupacion]
					  ,[ValorLimiteSuperior]
					  ,[ValorLimiteInferior]
					  ,[IdNivelDetalle]
					  ,[IdGenerico]
					  ,[Borrado]
        )
        values
        (             
					   
					   @IdAutoGuid
					  ,[cloneNivelDetalleReglaEstadistica].[ValorLimiteSuperior]
					  ,[cloneNivelDetalleReglaEstadistica].[ValorLimiteInferior]
					  ,[cloneNivelDetalleReglaEstadistica].[IdNivelDetalle]
					  ,[cloneNivelDetalleReglaEstadistica].[IdGenerico]
					  ,[cloneNivelDetalleReglaEstadistica].[Borrado]
        );



	set @IdConstructorCriterio =  @IdPadreOne;
	FETCH NEXT FROM select_cursor into @IdPadreOne,@IdPadreTwo
	set @IdAutoGuid = NEWID();
END   

CLOSE select_cursor;
DEALLOCATE select_cursor;


--Fin de clonación de Tabla Constructor Criterio Detalle Agrupación (Árboles)
/******************************************************************************
******************************************************************************/
--Inicio de clonación de Tabla Regla

merge Regla as [mainRegla]
    using 
    (
        select *
		from Regla as RE
		where RE.IdConstructorCriterio
		in(
			select IdConstructorCriterio
			from ConstructorCriterioDetalleAgrupacion
			where ConstructorCriterioDetalleAgrupacion.IdConstructor = @IdConstructor
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
		
--Fin de clonación de Tabla Regla
--Inicio de la clonación NivelDetalleReglaEstadistica 

----- fin
DROP TABLE #PadreClonacionTempTable;
update Constructor set Borrado = 2 where IdConstructor = @IdConstructor
Commit Tran;
End try
Begin catch
	Rollback Tran;
	set @IdConstructorLookup = null;
	--select ERROR_MESSAGE() as ErrorMessage;
End catch

--Select new Id Constructor
select top 1 * from Constructor where IdConstructor = @IdConstructorLookup;

END