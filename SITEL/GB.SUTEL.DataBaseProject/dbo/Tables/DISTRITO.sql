CREATE TABLE [dbo].[Distrito] (
    [IdDistrito] INT           NOT NULL,
    [IdCanton]   INT           NOT NULL,
    [Nombre]     NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DISTRITO] PRIMARY KEY CLUSTERED ([IdDistrito] ASC),
    CONSTRAINT [FK_DISTRITO_REFERENCE_CANTON] FOREIGN KEY ([IdCanton]) REFERENCES [dbo].[Canton] ([IdCanton])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'nombre del distrito', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Distrito', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Id del cantón', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Distrito', @level2type = N'COLUMN', @level2name = N'IdCanton';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id de distrito', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Distrito', @level2type = N'COLUMN', @level2name = N'IdDistrito';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Tabla de distritos', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Distrito';

