CREATE TABLE [dbo].[Genero] (
    [IdGenero]    INT          NOT NULL,
    [Descripcion] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_GENERO] PRIMARY KEY CLUSTERED ([IdGenero] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción del genero', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Genero', @level2type = N'COLUMN', @level2name = N'Descripcion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del genero', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Genero', @level2type = N'COLUMN', @level2name = N'IdGenero';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de generos', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Genero';

