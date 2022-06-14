CREATE TABLE [dbo].[TipoNivelDetalle] (
    [IdNivelDetalle] INT           NOT NULL,
    [Nombre]         NVARCHAR (50) NOT NULL,
    [Tabla]          NVARCHAR (50) NULL,
    CONSTRAINT [PK_TIPONIVELDETALLE] PRIMARY KEY CLUSTERED ([IdNivelDetalle] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Tabla de la cual se deben trae las opciones de detalle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoNivelDetalle', @level2type = N'COLUMN', @level2name = N'Tabla';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'nombre del nivel de detalle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoNivelDetalle', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id de nivel de detalle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoNivelDetalle', @level2type = N'COLUMN', @level2name = N'IdNivelDetalle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Detalla si los detalles agrupación van por provincia, canton, distrito o genero', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoNivelDetalle';

