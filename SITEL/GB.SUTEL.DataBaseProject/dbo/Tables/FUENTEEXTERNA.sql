CREATE TABLE [dbo].[FuenteExterna] (
    [IdFuenteExterna]     INT          NOT NULL,
    [NombreFuenteExterna] VARCHAR (50) NOT NULL,
    [Borrado]             TINYINT      NOT NULL,
    CONSTRAINT [PK_FUENTEEXTERNA] PRIMARY KEY CLUSTERED ([IdFuenteExterna] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico de la fuente externa', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FuenteExterna', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre de la fuente externa', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FuenteExterna', @level2type = N'COLUMN', @level2name = N'NombreFuenteExterna';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la fuente externa', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FuenteExterna', @level2type = N'COLUMN', @level2name = N'IdFuenteExterna';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Fuentes a las que pertenece los indicadores externos (BCCR e INEC)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FuenteExterna';

