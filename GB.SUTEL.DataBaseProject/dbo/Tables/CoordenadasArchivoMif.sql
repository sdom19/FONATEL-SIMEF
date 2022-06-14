CREATE TABLE [dbo].[CoordenadasArchivoMif] (
    [IdCoordenadasArchivoMif] INT          IDENTITY (1, 1) NOT NULL,
    [IdArchivoMif]            INT          NOT NULL,
    [Latitud]                 VARCHAR (15) NOT NULL,
    [Longitud]                VARCHAR (15) NOT NULL,
    [Region]                  INT          NOT NULL,
    CONSTRAINT [PK_CoordenadasArchivoMif] PRIMARY KEY CLUSTERED ([IdCoordenadasArchivoMif] ASC) WITH (FILLFACTOR = 90)
);



