CREATE TABLE [dbo].[IndicadorIndicadorUIT] (
    [IdIndicadorUIT] INT           NOT NULL,
    [IdIndicador]    NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_INDICADORINDICADORUIT] PRIMARY KEY CLUSTERED ([IdIndicadorUIT] ASC, [IdIndicador] ASC),
    CONSTRAINT [FK_INDICADO_REFERENCE_INDICADO2] FOREIGN KEY ([IdIndicadorUIT]) REFERENCES [dbo].[IndicadorUIT] ([IdIndicadorUIT]),
    CONSTRAINT [FK_INDICADO_REFERENCE_INDICADO3] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Id del indicador interno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorIndicadorUIT', @level2type = N'COLUMN', @level2name = N'IdIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Id del indicador uit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorIndicadorUIT', @level2type = N'COLUMN', @level2name = N'IdIndicadorUIT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Registra la relación de los indicadores internos con los indicadores UIT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorIndicadorUIT';

