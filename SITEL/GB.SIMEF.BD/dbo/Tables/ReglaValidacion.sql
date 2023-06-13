CREATE TABLE [dbo].[ReglaValidacion] (
    [IdReglaValidacion]   INT            IDENTITY (1, 1) NOT NULL,
    [Codigo]              VARCHAR (30)   NULL,
    [Nombre]              VARCHAR (500)  NULL,
    [Descripcion]         VARCHAR (3000) NULL,
    [IdIndicador]         INT            NULL,
    [FechaCreacion]       DATETIME       NOT NULL,
    [UsuarioCreacion]     VARCHAR (100)  NOT NULL,
    [FechaModificacion]   DATETIME       NULL,
    [UsuarioModificacion] VARCHAR (100)  NULL,
    [IdEstadoRegistro]    INT            NOT NULL,
    CONSTRAINT [PK_ReglaValidacion] PRIMARY KEY CLUSTERED ([IdReglaValidacion] ASC),
    CONSTRAINT [FK_ReglaValidacion_EstadoRegistro] FOREIGN KEY ([IdEstadoRegistro]) REFERENCES [dbo].[EstadoRegistro] ([IdEstadoRegistro]),
    CONSTRAINT [FK_ReglaValidacion_Indicador] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);

