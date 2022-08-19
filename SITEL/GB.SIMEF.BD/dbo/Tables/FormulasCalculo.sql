CREATE TABLE [dbo].[FormulasCalculo] (
    [idFormula]           INT            NOT NULL,
    [Codigo]              VARCHAR (30)   NOT NULL,
    [Nombre]              VARCHAR (60)   NOT NULL,
    [idIndicador]         INT            NOT NULL,
    [idIndicadorVariable] INT            NOT NULL,
    [Descripcion]         VARCHAR (3000) NOT NULL,
    [idFrecuencia]        INT            NOT NULL,
    [NivelCalculoTotal]   BIT            NOT NULL,
    [UsuarioModificacion] VARCHAR (100)  NULL,
    [FechaCreacion]       DATETIME       NULL,
    [UsuarioCreacion]     VARCHAR (100)  NULL,
    [FechaModificacion]   DATETIME       NULL,
    [IdEstado]            INT            NOT NULL,
    CONSTRAINT [PK_FormulasCalculo] PRIMARY KEY CLUSTERED ([idFormula] ASC),
    CONSTRAINT [FK_FormulasCalculo_DetalleIndicadorVariables] FOREIGN KEY ([idIndicadorVariable], [idIndicador]) REFERENCES [dbo].[DetalleIndicadorVariables] ([idDetalleIndicador], [idIndicador]),
    CONSTRAINT [FK_FormulasCalculo_DetalleIndicadorVariables1] FOREIGN KEY ([idIndicadorVariable], [idIndicador]) REFERENCES [dbo].[DetalleIndicadorVariables] ([idDetalleIndicador], [idIndicador]),
    CONSTRAINT [FK_FormulasCalculo_EstadoRegistro] FOREIGN KEY ([IdEstado]) REFERENCES [dbo].[EstadoRegistro] ([idEstado]),
    CONSTRAINT [FK_FormulasCalculo_FrecuenciaEnvio] FOREIGN KEY ([idFrecuencia]) REFERENCES [dbo].[FrecuenciaEnvio] ([idFrecuencia])
);

