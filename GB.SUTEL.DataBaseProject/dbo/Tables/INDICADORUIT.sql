CREATE TABLE [dbo].[IndicadorUIT] (
    [IdIndicadorUIT]   INT           IDENTITY (1, 1) NOT NULL,
    [DescIndicadorUIT] VARCHAR (250) NOT NULL,
    [Borrado]          TINYINT       NOT NULL,
    CONSTRAINT [PK_INDICADORUIT] PRIMARY KEY CLUSTERED ([IdIndicadorUIT] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico de indicador uit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorUIT', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción del indicador uit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorUIT', @level2type = N'COLUMN', @level2name = N'DescIndicadorUIT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del indicador UIT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorUIT', @level2type = N'COLUMN', @level2name = N'IdIndicadorUIT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de indicadores UIT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorUIT';

