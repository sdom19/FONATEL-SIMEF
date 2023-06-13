CREATE TABLE [dbo].[FrecuenciaEnvio] (
    [IdFrecuenciaEnvio] INT           NOT NULL,
    [Nombre]            VARCHAR (300) NOT NULL,
    [CantidadDia]       INT           NOT NULL,
    [CantidadMes]       INT           NOT NULL,
    [Estado]            BIT           CONSTRAINT [DF_FrecuenciaEnvio_Estado] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_FrecuenciaEnvio] PRIMARY KEY CLUSTERED ([IdFrecuenciaEnvio] ASC)
);

