CREATE TABLE [dbo].[RelacionCategoria] (
    [IdRelacionCategoria]      INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]                   VARCHAR (30)  NOT NULL,
    [Nombre]                   VARCHAR (500) NOT NULL,
    [CantidadCategoria]        INT           NOT NULL,
    [IdCategoriaDesagregacion] INT           NOT NULL,
    [FechaCreacion]            DATETIME      NOT NULL,
    [UsuarioCreacion]          VARCHAR (100) NOT NULL,
    [FechaModificacion]        DATETIME      NULL,
    [UsuarioModificacion]      VARCHAR (100) NULL,
    [IdEstadoRegistro]         INT           NOT NULL,
    [CantidadFila]             INT           CONSTRAINT [DF__RelacionC__Canti__40BA7AC1] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RelacionCategoria] PRIMARY KEY CLUSTERED ([IdRelacionCategoria] ASC),
    CONSTRAINT [FK_RelacionCategoria_CategoriaDesagregacion] FOREIGN KEY ([IdCategoriaDesagregacion]) REFERENCES [dbo].[CategoriaDesagregacion] ([IdCategoriaDesagregacion]),
    CONSTRAINT [FK_RelacionCategoria_EstadoRegistro1] FOREIGN KEY ([IdEstadoRegistro]) REFERENCES [dbo].[EstadoRegistro] ([IdEstadoRegistro])
);

