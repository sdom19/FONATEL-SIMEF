CREATE TABLE [dbo].[IndicadorResultadoDetalleVariable] (
    [IdIndicadorResultadoDetalleVariable] UNIQUEIDENTIFIER CONSTRAINT [DF_IndicadorResultadoDetalleVariable_IdResultadoDetalle] DEFAULT (newid()) NOT NULL,
    [IdIndicadorResultado]                UNIQUEIDENTIFIER NOT NULL,
    [IdIndicador]                         INT              NOT NULL,
    [NombreVariable]                      VARCHAR (300)    NULL,
    [IdDetalleIndicadorVariable]          INT              NOT NULL,
    [NumeroFila]                          INT              NOT NULL,
    [Valor]                               DECIMAL (18, 2)  NOT NULL,
    [IndicadorReferencia]                 INT              NULL,
    [Estado]                              BIT              CONSTRAINT [DF_IndicadorResultadoDetalleVariable_Estado] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_IndicadorResultadoDetalleVariable] PRIMARY KEY CLUSTERED ([IdIndicadorResultadoDetalleVariable] ASC),
    CONSTRAINT [FK_IndicadorResultadoDetalleVariable_DetalleIndicadorVariable] FOREIGN KEY ([IdDetalleIndicadorVariable], [IdIndicador]) REFERENCES [dbo].[DetalleIndicadorVariable] ([IdDetalleIndicadorVariable], [IdIndicador]),
    CONSTRAINT [FK_IndicadorResultadoDetalleVariable_Indicador] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador]),
    CONSTRAINT [FK_IndicadorResultadoDetalleVariable_IndicadorResultado] FOREIGN KEY ([IdIndicadorResultado]) REFERENCES [dbo].[IndicadorResultado] ([IdIndicadorResultado])
);

