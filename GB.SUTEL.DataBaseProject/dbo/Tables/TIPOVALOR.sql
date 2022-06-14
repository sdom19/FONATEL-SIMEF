CREATE TABLE [dbo].[TipoValor] (
    [IdTipoValor] INT          NOT NULL,
    [Descripcion] VARCHAR (50) NOT NULL,
    [Formato]     VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_TIPOVALOR] PRIMARY KEY CLUSTERED ([IdTipoValor] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Formato del tipo valor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoValor', @level2type = N'COLUMN', @level2name = N'Formato';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción del tipo de valor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoValor', @level2type = N'COLUMN', @level2name = N'Descripcion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del tipo de valor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoValor', @level2type = N'COLUMN', @level2name = N'IdTipoValor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de tipos de valor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoValor';

