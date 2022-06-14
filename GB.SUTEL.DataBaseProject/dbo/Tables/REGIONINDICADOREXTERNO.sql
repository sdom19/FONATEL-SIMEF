CREATE TABLE [dbo].[RegionIndicadorExterno] (
    [IdRegionIndicadorExterno]   INT          IDENTITY (1, 1) NOT NULL,
    [DescRegionIndicadorExterno] VARCHAR (50) NOT NULL,
    [Borrado]                    TINYINT      NOT NULL,
    CONSTRAINT [PK_REGIONINDICADOREXTERNO] PRIMARY KEY CLUSTERED ([IdRegionIndicadorExterno] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico de la region', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegionIndicadorExterno', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'descripción de la región', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegionIndicadorExterno', @level2type = N'COLUMN', @level2name = N'DescRegionIndicadorExterno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la región en el indicador externo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegionIndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdRegionIndicadorExterno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de regiones para el indicador externo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegionIndicadorExterno';

