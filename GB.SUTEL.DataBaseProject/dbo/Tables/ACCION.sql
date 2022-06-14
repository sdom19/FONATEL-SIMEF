CREATE TABLE [dbo].[Accion] (
    [IdAccion] INT          NOT NULL,
    [Nombre]   VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_ACCION] PRIMARY KEY CLUSTERED ([IdAccion] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre de la acción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Accion', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'ID de la acción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Accion', @level2type = N'COLUMN', @level2name = N'IdAccion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Contiene las acciones que puede realizar el usuario sobre las pantallas (Editar, modificar...)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Accion';

