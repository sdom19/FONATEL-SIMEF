CREATE TABLE [dbo].[FormulaCalculo] (
    [IdFormulaCalculo]           INT              IDENTITY (1, 1) NOT NULL,
    [Codigo]                     VARCHAR (30)     NOT NULL,
    [Nombre]                     VARCHAR (500)    NOT NULL,
    [IdIndicador]                INT              NULL,
    [IdDetalleIndicadorVariable] INT              NULL,
    [FechaCalculo]               DATETIME         NULL,
    [Descripcion]                VARCHAR (1500)   NULL,
    [IdFrecuenciaEnvio]          INT              NULL,
    [NivelCalculoTotal]          BIT              NULL,
    [UsuarioModificacion]        VARCHAR (100)    NULL,
    [FechaCreacion]              DATETIME         NULL,
    [UsuarioCreacion]            VARCHAR (100)    NULL,
    [FechaModificacion]          DATETIME         NULL,
    [IdEstadoRegistro]           INT              NOT NULL,
    [Formula]                    VARCHAR (8000)   NULL,
    [IdJob]                      UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_FormulaCalculo] PRIMARY KEY CLUSTERED ([IdFormulaCalculo] ASC),
    CONSTRAINT [FK_FormulaCalculo_DetalleIndicadorVariable] FOREIGN KEY ([IdDetalleIndicadorVariable], [IdIndicador]) REFERENCES [dbo].[DetalleIndicadorVariable] ([IdDetalleIndicadorVariable], [IdIndicador]),
    CONSTRAINT [FK_FormulaCalculo_EstadoRegistro] FOREIGN KEY ([IdEstadoRegistro]) REFERENCES [dbo].[EstadoRegistro] ([IdEstadoRegistro]),
    CONSTRAINT [FK_FormulaCalculo_FrecuenciaEnvio] FOREIGN KEY ([IdFrecuenciaEnvio]) REFERENCES [dbo].[FrecuenciaEnvio] ([IdFrecuenciaEnvio])
);

