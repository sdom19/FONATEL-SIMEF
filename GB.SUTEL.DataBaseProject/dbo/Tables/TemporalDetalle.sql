CREATE TABLE [dbo].[TemporalDetalle] (
    [IdConstructorCriterio]     UNIQUEIDENTIFIER NULL,
    [IdConstructorDetallePadre] UNIQUEIDENTIFIER NULL,
    [IdCriterio]                VARCHAR (20)     NULL,
    [IdOperador]                VARCHAR (20)     NULL,
    [IdIndicador]               VARCHAR (50)     NULL,
    [Descripcion]               VARCHAR (250)    NULL,
    [Orden]                     INT              NULL,
    [OrdenCorregido]            INT              NULL,
    [Idnivel]                   INT              NULL,
    [IdNivelDetalle]            INT              NULL,
    [DecIndicador]              VARCHAR (250)    NULL,
    [DescOperador]              VARCHAR (250)    NULL,
    [IdConstructor]             UNIQUEIDENTIFIER NULL,
    [OrdenTemporal]             INT              NULL,
    [IdServicio]                INT              NULL,
    [IdCanton]                  INT              NULL,
    [IdGenero]                  INT              NULL,
    [IdProvincia]               INT              NULL
);

