CREATE TABLE [dbo].[IndicadorExterno] (
    [IdIndicadorExterno] VARCHAR (50)  NOT NULL,
    [IdFuenteExterna]    INT           NOT NULL,
    [IdTipoValor]        INT           NULL,
    [Nombre]             VARCHAR (100) NULL,
    [Descripcion]        VARCHAR (250) NOT NULL,
    [Borrado]            TINYINT       CONSTRAINT [DF_IndicadorExterno_Borrado] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_IndicadorExterno] PRIMARY KEY CLUSTERED ([IdIndicadorExterno] ASC),
    CONSTRAINT [FK_INDICADO_REFERENCE_FUENTEEX] FOREIGN KEY ([IdFuenteExterna]) REFERENCES [dbo].[FuenteExterna] ([IdFuenteExterna]),
    CONSTRAINT [FK_INDICADO_REFERENCE_TIPOVALO] FOREIGN KEY ([IdTipoValor]) REFERENCES [dbo].[TipoValor] ([IdTipoValor])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorExterno', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripcion del indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorExterno', @level2type = N'COLUMN', @level2name = N'Descripcion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre del indicador externo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorExterno', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de tipo valor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdTipoValor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la fuente externa', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdFuenteExterna';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del indicador externo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdIndicadorExterno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de indicadores externos', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorExterno';

