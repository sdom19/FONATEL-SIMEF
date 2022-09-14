CREATE TABLE [dbo].[UnidadEstudio] (
    [IdUnidad] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre]   VARCHAR (50) NOT NULL,
    [Estado]   BIT          NOT NULL,
    CONSTRAINT [PK_UnidadEstudio] PRIMARY KEY CLUSTERED ([IdUnidad] ASC)
);



