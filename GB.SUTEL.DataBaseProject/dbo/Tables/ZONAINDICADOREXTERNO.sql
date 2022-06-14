CREATE TABLE [dbo].[ZonaIndicadorExterno] (
    [IdZonaIndicadorExterno]   INT          IDENTITY (1, 1) NOT NULL,
    [DescZonaIndicadorExterno] VARCHAR (50) NOT NULL,
    [Borrado]                  TINYINT      NOT NULL,
    CONSTRAINT [PK_ZONAINDICADOREXTERNO] PRIMARY KEY CLUSTERED ([IdZonaIndicadorExterno] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZonaIndicadorExterno', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción de la zona', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZonaIndicadorExterno', @level2type = N'COLUMN', @level2name = N'DescZonaIndicadorExterno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la zona', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZonaIndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdZonaIndicadorExterno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de zonas', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZonaIndicadorExterno';

