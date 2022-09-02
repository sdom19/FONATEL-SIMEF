CREATE TABLE [dbo].[ReglaValidacion] (
    [idRegla]             INT            NOT NULL,
    [Codigo]              VARCHAR (30)   NULL,
    [Nombre]              VARCHAR (300)  NULL,
    [IdTipo]              INT            NOT NULL,
    [Descripcion]         VARCHAR (3000) NULL,
    [idOperador]          INT            NOT NULL,
    [idIndicador]         INT            NULL,
    [FechaCreacion]       DATETIME       NOT NULL,
    [UsuarioCreacion]     VARCHAR (100)  NOT NULL,
    [FechaModificacion]   DATETIME       NULL,
    [UsuarioModificacion] VARCHAR (100)  NULL,
    [idEstado]            INT            NOT NULL,
    CONSTRAINT [PK_ReglaValidacion] PRIMARY KEY CLUSTERED ([idRegla] ASC),
    CONSTRAINT [FK_ReglaValidacion_EstadoRegistro] FOREIGN KEY ([idEstado]) REFERENCES [dbo].[EstadoRegistro] ([idEstado]),
    CONSTRAINT [FK_ReglaValidacion_Indicador] FOREIGN KEY ([idIndicador]) REFERENCES [dbo].[Indicador] ([idIndicador])
);

