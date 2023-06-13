CREATE TABLE [dbo].[TipoMedida] (
    [IdTipoMedida] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre]       VARCHAR (50) NOT NULL,
    [Estado]       BIT          NOT NULL,
    CONSTRAINT [PK_TipoMedida] PRIMARY KEY CLUSTERED ([IdTipoMedida] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estado del tipo de regla activo 1 inactivo 0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoMedida', @level2type = N'COLUMN', @level2name = N'Estado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de la medida, es una catálogo para indicadores', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoMedida', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LLave primaria ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoMedida', @level2type = N'COLUMN', @level2name = N'IdTipoMedida';

