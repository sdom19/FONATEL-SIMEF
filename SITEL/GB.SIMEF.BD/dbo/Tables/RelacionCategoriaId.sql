CREATE TABLE [dbo].[RelacionCategoriaId] (
    [IdRelacionCategoriaId]    INT          NOT NULL,
    [IdCategoriaDesagregacion] VARCHAR (50) NOT NULL,
    [IdEstadoRegistro]         INT          DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_RelacionCategoriaId] PRIMARY KEY CLUSTERED ([IdRelacionCategoriaId] ASC, [IdCategoriaDesagregacion] ASC),
    CONSTRAINT [FK_RelacionCategoriaId_EstadoRegistro1] FOREIGN KEY ([IdEstadoRegistro]) REFERENCES [dbo].[EstadoRegistro] ([IdEstadoRegistro]),
    CONSTRAINT [FK_RelacionCategoriaId_RelacionCategoria] FOREIGN KEY ([IdRelacionCategoriaId]) REFERENCES [dbo].[RelacionCategoria] ([IdRelacionCategoria])
);

