CREATE TABLE [dbo].[Provincia] (
    [IdProvincia] INT          NOT NULL,
    [Nombre]      VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PROVINCIA] PRIMARY KEY CLUSTERED ([IdProvincia] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre de la provincia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Provincia', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la provincia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Provincia', @level2type = N'COLUMN', @level2name = N'IdProvincia';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de provincias', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Provincia';

