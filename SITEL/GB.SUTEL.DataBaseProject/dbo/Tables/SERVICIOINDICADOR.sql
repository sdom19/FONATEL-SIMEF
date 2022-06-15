CREATE TABLE [dbo].[ServicioIndicador] (
    [IdIndicador] NVARCHAR (50) NOT NULL,
    [IdServicio]  INT           NOT NULL,
    [Mercado]     BIT           NULL,
    CONSTRAINT [PK__SERVICIO__B0D7C39765BCC72D] PRIMARY KEY CLUSTERED ([IdServicio] ASC, [IdIndicador] ASC),
    CONSTRAINT [FK_SERVICIOINDICADOR_INDICADOR] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador]),
    CONSTRAINT [FK_SERVICIOINDICADOR_SERVICIO] FOREIGN KEY ([IdServicio]) REFERENCES [dbo].[Servicio] ([IdServicio])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del servicio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioIndicador', @level2type = N'COLUMN', @level2name = N'IdServicio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioIndicador', @level2type = N'COLUMN', @level2name = N'IdIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Relación del servicio con el indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioIndicador';

