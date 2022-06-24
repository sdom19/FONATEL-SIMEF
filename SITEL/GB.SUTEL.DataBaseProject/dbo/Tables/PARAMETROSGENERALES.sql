CREATE TABLE [dbo].[ParametrosGenerales] (
    [IdParametros] INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]       VARCHAR (60)  NOT NULL,
    [Valor]        VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_PARAMETROS] PRIMARY KEY CLUSTERED ([IdParametros] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Valor del párametro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ParametrosGenerales', @level2type = N'COLUMN', @level2name = N'Valor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre del parámetro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ParametrosGenerales', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del parametro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ParametrosGenerales', @level2type = N'COLUMN', @level2name = N'IdParametros';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Parametros generales del sistema', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ParametrosGenerales';

