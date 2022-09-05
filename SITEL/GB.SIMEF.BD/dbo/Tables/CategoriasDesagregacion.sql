CREATE TABLE [dbo].[CategoriasDesagregacion] (
    [idCategoria]                  INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]                       VARCHAR (30)  NOT NULL,
    [NombreCategoria]              VARCHAR (300) NOT NULL,
    [CantidadDetalleDesagregacion] INT           NOT NULL,
    [idTipoDetalle]                INT           NOT NULL,
    [IdTipoCategoria]              INT           NOT NULL,
    [FechaCreacion]                DATETIME      CONSTRAINT [DF_CategoriasDesagregacion_FechaCreacion] DEFAULT (getdate()) NOT NULL,
    [UsuarioCreacion]              VARCHAR (100) NOT NULL,
    [FechaModificacion]            DATETIME      NULL,
    [UsuarioModificacion]          VARCHAR (100) NULL,
    [idEstado]                     INT           NOT NULL,
    CONSTRAINT [PK_CategoriasDesagregacion] PRIMARY KEY CLUSTERED ([idCategoria] ASC),
    CONSTRAINT [FK_CategoriasDesagregacion_EstadoRegistro] FOREIGN KEY ([idEstado]) REFERENCES [dbo].[EstadoRegistro] ([idEstado]),
    CONSTRAINT [FK_CategoriasDesagregacion_TipoCategoria] FOREIGN KEY ([IdTipoCategoria]) REFERENCES [dbo].[TipoCategoria] ([idTipoCategoria]),
    CONSTRAINT [FK_CategoriasDesagregacion_TiposDetalleCategoria] FOREIGN KEY ([idTipoDetalle]) REFERENCES [dbo].[TipoDetalleCategoria] ([idTipoCategoria])
);

