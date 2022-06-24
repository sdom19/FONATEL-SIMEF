CREATE TABLE [dbo].[ArchivoCsv] (
    [IdArchivoCsv]        INT         IDENTITY (1, 1) NOT NULL,
    [CodigoSitio]         VARCHAR (8) NOT NULL,
    [FechaCapturaArchivo] DATE        NOT NULL,
    [CodigoConsecutivo]   VARCHAR (5) NOT NULL,
    CONSTRAINT [PK_ArchivoCsv] PRIMARY KEY CLUSTERED ([IdArchivoCsv] ASC) WITH (FILLFACTOR = 90)
);

