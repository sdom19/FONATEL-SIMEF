CREATE TABLE [dbo].[ArchivoMif] (
    [IdArchivoMif]     INT           IDENTITY (1, 1) NOT NULL,
    [NombreArchivoMif] VARCHAR (100) NOT NULL,
    [Region]           INT           NOT NULL,
    CONSTRAINT [PK_ArchivoMif] PRIMARY KEY CLUSTERED ([IdArchivoMif] ASC)
);

