CREATE TABLE [dbo].[Frecuencia] (
    [IdFrecuencia]     INT          IDENTITY (1, 1) NOT NULL,
    [NombreFrecuencia] VARCHAR (20) NOT NULL,
    [Borrado]          TINYINT      NOT NULL,
    [CantidadMeses]    INT          NOT NULL,
    CONSTRAINT [PK_FRECUENCIA] PRIMARY KEY CLUSTERED ([IdFrecuencia] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Cantidad de meses que comprende la frecuencia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Frecuencia', @level2type = N'COLUMN', @level2name = N'CantidadMeses';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Frecuencia', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre de la frecuencia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Frecuencia', @level2type = N'COLUMN', @level2name = N'NombreFrecuencia';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Código de la frecuencia y el desglose', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Frecuencia', @level2type = N'COLUMN', @level2name = N'IdFrecuencia';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de frecuencias y desgloses', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Frecuencia';

