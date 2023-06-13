CREATE TABLE [dbo].[CategoriaDesagregacion] (
    [IdCategoriaDesagregacion]     INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]                       VARCHAR (30)  NOT NULL,
    [NombreCategoria]              VARCHAR (500) NOT NULL,
    [CantidadDetalleDesagregacion] INT           NOT NULL,
    [IdTipoDetalleCategoria]       INT           NOT NULL,
    [IdTipoCategoria]              INT           NOT NULL,
    [FechaCreacion]                DATETIME      CONSTRAINT [DF_CategoriaDesagregacion_FechaCreacion] DEFAULT (getdate()) NOT NULL,
    [UsuarioCreacion]              VARCHAR (100) NOT NULL,
    [FechaModificacion]            DATETIME      NULL,
    [UsuarioModificacion]          VARCHAR (100) NULL,
    [IdEstadoRegistro]             INT           NOT NULL,
    CONSTRAINT [PK_CategoriaDesagregacion] PRIMARY KEY CLUSTERED ([IdCategoriaDesagregacion] ASC),
    CONSTRAINT [FK_CategoriaDesagregacion_EstadoRegistro] FOREIGN KEY ([IdEstadoRegistro]) REFERENCES [dbo].[EstadoRegistro] ([IdEstadoRegistro]),
    CONSTRAINT [FK_CategoriaDesagregacion_TipoCategoria] FOREIGN KEY ([IdTipoCategoria]) REFERENCES [dbo].[TipoCategoria] ([IdTipoCategoria]),
    CONSTRAINT [FK_CategoriaDesagregacion_TiposDetalleCategoria] FOREIGN KEY ([IdTipoDetalleCategoria]) REFERENCES [dbo].[TipoDetalleCategoria] ([IdTipoDetalleCategoria])
);

