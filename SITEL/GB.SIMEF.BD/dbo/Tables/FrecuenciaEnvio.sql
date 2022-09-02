CREATE TABLE [dbo].[FrecuenciaEnvio] (
    [idFrecuencia] INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]       VARCHAR (300) NOT NULL,
    [CantidadDias] INT           NOT NULL,
    [Estado]       BIT           CONSTRAINT [DF_FrecuenciaEnvio_Estado] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_FrecuenciaEnvio] PRIMARY KEY CLUSTERED ([idFrecuencia] ASC)
);

