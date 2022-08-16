CREATE TABLE [dbo].[RelacionCategoria] (
    [idRelacionCategoria] INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]              VARCHAR (30)  NULL,
    [Nombre]              VARCHAR (300) NULL,
    [CantidadCategoria]   INT           NULL,
    [idCategoria]         INT           NULL,
    [idCategoriaValor]    VARCHAR (300) NULL,
    [FechaCreacion]       DATETIME      NOT NULL,
    [UsuarioCreacion]     VARCHAR (100) NOT NULL,
    [FechaModificacion]   DATETIME      NULL,
    [UsuarioModificacion] VARCHAR (100) NULL,
    [idEstado]            INT           NULL,
    CONSTRAINT [PK_RelacionCategoria] PRIMARY KEY CLUSTERED ([idRelacionCategoria] ASC),
    CONSTRAINT [FK_RelacionCategoria_CategoriasDesagregacion] FOREIGN KEY ([idCategoria]) REFERENCES [dbo].[CategoriasDesagregacion] ([idCategoria]),
    CONSTRAINT [FK_RelacionCategoria_EstadoRegistro1] FOREIGN KEY ([idEstado]) REFERENCES [dbo].[EstadoRegistro] ([idEstado])
);

