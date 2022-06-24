CREATE TABLE [dbo].[DetalleArchivoTelevisionDigitalCsv] (
    [idDetalle]     INT            IDENTITY (1, 1) NOT NULL,
    [IdArchivoCsv]  INT            NULL,
    [Tiempo]        DATETIME       NULL,
    [Frecuencia]    INT            NULL,
    [NombreSistema] VARCHAR (100)  NULL,
    [ISDBT]         DECIMAL (5, 2) NULL,
    [Acimut]        INT            NULL,
    [Logitud]       VARCHAR (50)   NULL,
    [Latitude]      VARCHAR (50)   NULL,
    [ArchInfo]      VARCHAR (100)  NULL,
    PRIMARY KEY CLUSTERED ([idDetalle] ASC),
    CONSTRAINT [FkDetalleArchivoDigitalArchivo] FOREIGN KEY ([IdArchivoCsv]) REFERENCES [dbo].[ArchivoTelevisionDigitalCsv] ([IdArchivoCsv])
);

