CREATE TABLE [dbo].[DefinicionIndicador] (
    [IdDefinicion] INT            NOT NULL,
    [Fuente]       VARCHAR (300)  NOT NULL,
    [Notas]        VARCHAR (3000) NOT NULL,
    [IdIndicador]  INT            NOT NULL,
    [IdEstado]     INT            NOT NULL,
    [Definicion]   VARCHAR (3000) NOT NULL,
    CONSTRAINT [FK_DefinicionIndicadores_EstadoRegistro] FOREIGN KEY ([IdEstado]) REFERENCES [dbo].[EstadoRegistro] ([IdEstado]),
    CONSTRAINT [FK_DefinicionIndicadores_Indicador] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);

