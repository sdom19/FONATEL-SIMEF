CREATE TABLE [dbo].[TipoIndicadorServicio] (
    [IdTipoInd]  INT     NOT NULL,
    [IdServicio] INT     NOT NULL,
    [Borrado]    TINYINT NOT NULL,
    CONSTRAINT [PK_TIPOINDICADORSERVICIO] PRIMARY KEY CLUSTERED ([IdTipoInd] ASC, [IdServicio] ASC),
    CONSTRAINT [FK_TIPOINDI_REFERENCE_SERVICIO] FOREIGN KEY ([IdServicio]) REFERENCES [dbo].[Servicio] ([IdServicio]),
    CONSTRAINT [FK_TIPOINDI_REFERENCE_TIPOINDI] FOREIGN KEY ([IdTipoInd]) REFERENCES [dbo].[TipoIndicador] ([IdTipoInd])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoIndicadorServicio', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del servicio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoIndicadorServicio', @level2type = N'COLUMN', @level2name = N'IdServicio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de tipo indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoIndicadorServicio', @level2type = N'COLUMN', @level2name = N'IdTipoInd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Relación del tipo de indicador con servicio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TipoIndicadorServicio';

