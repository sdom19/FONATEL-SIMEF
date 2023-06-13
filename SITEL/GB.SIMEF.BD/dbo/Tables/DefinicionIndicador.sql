CREATE TABLE [dbo].[DefinicionIndicador] (
    [IdDefinicionIndicador] INT            NOT NULL,
    [Fuente]                VARCHAR (300)  NOT NULL,
    [Nota]                  VARCHAR (3000) NOT NULL,
    [IdEstadoRegistro]      INT            NOT NULL,
    [Definicion]            VARCHAR (2000) NOT NULL,
    CONSTRAINT [PK_DefinicionIndicador] PRIMARY KEY CLUSTERED ([IdDefinicionIndicador] ASC),
    CONSTRAINT [FK_DefinicionIndicador_EstadoRegistro] FOREIGN KEY ([IdEstadoRegistro]) REFERENCES [dbo].[EstadoRegistro] ([IdEstadoRegistro]),
    CONSTRAINT [FK_DefinicionIndicador_Indicador] FOREIGN KEY ([IdDefinicionIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);

