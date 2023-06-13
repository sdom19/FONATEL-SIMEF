CREATE TABLE [dbo].[IndicadorResultado] (
    [IdIndicadorResultado] UNIQUEIDENTIFIER CONSTRAINT [DF_IndicadorResultado_IdResultado] DEFAULT (newid()) NOT NULL,
    [IdIndicador]          INT              NOT NULL,
    [NombreIndicador]      VARCHAR (300)    NOT NULL,
    [FechaCreacion]        DATE             NOT NULL,
    [Usuario]              VARCHAR (300)    NOT NULL,
    [TipoIndicador]        INT              NOT NULL,
    [Estado]               BIT              NOT NULL,
    [IdMes]                INT              NOT NULL,
    [IdAnno]               INT              NOT NULL,
    CONSTRAINT [PK_IndicadorResultado] PRIMARY KEY CLUSTERED ([IdIndicadorResultado] ASC)
);

