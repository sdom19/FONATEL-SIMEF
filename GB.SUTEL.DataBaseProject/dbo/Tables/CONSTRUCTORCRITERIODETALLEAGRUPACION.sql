CREATE TABLE [dbo].[ConstructorCriterioDetalleAgrupacion] (
    [IdConstructorCriterio]              UNIQUEIDENTIFIER NOT NULL,
    [IdConstructor]                      UNIQUEIDENTIFIER NOT NULL,
    [IdCriterio]                         VARCHAR (50)     NOT NULL,
    [IdOperador]                         VARCHAR (20)     NOT NULL,
    [IdAgrupacion]                       INT              NOT NULL,
    [IdConstructorDetallePadre]          UNIQUEIDENTIFIER NULL,
    [IdTipoValor]                        INT              NULL,
    [IdNivelDetalle]                     INT              NULL,
    [IdNivel]                            INT              NULL,
    [UltimoNivel]                        TINYINT          NULL,
    [Borrado]                            TINYINT          NULL,
    [IdDetalleAgrupacion]                INT              NOT NULL,
    [Orden]                              INT              NULL,
    [UsaReglaEstadisticaConNivelDetalle] TINYINT          NULL,
    [UsaReglaEstadistica]                TINYINT          NULL,
    [OrdenCorregido]                     INT              NULL,
    [IdNivelDetalleGenero]               INT              NULL,
    CONSTRAINT [PK_CONSTRUCTORCRITERIODETALLEA] PRIMARY KEY CLUSTERED ([IdConstructorCriterio] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CONSTRUC_REFERENCE_CONSTRUC2] FOREIGN KEY ([IdConstructorDetallePadre]) REFERENCES [dbo].[ConstructorCriterioDetalleAgrupacion] ([IdConstructorCriterio]),
    CONSTRAINT [FK_SOLICITU] FOREIGN KEY ([IdDetalleAgrupacion], [IdOperador], [IdAgrupacion]) REFERENCES [dbo].[DetalleAgrupacion] ([IdDetalleAgrupacion], [IdOperador], [IdAgrupacion])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Orden en que estan relacionados los detalles agrupación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'Orden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del detalle agrupación que corresponde', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdDetalleAgrupacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado lógico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica si es el ultimo nivel de detalle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'UltimoNivel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al nivel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdNivel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id de nivel de detalle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdNivelDetalle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al tipo de valor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdTipoValor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia a si mismo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdConstructorDetallePadre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al detalle de agrupación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdAgrupacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Refencia al operador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdOperador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al criterio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdCriterio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al constructor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdConstructor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'ID de los detalle agrupación asociados al constructor por criterio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdConstructorCriterio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Detalla el criterio con sus detallles agrupación relacionados en el constructor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterioDetalleAgrupacion';

