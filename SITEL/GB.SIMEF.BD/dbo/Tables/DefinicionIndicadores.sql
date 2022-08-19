CREATE TABLE [dbo].[DefinicionIndicadores] (
    [idDefinición] INT            NOT NULL,
    [Fuente]       VARCHAR (300)  NOT NULL,
    [Notas]        VARCHAR (3000) NOT NULL,
    [idIndicador]  INT            NOT NULL,
    [idEstado]     INT            NOT NULL,
    CONSTRAINT [FK_DefinicionIndicadores_EstadoRegistro] FOREIGN KEY ([idEstado]) REFERENCES [dbo].[EstadoRegistro] ([idEstado]),
    CONSTRAINT [FK_DefinicionIndicadores_Indicador] FOREIGN KEY ([idIndicador]) REFERENCES [dbo].[Indicador] ([idIndicador])
);

