CREATE TABLE [dbo].[IndicadorResultadoSolicitud] (
    [IdFormularioWeb]               INT              NOT NULL,
    [IdSolicitud]                   INT              NOT NULL,
    [IdIndicador]                   INT              NOT NULL,
    [IdIndicadorResultado]          UNIQUEIDENTIFIER NOT NULL,
    [IdIndicadorResultadoSolicitud] UNIQUEIDENTIFIER CONSTRAINT [DF_IndicadorResultadoSolicitud_IdIndicadorResultadoSolicitud] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_IndicadorResultadoSolicitud] PRIMARY KEY CLUSTERED ([IdIndicadorResultadoSolicitud] ASC),
    CONSTRAINT [FK_IndicadorResultadoSolicitud_IndicadorResultado] FOREIGN KEY ([IdIndicadorResultado]) REFERENCES [dbo].[IndicadorResultado] ([IdIndicadorResultado])
);

