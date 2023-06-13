CREATE TABLE [dbo].[TipoIndicador] (
    [IdTipoIndicador] INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]          VARCHAR (350) NOT NULL,
    [Estado]          BIT           CONSTRAINT [DF_TipoIndicador_Estado] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TipoIndicador] PRIMARY KEY CLUSTERED ([IdTipoIndicador] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estado del tipo de indicador 1 activo 0 inactivo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoIndicador', @level2type = N'COLUMN', @level2name = N'Estado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'nombre del tipo de indicador, catálogo para indicadores', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoIndicador', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'llave primaria ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoIndicador', @level2type = N'COLUMN', @level2name = N'IdTipoIndicador';

