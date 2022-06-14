CREATE TABLE [dbo].[Pantalla] (
    [IdPantalla]  INT           NOT NULL,
    [Nombre]      VARCHAR (50)  NULL,
    [Descripcion] VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_PANTALLA] PRIMARY KEY CLUSTERED ([IdPantalla] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción de donde se coloca la pantalla dentro del menu', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pantalla', @level2type = N'COLUMN', @level2name = N'Descripcion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre de la pantalla.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pantalla', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Id de la pantilla', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pantalla', @level2type = N'COLUMN', @level2name = N'IdPantalla';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de pantallas ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pantalla';

