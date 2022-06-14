CREATE TABLE [dbo].[Servicio] (
    [IdServicio]  INT           IDENTITY (1, 1) NOT NULL,
    [DesServicio] VARCHAR (250) NOT NULL,
    [Borrado]     TINYINT       NOT NULL,
    CONSTRAINT [PK_SERVICIO] PRIMARY KEY CLUSTERED ([IdServicio] ASC)
);














GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Servicio', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción del servicio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Servicio', @level2type = N'COLUMN', @level2name = N'DesServicio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del servicio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Servicio', @level2type = N'COLUMN', @level2name = N'IdServicio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de servicios', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Servicio';

