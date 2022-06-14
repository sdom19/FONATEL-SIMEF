CREATE TABLE [dbo].[Canton] (
    [IdCanton]      INT           NOT NULL,
    [IdProvincia]   INT           NOT NULL,
    [Nombre]        VARCHAR (250) NOT NULL,
    [CodigoOficial] INT           NULL,
    CONSTRAINT [PK_CANTON] PRIMARY KEY CLUSTERED ([IdCanton] ASC),
    CONSTRAINT [FK_CANTON_REFERENCE_PROVINCI] FOREIGN KEY ([IdProvincia]) REFERENCES [dbo].[Provincia] ([IdProvincia])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre del cantón', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Canton', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Id de la provincia a la que pertenece', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Canton', @level2type = N'COLUMN', @level2name = N'IdProvincia';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'ID del cantón', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Canton', @level2type = N'COLUMN', @level2name = N'IdCanton';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Contiene los cantones de CR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Canton';

