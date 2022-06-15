CREATE TABLE [dbo].[ArchivoTelevisionDigitalCsv] (
    [IdArchivoCsv]        INT         IDENTITY (1, 1) NOT NULL,
    [CodigoSitio]         VARCHAR (8) NOT NULL,
    [FechaCapturaArchivo] DATE        NOT NULL,
    [CodigoConsecutivo]   VARCHAR (5) NOT NULL,
    PRIMARY KEY CLUSTERED ([IdArchivoCsv] ASC)
);

