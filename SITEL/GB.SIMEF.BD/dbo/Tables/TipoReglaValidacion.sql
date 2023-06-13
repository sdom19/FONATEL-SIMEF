CREATE TABLE [dbo].[TipoReglaValidacion] (
    [IdTipoReglaValidacion] INT           NOT NULL,
    [Nombre]                VARCHAR (300) NOT NULL,
    [Estado]                BIT           NOT NULL,
    CONSTRAINT [PK_TipoReglaValidacion] PRIMARY KEY CLUSTERED ([IdTipoReglaValidacion] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estado del tipo de regla activo 1 inactivo 0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoReglaValidacion', @level2type = N'COLUMN', @level2name = N'Estado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de tipo de regla de validación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoReglaValidacion', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'llave primaria', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoReglaValidacion', @level2type = N'COLUMN', @level2name = N'IdTipoReglaValidacion';

