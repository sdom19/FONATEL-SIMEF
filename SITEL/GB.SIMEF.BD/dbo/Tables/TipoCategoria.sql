CREATE TABLE [dbo].[TipoCategoria] (
    [IdTipoCategoria] INT          NOT NULL,
    [Nombre]          VARCHAR (30) NOT NULL,
    [Estado]          BIT          NOT NULL,
    CONSTRAINT [PK_TipoCategoria] PRIMARY KEY CLUSTERED ([IdTipoCategoria] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estado del tipo categoria 1 activo 0 Inactivo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoCategoria', @level2type = N'COLUMN', @level2name = N'Estado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre del tipo de Categoría catálogo categoria desagregación ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoCategoria', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'llave primaria ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoCategoria', @level2type = N'COLUMN', @level2name = N'IdTipoCategoria';

