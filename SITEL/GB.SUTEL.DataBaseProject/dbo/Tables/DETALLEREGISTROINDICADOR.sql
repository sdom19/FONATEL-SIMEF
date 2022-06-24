CREATE TABLE [dbo].[DetalleRegistroIndicador] (
    [IdDetalleRegistroindicador] UNIQUEIDENTIFIER NOT NULL,
    [IdRegistroIndicador]        UNIQUEIDENTIFIER NOT NULL,
    [IdConstructorCriterio]      UNIQUEIDENTIFIER NOT NULL,
    [IdTipoValor]                INT              NOT NULL,
    [IdProvincia]                INT              NULL,
    [IdCanton]                   INT              NULL,
    [IdGenero]                   INT              NULL,
    [Anno]                       INT              NOT NULL,
    [NumeroDesglose]             INT              NOT NULL,
    [Valor]                      VARCHAR (250)    NOT NULL,
    [Comentario]                 VARCHAR (250)    NULL,
    [Modificado]                 TINYINT          NOT NULL,
    [FechaModificacion]          DATETIME         NULL,
    [IdDistrito]                 INT              NULL,
    CONSTRAINT [PK_DETALLEREGISTROINDICADOR] PRIMARY KEY CLUSTERED ([IdDetalleRegistroindicador] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DETALLER_REFERENCE_CANTON] FOREIGN KEY ([IdCanton]) REFERENCES [dbo].[Canton] ([IdCanton]),
    CONSTRAINT [FK_DETALLER_REFERENCE_CONSTRUC] FOREIGN KEY ([IdConstructorCriterio]) REFERENCES [dbo].[ConstructorCriterioDetalleAgrupacion] ([IdConstructorCriterio]),
    CONSTRAINT [FK_DETALLER_REFERENCE_GENERO] FOREIGN KEY ([IdGenero]) REFERENCES [dbo].[Genero] ([IdGenero]),
    CONSTRAINT [FK_DETALLER_REFERENCE_PROVINCI] FOREIGN KEY ([IdProvincia]) REFERENCES [dbo].[Provincia] ([IdProvincia]),
    CONSTRAINT [FK_DETALLER_REFERENCE_REGISTRO] FOREIGN KEY ([IdRegistroIndicador]) REFERENCES [dbo].[RegistroIndicador] ([IdRegistroIndicador]),
    CONSTRAINT [FK_DETALLER_REFERENCE_TIPOVALO] FOREIGN KEY ([IdTipoValor]) REFERENCES [dbo].[TipoValor] ([IdTipoValor])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Fecha en que se realizó la modificación del dato', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'FechaModificacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica si el valor se ha modificado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'Modificado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Comentario del operador por si no registra el valor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'Comentario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Guarda el valor que registra el operador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'Valor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica a que mes de desglose pertenece', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'NumeroDesglose';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica el año del valor del registro indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'Anno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia a Genero', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'IdGenero';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia a Cantón', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'IdCanton';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia a Provincia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'IdProvincia';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al tipo de valor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'IdTipoValor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'ID de los detalle agrupación asociados al constructor por criterio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'IdConstructorCriterio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia a registro indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'IdRegistroIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'ID del detalle de registro de indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador', @level2type = N'COLUMN', @level2name = N'IdDetalleRegistroindicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Contiene los detalle del registro del indicador por parte de los operadores', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleRegistroIndicador';

