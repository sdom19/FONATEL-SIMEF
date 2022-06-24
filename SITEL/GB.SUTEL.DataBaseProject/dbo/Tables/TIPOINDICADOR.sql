CREATE TABLE [dbo].[TipoIndicador] (
    [IdTipoInd]  INT           IDENTITY (1, 1) NOT NULL,
    [DesTipoInd] VARCHAR (250) NOT NULL,
    [Borrado]    TINYINT       NOT NULL,
    CONSTRAINT [PK_TIPOINDICADOR] PRIMARY KEY CLUSTERED ([IdTipoInd] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoIndicador', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción del tipo de indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoIndicador', @level2type = N'COLUMN', @level2name = N'DesTipoInd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del tipo de indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoIndicador', @level2type = N'COLUMN', @level2name = N'IdTipoInd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de tipos de indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoIndicador';

