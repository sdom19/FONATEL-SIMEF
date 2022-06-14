CREATE TABLE [dbo].[IndicadorDireccion] (
    [IdDireccion] INT           NOT NULL,
    [IdIndicador] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_INDICADORDIRECCION] PRIMARY KEY CLUSTERED ([IdDireccion] ASC, [IdIndicador] ASC),
    CONSTRAINT [FK_INDICADO_REFERENCE_DIRECCIO] FOREIGN KEY ([IdDireccion]) REFERENCES [dbo].[Direccion] ([IdDireccion]),
    CONSTRAINT [FK_INDICADO_REFERENCE_INDICADO] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Código del indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorDireccion', @level2type = N'COLUMN', @level2name = N'IdIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la dirección', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorDireccion', @level2type = N'COLUMN', @level2name = N'IdDireccion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Relación de los indicadores con las direcciones', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorDireccion';

