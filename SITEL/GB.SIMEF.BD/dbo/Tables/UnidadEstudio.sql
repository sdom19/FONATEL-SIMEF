CREATE TABLE [dbo].[UnidadEstudio] (
    [idUnidad] INT          NOT NULL,
    [Nombre]   VARCHAR (50) NOT NULL,
    [Estado]   BIT          NOT NULL,
    CONSTRAINT [PK_UnidadEstudio] PRIMARY KEY CLUSTERED ([idUnidad] ASC)
);

