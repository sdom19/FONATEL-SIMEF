CREATE TABLE [dbo].[DetalleIndicadorCruzado] (
    [IdDetalleIndicadorCruzado] UNIQUEIDENTIFIER CONSTRAINT [DF__DETALLEIN__IDDET__2C3393D0] DEFAULT (newid()) NOT NULL,
    [IdIndicadorCruzado]        NVARCHAR (50)    NOT NULL,
    [IdIndicadorExterno]        VARCHAR (50)     NULL,
    [IdIndicador]               NVARCHAR (50)    NULL,
    [IdIndicadorInterno]        NVARCHAR (50)    NULL,
    CONSTRAINT [PK_DETALLEINDICADORCRUZADO] PRIMARY KEY CLUSTERED ([IdDetalleIndicadorCruzado] ASC),
    FOREIGN KEY ([IdIndicadorExterno]) REFERENCES [dbo].[IndicadorExterno] ([IdIndicadorExterno]),
    CONSTRAINT [FK_DETALLEI_REFERENCE_INDICADO] FOREIGN KEY ([IdIndicadorCruzado]) REFERENCES [dbo].[IndicadorCruzado] ([IdIndicadorCruzado]),
    CONSTRAINT [FK_DETALLEI_REFERENCE_INDICADO2] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador]),
    CONSTRAINT [FK_DETALLEI_REFERENCE_INDICADO4] FOREIGN KEY ([IdIndicadorInterno]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al indicador interno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleIndicadorCruzado', @level2type = N'COLUMN', @level2name = N'IdIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al indicador externo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleIndicadorCruzado', @level2type = N'COLUMN', @level2name = N'IdIndicadorExterno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del indicador cruzado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleIndicadorCruzado', @level2type = N'COLUMN', @level2name = N'IdIndicadorCruzado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del detalle del indicador cruzado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleIndicadorCruzado', @level2type = N'COLUMN', @level2name = N'IdDetalleIndicadorCruzado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Detalle de las relaciones entre indicadores externos e internos', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleIndicadorCruzado';

