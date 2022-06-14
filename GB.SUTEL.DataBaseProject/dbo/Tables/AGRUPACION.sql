CREATE TABLE [dbo].[Agrupacion] (
    [IdAgrupacion]   INT            IDENTITY (1, 1) NOT NULL,
    [DescAgrupacion] NVARCHAR (600) NOT NULL,
    [Borrado]        TINYINT        NOT NULL,
    CONSTRAINT [PK_AGRUPACION] PRIMARY KEY CLUSTERED ([IdAgrupacion] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'indica si esta borrado logicamente', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Agrupacion', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'descripción de la agrupación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Agrupacion', @level2type = N'COLUMN', @level2name = N'DescAgrupacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Id de la agrupación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Agrupacion', @level2type = N'COLUMN', @level2name = N'IdAgrupacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Contiene las agrupaciones', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Agrupacion';

