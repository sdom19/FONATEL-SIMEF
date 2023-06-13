CREATE TABLE [dbo].[IndicardorResultadoDetalleCategoria] (
    [IdIndicardorResultadoDetalleCategoria] UNIQUEIDENTIFIER CONSTRAINT [DF_IndicardorResultadoDetalleCategoria_IdResultadoDetalle] DEFAULT (newid()) NOT NULL,
    [IdIndicadorResultado]                  UNIQUEIDENTIFIER NOT NULL,
    [IdCategoriaDesagregacion]              INT              NOT NULL,
    [NumeroFila]                            INT              NOT NULL,
    [TipoCategoria]                         INT              NOT NULL,
    [Valor]                                 VARCHAR (2000)   NOT NULL,
    [IndicadorReferencia]                   INT              NULL,
    [Estado]                                BIT              NULL,
    [FechaCreacion]                         DATETIME         CONSTRAINT [DF_IndicardorResultadoDetalleCategoria_FechaCreacion] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_IndicardorResultadoDetalle] PRIMARY KEY CLUSTERED ([IdIndicardorResultadoDetalleCategoria] ASC),
    CONSTRAINT [FK_IndicardorResultadoDetalleCategoria_IndicadorResultado] FOREIGN KEY ([IdIndicadorResultado]) REFERENCES [dbo].[IndicadorResultado] ([IdIndicadorResultado])
);

