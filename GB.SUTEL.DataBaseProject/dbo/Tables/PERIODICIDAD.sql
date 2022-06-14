CREATE TABLE [dbo].[Periodicidad] (
    [IdPeridiocidad]   INT          IDENTITY (1, 1) NOT NULL,
    [DescPeriodicidad] VARCHAR (50) NOT NULL,
    [Borrado]          TINYINT      NOT NULL,
    CONSTRAINT [PK_PERIODICIDAD] PRIMARY KEY CLUSTERED ([IdPeridiocidad] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico de la periodicidad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Periodicidad', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción de la periodicidad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Periodicidad', @level2type = N'COLUMN', @level2name = N'DescPeriodicidad';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la periodicidad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Periodicidad', @level2type = N'COLUMN', @level2name = N'IdPeridiocidad';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Periodicidad del indicador externo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Periodicidad';

