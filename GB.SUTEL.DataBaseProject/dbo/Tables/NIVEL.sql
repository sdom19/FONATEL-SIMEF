CREATE TABLE [dbo].[Nivel] (
    [IdNivel]   INT          IDENTITY (1, 1) NOT NULL,
    [DescNivel] VARCHAR (50) NOT NULL,
    [Borrado]   TINYINT      NOT NULL,
    CONSTRAINT [PK_NIVEL] PRIMARY KEY CLUSTERED ([IdNivel] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico del dato', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Nivel', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción del nivel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Nivel', @level2type = N'COLUMN', @level2name = N'DescNivel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Id del nivel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Nivel', @level2type = N'COLUMN', @level2name = N'IdNivel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de Niveles en el árbol del constructor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Nivel';

